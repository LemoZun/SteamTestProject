using UnityEngine;

public class TitleCanvas : BaseCanvas
{
    public enum Panel
    {
        Title,
        Game,
        SIZE
    }

     void OnEnable()
    {
        ChangePanel((int)Panel.Title);
    }
}
