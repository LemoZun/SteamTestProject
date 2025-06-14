using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_MVVM
{
    public partial interface ISavable
    {
        string Save();
        string Load(List<string> saveEntrys);
    }
}