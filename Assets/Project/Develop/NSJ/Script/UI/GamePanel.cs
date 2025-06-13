using NSJ_MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : BasePanel
{
    public enum Group{ Game, SIZE }

    void OnEnable()
    {
        ChangeGroup(Group.Game);
    }
}
