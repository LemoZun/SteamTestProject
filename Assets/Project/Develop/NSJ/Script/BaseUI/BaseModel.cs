using NSJ_SaveUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_MVVM
{
    [System.Serializable]
    public abstract class BaseModel
    {
        
        public bool IsLoaded { get {  return false; } set { _isLoaded = value; OnIsLoadedChanged?.Invoke(value); } }
        public bool HasViewID { get { return _hasViewID; } set { _hasViewID = value; OnHasViewIDChanged?.Invoke(value); } }
        public int ViewID { get { return _viewID; } set { _viewID = value; OnViewIDChanged?.Invoke(value); } }

        public event Action<bool> OnIsLoadedChanged;
        public event Action<bool> OnHasViewIDChanged;
        public event Action<int> OnViewIDChanged;

        [SerializeField]private bool _isLoaded;
        [SerializeField]private bool _hasViewID;
        [SerializeField]private int _viewID;

        public event Action OnLoadEvent;
        /// <summary>
        /// ���� �ʱ�ȭ�ϴ� �޼����Դϴ�.
        /// </summary>
        public void InitModel()
        {
            Init();
        }

        /// <summary>
        /// ���� �ʱ�ȭ�ϴ� �޼����Դϴ�. �� �޼���� ���� ������ �� ȣ��˴ϴ�.
        /// </summary>
        public virtual void Init() { }


        /// <summary>
        /// ���� �����͸� Json �������� �����ϴ� �޼����Դϴ�.
        /// </summary>
        /// <typeparam name="T">Model Ÿ�� </typeparam>
        public virtual void SaveData<T>() where T : BaseModel
        {
            string json = ToJson(this);
            SaveEntry entry = new SaveEntry
            {
                SaveID = $"{typeof(T)}/{ViewID}",
                Json = json
            };
            // SaveEntry ���� ����
            string saveJson = ToJson(entry);
            // ���⿡ SaveEntry�� �����ϴ� ������ �߰��մϴ�.
            // ����� �׽�Ʈ ���ӸŴ����� �־����� ���� SaveManager�� ������ �����Դϴ�.
            SaveManager.Instance.AddSaveModelData(saveJson);
        }
        /// <summary>
        /// �����͸� �ε��ϴ� �޼����Դϴ�.
        /// </summary>
        /// <typeparam name="T">Model Ÿ�� ��</typeparam>
        /// <param name="saveEntrys"></param>
        public virtual void LoadData<T>() where T : BaseModel, ICopyable<T>
        {
            T model = null;

            GameData data = SaveManager.Instance.Data;
            List<string> saveEntrys = data.Models;

            // SaveEntry�� ã�Ƽ� �ε��ϴ� ����
            foreach (string entryJson in saveEntrys)
            {
                SaveEntry saveEntry = FromJson<SaveEntry>(entryJson);
                // entryJson.SaveID�� ���� ���� SaveID�� ��ġ�ϴ��� Ȯ���մϴ�.
                if (saveEntry.SaveID == $"{typeof(T)}/{ViewID}")
                {
                    // entryJson.Json�� ����Ͽ� ���� �ε��մϴ�.
                    model = FromJson<T>(saveEntry.Json);
                }
            }
            // ���� �Ӽ��� �����մϴ�.
            AllCopyFrom(model);
            OnLoadEvent?.Invoke();
        }
        /// <summary>
        /// ���� ��� �����͸� �����ϴ� �޼����Դϴ�.
        /// </summary>
        private void AllCopyFrom<T>(T model) where T : BaseModel, ICopyable<T>
        {
            if (model == null)
            {
                // ���� �����Ͱ� ���� ��� �ε���� ���� ǥ��
                IsLoaded = false;
            }
            else
            {
                IsLoaded = true;
                HasViewID = model.HasViewID;
                ViewID = model.ViewID;
                ((ICopyable<T>)this).CopyFrom(model);
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

        public void SubscribeSaveEvent<TModel>() where TModel : BaseModel
        {
            SaveManager.Instance.OnSaveBeforeEvent += SaveData<TModel>;
        }
        public void UnsubscribeSaveEvent<TModel>() where TModel : BaseModel
        {
            SaveManager.Instance.OnSaveBeforeEvent -= SaveData<TModel>;
        }
    }
}