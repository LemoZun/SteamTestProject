using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : BasePanel
{
    public enum Box{ Game, SIZE }

    void OnEnable()
    {
        ChangeBox(Box.Game);
    }
}
