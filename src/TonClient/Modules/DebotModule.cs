using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.4.0, debot module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Describes a debot action in a Debot Context.
    /// </summary>
    public class DebotAction
    {
        /// <summary>
        /// Should be used by Debot Browser as name ofmenu item.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Can be a debot function name or a print string(for Print Action).
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Action type.
        /// </summary>
        [JsonProperty("action_type", NullValueHandling = NullValueHandling.Ignore)]
        public byte ActionType { get; set; }

        /// <summary>
        /// ID of debot context to switch after action execution.
        /// </summary>
        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public byte To { get; set; }

        /// <summary>
        /// In the form of "param=value,flag".attribute example: instant, args, fargs, sign.
        /// </summary>
        [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public string Attributes { get; set; }

        /// <summary>
        /// Used by debot only.
        /// </summary>
        [JsonProperty("misc", NullValueHandling = NullValueHandling.Ignore)]
        public string Misc { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Parameters to start debot.
    /// </summary>
    public class ParamsOfStart
    {
        /// <summary>
        /// Debot smart contract address
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Structure for storing debot handle returned from `start` and `fetch`
    /// functions.
    /// </summary>
    public class RegisteredDebot
    {
        /// <summary>
        /// Debot handle which references an instance of debot engine.
        /// </summary>
        [JsonProperty("debot_handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint DebotHandle { get; set; }
    }

    /// <summary>
    /// Called by debot engine to communicate with debot browser.
    /// </summary>
    public abstract class ParamsOfAppDebotBrowser
    {
        /// <summary>
        /// Print message to user.
        /// </summary>
        public class Log : ParamsOfAppDebotBrowser
        {
            /// <summary>
            /// A string that must be printed to user.
            /// </summary>
            [JsonProperty("msg", NullValueHandling = NullValueHandling.Ignore)]
            public string Msg { get; set; }
        }

        /// <summary>
        /// Switch debot to another context (menu).
        /// </summary>
        public class Switch : ParamsOfAppDebotBrowser
        {
            /// <summary>
            /// Debot context ID to which debot is switched.
            /// </summary>
            [JsonProperty("context_id", NullValueHandling = NullValueHandling.Ignore)]
            public byte ContextId { get; set; }
        }

        /// <summary>
        /// Notify browser that all context actions are shown.
        /// </summary>
        public class SwitchCompleted : ParamsOfAppDebotBrowser
        {
        }

        /// <summary>
        /// Show action to the user. Called after `switch` for each action in context.
        /// </summary>
        public class ShowAction : ParamsOfAppDebotBrowser
        {
            /// <summary>
            /// Debot action that must be shown to user as menu item. At least `description` property must be shown
            /// from [DebotAction] structure.
            /// </summary>
            [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
            public DebotAction Action { get; set; }
        }

        /// <summary>
        /// Request user input.
        /// </summary>
        public class Input : ParamsOfAppDebotBrowser
        {
            /// <summary>
            /// A prompt string that must be printed to user before input request.
            /// </summary>
            [JsonProperty("prompt", NullValueHandling = NullValueHandling.Ignore)]
            public string Prompt { get; set; }
        }

        /// <summary>
        /// Signing box returned is owned and disposed by debot engine
        /// </summary>
        public class GetSigningBox : ParamsOfAppDebotBrowser
        {
        }

        /// <summary>
        /// Execute action of another debot.
        /// </summary>
        public class InvokeDebot : ParamsOfAppDebotBrowser
        {
            /// <summary>
            /// Address of debot in blockchain.
            /// </summary>
            [JsonProperty("debot_addr", NullValueHandling = NullValueHandling.Ignore)]
            public string DebotAddr { get; set; }

            /// <summary>
            /// Debot action to execute.
            /// </summary>
            [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
            public DebotAction Action { get; set; }
        }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Returning values from Debot Browser callbacks.
    /// </summary>
    public abstract class ResultOfAppDebotBrowser
    {
        /// <summary>
        /// Result of user input.
        /// </summary>
        public class Input : ResultOfAppDebotBrowser
        {
            /// <summary>
            /// String entered by user.
            /// </summary>
            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public string Value { get; set; }
        }

        /// <summary>
        /// Result of getting signing box.
        /// </summary>
        public class GetSigningBox : ResultOfAppDebotBrowser
        {
            /// <summary>
            /// Signing box is owned and disposed by debot engine
            /// </summary>
            [JsonProperty("signing_box", NullValueHandling = NullValueHandling.Ignore)]
            public uint SigningBox { get; set; }
        }

        /// <summary>
        /// Result of debot invoking.
        /// </summary>
        public class InvokeDebot : ResultOfAppDebotBrowser
        {
        }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Parameters to fetch debot.
    /// </summary>
    public class ParamsOfFetch
    {
        /// <summary>
        /// Debot smart contract address
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Parameters for executing debot action.
    /// </summary>
    public class ParamsOfExecute
    {
        /// <summary>
        /// Debot handle which references an instance of debot engine.
        /// </summary>
        [JsonProperty("debot_handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint DebotHandle { get; set; }

        /// <summary>
        /// Debot Action that must be executed.
        /// </summary>
        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public DebotAction Action { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Module for working with debot.
    /// </summary>
    public interface IDebotModule
    {
        /// <summary>
        /// Downloads debot smart contract from blockchain and switches it to
        /// context zero.
        /// Returns a debot handle which can be used later in `execute` function.
        /// This function must be used by Debot Browser to start a dialog with debot.
        /// While the function is executing, several Browser Callbacks can be called,
        /// since the debot tries to display all actions from the context 0 to the user.
        /// 
        /// # Remarks
        /// `start` is equivalent to `fetch` + switch to context 0.
        /// </summary>
        Task<RegisteredDebot> StartAsync(ParamsOfStart @params, Func<ParamsOfAppDebotBrowser, Task<ResultOfAppDebotBrowser>> app_object);

        /// <summary>
        /// Downloads debot smart contract (code and data) from blockchain and creates
        /// an instance of Debot Engine for it.
        /// 
        /// # Remarks
        /// It does not switch debot to context 0. Browser Callbacks are not called.
        /// </summary>
        Task<RegisteredDebot> FetchAsync(ParamsOfFetch @params, Func<ParamsOfAppDebotBrowser, Task<ResultOfAppDebotBrowser>> app_object);

        /// <summary>
        /// Calls debot engine referenced by debot handle to execute input action.
        /// Calls Debot Browser Callbacks if needed.
        /// 
        /// # Remarks
        /// Chain of actions can be executed if input action generates a list of subactions.
        /// </summary>
        Task ExecuteAsync(ParamsOfExecute @params);

        /// <summary>
        /// Removes handle from Client Context and drops debot engine referenced by that handle.
        /// </summary>
        Task RemoveAsync(RegisteredDebot @params);
    }

    internal class DebotModule : IDebotModule
    {
        private readonly TonClient _client;

        internal DebotModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<RegisteredDebot> StartAsync(ParamsOfStart @params, Func<ParamsOfAppDebotBrowser, Task<ResultOfAppDebotBrowser>> app_object)
        {
            return await _client.CallFunctionAsync<RegisteredDebot, ParamsOfAppDebotBrowser, ResultOfAppDebotBrowser>("debot.start", @params, app_object).ConfigureAwait(false);
        }

        public async Task<RegisteredDebot> FetchAsync(ParamsOfFetch @params, Func<ParamsOfAppDebotBrowser, Task<ResultOfAppDebotBrowser>> app_object)
        {
            return await _client.CallFunctionAsync<RegisteredDebot, ParamsOfAppDebotBrowser, ResultOfAppDebotBrowser>("debot.fetch", @params, app_object).ConfigureAwait(false);
        }

        public async Task ExecuteAsync(ParamsOfExecute @params)
        {
            await _client.CallFunctionAsync("debot.execute", @params).ConfigureAwait(false);
        }

        public async Task RemoveAsync(RegisteredDebot @params)
        {
            await _client.CallFunctionAsync("debot.remove", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        IDebotModule Debot { get; }
    }

    public partial class TonClient
    {
        private DebotModule _debotModule;

        public IDebotModule Debot
        {
            get
            {
                return _debotModule ?? (_debotModule = new DebotModule(this));
            }
        }
    }
}

