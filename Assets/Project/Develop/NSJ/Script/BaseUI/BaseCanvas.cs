using UnityEngine;

public class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected BasePanel[] _panels;
    
    void Awake()
    {
        BindPanel();
    }

    public void ChangePanel(int panel)
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].gameObject.SetActive(i == panel);
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