using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    UnSet,
    Normal,
    Start,
    Boss,
    Shop,
    Special,
    Secret,
    End,
}

[System.Serializable]
public class RoomModel : BaseModel
{
    public RoomType Type;
}
