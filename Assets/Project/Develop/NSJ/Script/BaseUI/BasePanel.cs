using System;
using UnityEngine;

namespace NSJ_MVVM
{
    public abstract class BasePanel : MonoBehaviour
    {
        [HideInInspector] public BaseCanvas Canvas;

        [SerializeField] protected BaseGroup[] _groups;

        void Awake()
        {
            BindGroup();
        }

        public void ChangeGroup<TEnum>(TEnum view) where TEnum : Enum
        {
            int boxIndex = Util.ToIndex(view);

            for (int i = 0; i < _groups.Length; i++)
            {
                _groups[i].gameObject.SetActive(i == boxIndex);
            }
        }

        private void BindGroup()
        {
            foreach (BaseGroup group in _groups)
            {
                group.Panel = this;
            }
        }
    }
}