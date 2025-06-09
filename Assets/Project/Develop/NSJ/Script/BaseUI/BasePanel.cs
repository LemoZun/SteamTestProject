using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [HideInInspector] public BaseCanvas Canvas;

    [SerializeField] protected BaseBox[] _boxs;

    void Awake()
    {
        BindBox();
    }

    public void ChangeBox(int box)
    {
        for (int i = 0; i < _boxs.Length; i++)
        {
            _boxs[i].gameObject.SetActive(i == box);
        }
    }

    private void BindBox()
    {
        foreach (BaseBox box in _boxs)
        {
            box.Panel = this;
        }
    }
}
