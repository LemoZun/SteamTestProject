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
    /// �г��� �����մϴ�. Enum Ÿ���� ����Ͽ� �г��� ������ �� �ֽ��ϴ�.
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
    /// �г��� ���ε��մϴ�. �� �г��� Canvas �Ӽ��� �����Ͽ� ��ȣ�ۿ��� �� �ֵ��� �մϴ�.
    /// </summary>
    private void BindPanel()
    {
        foreach (BasePanel panel in _panels)
        {
            panel.Canvas = this;
        }
    }
}