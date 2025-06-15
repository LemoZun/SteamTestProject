using NSJ_SaveUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_MVVM
{
    [System.Serializable]
    public abstract class BaseModel
    {
        public bool CanSave { get { return _canSave; } set { _canSave = value; OnCanSaveChanged?.Invoke(value); } }
        public bool IsLoaded { get { return _isLoaded; } set { _isLoaded = value; OnIsLoadedChanged?.Invoke(value); } }
        public bool HasViewID { get { return _hasViewID; } set { _hasViewID = value; OnHasViewIDChanged?.Invoke(value); } }
        public int ViewID { get { return _viewID; } set { _viewID = value; OnViewIDChanged?.Invoke(value); } }

        public event Action<bool> OnCanSaveChanged;
        public event Action<bool> OnIsLoadedChanged;
        public event Action<bool> OnHasViewIDChanged;
        public event Action<int> OnViewIDChanged;

        [Tooltip("세이브 여부")]
  
        [SerializeField] public bool _canSave;
        [SerializeField] public bool _isLoaded;
        [SerializeField] public bool _hasViewID;
        [SerializeField] public int _viewID;

        public event Action OnLoadEvent;
        public event Action OnDestroyEvent;

        public ModelSaveHandler _saveHandler;
        /// <summary>
        /// 모델을 초기화하는 메서드입니다.
        /// </summary>
        public void InitModel()
        {
            _saveHandler = new ModelSaveHandler(this);
            Init();
        }


        /// <summary>
        /// 객체가 파괴되었을떄 호출되는 메서드입니다
        /// </summary>
        public void DestroyModel()
        {
            OnDestroyEvent?.Invoke();
        }
        /// <summary>
        /// 모델을 초기화하는 메서드입니다. 이 메서드는 모델이 설정될 때 호출됩니다.
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 모델 데이터가 로드된 후 호출되는 메서드입니다
        /// </summary>
        protected abstract void OnLoadModel();
        /// <summary>
        /// 모델의 데이터를 Json 형식으로 저장하는 메서드입니다.
        /// </summary>
        /// <typeparam name="T">Model 타입 </typeparam>
        public virtual void SaveData<T>() where T : BaseModel
        {
            _saveHandler.Save<T>();
        }
        /// <summary>
        /// 데이터를 로드하는 메서드입니다.
        /// </summary>
        /// <typeparam name="T">Model 타입 명</typeparam>
        /// <param name="saveEntrys"></param>
        public virtual void LoadData<T>() where T : BaseModel, ICopyable<T>
        {
            _saveHandler.Load<T>();
            OnLoadModel();
            OnLoadEvent?.Invoke();
        }

        /// <summary>
        /// 세이브 하기 전의 이벤트에 대해 구독합니다
        /// </summary>
        public void SubscribeSaveEvent<T>() where T : BaseModel
        {
            SaveManager.Instance.OnSaveBeforeEvent += SaveData<T>;
        }
        /// <summary>
        /// 객체가 파괴될 때 세이브 하기 전의 이벤트에 대해 구독을 끊습니다
        /// </summary>
        public void UnsubscribeSaveEvent<T>() where T : BaseModel
        {
            if (SaveManager.Instance == null) return;
            SaveManager.Instance.OnSaveBeforeEvent -= SaveData<T>;
        }
    }
}