using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSJ_MVVM
{
    public class ViewResistry<TView> where TView : IView
    {
        private static ViewResistry<TView> _instance;
        public static ViewResistry<TView> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ViewResistry<TView>();
                    SceneManager.sceneUnloaded += ClearUnUsed;
                }
                return _instance;
            }
        }

        class BindingEntry
        {
            public bool HasViewID;
            public int ViewID;
            public List<IView> Views = new List<IView>();
            public IViewModel ViewModel;

            public bool IsAvailable => ViewModel == null;
            public bool IsMahch(IViewModel viewModel) =>
                Views[0].HasViewID && viewModel.HasViewID.Value && Views[0].ViewID == viewModel.ViewID.Value;
            public BindingEntry(IView view, IViewModel viewModel)
            {
                Views.Add(view);
                ViewModel = viewModel;
                ViewID = view.ViewID;
                HasViewID = view.HasViewID;
            }
        }
        /// <summary>
        /// 뷰모델과 뷰를 바인딩하는 리스트입니다.
        /// </summary>
        private List<BindingEntry> _bindings;

        /// <summary>
        /// 뷰모델을 지연 저장하기 위한 큐입니다. 뷰가 없을 때 뷰모델을 저장합니다.
        /// </summary>
        private List<IViewModel> _viewModelStroage;

        /// <summary>
        /// 뷰모델과 뷰를 바인딩하는 레지스트리입니다.
        /// </summary>
        /// <param name="view"></param>
        public static void Resister(TView view)
        {

            if (Instance._bindings == null)
            {
                Instance._bindings = new();
            }

            // 동일 뷰ID 뷰리스트에 본인을 추가합니다
            AddViewToBinding(view);

            int index = 0;


            // 뷰모델이 저장 리스트에 있는지 확인합니다.
            if (Instance._viewModelStroage != null)
            {

                // 뷰모델이 이미 바인딩되어 있는지 확인합니다.
                if (Instance._viewModelStroage.Count <= 0)
                    return; // 지연 저장 큐가 비어있으면 아무 작업도 하지 않습니다.

                // 뷰모델이 ViewID를 가지고 있는지 확인합니다.
                if (view.HasViewID == true)
                {
                    IViewModel viewModel = null;
                    // ViewID값이 존재하는 경우 해당 뷰모델을 꺼내서 뷰에 설정합니다.
                    for (int i = 0; i < Instance._viewModelStroage.Count; i++)
                    {
                        viewModel = Instance._viewModelStroage[i];
                        // 뷰모델의 ViewID와 현재 뷰의 ViewID가 일치하는지 확인합니다.
                        // 뷰모델이 로드되지 않았거나, ViewID가 일치하는 경우에만 바인딩합니다.
                        if (viewModel.ViewID.Value == view.ViewID || viewModel.IsLoaded.Value == false)
                        {

                            // 뷰모델이 일치하는 경우, 해당 뷰모델을 뷰에 설정하고 지연 저장 리스트에서 제거합니다.
                            // 뷰모델이 로드되지 않은 경우에도 바인딩합니다.
                            view.OnSetViewModel(viewModel);

                            index = Instance._bindings.FindIndex(x => x.ViewID == view.ViewID);
                            // 바인딩 됬음을 표시합니다
                            Instance._bindings[index].ViewModel = viewModel;

                            viewModel.IsLoaded.Value = true; // 뷰모델이 로드되었음을 표시합니다.

                            break;
                        }
                    }
                    // 찾은 뷰모델이 없을 때 
                    if (viewModel == null)
                    {
                        index = Instance._viewModelStroage.FindIndex(vm => vm.IsLoaded.Value == false);
                        if (index >= 0)
                        {
                            viewModel = Instance._viewModelStroage[index];
                            view.OnSetViewModel(viewModel);
                            Instance._bindings[index].ViewModel = viewModel;
                            viewModel.IsLoaded.Value = true; // 뷰모델이 로드되었음을 표시합니다.
                        }
                    }
                    return;

                }
                else
                {
                    // VIewID값이 Default인 경우, 지연 저장 리스트에서 첫번째 뷰모델을 꺼내서 뷰에 설정합니다.
                    IViewModel viewModel = Instance._viewModelStroage[0];
                    view.OnSetViewModel(viewModel);
                    // 바인딩 됬음을 표시합니다
                    Instance._bindings[index].ViewModel = viewModel;

                }

            }
        }

        /// <summary>
        /// 뷰의 삭제시에 바인딩을 해제하는 레지스트리입니다.
        /// </summary>
        /// <param name="view"></param>
        public static void UnResister(IView view)
        {
            view.OnRemoveViewModel();
            // 뷰모델 해제
            RemoveViewFromBinding(view);
        }

        /// <summary>
        /// 뷰모델을 뷰에 바인딩합니다. 뷰모델이 이미 바인딩되어 있으면 true를 반환하고, 새로운 뷰에 바인딩되면 true를 반환합니다.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static bool TryBind(IViewModel viewModel)
        {
            if (Instance._viewModelStroage == null)
                Instance._viewModelStroage = new List<IViewModel>();

            if (Instance._viewModelStroage.Contains(viewModel) == false)
            {
                Instance._viewModelStroage.Add(viewModel);
            }

            if (Instance._bindings == null || Instance._bindings.FirstOrDefault(v => v.ViewModel == null) == null)
            {
                return false;
            }

            if (Instance._bindings.Any(x => x.ViewModel == viewModel))
                return true;// 이미 바인딩된 뷰모델이므로 true 반환

            List<IView> targetViews = null;
            // 뷰모델에 바인딩된 뷰를 찾습니다.
            // 뷰모델이 로드되지 않은경우도 찾습니다
            if (viewModel.HasViewID.Value == false || viewModel.IsLoaded.Value == false)
            {
                // 뷰모델이 ViewID를 가지고 있지 않은 경우, 바인딩되지 않은 뷰를 찾습니다.
                targetViews = Instance._bindings.FirstOrDefault(v => v.IsAvailable).Views;
                viewModel.IsLoaded.Value = true; // 뷰모델이 로드되었음을 표시합니다.
            }
            else
            {
                // 뷰모델이 ViewID를 가지고 있는 경우, 해당 ViewID를 가진 뷰를 찾습니다.
                targetViews = Instance._bindings.FirstOrDefault(
                    v =>
                      v.HasViewID &&
                      v.ViewID == viewModel.ViewID.Value).Views;
            }
            if (targetViews == null) return false;

            // 뷰모델이 바인딩된 뷰를 찾았으므로, 해당 뷰에 뷰모델을 설정합니다.
            SetViewModelToViews(targetViews, viewModel);

            int index = Instance._bindings.FindIndex(x => x.Views == targetViews);
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
        public static bool TryRebind(IViewModel viewModel)
        {
            if (Instance._bindings == null)
                return false;

            // 로드되지않았거나 뷰ID가 없는경우
            if (viewModel.HasViewID.Value == false || viewModel.IsLoaded.Value == false)
            {

                // 이미 매핑된 경우
                int index = Instance._bindings.FindIndex(v => v.ViewModel == viewModel);
                if (index >= 0)
                {
                    List<IView> targetViews = Instance._bindings[index].Views;
                    SetViewModelToViews(targetViews, viewModel);
                    return true;
                }

                // 뷰모델이 빈곳이 있는 경우
                index = Instance._bindings.FindIndex(v => v.IsAvailable);
                if (index >= 0)
                {
                    List<IView> targetViews = Instance._bindings[index].Views;
                    SetViewModelToViews(targetViews, viewModel);
                    Instance._bindings[index].ViewModel = viewModel;
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
                    List<IView> targetViews = Instance._bindings[index].Views;

                    // 원래 매핑되있던 뷰의 뷰모델 제거 후 바인딩 안된 리스트로옮깁니다.
                    int oldIndex = Instance._bindings.FindIndex(v => v.ViewModel == viewModel);
                    if (oldIndex >= 0)
                    {
                        // 이미 해당 View에 바인딩되어 있음. 추가 작업 불필요
                        if (Instance._bindings[oldIndex].Views == targetViews)
                            return true;

                        RemoveViewModelToViews(Instance._bindings[oldIndex].Views);
                        Instance._bindings[oldIndex].ViewModel = null;
                    }

                    RemoveViewModelToViews(targetViews);
                    SetViewModelToViews(targetViews, viewModel);

                    Instance._bindings[index].ViewModel = viewModel;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 뷰모델 제거하여 바인딩
        /// </summary>
        public static bool RemoveRebind(IView view)
        {
            if (Instance._bindings == null)
                return false;
            if (view == null)
                return false;

            // 바인딩 된 뷰 찾기
            int index = Instance._bindings.FindIndex(x => x.ViewID == view.ViewID);
            if (index >= 0)
            {
                // 뷰모델 찾아서 지우고 null로 교체
                view.OnRemoveViewModel();
                // 동일 뷰ID 뷰리스트에 본인을 추가합니다
                RemoveViewFromBinding(view);
            }
            return true;
        }

        /// <summary>
        /// 뷰 끼리 교체하여 바인딩
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool ExchangeRebind(IView viewA, IView viewB)
        {
            if (Instance._bindings == null)
                return false;
            if (viewA == null || viewB == null || viewA == viewB)
                return false;

            IViewModel viewModelA = null;
            IViewModel viewModelB = null;
            // 기존 뷰A에 있던 뷰모델 제거
            int AIndex = Instance._bindings.FindIndex(x => x.ViewID == viewA.ViewID);
            if (AIndex >= 0)
            {
                viewModelA = Instance._bindings[AIndex].ViewModel;
                viewA.OnRemoveViewModel();
                RemoveViewFromBinding(viewA);
            }
            // 기존 뷰B에 있던 뷰모델 제거
            int BIndex = Instance._bindings.FindIndex(x => x.ViewID == viewB.ViewID);
            if (BIndex >= 0)
            {
                viewModelB = Instance._bindings[BIndex].ViewModel;
                viewB.OnRemoveViewModel();
                RemoveViewFromBinding(viewB);
            }

            // 새로운 뷰모델 삽입
            viewA.OnSetViewModel(viewModelB);
            AddViewToBinding(viewA);

            viewB.OnSetViewModel(viewModelA);
            AddViewToBinding(viewB);
            return true;
        }

        /// <summary>
        /// 바인딩되지 않은 뷰를 제거합니다. 바인딩된 뷰가 없고, 뷰가 있는 경우에만 호출됩니다.
        /// </summary>
        public static void ClearUnUsed(Scene scene)
        {
            if (Instance._bindings != null) Instance._bindings.Clear();

            if (Instance._viewModelStroage != null) Instance._viewModelStroage.Clear();

        }

        private static void AddViewToBinding(IView targetView)
        {

            int index = Instance._bindings.FindIndex(v => v.ViewID == targetView.ViewID);
            if (index >= 0)
            {
                Instance._bindings[index].Views.Add(targetView);
            }
            else
            {
                Instance._bindings.Add(new BindingEntry(targetView, null));
            }
        }
        private static void RemoveViewFromBinding(IView targetView)
        {
            // 뷰모델 해제
            int index = Instance._bindings.FindIndex(x => x.ViewID == targetView.ViewID);
            if (index >= 0)
            {
                List<IView> views = Instance._bindings[index].Views;

                index = views.FindIndex(x => x == targetView);
                if (index >= 0)
                {
                    views.Remove(targetView);
                }
            }
        }

        private static void SetViewModelToViews(List<IView> views,IViewModel viewModel)
        {
            foreach (IView view in views)
            {
                view.OnSetViewModel(viewModel);
            }
            viewModel.IsLoaded.Value = true;
        }
        private static void RemoveViewModelToViews(List<IView> views)
        {
            foreach (IView view in views)
            {
                view.OnRemoveViewModel();
            }
        }
    }
}