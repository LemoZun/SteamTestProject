using System;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [HideInInspector] public BaseCanvas Canvas;

    [SerializeField] protected BaseView[] _views;

    void Awake()
    {
        BindView();
    }

    public void ChangeView<TEnum>(TEnum view) where TEnum : Enum
    {
        int boxIndex = Util.ToIndex(view);

        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].gameObject.SetActive(i == boxIndex);
        }
    }

    private void BindView()
    {
        foreach (BaseView view in _views)
        {
            view.Panel = this;
        }
    }
}
