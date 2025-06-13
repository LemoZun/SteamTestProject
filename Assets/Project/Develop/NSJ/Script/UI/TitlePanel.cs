using NSJ_MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : BasePanel 
{
    public enum Group
    {
        Title,
        Start,
        SIZE,
    }

    void OnEnable()
    {
        ChangeGroup(Group.Title);
    }
}
