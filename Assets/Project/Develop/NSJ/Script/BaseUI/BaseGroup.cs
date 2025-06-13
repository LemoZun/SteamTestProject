using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_MVVM
{
    public class BaseGroup : MonoBehaviour
    {
        [HideInInspector] public BasePanel Panel;
        [HideInInspector] public BaseCanvas Canvas => Panel.Canvas;

        [SerializeField] protected BaseView[] _views;

        void Awake()
        {
            BindGroup();
        }

        private void BindGroup()
        {
            foreach (BaseView view in _views)
            {
                view.Group = this;
            }
        }
    }
}