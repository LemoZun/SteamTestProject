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
        /// 모델을 초기화하는 메서드입니다.
        /// </summary>
        public void InitModel()
        {
            Init();
        }

        /// <summary>
        /// 모델을 초기화하는 메서드입니다. 이 메서드는 모델이 설정될 때 호출됩니다.
        /// </summary>
        public virtual void Init() { }


        /// <summary>
        /// 모델의 데이터를 Json 형식으로 저장하는 메서드입니다.
        /// </summary>
        /// <typeparam name="T">Model 타입 </typeparam>
        public virtual void SaveData<T>() where T : BaseModel
        {
            string json = ToJson(this);
            SaveEntry entry = new SaveEntry
            {
                SaveID = $"{typeof(T)}/{ViewID}",
                Json = json
            };
            // SaveEntry 저장 로직
            string saveJson = ToJson(entry);
            // 여기에 SaveEntry를 저장하는 로직을 추가합니다.
            // 현재는 테스트 게임매니저에 넣었지만 이후 SaveManager로 변경할 예정입니다.
            SaveManager.Instance.AddSaveModelData(saveJson);
        }
        /// <summary>
        /// 데이터를 로드하는 메서드입니다.
        /// </summary>
        /// <typeparam name="T">Model 타입 명</typeparam>
        /// <param name="saveEntrys"></param>
        public virtual void LoadData<T>() where T : BaseModel, ICopyable<T>
        {
            T model = null;

            GameData data = SaveManager.Instance.Data;
            List<string> saveEntrys = data.Models;

            // SaveEntry를 찾아서 로드하는 로직
            foreach (string entryJson in saveEntrys)
            {
                SaveEntry saveEntry = FromJson<SaveEntry>(entryJson);
                // entryJson.SaveID가 현재 모델의 SaveID와 일치하는지 확인합니다.
                if (saveEntry.SaveID == $"{typeof(T)}/{ViewID}")
                {
                    // entryJson.Json을 사용하여 모델을 로드합니다.
                    model = FromJson<T>(saveEntry.Json);
                }
            }
            // 모델의 속성에 복사합니다.
            AllCopyFrom(model);
            OnLoadEvent?.Invoke();
        }
        /// <summary>
        /// 모델의 모든 데이터를 복사하는 메서드입니다.
        /// </summary>
        private void AllCopyFrom<T>(T model) where T : BaseModel, ICopyable<T>
        {
            if (model == null)
            {
                // 저장 데이터가 없는 경우 로드되지 않음 표시
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