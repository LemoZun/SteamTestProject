using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseModel
{
    public bool HasViewID;
    public int ViewID;

    public virtual void SaveData<T>()
    {
        Debug.Log("Json세이브");
        string json = ToJson(this);
        SaveEntry entry = new SaveEntry
        {
            SaveID = $"{typeof(T)}/{ViewID}",
            Json = json
        };
        // SaveEntry 저장 로직
        string saveJson = ToJson(entry);
        Debug.Log(saveJson);
        // 여기에 SaveEntry를 저장하는 로직을 추가합니다.
        // 현재는 테스트 게임매니저에 넣었지만 이후 SaveManager로 변경할 예정입니다.
        TestGameManager.Instance.AddSaveModelData(saveJson);
    }
    public virtual void LoadData<T>(List<string> saveEntrys)
    {
        // SaveEntry를 찾아서 로드하는 로직
        foreach (string entryJson in saveEntrys)
        {
            SaveEntry saveEntry = FromJson<SaveEntry>(entryJson);
            Debug.Log(entryJson);
            // entryJson.SaveID가 현재 모델의 SaveID와 일치하는지 확인합니다.
            if (saveEntry.SaveID == $"{typeof(T)}/{ViewID}")
            {

                // entryJson.Json을 사용하여 모델을 로드합니다.
                T model = FromJson<T>(saveEntry.Json);
            }
        }
    }

    private string ToJson<T>(T instance) where T : class
    {
        string json = JsonUtility.ToJson(instance);
        return json;
    }

    private T FromJson<T>(string json)
    {
        T model = JsonUtility.FromJson<T>(json);
        return model;
    }

    public void SubscribeSaveEvent<TModel>()
    {
    
        TestGameManager.Instance.OnSaveEvent += SaveData<TModel>;
        TestGameManager.Instance.OnLoadEvent += LoadData<TModel>;

        LoadData<TModel>(TestGameManager.Instance.Models);
    }
    public void UnsubscribeSaveEvent<TModel>()
    {
        TestGameManager.Instance.OnSaveEvent -= SaveData<TModel>;
        TestGameManager.Instance.OnLoadEvent -= LoadData<TModel>;
    }
}
