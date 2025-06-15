using NSJ_MVVM;
using NSJ_SaveUtility;

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

    protected override void Init() { }

    protected override void OnLoadModel()
    {
        
    }
}
