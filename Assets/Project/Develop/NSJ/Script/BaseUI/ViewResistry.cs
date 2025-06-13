using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSJ_MVVM
{
    public class ViewResistry<TViewModel> where TViewModel : BaseViewModel
    {
        private static ViewResistry<TViewModel> _instance;
        public static ViewResistry<TViewModel> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ViewResistry<TViewModel>();
                    SceneManager.sceneUnloaded += ClearUnUsed;
                }
                return _instance;
            }
        }

        class BindingEntry
        {
            public IView<TViewModel> View;
            public TViewModel ViewModel;

            public bool IsAvailable => ViewModel == null;
            public bool IsMahch(TViewModel viewModel) =>
                View.HasViewID && viewModel.HasViewID.Value && View.ViewID == viewModel.ViewID.Value;
            public BindingEntry(IView<TViewModel> view, TViewModel viewModel)
            {
                View = view;
                ViewModel = viewModel;
            }
        }
        /// <summary>
        /// 뷰모델과 뷰를 바인딩하는 리스트입니다.
        /// </summary>
        private List<BindingEntry> _bindings;

        /// <summary>
        /// 뷰모델을 지연 저장하기 위한 큐입니다. 뷰가 없을 때 뷰모델을 저장합니다.
        /// </summary>
        private List<TViewModel> _delayStroage;

        /// <summary>
        /// 뷰모델과 뷰를 바인딩하는 레지스트리입니다.
        /// </summary>
        /// <param name="view"></param>
        public static void Resister(IView<TViewModel> view)
        {

            if (Instance._bindings == null)
            {
                Instance._bindings = new();
            }


            int index = Instance._bindings.FindIndex(x => x.View == view);
            // 뷰모델과 뷰의 바인딩을 확인합니다.
            if (index < 0)
            {
                Instance._bindings.Add(new BindingEntry(view, null));
            }


            // 뷰모델이 지연 저장 큐에 있는지 확인합니다.
            if (Instance._delayStroage != null)
            {
                // 뷰모델이 이미 바인딩되어 있는지 확인합니다.
                if (Instance._delayStroage.Count <= 0)
                    return; // 지연 저장 큐가 비어있으면 아무 작업도 하지 않습니다.

                // 뷰모델이 ViewID를 가지고 있는지 확인합니다.
                if (view.HasViewID == true)
                {
                    // ViewID값이 존재하는 경우 해당 뷰모델을 꺼내서 뷰에 설정합니다.
                    for (int i = 0; i < Instance._delayStroage.Count; i++)
                    {
                        TViewModel viewModel = Instance._delayStroage[i];
                        // 뷰모델의 ViewID와 현재 뷰의 ViewID가 일치하는지 확인합니다.
                        // 뷰모델이 로드되지 않았거나, ViewID가 일치하는 경우에만 바인딩합니다.
                        if (viewModel.ViewID.Value == view.ViewID || viewModel.IsLoaded.Value == false)
                        {
                            // 뷰모델이 일치하는 경우, 해당 뷰모델을 뷰에 설정하고 지연 저장 리스트에서 제거합니다.
                            // 뷰모델이 로드되지 않은 경우에도 바인딩합니다.
                            view.OnSetViewModel(Instance._delayStroage[i]);
                            Instance._delayStroage.RemoveAt(i);

                            index = Instance._bindings.FindIndex(x => x.View == view);
                            // 바인딩 됬음을 표시합니다
                            Instance._bindings[index].ViewModel = index >= 0 ? viewModel : null;

                            viewModel.IsLoaded.Value = true; // 뷰모델이 로드되었음을 표시합니다.
                            return;
                        }
                    }
                }
                else
                {
                    // VIewID값이 Default인 경우, 지연 저장 리스트에서 첫번째 뷰모델을 꺼내서 뷰에 설정합니다.
                    TViewModel viewModel = Instance._delayStroage[0];
                    view.OnSetViewModel(viewModel);
                    Instance._delayStroage.RemoveAt(0);

                    // 바인딩 됬음을 표시합니다
                    Instance._bindings[index].ViewModel = viewModel;
                }

            }
        }

        /// <summary>
        /// 뷰모델과 뷰의 바인딩을 해제하는 레지스트리입니다.
        /// </summary>
        /// <param name="view"></param>
        public static void UnResister(IView<TViewModel> view)
        {
            // 뷰모델 해제
            int index = Instance._bindings.FindIndex(x => x.View == view);
            if (index >= 0)
            {
                Instance._bindings[index].ViewModel = null;
            }



        }

        /// <summary>
        /// 뷰모델을 뷰에 바인딩합니다. 뷰모델이 이미 바인딩되어 있으면 true를 반환하고, 새로운 뷰에 바인딩되면 true를 반환합니다.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static bool TryBind(TViewModel viewModel)
        {
            if (Instance._bindings == null || Instance._bindings.Count <= 0)
            {
                if (Instance._delayStroage == null)
                    Instance._delayStroage = new List<TViewModel>();
                // 뷰가 아직 없을 때 뷰모델을 지연 저장합니다.
                Instance._delayStroage.Add(viewModel);
                return false;
            }

            if (Instance._bindings.Any(x => x.ViewModel == viewModel))
                return true;// 이미 바인딩된 뷰모델이므로 true 반환

            IView<TViewModel> targetView = null;
            // 뷰모델에 바인딩된 뷰를 찾습니다.
            // 뷰모델이 로드되지 않은경우도 찾습니다
            if (viewModel.HasViewID.Value == false || viewModel.IsLoaded.Value == false)
            {
                // 뷰모델이 ViewID를 가지고 있지 않은 경우, 바인딩되지 않은 뷰를 찾습니다.
                targetView = Instance._bindings.FirstOrDefault(v => v.IsAvailable).View;
                viewModel.IsLoaded.Value = true; // 뷰모델이 로드되었음을 표시합니다.
            }
            else
            {
                // 뷰모델이 ViewID를 가지고 있는 경우, 해당 ViewID를 가진 뷰를 찾습니다.
                targetView = Instance._bindings.FirstOrDefault(
                    v =>
                      v.View.HasViewModel == false &&
                      v.View.HasViewID &&
                      v.View.ViewID == viewModel.ViewID.Value).View;
            }
            if (targetView == null) return false;

            // 뷰모델이 바인딩된 뷰를 찾았으므로, 해당 뷰에 뷰모델을 설정합니다.
            targetView.OnSetViewModel(viewModel);

            int index = Instance._bindings.FindIndex(x => x.View == targetView);
            // 이미 바인딩된 뷰가 있는 경우, 해당 뷰에 뷰모델을 다시 설정합니다.
            Instance._bindings[index].ViewModel = index >= 0 ? viewModel : null;


            return true;
        }

        /// <summary>
        /// 뷰모델을 뷰에 재바인딩합니다. 뷰모델이 바인딩되어 있으면 해당 뷰에 다시 바인딩하고 true를 반환합니다.
        /// 동적으로 뷰모델을 변경할 때 사용됩니다.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static bool TryRebind(TViewModel viewModel)
        {
            if (Instance._bindings == null)
                return false;

            if (viewModel.HasViewID.Value == false)
            {
                int index = Instance._bindings.FindIndex(v => v.IsAvailable);
                if (index >= 0)
                {
                    IView<TViewModel> targetView = Instance._bindings[index].View;
                    Instance._bindings[index].ViewModel = viewModel;
                    targetView.OnSetViewModel(viewModel);
                    return true;
                }
            }
            else
            {
                // 뷰중에서 찾기
                // 뷰모델에 뷰ID가 있는경우 알맞은 뷰에 매핑 
                int index = Instance._bindings.FindIndex(v => v.IsMahch(viewModel));
                if (index >= 0)
                {
                    IView<TViewModel> targetView = Instance._bindings[index].View;

                    // 원래 매핑되있던 뷰의 뷰모델 제거 후 바인딩 안된 리스트로옮깁니다.
                    int oldIndex = Instance._bindings.FindIndex(v => v.ViewModel == viewModel);
                    if (oldIndex >= 0)
                    {
                        // 이미 해당 View에 바인딩되어 있음. 추가 작업 불필요
                        if (Instance._bindings[oldIndex].View == targetView)
                            return true;
                        Instance._bindings[oldIndex].View.OnRemoveViewModel();
                        Instance._bindings[oldIndex].ViewModel = null;
                    }

                    targetView.OnRemoveViewModel(); // 기존 뷰모델을 제거합니다.
                    targetView.OnSetViewModel(viewModel);
                    Instance._bindings[index].ViewModel = viewModel;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 뷰모델 제거하여 바인딩
        /// </summary>
        public static bool TryRebind(IView<TViewModel> view)
        {
            if (Instance._bindings == null)
                return false;
            if (view == null)
                return false;

            // 바인딩 된 뷰 찾기
            int index = Instance._bindings.FindIndex(x => x.View == view);
            if (index >= 0) 
            {
                // 뷰모델 찾아서 지우고 null로 교체
                view.OnRemoveViewModel();
                Instance._bindings[index].ViewModel = null;
            }
            return true;
        }

        /// <summary>
        /// 뷰 끼리 교체하여 바인딩
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool TryRebind(IView<TViewModel> viewA, IView<TViewModel> viewB)
        {
            if (Instance._bindings == null)
                return false;
            if (viewA == null || viewB == null || viewA == viewB)
                return false;

            TViewModel viewModelA = null;
            TViewModel viewModelB = null;
            // 기존 뷰A에 있던 뷰모델 제거
            int AIndex = Instance._bindings.FindIndex(x => x.View == viewA);
            if (AIndex >= 0)
            {
                viewModelA = Instance._bindings[AIndex].ViewModel;
                viewA.OnRemoveViewModel();
            }
            // 기존 뷰B에 있던 뷰모델 제거
            int BIndex = Instance._bindings.FindIndex(x => x.View == viewB);
            if (BIndex >= 0)
            {
                viewModelB = Instance._bindings[BIndex].ViewModel;
                viewB.OnRemoveViewModel();
            }

            // 새로운 뷰모델 삽입
            viewA.OnSetViewModel(viewModelB);
            Instance._bindings[AIndex].ViewModel = viewModelB;

            viewB.OnSetViewModel(viewModelA);
            Instance._bindings[BIndex].ViewModel = viewModelA;
            return true;
        }

        /// <summary>
        /// 바인딩되지 않은 뷰를 제거합니다. 바인딩된 뷰가 없고, 뷰가 있는 경우에만 호출됩니다.
        /// </summary>
        public static void ClearUnUsed(Scene scene)
        {
            if (Instance._bindings != null) Instance._bindings.Clear();

            if (Instance._delayStroage != null) Instance._delayStroage.Clear();

        }
    }
}