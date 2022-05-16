using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.32.0, proofs module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public enum ProofsErrorCode
    {
        InvalidData = 901,
        ProofCheckFailed = 902,
        InternalError = 903,
        DataDiffersFromProven = 904,
    }

    public class ParamsOfProofBlockData
    {
        /// <summary>
        /// Single block's data, retrieved from TONOS API, that needs proof. Required fields are `id` and/or
        /// top-level `boc` (for block identification), others are optional.
        /// </summary>
        [JsonProperty("block", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Block { get; set; }
    }

    public class ParamsOfProofTransactionData
    {
        /// <summary>
        /// Single transaction's data as queried from DApp server, without modifications. The required fields
        /// are `id` and/or top-level `boc`, others are optional. In order to reduce network requests count, it
        /// is recommended to provide `block_id` and `boc` of transaction.
        /// </summary>
        [JsonProperty("transaction", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Transaction { get; set; }
    }

    public class ParamsOfProofMessageData
    {
        /// <summary>
        /// Single message's data as queried from DApp server, without modifications. The required fields are
        /// `id` and/or top-level `boc`, others are optional. In order to reduce network requests count, it is
        /// recommended to provide at least `boc` of message and non-null `src_transaction.id` or
        /// `dst_transaction.id`.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Message { get; set; }
    }

    /// <summary>
    /// [UNSTABLE](UNSTABLE.md) Module for proving data, retrieved from TONOS API.
    /// </summary>
    public interface IProofsModule
    {
        /// <summary>
        /// This function checks block proofs and compares given data with the proven.
        /// If the given data differs from the proven, the exception will be thrown.
        /// The input param is a single block's JSON object, which was queried from DApp server using
        /// functions such as `net.query`, `net.query_collection` or `net.wait_for_collection`.
        /// If block's BOC is not provided in the JSON, it will be queried from DApp server
        /// (in this case it is required to provide at least `id` of block).
        /// 
        /// Please note, that joins (like `signatures` in `Block`) are separated entities and not supported,
        /// so function will throw an exception in a case if JSON being checked has such entities in it.
        /// 
        /// If `cache_in_local_storage` in config is set to `true` (default), downloaded proofs and
        /// master-chain BOCs are saved into the persistent local storage (e.g. file system for native
        /// environments or browser's IndexedDB for the web); otherwise all the data is cached only in
        /// memory in current client's context and will be lost after destruction of the client.
        /// 
        /// **Why Proofs are needed**
        /// 
        /// Proofs are needed to ensure that the data downloaded from a DApp server is real blockchain
        /// data. Checking proofs can protect from the malicious DApp server which can potentially provide
        /// fake data, or also from "Man in the Middle" attacks class.
        /// 
        /// **What Proofs are**
        /// 
        /// Simply, proof is a list of signatures of validators', which have signed this particular master-
        /// block.
        /// 
        /// The very first validator set's public keys are included in the zero-state. Whe know a root hash
        /// of the zero-state, because it is stored in the network configuration file, it is our authority
        /// root. For proving zero-state it is enough to calculate and compare its root hash.
        /// 
        /// In each new validator cycle the validator set is changed. The new one is stored in a key-block,
        /// which is signed by the validator set, which we already trust, the next validator set will be
        /// stored to the new key-block and signed by the current validator set, and so on.
        /// 
        /// In order to prove any block in the master-chain we need to check, that it has been signed by
        /// a trusted validator set. So we need to check all key-blocks' proofs, started from the zero-state
        /// and until the block, which we want to prove. But it can take a lot of time and traffic to
        /// download and prove all key-blocks on a client. For solving this, special trusted blocks are used
        /// in TON-SDK.
        /// 
        /// The trusted block is the authority root, as well, as the zero-state. Each trusted block is the
        /// `id` (e.g. `root_hash`) of the already proven key-block. There can be plenty of trusted
        /// blocks, so there can be a lot of authority roots. The hashes of trusted blocks for MainNet
        /// and DevNet are hardcoded in SDK in a separated binary file (trusted_key_blocks.bin) and is
        /// being updated for each release by using `update_trusted_blocks` utility.
        /// 
        /// See [update_trusted_blocks](../../../tools/update_trusted_blocks) directory for more info.
        /// 
        /// In future SDK releases, one will also be able to provide their hashes of trusted blocks for
        /// other networks, besides for MainNet and DevNet.
        /// By using trusted key-blocks, in order to prove any block, we can prove chain of key-blocks to
        /// the closest previous trusted key-block, not only to the zero-state.
        /// 
        /// But shard-blocks don't have proofs on DApp server. In this case, in order to prove any shard-
        /// block data, we search for a corresponding master-block, which contains the root hash of this
        /// shard-block, or some shard block which is linked to that block in shard-chain. After proving
        /// this master-block, we traverse through each link and calculate and compare hashes with links,
        /// one-by-one. After that we can ensure that this shard-block has also been proven.
        /// </summary>
        Task ProofBlockDataAsync(ParamsOfProofBlockData @params);

        /// <summary>
        /// This function requests the corresponding block, checks block proofs, ensures that given
        /// transaction exists in the proven block and compares given data with the proven.
        /// If the given data differs from the proven, the exception will be thrown.
        /// The input parameter is a single transaction's JSON object (see params description),
        /// which was queried from TONOS API using functions such as `net.query`, `net.query_collection`
        /// or `net.wait_for_collection`.
        /// 
        /// If transaction's BOC and/or `block_id` are not provided in the JSON, they will be queried from
        /// TONOS API.
        /// 
        /// Please note, that joins (like `account`, `in_message`, `out_messages`, etc. in `Transaction`
        /// entity) are separated entities and not supported, so function will throw an exception in a case
        /// if JSON being checked has such entities in it.
        /// 
        /// For more information about proofs checking, see description of `proof_block_data` function.
        /// </summary>
        Task ProofTransactionDataAsync(ParamsOfProofTransactionData @params);

        /// <summary>
        /// This function first proves the corresponding transaction, ensures that the proven transaction
        /// refers to the given message and compares given data with the proven.
        /// If the given data differs from the proven, the exception will be thrown.
        /// The input parameter is a single message's JSON object (see params description),
        /// which was queried from TONOS API using functions such as `net.query`, `net.query_collection`
        /// or `net.wait_for_collection`.
        /// 
        /// If message's BOC and/or non-null `src_transaction.id` or `dst_transaction.id` are not provided
        /// in the JSON, they will be queried from TONOS API.
        /// 
        /// Please note, that joins (like `block`, `dst_account`, `dst_transaction`, `src_account`,
        /// `src_transaction`, etc. in `Message` entity) are separated entities and not supported,
        /// so function will throw an exception in a case if JSON being checked has such entities in it.
        /// 
        /// For more information about proofs checking, see description of `proof_block_data` function.
        /// </summary>
        Task ProofMessageDataAsync(ParamsOfProofMessageData @params);
    }

    internal class ProofsModule : IProofsModule
    {
        private readonly TonClient _client;

        internal ProofsModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task ProofBlockDataAsync(ParamsOfProofBlockData @params)
        {
            await _client.CallFunctionAsync("proofs.proof_block_data", @params).ConfigureAwait(false);
        }

        public async Task ProofTransactionDataAsync(ParamsOfProofTransactionData @params)
        {
            await _client.CallFunctionAsync("proofs.proof_transaction_data", @params).ConfigureAwait(false);
        }

        public async Task ProofMessageDataAsync(ParamsOfProofMessageData @params)
        {
            await _client.CallFunctionAsync("proofs.proof_message_data", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        IProofsModule Proofs { get; }
    }

    public partial class TonClient
    {
        private ProofsModule _proofsModule;

        public IProofsModule Proofs
        {
            get
            {
                return _proofsModule ?? (_proofsModule = new ProofsModule(this));
            }
        }
    }
}

