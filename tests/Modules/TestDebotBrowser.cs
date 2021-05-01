using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;

namespace TonSdk.Tests.Modules
{
    public class TestDebotBrowser
    {
        public const int ExitChoice = 9;
        private const int DebotWc = -31;

        private readonly ITestDebot _debot;
        private readonly ILogger _logger;

        private readonly string[] _supportedInterfaces =
        {
            "f6927c0d4bdb69e1b52d27f018d156ff04152f00558042ff674f0fec32e4369d", // echo
            "8796536366ee21852db56dccb60bc564598b618c865fc50c8b1ab740bba128e3", // terminal
        };

        private readonly string EchoAbi = @"
{
	""ABI version"": 2,
	""header"": [""time""],
	""functions"": [
		{
			""name"": ""echo"",
			""inputs"": [
				{""name"":""answerId"",""type"":""uint32""},
				{""name"":""request"",""type"":""bytes""}
			],
			""outputs"": [
				{""name"":""response"",""type"":""bytes""}
			]
		}
	],
	""data"": [],
	""events"": []
}
";


        private readonly string TerminalAbi = @"
{
	""ABI version"": 2,
	""header"": [""time""],
	""functions"": [
		{
			""name"": ""print"",
			""inputs"": [
				{""name"":""answerId"",""type"":""uint32""},
				{""name"":""message"",""type"":""bytes""}
			],
			""outputs"": []
        },
        {
			""name"": ""inputInt"",
			""inputs"": [
				{""name"":""answerId"",""type"":""uint32""},
				{""name"":""prompt"",""type"":""bytes""}
			],
			""outputs"": [
				{""name"":""value"",""type"":""int256""}
			]
		},
        {
			""name"": ""input"",
			""inputs"": [
				{""name"":""answerId"",""type"":""uint32""},
				{""name"":""prompt"",""type"":""bytes""},
				{""name"":""multiline"",""type"":""bool""}
			],
			""outputs"": [
				{""name"":""value"",""type"":""bytes""}
			]
		}
	],
	""data"": [],
	""events"": []
}
";

        public TestDebotBrowser(ITestDebot debot, ILogger logger)
        {
            _debot = debot ?? throw new ArgumentNullException(nameof(debot));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync(List<DebotStep> steps, List<string> terminalOutputs = null)
        {
            var state = new BrowserData
            {
                Current = new Mutex<CurrentStepData>(new CurrentStepData()),
                Next = new Mutex<List<DebotStep>>(steps),
                Keys = _debot.Keys,
                Address = _debot.Address,
                Finished = false,
                Info = new DebotInfo
                {
                    Dabi = ((Abi.Contract)_debot.Abi).Value.ToJson().ToString()
                },
                Terminal = new Mutex<Terminal>(new Terminal(terminalOutputs ?? new List<string>()))
            };

            await ExecuteFromStateAsync(state, true);
        }

        public async Task ExecuteWithDetailsAsync(
            List<DebotStep> steps, 
            DebotInfo debotInfo,
            List<ExpectedTransaction> activity,
            List<string> terminalOutputs = null)
        {
            var state = new BrowserData
            {
                Current = new Mutex<CurrentStepData>(new CurrentStepData()),
                Next = new Mutex<List<DebotStep>>(steps),
                Keys = _debot.Keys,
                Address = _debot.Address,
                Finished = false,
                Info = debotInfo,
                Activity = new Mutex<List<ExpectedTransaction>>(activity),
                Terminal = new Mutex<Terminal>(new Terminal(terminalOutputs ?? new List<string>()))
            };

            await ExecuteFromStateAsync(state, true);
        }

        private async Task<RegisteredDebot> FetchDebotAsync(BrowserData state, string addr)
        {
            var handle = await _debot.Client.Debot.InitAsync(new ParamsOfInit
            {
                Address = addr
            }, GetExecuteCallback(state));

            using var bots = await state.Bots.LockAsync();

            bots.Instance.Add(addr, new RegisteredDebot
            {
                DebotAbi = handle.DebotAbi,
                DebotHandle = handle.DebotHandle,
                Info = handle.Info
            });

            return handle;
        }

        private async Task ExecuteFromStateAsync(BrowserData state, bool callStart)
        {
            if (callStart)
            {
                var res = await _debot.Client.Debot.FetchAsync(new ParamsOfFetch
                {
                    Address = state.Address
                });
                var serializer = new TonSerializer();
                var expectedAbi = serializer.Deserialize<JToken>(state.Info.Dabi);
                var actualAbi = serializer.Deserialize<JToken>(res.Info.Dabi);
                Assert.Equal(expectedAbi, actualAbi);
                Assert.Equal(state.Info?.Author, res.Info?.Author);
                Assert.Equal(state.Info?.Dabi, res.Info?.Dabi);
                Assert.Equal(state.Info?.Hello, res.Info?.Hello);
                Assert.Equal(state.Info?.Icon, res.Info?.Icon);
                Assert.Equal(state.Info?.Interfaces ?? new string[0], res.Info?.Interfaces);
                Assert.Equal(state.Info?.Key, res.Info?.Key);
                Assert.Equal(state.Info?.Language, res.Info?.Language);
                Assert.Equal(state.Info?.Name, res.Info?.Name);
                Assert.Equal(state.Info?.Publisher, res.Info?.Publisher);
            }

            var handle = await FetchDebotAsync(state, state.Address);

            if (callStart)
            {
                await _debot.Client.Debot.StartAsync(new ParamsOfStart
                {
                    DebotHandle = handle.DebotHandle
                });
            }

            while (!state.Finished)
            {
                await HandleMessageQueueAsync(handle, state);

                DebotAction action;
                using (var step = await state.Current.LockAsync())
                {
                    if (!step.Instance.AvailableActions.Any())
                    {
                        break;
                    }
                    using (var next = await state.Next.LockAsync())
                    {
                        step.Instance.Step = next.Instance.RemoveFirst();
                    }
                    step.Instance.Outputs.Clear();
                    action = step.Instance.AvailableActions[step.Instance.Step.Choice - 1];
                }

                _logger.Information($"Executing action {action.Name}");

                await _debot.Client.Debot.ExecuteAsync(new ParamsOfExecute
                {
                    Action = action,
                    DebotHandle = handle.DebotHandle
                });

                using (var step = await state.Current.LockAsync())
                {
                    var leftArr = step.Instance.Step.Outputs;
                    var rightArr = step.Instance.Outputs;
                    Assert.Equal(leftArr.Count, rightArr.Count);
                    for (var i = 0; i < leftArr.Count; ++i)
                    {
                        var left = leftArr[i];
                        var right = rightArr[i];
                        Assert.NotNull(left);
                        Assert.NotNull(right);
                        var pos = left.IndexOf("{}", StringComparison.Ordinal);
                        if (pos >= 0)
                        {
                            Assert.Equal(left.Substring(0, pos), right.Substring(0, pos));
                        }
                        else
                        {
                            Assert.Equal(left, right);
                        }
                    }

                    Assert.Empty(step.Instance.Step.Inputs);
                    Assert.Empty(step.Instance.Step.Invokes);

                    if (!step.Instance.AvailableActions.Any())
                    {
                        break;
                    }
                }
            }

            using (var next = await state.Next.LockAsync())
            {
                Assert.Empty(next.Instance);
            }

            using var terminal = await state.Terminal.LockAsync();
            Assert.Empty(terminal.Instance.Messages);
        }

        private async Task HandleMessageQueueAsync(RegisteredDebot debot, BrowserData data)
        {
            var msg = await data.PopMessageAsync();
            while (msg != null)
            {
                var parsed = await _debot.Client.Boc.ParseMessageAsync(new ParamsOfParse
                {
                    Boc = msg
                });

                var body = parsed.Parsed["body"]?.ToString();
                Assert.NotNull(body);

                var destAddr = parsed.Parsed["dst"]?.ToString();
                Assert.NotNull(destAddr);

                var srcAddr = parsed.Parsed["src"]?.ToString();

                var wcAndAddr = destAddr.Split(":");
                Assert.True(wcAndAddr.Length > 1);

                var wc = int.Parse(wcAndAddr[0]);
                var interfaceId = wcAndAddr[1];

                if (wc == DebotWc)
                {
                    Assert.Contains(interfaceId, _supportedInterfaces);

                    var decoded = await _debot.Client.Abi.DecodeMessageBodyAsync(new ParamsOfDecodeMessageBody
                    {
                        Abi = GetInterfaceAbi(interfaceId),
                        Body = body,
                        IsInternal = true
                    });

                    _logger.Information($"call for interface id {interfaceId}");
                    _logger.Information($"request: {decoded.Name} ({decoded.Value})");

                    var (funcId, returnArgs) = _supportedInterfaces[0] == interfaceId
                        ? data.Echo.Call(decoded.Name, decoded.Value)
                        : await CallTerminalAsync(data, decoded.Name, decoded.Value);

                    _logger.Information($"response: {funcId} ({returnArgs})");

                    var srcDebot = await data.GetDebotAsync(srcAddr);
                    var message = await _debot.Client.Abi.EncodeInternalMessageAsync(new ParamsOfEncodeInternalMessage
                    {
                        Abi = new Abi.Json
                        {
                            Value = srcDebot.DebotAbi
                        },
                        Address = srcAddr,
                        CallSet = funcId == 0
                            ? null
                            : new CallSet
                            {
                                FunctionName = $"0x{funcId:X}",
                                Input = returnArgs
                            },
                        Value = "1000000000000000"
                    });


                    await _debot.Client.Debot.SendAsync(new ParamsOfSend
                    {
                        DebotHandle = debot.DebotHandle,
                        Message = message.Message
                    });
                }
                else
                {
                    if (!await data.DebotFetchedAsync(destAddr))
                    {
                        await FetchDebotAsync(data, destAddr);
                    }

                    var debotHandle = (await data.GetDebotAsync(destAddr)).DebotHandle;
                    await _debot.Client.Debot.SendAsync(new ParamsOfSend
                    {
                        DebotHandle = debotHandle,
                        Message = msg
                    });
                }

                msg = await data.PopMessageAsync();
            }
        }

        private Abi GetInterfaceAbi(string interfaceId)
        {
            var abiJson = _supportedInterfaces[0] == interfaceId
                ? EchoAbi
                : _supportedInterfaces[1] == interfaceId
                    ? TerminalAbi
                    : throw new NotSupportedException($"Interface not supported: {interfaceId}");

            return new Abi.Json
            {
                Value = abiJson
            };
        }

        private async Task<(uint, JToken)> CallTerminalAsync(BrowserData data, string func, JToken args)
        {
            using var terminal = await data.Terminal.LockAsync();
            return terminal.Instance.Call(func, args);
        }

        private Func<ParamsOfAppDebotBrowser, Task<ResultOfAppDebotBrowser>> GetExecuteCallback(BrowserData state)
        {
            return async @params =>
            {
                if (@params == null)
                {
                    throw new ArgumentNullException(nameof(@params));
                }

                switch (@params)
                {
                    case ParamsOfAppDebotBrowser.Log log:
                        using (var step = await state.Current.LockAsync())
                        {
                            step.Instance.Outputs.Add(log.Msg);
                        }
                        return null;

                    case ParamsOfAppDebotBrowser.Switch @switch:
                        using (var switchStarted = await state.SwitchStarted.LockAsync())
                        {
                            Assert.False(switchStarted.Swap(true));
                        }
                        if (@switch.ContextId == 255) // STATE_EXIT
                        {
                            state.Finished = true;
                        }
                        using (var step = await state.Current.LockAsync())
                        {
                            step.Instance.AvailableActions.Clear();
                        }
                        return null;

                    case ParamsOfAppDebotBrowser.ShowAction action:
                        using (var step = await state.Current.LockAsync())
                        {
                            step.Instance.AvailableActions.Add(action.Action);
                        }
                        return null;

                    case ParamsOfAppDebotBrowser.Send message:
                        using (var queue = await state.MsgQueue.LockAsync())
                        {
                            queue.Instance.Enqueue(message.Message);
                        }
                        return null;

                    case ParamsOfAppDebotBrowser.SwitchCompleted:
                        using (var switchStarted = await state.SwitchStarted.LockAsync())
                        {
                            Assert.True(switchStarted.Swap(false));
                        }
                        return null;

                    case ParamsOfAppDebotBrowser.Input:
                        using (var step = await state.Current.LockAsync())
                        {
                            var value = step.Instance.Step.Inputs.RemoveFirst();
                            return new ResultOfAppDebotBrowser.Input
                            {
                                Value = value
                            };
                        }

                    case ParamsOfAppDebotBrowser.GetSigningBox:
                        var signingBox = await _debot.Client.Crypto.GetSigningBoxAsync(state.Keys);
                        return new ResultOfAppDebotBrowser.GetSigningBox
                        {
                            SigningBox = signingBox.Handle
                        };

                    case ParamsOfAppDebotBrowser.InvokeDebot invoke:
                        List<DebotStep> steps;
                        using (var step = await state.Current.LockAsync())
                        {
                            steps = step.Instance.Step.Invokes.RemoveFirst();
                        }

                        steps[0].Choice = 1;

                        var current = new CurrentStepData
                        {
                            AvailableActions = new List<DebotAction> { invoke.Action }
                        };

                        var newState = new BrowserData
                        {
                            Current = new Mutex<CurrentStepData>(current),
                            Next = new Mutex<List<DebotStep>>(steps),
                            Keys = state.Keys,
                            Address = invoke.DebotAddr,
                            Finished = false
                        };

                        await ExecuteFromStateAsync(newState, false);

                        return new ResultOfAppDebotBrowser.InvokeDebot();

                    case ParamsOfAppDebotBrowser.Approve approve:
                        var approved = false;
                        using (var activityLock = await state.Activity.LockAsync())
                        {
                            var expected = activityLock.Instance.RemoveFirst();
                            if (expected != null)
                            {
                                approved = expected.Approved;
                                var activity = approve.Activity as DebotActivity.Transaction;
                                Assert.NotNull(activity);
                                Assert.Equal(expected.Dst, activity.Dst);
                                Assert.Equal(expected.Out, activity.Out);
                                Assert.Equal(expected.Setcode, activity.Setcode);
                                Assert.Equal(expected.Signkey, activity.Signkey);
                                Assert.True(activity.Fee > BigInteger.Zero);
                            }
                        }

                        return new ResultOfAppDebotBrowser.Approve
                        {
                            Approved = approved
                        };

                    default:
                        throw new NotSupportedException($"Callback parameter type not supported: {@params.GetType()}");
                }
            };
        }
    }

    public class DebotStep
    {
        public byte Choice { get; set; }
        public List<string> Inputs { get; set; } = new List<string>();
        public List<string> Outputs { get; set; } = new List<string>();
        public List<List<DebotStep>> Invokes { get; set; } = new List<List<DebotStep>>();
    }

    public class CurrentStepData
    {
        public List<DebotAction> AvailableActions { get; set; } = new List<DebotAction>();
        public List<string> Outputs { get; set; } = new List<string>();
        public DebotStep Step { get; set; }
    }

    public class ExpectedTransaction
    {
        public string Dst { get; set; }
        public List<Spending> Out { get; set; }
        public bool Setcode { get; set; }
        public string Signkey { get; set; }
        public bool Approved { get; set; }
    }

    public class BrowserData
    {
        public Mutex<CurrentStepData> Current { get; set; }
        public Mutex<List<DebotStep>> Next { get; set; }
        public KeyPair Keys { get; set; }
        public string Address { get; set; }
        public bool Finished { get; set; }
        public Mutex<bool> SwitchStarted { get; set; } = new Mutex<bool>(false);
        public Mutex<Queue<string>> MsgQueue = new Mutex<Queue<string>>(new Queue<string>());
        public Mutex<Terminal> Terminal { get; set; } = new Mutex<Terminal>(new Terminal(new List<string>()));
        public Echo Echo { get; } = new Echo();
        public Mutex<IDictionary<string, RegisteredDebot>> Bots = new Mutex<IDictionary<string, RegisteredDebot>>(new Dictionary<string, RegisteredDebot>());
        public DebotInfo Info { get; set; } = new DebotInfo();
        public Mutex<List<ExpectedTransaction>> Activity { get; set; } = new Mutex<List<ExpectedTransaction>>(new List<ExpectedTransaction>());

        public async Task<string> PopMessageAsync()
        {
            using var queue = await MsgQueue.LockAsync();
            return queue.Instance.Any() ? queue.Instance.Dequeue() : null;
        }

        public async Task<bool> DebotFetchedAsync(string addr)
        {
            using (var bots = await Bots.LockAsync())
            {
                return bots.Instance.ContainsKey(addr);
            }
        }

        public async Task<RegisteredDebot> GetDebotAsync(string addr)
        {
            using var bots = await Bots.LockAsync();
            return bots.Instance.ContainsKey(addr)
                ? bots.Instance[addr]
                : null;
        }
    }

    public class Echo
    {
        public (uint, JToken) Call(string func, JToken args)
        {
            switch (func)
            {
                case "echo":
                    var answerId = args.Value<uint>("answerId");
                    var requestVec = args.Value<string>("request").FromHexString();
                    var request = Encoding.UTF8.GetString(requestVec);
                    return (answerId, new
                    {
                        response = request.ToHexString()
                    }.ToJson());

                default:
                    throw new NotSupportedException($"interface function {func} not found");
            }
        }
    }

    public class Terminal
    {
        public List<string> Messages { get; }

        public Terminal(List<string> messages)
        {
            Messages = messages;
        }

        public (uint, JToken) Print(uint answerId, string message)
        {
            Assert.True(Messages.Any(), $"Unexpected terminal message received: \"{message}\"");
            Assert.Contains(message, Messages);
            Assert.Equal(message, Messages[0]);
            Messages.RemoveAt(0);
            return (answerId, new { }.ToJson());
        }

        public (uint, JToken) Call(string func, JToken args)
        {
            uint answerId;
            string prompt;
            switch (func)
            {
                case "print":
                    answerId = DecodeAbiNumber(args.Value<string>("answerId"));
                    var message = Encoding.UTF8.GetString(args.Value<string>("message").FromHexString());
                    return Print(answerId, message);

                case "inputInt":
                    answerId = DecodeAbiNumber(args.Value<string>("answerId"));
                    prompt = Encoding.UTF8.GetString(args.Value<string>("prompt").FromHexString());
                    Print(answerId, prompt);
                    return (answerId, new { value = 1 }.ToJson());

                case "input":
                    answerId = DecodeAbiNumber(args.Value<string>("answerId"));
                    prompt = Encoding.UTF8.GetString(args.Value<string>("prompt").FromHexString());
                    Print(answerId, prompt);
                    return (answerId, new { value = "testinput".ToHexString() }.ToJson());


                default:
                    throw new NotSupportedException($"interface function {func} not found");
            }
        }

        private static uint DecodeAbiNumber(string str)
        {
            if (str.StartsWith("-0x") || str.StartsWith("-0X"))
            {
                return (uint)-Convert.ToUInt32(str.Substring(3));
            }

            if (str.StartsWith("0x") || str.StartsWith("0X"))
            {
                return Convert.ToUInt32(str.Substring(2));
            }

            return Convert.ToUInt32(str);
        }
    }
}