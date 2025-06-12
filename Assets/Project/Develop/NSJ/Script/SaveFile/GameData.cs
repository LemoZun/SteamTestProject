using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int SaveNumber;

    public string LastSaveTime;
    public double TotalPlayTime;

    public List<string> Models = new List<string>();
}
