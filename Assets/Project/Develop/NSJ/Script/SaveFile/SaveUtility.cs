using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public class SaveUtility
{
    public static bool Save(ref GameData data, int saveNumber)
    {
        string path = $"{Application.persistentDataPath}/Save";

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string json = JsonUtility.ToJson(data);

        File.WriteAllText($"{path}/save{saveNumber}.txt", json);

        return true;
    }

    public static bool Load(ref GameData data, int saveNumber) 
    {
        string path = $"{Application.persistentDataPath}/Save/save{saveNumber}.txt";

        if (!File.Exists(path))
        {
            return false;
        }

        string json = File.ReadAllText(path);

        data = JsonUtility.FromJson<GameData>(json);

        return true;
    }
}
