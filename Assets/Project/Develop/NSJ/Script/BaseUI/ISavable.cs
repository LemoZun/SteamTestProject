using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_MVVM
{
    public partial interface ISavable
    {

        /// <summary>
        ///  ISavable 인터페이스를 통해 세이브할 데이터를 주고 받습니다
        /// </summary>
        /// <returns></returns>
        string Save();

        /// <summary>
        /// ISavable 인터페이스를 통해 로드된 데이터를 주고 받습니다
        /// </summary>
        /// <param name="saveEntrys"></param>
        /// <returns></returns>
        string Load(List<string> saveEntrys);
    }
}