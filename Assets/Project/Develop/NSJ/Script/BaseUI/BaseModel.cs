using NSJ_SaveUtility;
using System;
using UnityEngine;

namespace NSJ_MVVM
{
    [System.Serializable]
    public abstract class BaseModel
    {
        public bool CanSave { get { return _canSave; } set { _canSave = value; OnCansSaveChanged?.Invoke(value); } }
        public bool IsLoaded { get { return _isLoaded; } set { _isLoaded = value; OnIsLoadedChanged?.Invoke(value); } }
        public bool HasViewID { get { return _hasViewID; } set { _hasViewID = value; OnHasViewIDChanged?.Invoke(value); } }
        public int ViewID { get { return _viewID; } set { _viewID = value; OnViewIDChanged?.Invoke(value); } }

        public event Action<bool> OnCansSaveChanged;
        public event Action<bool> OnIsLoadedChanged;
        public event Action<bool> OnHasViewIDChanged;
        public event Action<int> OnViewIDChanged;

        [Tooltip("세이브 여부")]
        [SerializeField] public bool _canSave;
        [HideInInspector] public bool _isLoaded;
        [HideInInspector] public bool _hasViewID;
        [HideInInspector] public int _viewID;

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
        public abstract void Init();


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
            OnLoadEvent?.Invoke();
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