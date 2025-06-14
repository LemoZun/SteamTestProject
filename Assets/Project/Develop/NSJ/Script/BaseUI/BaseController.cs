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

        protected virtual void Start()
        {
            LoadData();
            OnStart();
        }

        /// <summary>
        /// Awake대신 호출합니다. 모델 생성 후 호출됩니다
        /// </summary>
        protected abstract void OnAwake();
        /// <summary>
        /// Start대신 호출합니다. 모델 데이터 로드 후 호출됩니다
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// 데이터를 로드합니다
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
        /// 모델을 세이브하도록 합니다. Awake에서 호출해야합니다
        /// </summary>
        protected void EnableSave()
        {
            _model.CanSave = true;
        }
        /// <summary>
        /// 모델을 세이브하지 않도록 합니다. Awake에서 호출해야합니다
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