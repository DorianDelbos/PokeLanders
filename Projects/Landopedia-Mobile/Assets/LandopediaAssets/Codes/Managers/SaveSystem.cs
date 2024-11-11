using Lander.Module;
using Lander.Module.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landopedia
{
    /// <summary>
    /// Save system for managing IDs using a bitmask to enhance performance and reduce saved data size.
    /// </summary>
    public static class SaveSystem
    {
        // Maximum number of IDs to manage, defined by the number of available 'Lander' objects
        private static readonly int MaxID = GameManager.Instance.Landers.Length;

        // Save key for the IDs bitmask in the player's preferences
        private const string BitmaskKey = "IDsBitmask";

        /// <summary>
        /// Saves a list of IDs in PlayerPrefs using a BitArray to save space.
        /// </summary>
        /// <param name="ids">List of IDs to save.</param>
        public static void SaveIDs(List<int> ids)
        {
            BitArray bitArray = new BitArray(MaxID);
            foreach (int id in ids)
            {
                if (id >= 0 && id < MaxID)
                    bitArray[id] = true;
            }
            SaveBitArray(bitArray);
        }

        /// <summary>
        /// Loads the list of saved IDs from PlayerPrefs.
        /// </summary>
        /// <returns>List of loaded IDs.</returns>
        public static List<int> LoadIDs()
        {
            BitArray bitArray = LoadBitArray();
            List<int> ids = new List<int>();
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                    ids.Add(i);
            }
            return ids;
        }

        /// <summary>
        /// Adds new IDs to the list of already saved IDs.
        /// </summary>
        /// <param name="newIds">List of new IDs to add.</param>
        public static void AddID(List<int> newIds)
        {
            BitArray bitArray = LoadBitArray();
            foreach (int id in newIds)
            {
                if (id >= 0 && id < MaxID)
                    bitArray[id] = true;
            }
            SaveBitArray(bitArray);
        }

        /// <summary>
        /// Adds a new ID to the list of already saved IDs.
        /// </summary>
        /// <param name="newId">New ID to add.</param>
        public static void AddID(int newId)
        {
            if (newId >= 0 && newId < MaxID)
            {
                BitArray bitArray = LoadBitArray();
                bitArray[newId] = true;
                SaveBitArray(bitArray);
            }
        }

        /// <summary>
        /// Saves a BitArray in PlayerPrefs as a Base64 string.
        /// </summary>
        /// <param name="bitArray">BitArray representing the IDs to save.</param>
        private static void SaveBitArray(BitArray bitArray)
        {
            byte[] bytes = new byte[(MaxID + 7) / 8];
            bitArray.CopyTo(bytes, 0);
            string bitmaskString = System.Convert.ToBase64String(bytes);
            PlayerPrefs.SetString(BitmaskKey, bitmaskString);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Loads a BitArray from PlayerPrefs.
        /// </summary>
        /// <returns>BitArray representing the loaded IDs.</returns>
        private static BitArray LoadBitArray()
        {
            string bitmaskString = PlayerPrefs.GetString(BitmaskKey, "");
            if (string.IsNullOrEmpty(bitmaskString))
            {
                return new BitArray(MaxID);
            }

            byte[] bytes = System.Convert.FromBase64String(bitmaskString);
            return new BitArray(bytes);
        }
    }
}
