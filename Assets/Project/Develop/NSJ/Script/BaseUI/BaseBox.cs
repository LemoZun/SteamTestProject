using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBox : BaseUI
{
    [HideInInspector] public BasePanel Panel;
    [HideInInspector] public BaseCanvas Canvas => Panel.Canvas;

}
