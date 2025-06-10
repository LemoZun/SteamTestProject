using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NSJTest
{
    public class Test : MonoBehaviour
    {
        public void TestFunc(string apiName)
        {
            GameData data = new GameData();

            string json1 = JsonUtility.ToJson(data);
            byte[] bytes1 = Encoding.UTF8.GetBytes(json1);
            SteamRemoteStorage.FileWrite("save.json", bytes1, bytes1.Length);

            int size = SteamRemoteStorage.GetFileSize("save.json");
            byte[] bytes2 = new byte[size];
            SteamRemoteStorage.FileRead("save.json", bytes2, size);
            string json2 = Encoding.UTF8.GetString(bytes2);
            JsonUtility.FromJson<GameData>(json2);
        }
    }

}
