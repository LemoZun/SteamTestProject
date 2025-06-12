using System.IO;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public class SaveUtility
    {
        public static bool Save(ref GameData data, int saveNumber)
        {

            string json = JsonUtility.ToJson(data);
#if UNITY_EDITOR
            string path = $"{Application.persistentDataPath}/Save";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText($"{path}/save{saveNumber}.txt", json);

#else
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        SteamRemoteStorage.FileWrite($"save{saveNumber}.json", bytes, bytes.Length);
#endif
            return true;
        }

        public static GameData Load(int saveNumber, out bool success)
        {

#if UNITY_EDITOR
            string path = $"{Application.persistentDataPath}/Save/save{saveNumber}.txt";

            if (!File.Exists(path))
            {
                success = false;
                return null;
            }

            string json = File.ReadAllText(path);

            success = true;
            return JsonUtility.FromJson<GameData>(json);

#else
        if (SteamRemoteStorage.FileExists($"save{saveNumber}.json"))
        {
            int size = SteamRemoteStorage.GetFileSize($"save{saveNumber}.json");
            byte[] bytes = new byte[size];
            SteamRemoteStorage.FileRead($"save{saveNumber}.json", bytes, size);
            string json = Encoding.UTF8.GetString(bytes);

            success = true;
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            success = false;
            return null;
        }
#endif
        }
    }
}