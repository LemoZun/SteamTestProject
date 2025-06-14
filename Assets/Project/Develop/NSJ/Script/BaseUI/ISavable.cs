using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_MVVM
{
    public partial interface ISavable
    {

        /// <summary>
        ///  ISavable �������̽��� ���� ���̺��� �����͸� �ְ� �޽��ϴ�
        /// </summary>
        /// <returns></returns>
        string Save();

        /// <summary>
        /// ISavable �������̽��� ���� �ε�� �����͸� �ְ� �޽��ϴ�
        /// </summary>
        /// <param name="saveEntrys"></param>
        /// <returns></returns>
        string Load(List<string> saveEntrys);
    }
}