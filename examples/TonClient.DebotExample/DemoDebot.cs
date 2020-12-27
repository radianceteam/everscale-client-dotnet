using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using TonSdk;
using TonSdk.Modules;

namespace TonClient.DebotExample
{
    internal class DemoDebot
    {
        private readonly ITonClient _client;
        private readonly string _address;
        private readonly KeyPair _keys;
        private bool _finished;
        private List<DebotAction> _actions = new List<DebotAction>();

        public DemoDebot(ITonClient client, string address, KeyPair keys)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _address = address ?? throw new ArgumentNullException(nameof(address));
            _keys = keys ?? throw new ArgumentNullException(nameof(keys));
        }

        public async Task StartAsync()
        {
            var debot = await _client.Debot.StartAsync(new ParamsOfStart
            {
                Address = _address
            }, GetCallback());

            await LoopAsync(debot, () =>
            {
                // let user enter the action number
                string userInput;
                int actionIndex;
                do
                {
                    Console.WriteLine($"Select action (1 - {_actions.Count})");
                    userInput = Console.ReadLine();
                } while (!int.TryParse(userInput, out actionIndex) ||
                         !(actionIndex > 0 && actionIndex <= _actions.Count));
                return _actions[actionIndex - 1];
            });

            await _client.Debot.RemoveAsync(debot);
        }

        public async Task FetchAsync(DebotAction action)
        {
            var debot = await _client.Debot.FetchAsync(new ParamsOfFetch
            {
                Address = _address
            }, GetCallback());

            await LoopAsync(debot, () => action);

            await _client.Debot.RemoveAsync(debot);
        }

        private Func<ParamsOfAppDebotBrowser, Task<ResultOfAppDebotBrowser>> GetCallback()
        {
            return async (p) =>
            {
                switch (p)
                {
                    case ParamsOfAppDebotBrowser.Log log:
                        Console.WriteLine(log.Msg);
                        return null;
                    case ParamsOfAppDebotBrowser.Switch sw:
                        if (sw.ContextId == 255) // STATE_EXIT
                        {
                            _finished = true;
                        }
                        _actions = new List<DebotAction>();
                        return null;
                    case ParamsOfAppDebotBrowser.SwitchCompleted sw:
                        return null;
                    case ParamsOfAppDebotBrowser.ShowAction show:
                        var action = show.Action;
                        _actions.Add(action);
                        Console.WriteLine($"{_actions.Count}: {action.Description}");
                        return null;
                    case ParamsOfAppDebotBrowser.Input input:
                        Console.Write($"{input.Prompt}: ");
                        var value = Console.ReadLine();
                        return new ResultOfAppDebotBrowser.Input
                        {
                            Value = value
                        };
                    case ParamsOfAppDebotBrowser.GetSigningBox s:
                        var signingBox = await _client.Crypto.GetSigningBoxAsync(_keys);
                        return new ResultOfAppDebotBrowser.GetSigningBox
                        {
                            SigningBox = signingBox.Handle
                        };
                    case ParamsOfAppDebotBrowser.InvokeDebot invoke:
                        var debot = new DemoDebot(_client, _address, _keys);
                        await debot.FetchAsync(invoke.Action);
                        return new ResultOfAppDebotBrowser.InvokeDebot();
                    default:
                        throw new NotSupportedException($"Parameter type {p.GetType()} is not supported");

                }
            };
        }

        private async Task LoopAsync(RegisteredDebot debot, Func<DebotAction> selectActionFunc)
        {
            while (!_finished && _actions.Any())
            {
                var action = selectActionFunc();
                Log.Information("Executing action {Action}", action.Description);
                await _client.Debot.ExecuteAsync(new ParamsOfExecute
                {
                    Action = action,
                    DebotHandle = debot.DebotHandle
                });
            }
        }
    }
}
