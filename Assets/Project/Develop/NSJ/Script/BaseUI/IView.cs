using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView<T>
{
    bool HasViewModel { get; set; }

    void SetViewModel(T viewModel);
}