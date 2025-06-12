using NSJ_MVVM;
using UnityEngine;

public class TitleCanvas : BaseCanvas
{
    public enum Panel
    {
        Title,
        SIZE
    }

     void OnEnable()
    {
        ChangePanel(Panel.Title);
    }
}
