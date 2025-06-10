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
        ChangePanel((int)Panel.Title);
    }
}
