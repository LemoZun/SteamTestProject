using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TempLayerList
{
    public static readonly int Tail = LayerMask.NameToLayer("Tail");
    public static readonly int Food = LayerMask.NameToLayer("Food");
    
    public static readonly LayerMask TailMask = LayerMask.GetMask("Tail");
    public static readonly LayerMask FoodMask = LayerMask.GetMask("Food");
}
