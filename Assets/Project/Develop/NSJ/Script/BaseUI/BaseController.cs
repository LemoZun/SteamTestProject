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

            // TODO: 임시로 넣은 강제 컴포넌트 주입, 이후 어떻게 타입을 여기서 명시하지않고 자동으로 컴포넌트를 주입할지 고민해야됨
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