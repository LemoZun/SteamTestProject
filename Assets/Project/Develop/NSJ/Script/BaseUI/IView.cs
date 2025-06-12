using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView<T>
{
    bool HasViewID { get; set; }
    int ViewID { get; set; }

    bool HasViewModel { get; set; }

    void SetViewModel(T viewModel);
}