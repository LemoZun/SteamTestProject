using NSJ_MVVM;
using NSJ_SaveUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoxModel : BaseModel
{
    // TODO : BoxData �ʵ�

    protected string BoxID;
    protected override void Init()
    {

    }

    protected override void OnLoadModel()
    {
        SetBoxData();
    }

    /// <summary>
    /// �ڽ� �����͸� BoxDataBase���� �޾ƿͼ� ����
    /// </summary>
    private void SetBoxData()
    {
        // BoxID�� �ε��. (���� S/O�� BoxData ��)
        // BoxID�� ������ BoxDataBase(�̱���)�� ����
        // BoxDataBase�� �ִ� ��ųʸ��� ����
        // Ű������ BoxID�� ����
        // ��������� S/O Ÿ���� BoxData ��ȯ
        // BoxData �ʵ忡 ��ȯ���� ���� BoxData ����
    }
}
