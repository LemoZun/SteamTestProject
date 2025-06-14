using NSJ_MVVM;
using System;
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

        public string Save<TModel>() where TModel : BaseModel
        {
            // ���̺� ���Ѵٰ� ǥ���� �� �н�
            if (_model.CanSave == false)
                return string.Empty;

            string json = ToJson(_model);
            SaveEntry entry = new SaveEntry
            {
                SaveID = $"{typeof(TModel)}",
                Json = json
            };
            // SaveEntry ���� ����
            string saveJson = ToJson(entry);

            return saveJson;
            // ���⿡ SaveEntry�� �����ϴ� ������ �߰��մϴ�.
            // ����� �׽�Ʈ ���ӸŴ����� �־����� ���� SaveManager�� ������ �����Դϴ�.
            //SaveManager.Instance.AddSaveModelData(saveJson);
        }

        public string Load<TModel>(List<string> saveEntrys) where TModel : BaseModel, ICopyable<TModel>
        {
            string returnJson = string.Empty;

            TModel loadData = null;

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
                    returnJson = entryJson;
                    break;
                }
            }
            // ���� �Ӽ��� �����մϴ�.
            AllCopyFrom(loadData);

            return returnJson;
        }
        /// <summary>
        /// ���� ��� �����͸� �����ϴ� �޼����Դϴ�.
        /// </summary>
        private void AllCopyFrom<T>(T loadData) where T : BaseModel, ICopyable<T>
        {
            if (loadData == null)
            {
                // ���� �����Ͱ� ���� ��� �ε���� ���� ǥ��
                Debug.Log($"{typeof(T)} : �ε��� ������ ����");
                _model.IsLoaded = false;
            }
            else
            {
                _model.CanSave = loadData.CanSave;
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