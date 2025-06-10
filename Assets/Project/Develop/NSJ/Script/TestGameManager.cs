using System;
using UnityEngine.Events;
using UnityEngine;

public class TestGameManager : SingleTon<TestGameManager>
{
    public GameData Data;

    public int Score => Data.Score;

    public event UnityAction OnSaveEvent;
    public event UnityAction<int> OnScoreChangeEvent;

    public bool SaveData(int saveNumber = int.MinValue)
    {
        if (saveNumber == int.MinValue)
        {
            saveNumber = Data.SaveNumber;
        }

        DateTime now  = DateTime.Now;
        Data.LastSaveTime = now.ToString("yyyy-MM-dd HH:mm:ss");


        bool success = SaveUtility.Save(ref Data, saveNumber);

        if (success)
        {

            OnSaveEvent?.Invoke();
        }


        return success;
    }

    public bool LoadData(int saveNumber)
    {
        Data = SaveUtility.Load(saveNumber, out bool success);

        if(success == false)
        {
            if (Data == null)
            {
                Data = new GameData();
                Data.SaveNumber = saveNumber;
            }
        }
        
        return success;

    }

    public void AddScore(int value)
    {
        Data.AddScore(value);
        OnScoreChangeEvent?.Invoke(Data.Score);
    }
}
