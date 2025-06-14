using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public interface ISaveProvidable
    {
        /// <summary>
        /// ���̺�Ŵ����� �𵨵����͸� �����մϴ�
        /// </summary>
        void SaveModel();
        /// <summary>
        /// ���̺�Ŵ������� �� �����͵��� �ҷ��ɴϴ�
        /// </summary>
        void LoadModel();
    }
}