using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestGameManager : SingleTon<TestGameManager>
{
    public GameData Data;

    public List<string> Models => Data.Models;

    public event UnityAction OnSaveBeforeEvent;
    public event UnityAction OnSaveEvent;
    public event UnityAction<List<string>> OnLoadEvent;
    public bool SaveData(int saveNumber = int.MinValue)
    {
        OnSaveBeforeEvent?.Invoke();

        if (saveNumber == int.MinValue)
        {
            saveNumber = Data.SaveNumber;
        }

        DateTime now = DateTime.Now;
        Data.LastSaveTime = now.ToString("yyyy-MM-dd HH:mm:ss");


        bool success = SaveUtility.Save(ref Data, saveNumber);

        if (success)
        {

            OnSaveEvent?.Invoke();
        }

        Debug.Log("세이브 성공");
        return success;
    }

    public bool LoadData(int saveNumber)
    {
        Data = SaveUtility.Load(saveNumber, out bool success);

        if (success == false)
        {
            if (Data == null)
            {
                Data = new GameData();
                Data.SaveNumber = saveNumber;
            }
        }
        OnLoadEvent?.Invoke(Data.Models);

        return success;
    }

    /// <summary>
    /// 모델 데이터를 추가합니다.
    /// </summary>
    public void AddSaveModelData(string saveJson)
    {
        Data.Models.Add(saveJson);
    }
}
