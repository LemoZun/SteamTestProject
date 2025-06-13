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

        public ModelSaveHandler _saveHandler;
        /// <summary>
        /// ���� �ʱ�ȭ�ϴ� �޼����Դϴ�.
        /// </summary>
        public void InitModel()
        {
            _saveHandler = new ModelSaveHandler(this);
            Init();
        }

        /// <summary>
        /// ���� �ʱ�ȭ�ϴ� �޼����Դϴ�. �� �޼���� ���� ������ �� ȣ��˴ϴ�.
        /// </summary>
        public abstract void Init();


        /// <summary>
        /// ���� �����͸� Json �������� �����ϴ� �޼����Դϴ�.
        /// </summary>
        /// <typeparam name="T">Model Ÿ�� </typeparam>
        public virtual void SaveData<T>() where T : BaseModel
        {
            _saveHandler.Save<T>();
        }
        /// <summary>
        /// �����͸� �ε��ϴ� �޼����Դϴ�.
        /// </summary>
        /// <typeparam name="T">Model Ÿ�� ��</typeparam>
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