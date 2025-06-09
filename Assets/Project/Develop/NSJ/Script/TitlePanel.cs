using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : BasePanel 
{
    public enum Box
    {
        Title,
        Start,
        SIZE,
    }

    void OnEnable()
    {
        ChangeBox((int)Box.Title);
    }
}
