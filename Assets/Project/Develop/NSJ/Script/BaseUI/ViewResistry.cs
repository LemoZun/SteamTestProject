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
        /// <summary>
        /// 뷰모델과 뷰를 바인딩하는 딕셔너리입니다.
        /// </summary>
        private Dictionary<IView<TViewModel>, TViewModel> _bindings;
        /// <summary>
        /// 뷰모델과 바인딩할 수 있는 뷰의 목록입니다.
        /// </summary>
        private List<IView<TViewModel>> _views;

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
                Instance._bindings = new Dictionary<IView<TViewModel>, TViewModel>();
            }

            if (Instance._views == null)
            {
                Instance._views = new List<IView<TViewModel>>();
            }

            // 뷰모델과 뷰의 바인딩을 확인합니다.
            if (Instance._views.Contains(view) == false)
                Instance._views.Add(view);

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
                            view.SetViewModel(Instance._delayStroage[i]);
                            Instance._delayStroage.RemoveAt(i);
                            viewModel.IsLoaded.Value = true; // 뷰모델이 로드되었음을 표시합니다.
                            return;
                        }
                    }
                }
                else
                {
                    // VIewID값이 Default인 경우, 지연 저장 리스트에서 첫번째 뷰모델을 꺼내서 뷰에 설정합니다.
                    view.SetViewModel(Instance._delayStroage[0]);
                    Instance._delayStroage.RemoveAt(0);
                }

            }
        }

        /// <summary>
        /// 뷰모델과 뷰의 바인딩을 해제하는 레지스트리입니다.
        /// </summary>
        /// <param name="view"></param>
        public static void UnResister(IView<TViewModel> view)
        {
            Instance._views.Remove(view);

            Instance._bindings[view] = null;

        }

        /// <summary>
        /// 뷰모델을 뷰에 바인딩합니다. 뷰모델이 이미 바인딩되어 있으면 true를 반환하고, 새로운 뷰에 바인딩되면 true를 반환합니다.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static bool TryBind(TViewModel viewModel)
        {
            if (Instance._bindings == null)
            {
                if (Instance._delayStroage == null)
                    Instance._delayStroage = new List<TViewModel>();
                // 뷰가 아직 없을 때 뷰모델을 지연 저장합니다.
                Instance._delayStroage.Add(viewModel);
                return false;
            }

            if (Instance._bindings.ContainsValue(viewModel))
                return true;// 이미 바인딩된 뷰모델이므로 true 반환

            IView<TViewModel> targetView = null;
            // 뷰모델에 바인딩된 뷰를 찾습니다.
            // 뷰모델이 로드되지 않은경우도 찾습니다
            if (viewModel.HasViewID.Value == false || viewModel.IsLoaded.Value == false)
            {
                // 뷰모델이 ViewID를 가지고 있지 않은 경우, 바인딩되지 않은 뷰를 찾습니다.
                targetView = Instance._views.FirstOrDefault(v => v.HasViewModel == false);
                viewModel.IsLoaded.Value = true; // 뷰모델이 로드되었음을 표시합니다.
            }
            else
            {
                // 뷰모델이 ViewID를 가지고 있는 경우, 해당 ViewID를 가진 뷰를 찾습니다.
                targetView = Instance._views.FirstOrDefault(v => v.HasViewModel == false && v.HasViewID && v.ViewID == viewModel.ViewID.Value);
            }
            if (targetView == null) return false;

            // 뷰모델이 바인딩된 뷰를 찾았으므로, 해당 뷰에 뷰모델을 설정합니다.
            targetView.SetViewModel(viewModel);
            if (Instance._bindings.ContainsKey(targetView))
            {
                // 이미 바인딩된 뷰가 있는 경우, 해당 뷰에 뷰모델을 다시 설정합니다.
                Instance._bindings[targetView] = viewModel;
            }
            else
            {
                // 뷰모델과 뷰의 바인딩을 추가합니다.
                Instance._bindings.Add(targetView, viewModel);
            }

            Instance._views.Remove(targetView);

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

            KeyValuePair<IView<TViewModel>, TViewModel> targetEntry = Instance._bindings.FirstOrDefault(x => x.Value.Equals(viewModel));

            if (targetEntry.Equals(default(KeyValuePair<IView<TViewModel>, TViewModel>)))
                return false;

            targetEntry.Key.RemoveViewModel(); // 기존 뷰모델을 제거합니다.
            targetEntry.Key.SetViewModel(viewModel);

            Instance._bindings[targetEntry.Key] = viewModel;

            return true;
        }

        /// <summary>
        /// 바인딩되지 않은 뷰를 제거합니다. 바인딩된 뷰가 없고, 뷰가 있는 경우에만 호출됩니다.
        /// </summary>
        public static void ClearUnUsed(Scene scene)
        {
            Instance._views = null;
            Instance._bindings = null;
            Instance._delayStroage = null;
        }
    }
}