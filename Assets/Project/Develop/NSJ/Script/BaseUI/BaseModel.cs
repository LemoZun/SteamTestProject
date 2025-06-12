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
        Debug.Log("Json���̺�");
        string json = ToJson(this);
        SaveEntry entry = new SaveEntry
        {
            SaveID = $"{typeof(T)}/{ViewID}",
            Json = json
        };
        // SaveEntry ���� ����
        string saveJson = ToJson(entry);
        Debug.Log(saveJson);
        // ���⿡ SaveEntry�� �����ϴ� ������ �߰��մϴ�.
        // ����� �׽�Ʈ ���ӸŴ����� �־����� ���� SaveManager�� ������ �����Դϴ�.
        TestGameManager.Instance.AddSaveModelData(saveJson);
    }
    public virtual void LoadData<T>(List<string> saveEntrys)
    {
        // SaveEntry�� ã�Ƽ� �ε��ϴ� ����
        foreach (string entryJson in saveEntrys)
        {
            SaveEntry saveEntry = FromJson<SaveEntry>(entryJson);
            Debug.Log(entryJson);
            // entryJson.SaveID�� ���� ���� SaveID�� ��ġ�ϴ��� Ȯ���մϴ�.
            if (saveEntry.SaveID == $"{typeof(T)}/{ViewID}")
            {

                // entryJson.Json�� ����Ͽ� ���� �ε��մϴ�.
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
