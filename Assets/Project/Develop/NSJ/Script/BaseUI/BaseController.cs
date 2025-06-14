using NSJ_SaveUtility;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NSJ_MVVM
{
    public abstract class BaseController<TModel, TViewModel> : MonoBehaviour, ISavable
            where TModel : BaseModel, ICopyable<TModel>, new()
            where TViewModel : BaseViewModel<TModel, TViewModel>, new()
    {

        [SerializeField] protected TModel _model;


        protected virtual void Awake()
        {
            _model = ModelFactory.CreateModel<TModel, TViewModel>(this);

            // TODO: �ӽ÷� ���� ���� ������Ʈ ����, ���� ��� Ÿ���� ���⼭ ��������ʰ� �ڵ����� ������Ʈ�� �������� ����ؾߵ�
            gameObject.AddComponent<SaveHandler>();

            OnAwake();
        }

        protected virtual void Start()
        {
            LoadData();
            OnStart();
        }

        /// <summary>
        /// Awake��� ȣ���մϴ�. �� ���� �� ȣ��˴ϴ�
        /// </summary>
        protected abstract void OnAwake();
        /// <summary>
        /// Start��� ȣ���մϴ�. �� ������ �ε� �� ȣ��˴ϴ�
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// �����͸� �ε��մϴ�
        /// </summary>
        protected void LoadData()
        {
            ISaveProvidable providable = GetComponent<ISaveProvidable>();
            if (providable != null)
            {
                providable.LoadModel();
            }
        }

        /// <summary>
        /// ���� ���̺��ϵ��� �մϴ�. Awake���� ȣ���ؾ��մϴ�
        /// </summary>
        protected void EnableSave()
        {
            _model.CanSave = true;
        }
        /// <summary>
        /// ���� ���̺����� �ʵ��� �մϴ�. Awake���� ȣ���ؾ��մϴ�
        /// </summary>
        protected void DisableSave()
        {
            _model.CanSave = false;
        }


        public string Load(List<string> saveEntrys)
        {
            return _model.LoadData<TModel>(saveEntrys);
        }

        public string Save()
        {
            return _model.SaveData<TModel>();
        }
    }
}