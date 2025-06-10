using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : BaseCanvas
{
    public enum Panel
    {
        Game,
        SIZE
    }

    void OnEnable()
    {
        ChangePanel(Panel.Game);
    }
}
