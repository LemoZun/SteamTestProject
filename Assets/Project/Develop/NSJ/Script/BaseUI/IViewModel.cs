using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_MVVM
{
    public interface IViewModel
    {
        Bindable<bool> IsLoaded { get; }
        Bindable<bool> HasViewID { get; }
        Bindable<int> ViewID { get; }
    }
}