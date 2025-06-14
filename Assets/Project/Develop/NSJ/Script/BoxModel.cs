using NSJ_MVVM;
using NSJ_SaveUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoxModel : BaseModel
{
    // TODO : BoxData 필드

    protected string BoxID;
    protected override void Init()
    {

    }

    protected override void OnLoadModel()
    {
        SetBoxData();
    }

    /// <summary>
    /// 박스 데이터를 BoxDataBase에서 받아와서 설정
    /// </summary>
    private void SetBoxData()
    {
        // BoxID가 로드됨. (아직 S/O인 BoxData 모름)
        // BoxID를 가지고 BoxDataBase(싱글톤)에 접근
        // BoxDataBase에 있는 딕셔너리에 접근
        // 키값으로 BoxID를 넣음
        // 밸류값으로 S/O 타입의 BoxData 반환
        // BoxData 필드에 반환으로 받은 BoxData 대입
    }
}
