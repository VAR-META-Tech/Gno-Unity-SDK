//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Wallet : MonoBehaviour
//{
//    /// <summary>
//        /// The derivation path.
//        /// </summary>
//        private const string DerivationPath = "m/44'/637'/x'/0'/0'";
//        /// <summary>
//        /// The seed mode used for key generation.
//        /// Aptos currently supports BIP39
//        /// </summary>
//        private readonly SeedMode _seedMode;

//        /// <summary>
//        /// The seed derived from the mnemonic and/or passphrase.
//        /// </summary>
//        private byte[] _seed;

//        /// <summary>
//        /// The method used for <see cref="SeedMode.Ed25519Bip32"/> key generation.
//        /// </summary>
//        private Ed25519Bip32 _ed25519Bip32;

//        /// <summary>
//        /// The passphrase string.
//        /// </summary>
//        private string Passphrase { get; }

//        /// <summary>
//        /// The key pair.
//        /// </summary>
//        public Account Account { get; private set; }

//        /// <summary>
//        /// Gets the account at the passed index using the ed25519 bip32 derivation path.
//        /// </summary>
//        /// <param name="index">The index of the account.</param>
//        /// <returns>The account.</returns>
//        public Account GetAccount(int index)
//        {
//            if (_seedMode != SeedMode.Ed25519Bip32)
//                throw new Exception($"seed mode: {_seedMode} cannot derive Ed25519 based BIP32 keys");

//            string path = DerivationPath.Replace("x", index.ToString());
//            (byte[] account, byte[] _) = _ed25519Bip32.DerivePath(path);
//            (byte[] privateKey, byte[] publicKey) = EdKeyPairFromSeed(account);
//            return new Account(privateKey, publicKey);
//        }

//}
