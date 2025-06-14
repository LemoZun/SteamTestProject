using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public interface ISaveProvidable
    {
        /// <summary>
        /// 세이브매니저에 모델데이터를 저장합니다
        /// </summary>
        void SaveModel();
        /// <summary>
        /// 세이브매니저에서 모델 데이터들을 불러옵니다
        /// </summary>
        void LoadModel();
    }
}