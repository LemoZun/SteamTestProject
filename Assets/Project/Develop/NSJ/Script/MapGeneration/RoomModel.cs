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
public class RoomModel : BaseModel, ICopyable<RoomModel>
{
    public RoomType Type;

    public void CopyFrom(RoomModel model)
    {
        
    }
}
