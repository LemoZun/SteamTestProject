using UnityEngine;

public class TitleCanvas : BaseCanvas
{
    public enum Panel
    {
        Title,
        SIZE
    }

    private void OnEnable()
    {
        ChangePanel((int)Panel.Title);
    }
}
