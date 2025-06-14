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

        protected abstract void OnAwake();

        protected void LoadData()
        {
            ProcessLoad();
        }

        private void ProcessLoad()
        {
            ISaveProvidable providable = GetComponent<ISaveProvidable>();
            if (providable != null)
            {
                providable.LoadModel();
            }
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