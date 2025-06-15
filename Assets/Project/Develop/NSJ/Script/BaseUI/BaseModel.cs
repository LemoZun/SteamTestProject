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

        [Tooltip("���̺� ����")]
  
        [SerializeField] public bool _canSave;
        [SerializeField] public bool _isLoaded;
        [SerializeField] public bool _hasViewID;
        [SerializeField] public int _viewID;

        public event Action OnLoadEvent;
        public event Action OnDestroyEvent;

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
        /// ��ü�� �ı��Ǿ����� ȣ��Ǵ� �޼����Դϴ�
        /// </summary>
        public void DestroyModel()
        {
            OnDestroyEvent?.Invoke();
        }
        /// <summary>
        /// ���� �ʱ�ȭ�ϴ� �޼����Դϴ�. �� �޼���� ���� ������ �� ȣ��˴ϴ�.
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// �� �����Ͱ� �ε�� �� ȣ��Ǵ� �޼����Դϴ�
        /// </summary>
        protected abstract void OnLoadModel();
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
            OnLoadModel();
            OnLoadEvent?.Invoke();
        }

        /// <summary>
        /// ���̺� �ϱ� ���� �̺�Ʈ�� ���� �����մϴ�
        /// </summary>
        public void SubscribeSaveEvent<T>() where T : BaseModel
        {
            SaveManager.Instance.OnSaveBeforeEvent += SaveData<T>;
        }
        /// <summary>
        /// ��ü�� �ı��� �� ���̺� �ϱ� ���� �̺�Ʈ�� ���� ������ �����ϴ�
        /// </summary>
        public void UnsubscribeSaveEvent<T>() where T : BaseModel
        {
            if (SaveManager.Instance == null) return;
            SaveManager.Instance.OnSaveBeforeEvent -= SaveData<T>;
        }
    }
}