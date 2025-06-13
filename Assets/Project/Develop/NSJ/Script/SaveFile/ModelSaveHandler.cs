using NSJ_MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public class ModelSaveHandler 
    {
        private readonly BaseModel _model;

        public ModelSaveHandler(BaseModel model)
        {
            _model = model; 
        }

        public void Save<TModel>() where TModel : BaseModel
        {
            string json = ToJson(_model);
            SaveEntry entry = new SaveEntry
            {
                SaveID = $"{typeof(TModel)}",
                Json = json
            };
            // SaveEntry ���� ����
            string saveJson = ToJson(entry);
            // ���⿡ SaveEntry�� �����ϴ� ������ �߰��մϴ�.
            // ����� �׽�Ʈ ���ӸŴ����� �־����� ���� SaveManager�� ������ �����Դϴ�.
            SaveManager.Instance.AddSaveModelData(saveJson);
        }

        public void Load<TModel>() where TModel : BaseModel, ICopyable<TModel>
        {
            TModel loadData = null;

            GameData data = SaveManager.Instance.Data;

            List<string> saveEntrys = data.Models;

            // SaveEntry�� ã�Ƽ� �ε��ϴ� ����
            foreach (string entryJson in saveEntrys)
            {
                SaveEntry saveEntry = FromJson<SaveEntry>(entryJson);
                // entryJson.SaveID�� ���� ���� SaveID�� ��ġ�ϴ��� Ȯ���մϴ�.
                if (saveEntry.SaveID == $"{typeof(TModel)}")
                {
                    // entryJson.Json�� ����Ͽ� ���� �ε��մϴ�.
                    loadData = FromJson<TModel>(saveEntry.Json);
                    // ����� �� ������ ����
                    SaveManager.Instance.RemoveModelData(entryJson);
                    break;
                }
            }
            // ���� �Ӽ��� �����մϴ�.
            AllCopyFrom(loadData);
        }
        /// <summary>
        /// ���� ��� �����͸� �����ϴ� �޼����Դϴ�.
        /// </summary>
        private void AllCopyFrom<T>(T loadData) where T : BaseModel, ICopyable<T>
        {
            if (loadData == null)
            {
                // ���� �����Ͱ� ���� ��� �ε���� ���� ǥ��
                Debug.LogError($"�����;���");
                _model.IsLoaded = false;
            }
            else
            {
                _model.IsLoaded = true;
                _model.HasViewID = loadData.HasViewID;
                _model.ViewID = loadData.ViewID;
                ((ICopyable<T>)_model).CopyFrom(loadData);
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


    }
}