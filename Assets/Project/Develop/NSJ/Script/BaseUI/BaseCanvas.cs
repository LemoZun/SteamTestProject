using UnityEngine;
using System;

public class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected BasePanel[] _panels;
    
    void Awake()
    {
        BindPanel();
    }

    public void ChangePanel<TEnum>(TEnum panel) where TEnum : Enum
    {
        int panelIndex = Util.ToIndex(panel);

        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].gameObject.SetActive(i == panelIndex);
        }
    }

    private void BindPanel()
    {
        foreach (BasePanel panel in _panels)
        {
            panel.Canvas = this;
        }
    }
}