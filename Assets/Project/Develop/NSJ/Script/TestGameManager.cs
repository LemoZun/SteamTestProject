using System;
using UnityEngine.Events;

public class TestGameManager : SingleTon<TestGameManager>
{
    public GameData Data;

    public event UnityAction OnSaveEvent;

    public bool SaveData(int saveNumber = -1)
    {
        if (saveNumber == -1)
        {
            saveNumber = Data.saveNumber;
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
        Data = null;
        bool success = SaveUtility.Load(ref Data, saveNumber);

        if(success == false)
        {
            if (Data == null)
            {
                Data = new GameData();
                Data.saveNumber = saveNumber;
            }
        }
        
        return success;

    }
}
