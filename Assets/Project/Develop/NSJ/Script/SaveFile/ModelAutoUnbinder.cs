using NSJ_MVVM;
using System;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public class ModelAutoUnbinder : MonoBehaviour
    {
        private BaseModel _model;

        /// <summary>
        /// ���� ĳ���ϰ� �����մϴ�
        /// </summary>
        public void Track<TModel>(TModel model) where TModel : BaseModel
        {
            _model = model;
            SaveManager.RegisterModel<TModel>(model);
        }

        /// <summary>
        /// ��ü�� �ı��� �� �𵨿��� ��ü�� �ı��Ǿ����� �˸��ϴ�
        /// </summary>
        void OnDestroy()
        {
            UnTrack();
        }

        private void UnTrack() 
        {
            if (_model != null)
            {
                SaveManager.UnRegisterModel(_model);
                _model.DestroyModel();
            }
        }
    }
}