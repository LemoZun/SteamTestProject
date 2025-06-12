using NSJ_MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : BasePanel
{
    public enum View{ Game, SIZE }

    void OnEnable()
    {
        ChangeView(View.Game);
    }
}
