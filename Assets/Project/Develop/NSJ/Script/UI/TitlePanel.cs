using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : BasePanel 
{
    public enum View
    {
        Title,
        Start,
        SIZE,
    }

    void OnEnable()
    {
        ChangeView(View.Title);
    }
}
