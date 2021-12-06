using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.26.1, debot module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public enum DebotErrorCode
    {
        DebotStartFailed = 801,
        DebotFetchFailed = 802,
        DebotExecutionFailed = 803,
        DebotInvalidHandle = 804,
        DebotInvalidJsonParams = 805,
        DebotInvalidFunctionId = 806,
        DebotInvalidAbi = 807,
        DebotGetMethodFailed = 808,
        DebotInvalidMsg = 809,
        DebotExternalCallFailed = 810,
        DebotBrowserCallbackFailed = 811,
        DebotOperationRejected = 812,
        DebotNoCode = 813,
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Describes a debot action in a Debot Context.
    /// </summary>
    public class DebotAction
    {
        /// <summary>
        /// Should be used by Debot Browser as name of menu item.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Can be a debot function name or a print string (for Print Action).
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
        /// In the form of "param=value,flag". attribute example: instant, args, fargs, sign.
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
    /// [UNSTABLE](UNSTABLE.md) Describes DeBot metadata.
    /// </summary>
    public class DebotInfo
    {
        /// <summary>
        /// DeBot short name.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// DeBot semantic version.
        /// </summary>
        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        /// <summary>
        /// The name of DeBot deployer.
        /// </summary>
        [JsonProperty("publisher", NullValueHandling = NullValueHandling.Ignore)]
        public string Publisher { get; set; }

        /// <summary>
        /// Short info about DeBot.
        /// </summary>
        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption { get; set; }

        /// <summary>
        /// The name of DeBot developer.
        /// </summary>
        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public string Author { get; set; }

        /// <summary>
        /// TON address of author for questions and donations.
        /// </summary>
        [JsonProperty("support", NullValueHandling = NullValueHandling.Ignore)]
        public string Support { get; set; }

        /// <summary>
        /// String with the first messsage from DeBot.
        /// </summary>
        [JsonProperty("hello", NullValueHandling = NullValueHandling.Ignore)]
        public string Hello { get; set; }

        /// <summary>
        /// String with DeBot interface language (ISO-639).
        /// </summary>
        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        /// <summary>
        /// String with DeBot ABI.
        /// </summary>
        [JsonProperty("dabi", NullValueHandling = NullValueHandling.Ignore)]
        public string Dabi { get; set; }

        /// <summary>
        /// DeBot icon.
        /// </summary>
        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        /// <summary>
        /// Vector with IDs of DInterfaces used by DeBot.
        /// </summary>
        [JsonProperty("interfaces", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Interfaces { get; set; }

        /// <summary>
        /// ABI version ("x.y") supported by DeBot
        /// </summary>
        [JsonProperty("dabiVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string DabiVersion { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Describes the operation that the DeBot wants to perform.
    /// </summary>
    public abstract class DebotActivity
    {
        /// <summary>
        /// DeBot wants to create new transaction in blockchain.
        /// </summary>
        public class Transaction : DebotActivity
        {
            /// <summary>
            /// External inbound message BOC.
            /// </summary>
            [JsonProperty("msg", NullValueHandling = NullValueHandling.Ignore)]
            public string Msg { get; set; }

            /// <summary>
            /// Target smart contract address.
            /// </summary>
            [JsonProperty("dst", NullValueHandling = NullValueHandling.Ignore)]
            public string Dst { get; set; }

            /// <summary>
            /// List of spendings as a result of transaction.
            /// </summary>
            [JsonProperty("out", NullValueHandling = NullValueHandling.Ignore)]
            public Spending[] Out { get; set; }

            /// <summary>
            /// Transaction total fee.
            /// </summary>
            [JsonProperty("fee", NullValueHandling = NullValueHandling.Ignore)]
            public BigInteger Fee { get; set; }

            /// <summary>
            /// Indicates if target smart contract updates its code.
            /// </summary>
            [JsonProperty("setcode", NullValueHandling = NullValueHandling.Ignore)]
            public bool Setcode { get; set; }

            /// <summary>
            /// Public key from keypair that was used to sign external message.
            /// </summary>
            [JsonProperty("signkey", NullValueHandling = NullValueHandling.Ignore)]
            public string Signkey { get; set; }

            /// <summary>
            /// Signing box handle used to sign external message.
            /// </summary>
            [JsonProperty("signing_box_handle", NullValueHandling = NullValueHandling.Ignore)]
            public uint SigningBoxHandle { get; set; }
        }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Describes how much funds will be debited from the target  contract balance
    /// as a result of the transaction.
    /// </summary>
    public class Spending
    {
        /// <summary>
        /// Amount of nanotokens that will be sent to `dst` address.
        /// </summary>
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger Amount { get; set; }

        /// <summary>
        /// Destination address of recipient of funds.
        /// </summary>
        [JsonProperty("dst", NullValueHandling = NullValueHandling.Ignore)]
        public string Dst { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Parameters to init DeBot.
    /// </summary>
    public class ParamsOfInit
    {
        /// <summary>
        /// Debot smart contract address
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Structure for storing debot handle returned from `init` function.
    /// </summary>
    public class RegisteredDebot
    {
        /// <summary>
        /// Debot handle which references an instance of debot engine.
        /// </summary>
        [JsonProperty("debot_handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint DebotHandle { get; set; }

        /// <summary>
        /// Debot abi as json string.
        /// </summary>
        [JsonProperty("debot_abi", NullValueHandling = NullValueHandling.Ignore)]
        public string DebotAbi { get; set; }

        /// <summary>
        /// Debot metadata.
        /// </summary>
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public DebotInfo Info { get; set; }
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

        /// <summary>
        /// Used by Debot to call DInterface implemented by Debot Browser.
        /// </summary>
        public class Send : ParamsOfAppDebotBrowser
        {
            /// <summary>
            /// Message body contains interface function and parameters.
            /// </summary>
            [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }
        }

        /// <summary>
        /// Requests permission from DeBot Browser to execute DeBot operation.
        /// </summary>
        public class Approve : ParamsOfAppDebotBrowser
        {
            /// <summary>
            /// DeBot activity details.
            /// </summary>
            [JsonProperty("activity", NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(PolymorphicTypeConverter))]
            public DebotActivity Activity { get; set; }
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

        /// <summary>
        /// Result of `approve` callback.
        /// </summary>
        public class Approve : ResultOfAppDebotBrowser
        {
            /// <summary>
            /// Indicates whether the DeBot is allowed to perform the specified operation.
            /// </summary>
            [JsonProperty("approved", NullValueHandling = NullValueHandling.Ignore)]
            public bool Approved { get; set; }
        }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Parameters to start DeBot. DeBot must be already initialized with init()
    /// function.
    /// </summary>
    public class ParamsOfStart
    {
        /// <summary>
        /// Debot handle which references an instance of debot engine.
        /// </summary>
        [JsonProperty("debot_handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint DebotHandle { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Parameters to fetch DeBot metadata.
    /// </summary>
    public class ParamsOfFetch
    {
        /// <summary>
        /// Debot smart contract address.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md)
    /// </summary>
    public class ResultOfFetch
    {
        /// <summary>
        /// Debot metadata.
        /// </summary>
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public DebotInfo Info { get; set; }
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
    /// [UNSTABLE](UNSTABLE.md) Parameters of `send` function.
    /// </summary>
    public class ParamsOfSend
    {
        /// <summary>
        /// Debot handle which references an instance of debot engine.
        /// </summary>
        [JsonProperty("debot_handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint DebotHandle { get; set; }

        /// <summary>
        /// BOC of internal message to debot encoded in base64 format.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md)
    /// </summary>
    public class ParamsOfRemove
    {
        /// <summary>
        /// Debot handle which references an instance of debot engine.
        /// </summary>
        [JsonProperty("debot_handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint DebotHandle { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Module for working with debot.
    /// </summary>
    public interface IDebotModule
    {
        /// <summary>
        /// Downloads debot smart contract (code and data) from blockchain and creates
        /// an instance of Debot Engine for it.
        /// 
        /// # Remarks
        /// It does not switch debot to context 0. Browser Callbacks are not called.
        /// </summary>
        Task<RegisteredDebot> InitAsync(ParamsOfInit @params, Func<ParamsOfAppDebotBrowser, Task<ResultOfAppDebotBrowser>> app_object);

        /// <summary>
        /// Downloads debot smart contract from blockchain and switches it to
        /// context zero.
        /// 
        /// This function must be used by Debot Browser to start a dialog with debot.
        /// While the function is executing, several Browser Callbacks can be called,
        /// since the debot tries to display all actions from the context 0 to the user.
        /// 
        /// When the debot starts SDK registers `BrowserCallbacks` AppObject.
        /// Therefore when `debote.remove` is called the debot is being deleted and the callback is called
        /// with `finish`=`true` which indicates that it will never be used again.
        /// </summary>
        Task StartAsync(ParamsOfStart @params);

        /// <summary>
        /// Downloads DeBot from blockchain and creates and fetches its metadata.
        /// </summary>
        Task<ResultOfFetch> FetchAsync(ParamsOfFetch @params);

        /// <summary>
        /// Calls debot engine referenced by debot handle to execute input action.
        /// Calls Debot Browser Callbacks if needed.
        /// 
        /// # Remarks
        /// Chain of actions can be executed if input action generates a list of subactions.
        /// </summary>
        Task ExecuteAsync(ParamsOfExecute @params);

        /// <summary>
        /// Used by Debot Browser to send response on Dinterface call or from other Debots.
        /// </summary>
        Task SendAsync(ParamsOfSend @params);

        /// <summary>
        /// Removes handle from Client Context and drops debot engine referenced by that handle.
        /// </summary>
        Task RemoveAsync(ParamsOfRemove @params);
    }

    internal class DebotModule : IDebotModule
    {
        private readonly TonClient _client;

        internal DebotModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<RegisteredDebot> InitAsync(ParamsOfInit @params, Func<ParamsOfAppDebotBrowser, Task<ResultOfAppDebotBrowser>> app_object)
        {
            return await _client.CallFunctionAsync<RegisteredDebot, ParamsOfAppDebotBrowser, ResultOfAppDebotBrowser>("debot.init", @params, app_object).ConfigureAwait(false);
        }

        public async Task StartAsync(ParamsOfStart @params)
        {
            await _client.CallFunctionAsync("debot.start", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfFetch> FetchAsync(ParamsOfFetch @params)
        {
            return await _client.CallFunctionAsync<ResultOfFetch>("debot.fetch", @params).ConfigureAwait(false);
        }

        public async Task ExecuteAsync(ParamsOfExecute @params)
        {
            await _client.CallFunctionAsync("debot.execute", @params).ConfigureAwait(false);
        }

        public async Task SendAsync(ParamsOfSend @params)
        {
            await _client.CallFunctionAsync("debot.send", @params).ConfigureAwait(false);
        }

        public async Task RemoveAsync(ParamsOfRemove @params)
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

