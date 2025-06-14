using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public interface ISaveProvidable
    {
        void SaveModel();
        void LoadModel();
    }
}