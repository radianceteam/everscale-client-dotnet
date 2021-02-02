using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests/* : IClassFixture<DebotFixture> FIXME: re-enable debot tests */
    {
        private readonly DebotFixture _fixture;
        private readonly ILogger _logger;

        private const int ExitChoice = 9;

        private const string DisabledNote =
            "Debot tests are DISABLED because the same was done in the original Rust sources";

        public DebotModuleTests(/*DebotFixture fixture, */ITestOutputHelper outputHelper)
        {
            //_fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact(Skip = DisabledNote)]
        public async Task Test_Debot_Goto()
        {
            await new TestBrowser(_fixture.Client, _logger)
                .ExecuteAsync(_fixture.DebotAddr, _fixture.Keys, new List<DebotStep>
                {
                    new DebotStep { Choice = 1, Outputs = new List<string> { "Test Goto Action" } },
                    new DebotStep { Choice = 1, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = ExitChoice, Outputs = new List<string>() }
                });
        }

        [EnvDependentFact(Skip = DisabledNote)]
        public async Task Test_Debot_Print()
        {
            await new TestBrowser(_fixture.Client, _logger)
                .ExecuteAsync(_fixture.DebotAddr, _fixture.Keys, new List<DebotStep>
                {
                    new DebotStep { Choice = 2, Outputs = new List<string> { "Test Print Action", "test2: instant print", "test instant print" } },
                    new DebotStep { Choice = 1, Outputs = new List<string> { "test simple print" } },
                    new DebotStep { Choice = 2, Outputs = new List<string> { $"integer=1,addr={_fixture.TargetAddr},string=test_string_1" } },
                    new DebotStep { Choice = 3, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = ExitChoice }
                });
        }

        [EnvDependentFact(Skip = DisabledNote)]
        public async Task Test_Debot_RunAct()
        {
            await new TestBrowser(_fixture.Client, _logger)
                .ExecuteAsync(_fixture.DebotAddr, _fixture.Keys, new List<DebotStep>
                {
                    new DebotStep { Choice = 3, Outputs = new List<string> { "Test Run Action" } },
                    new DebotStep { Choice = 1, Inputs = new List<string>{ "-1:1111111111111111111111111111111111111111111111111111111111111111" }, Outputs = new List<string> { "Test Instant Run", "test1: instant run 1", "test2: instant run 2" } },
                    new DebotStep { Choice = 1, Outputs = new List<string> { "Test Run Action" } },
                    new DebotStep { Choice = 2, Inputs = new List<string> { "hello" } },
                    new DebotStep { Choice = 3, Outputs = new List<string> { "integer=2,addr=-1:1111111111111111111111111111111111111111111111111111111111111111,string=hello" } },
                    new DebotStep { Choice = 4, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = ExitChoice }
                });
        }

        [EnvDependentFact(Skip = DisabledNote)]
        public async Task Test_Debot_Run_Method()
        {
            await new TestBrowser(_fixture.Client, _logger)
                .ExecuteAsync(_fixture.DebotAddr, _fixture.Keys, new List<DebotStep>
                {
                    new DebotStep { Choice = 4, Outputs = new List<string>{ "Test Run Method Action" } },
                    new DebotStep { Choice = 1 },
                    new DebotStep { Choice = 2, Outputs = new List<string> { "data=64" } },
                    new DebotStep { Choice = 3, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = ExitChoice }
                });
        }

        [EnvDependentFact(Skip = DisabledNote)]
        public async Task Test_Debot_Send_Msg()
        {
            await new TestBrowser(_fixture.Client, _logger)
                .ExecuteAsync(_fixture.DebotAddr, _fixture.Keys, new List<DebotStep>
                {
                    new DebotStep { Choice = 5, Outputs = new List<string>{ "Test Send Msg Action" } },
                    new DebotStep { Choice = 1, Outputs = new List<string> { "Sending message {}", "Transaction succeeded." } },
                    new DebotStep { Choice = 2 },
                    new DebotStep { Choice = 3, Outputs = new List<string> { "data=100" } },
                    new DebotStep { Choice = 4, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = ExitChoice }
                });
        }

        [EnvDependentFact(Skip = DisabledNote)]
        public async Task Test_Debot_Invoke_Debot()
        {
            await new TestBrowser(_fixture.Client, _logger)
                .ExecuteAsync(_fixture.DebotAddr, _fixture.Keys, new List<DebotStep>
                {
                    new DebotStep { Choice = 6, Inputs = new List<string>{ _fixture.DebotAddr }, Outputs = new List<string>{ "Test Invoke Debot Action", "enter debot address:" } },
                    new DebotStep { Choice = 1, Inputs = new List<string>{ _fixture.DebotAddr }, Outputs = new List<string>{ "Test Invoke Debot Action", "enter debot address:" },
                        Invokes = new List<List<DebotStep>> { new List<DebotStep>
                        {
                            new DebotStep
                            {
                                Choice = 1,
                                Outputs = new List<string>{ "Print test string", "Debot is invoked" }
                            },
                            new DebotStep
                            {
                                Choice = 1,
                                Outputs = new List<string>{ "Sending message {}", "Transaction succeeded." }
                            }
                        }
                        } },
                    new DebotStep { Choice = 2, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = ExitChoice }
                });
        }

        [EnvDependentFact(Skip = DisabledNote)]
        public async Task Test_Debot_Engine_Calls()
        {
            await new TestBrowser(_fixture.Client, _logger)
                .ExecuteAsync(_fixture.DebotAddr, _fixture.Keys, new List<DebotStep>
                {
                    new DebotStep { Choice = 7, Outputs = new List<string>{ "Test Engine Calls" } },
                    new DebotStep { Choice = 1 },
                    new DebotStep { Choice = 2 },
                    new DebotStep { Choice = 3 },
                    new DebotStep { Choice = 4 },
                    new DebotStep { Choice = 5 },
                    new DebotStep { Choice = 6, Outputs = new List<string>{ "Debot Tests" } },
                    new DebotStep { Choice = ExitChoice }
                });
        }

        [EnvDependentFact(Skip = DisabledNote)]
        public async Task Test_Debot_Interface_Call()
        {
            await new TestBrowser(_fixture.Client, _logger)
                .ExecuteAsync(_fixture.DebotAddr, _fixture.Keys, new List<DebotStep>
                {
                    new DebotStep { Choice = 8, Outputs = new List<string>{ "", "test1 - call interface" } },
                    new DebotStep { Choice = 1, Outputs = new List<string>{ "Debot Tests" } },
                    new DebotStep { Choice = ExitChoice }
                });
        }
    }

    public class DebotStep
    {
        public byte Choice { get; set; }
        public List<string> Inputs { get; set; } = new List<string>();
        public List<string> Outputs { get; set; } = new List<string>();
        public List<List<DebotStep>> Invokes { get; set; } = new List<List<DebotStep>>();
    }

    internal class CurrentStepData
    {
        public List<DebotAction> AvailableActions { get; set; } = new List<DebotAction>();
        public List<string> Outputs { get; set; } = new List<string>();
        public DebotStep Step { get; set; }
    }

    internal class BrowserData
    {
        public Mutex<CurrentStepData> Current { get; set; }
        public Mutex<List<DebotStep>> Next { get; set; }
        public KeyPair Keys { get; set; }
        public string Address { get; set; }
        public bool Finished { get; set; }
        public Mutex<bool> SwitchStarted { get; set; } = new Mutex<bool>(false);
        public Mutex<Queue<string>> MsgQueue = new Mutex<Queue<string>>(new Queue<string>());
    }

    internal class Echo
    {
        public static (uint, JToken) GetEcho(uint answerId, string request)
        {
            return (answerId, JsonConvert.SerializeObject(new
            {
                response = request.ToHexString()
            }));
        }

        public static (uint, JToken) Call(string func, JToken args)
        {
            switch (func)
            {
                case "echo":
                    var answerId = args.Value<uint>("answerId");
                    var requestVec = args.Value<string>("request").FromHexString();
                    var request = Encoding.UTF8.GetString(requestVec);
                    return GetEcho(answerId, request);

                default:
                    throw new NotSupportedException($"interface function {func} not found");
            }
        }
    }

    internal class TestBrowser
    {
        private readonly ITonClient _client;
        private readonly ILogger _logger;

        private const int DebotWc = -31;
        private readonly string[] SupportedInterfaces = { "f6927c0d4bdb69e1b52d27f018d156ff04152f00558042ff674f0fec32e4369d" };

        private readonly string InterfaceAbi = @"{
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
		},
		{
			""name"": ""constructor"",
			""inputs"": [
			],
			""outputs"": [
			]
		}
	],
	""data"": [
	],
	""events"": [
	]
}";

        public TestBrowser(ITonClient client, ILogger logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync(string address, KeyPair keys, List<DebotStep> steps)
        {
            var state = new BrowserData
            {
                Current = new Mutex<CurrentStepData>(new CurrentStepData()),
                Next = new Mutex<List<DebotStep>>(steps),
                Keys = keys,
                Address = address,
                Finished = false
            };

            await ExecuteFromStateAsync(state, data => _client.Debot.StartAsync(new ParamsOfStart
            {
                Address = state.Address
            }, GetExecuteCallback(state)));
        }

        private async Task ExecuteFromStateAsync(BrowserData state, Func<BrowserData, Task<RegisteredDebot>> startFunc)
        {
            var handle = await startFunc(state);

            while (!state.Finished)
            {
                await ExecuteInterfaceCallsAsync(handle, state);

                DebotAction action;
                using (var step = await state.Current.LockAsync())
                {
                    using (var next = await state.Next.LockAsync())
                    {
                        step.Instance.Step = next.Instance.RemoveFirst();
                    }
                    step.Instance.Outputs.Clear();
                    action = step.Instance.AvailableActions[step.Instance.Step.Choice - 1];
                }

                _logger.Information($"Executing action {action.Name}");

                await _client.Debot.ExecuteAsync(new ParamsOfExecute
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
        }

        private async Task ExecuteInterfaceCallsAsync(RegisteredDebot debot, BrowserData data)
        {
            using var queue = await data.MsgQueue.LockAsync();
            while (queue.Instance.Any())
            {
                var msg = queue.Instance.Dequeue();
                var parsed = await _client.Boc.ParseMessageAsync(new ParamsOfParse
                {
                    Boc = msg
                });

                var body = parsed.Parsed["body"]?.ToString();
                Assert.NotNull(body);

                var ifaceAddr = parsed.Parsed["dst"]?.ToString();
                Assert.NotNull(ifaceAddr);

                var wcAndAddr = ifaceAddr.Split(":");
                Assert.True(wcAndAddr.Length > 1);

                var wc = int.Parse(wcAndAddr[0]);
                Assert.Equal(DebotWc, wc);

                var interfaceId = wcAndAddr[1];
                Assert.Contains(interfaceId, SupportedInterfaces);

                var decoded = await _client.Abi.DecodeMessageBodyAsync(new ParamsOfDecodeMessageBody()
                {
                    Abi = new Abi.Json
                    {
                        Value = InterfaceAbi
                    },
                    Body = body,
                    IsInternal = true
                });

                _logger.Information($"call for interface id {interfaceId}");
                _logger.Information($"request: {decoded.Name} ({decoded.Value})");

                var (funcId, returnArgs) = Echo.Call(decoded.Name, decoded.Value);
                _logger.Information($"response: {funcId} ({returnArgs})");

                await _client.Debot.SendAsync(new ParamsOfSend
                {
                    DebotHandle = debot.DebotHandle,
                    Source = ifaceAddr,
                    FuncId = funcId,
                    Params = returnArgs.ToString()
                });
            }
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
                        var signingBox = await _client.Crypto.GetSigningBoxAsync(state.Keys);
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

                        await ExecuteFromStateAsync(newState, data => _client.Debot.FetchAsync(new ParamsOfFetch
                        {
                            Address = data.Address
                        }, GetExecuteCallback(newState)));

                        return new ResultOfAppDebotBrowser.InvokeDebot();

                    default:
                        throw new NotSupportedException($"Callback parameter type not supported: {@params.GetType()}");
                }
            };
        }
    }
}
