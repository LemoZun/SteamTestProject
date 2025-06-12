using NSJ_MVVM;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public static class AutoBinding
    {
        /// <summary>
        /// ���� �ڵ����� ���ε��ϴ� �޼����Դϴ�. MonoBehaviour�� ��ӹ޴� Ŭ�������� ����մϴ�.
        /// </summary>
        public static void Bind<TModel>(TModel model, MonoBehaviour owner) where TModel : BaseModel, ICopyable<TModel>
        {
            // ���̺� �Ŵ����� ���� ����մϴ�
            SaveManager.RegisterModel(model);

            // ���� �ڵ����� ����ε��� �� �ֵ��� �����մϴ�.
            ModelAutoUnbinder unbinder = owner.GetComponentInChildren<ModelAutoUnbinder>();
            if (unbinder == null)
            {
                unbinder = owner.gameObject.AddComponent<ModelAutoUnbinder>();
                unbinder.Track(model);
            }
        }
    }
}