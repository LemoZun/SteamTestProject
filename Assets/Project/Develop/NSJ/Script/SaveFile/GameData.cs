using UnityEngine.Events;

[System.Serializable]
public class GameData
{
    public int SaveNumber;

    public string LastSaveTime;
    public double TotalPlayTime;

    public int Score;

    public void AddScore(int value)
    {
        Score += value;
    }
}
