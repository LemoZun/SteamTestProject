using UnityEngine;
using System;

public class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected BasePanel[] _panels;
    
    void Awake()
    {
        BindPanel();
    }

    /// <summary>
    /// 패널을 변경합니다. Enum 타입을 사용하여 패널을 지정할 수 있습니다.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="panel"></param>
    public void ChangePanel<TEnum>(TEnum panel) where TEnum : Enum
    {
        int panelIndex = Util.ToIndex(panel);

        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].gameObject.SetActive(i == panelIndex);
        }
    }

    /// <summary>
    /// 패널을 바인딩합니다. 각 패널의 Canvas 속성을 설정하여 상호작용할 수 있도록 합니다.
    /// </summary>
    private void BindPanel()
    {
        foreach (BasePanel panel in _panels)
        {
            panel.Canvas = this;
        }
    }
}