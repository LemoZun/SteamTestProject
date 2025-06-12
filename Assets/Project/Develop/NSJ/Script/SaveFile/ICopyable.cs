using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ ���� �����մϴ�.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICopyable<T>
{
    /// <summary>
    /// ���� �����͸� �����մϴ�.
    /// </summary>
    /// <param name="model"></param>
    void CopyFrom(T model);
}
