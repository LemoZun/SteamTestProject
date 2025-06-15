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
        /// ��𵨰� �並 ���ε��ϴ� ����Ʈ�Դϴ�.
        /// </summary>
        private List<BindingEntry> _bindings;

        /// <summary>
        /// ����� ���� �����ϱ� ���� ť�Դϴ�. �䰡 ���� �� ����� �����մϴ�.
        /// </summary>
        private List<IViewModel> _viewModelStroage;

        /// <summary>
        /// ��𵨰� �並 ���ε��ϴ� ������Ʈ���Դϴ�.
        /// </summary>
        /// <param name="view"></param>
        public static void Resister(TView view)
        {

            if (Instance._bindings == null)
            {
                Instance._bindings = new();
            }

            // ���� ��ID �丮��Ʈ�� ������ �߰��մϴ�
            AddViewToBinding(view);

            int index = 0;


            // ����� ���� ����Ʈ�� �ִ��� Ȯ���մϴ�.
            if (Instance._viewModelStroage != null)
            {

                // ����� �̹� ���ε��Ǿ� �ִ��� Ȯ���մϴ�.
                if (Instance._viewModelStroage.Count <= 0)
                    return; // ���� ���� ť�� ��������� �ƹ� �۾��� ���� �ʽ��ϴ�.

                // ����� ViewID�� ������ �ִ��� Ȯ���մϴ�.
                if (view.HasViewID == true)
                {
                    IViewModel viewModel = null;
                    // ViewID���� �����ϴ� ��� �ش� ����� ������ �信 �����մϴ�.
                    for (int i = 0; i < Instance._viewModelStroage.Count; i++)
                    {
                        viewModel = Instance._viewModelStroage[i];
                        // ����� ViewID�� ���� ���� ViewID�� ��ġ�ϴ��� Ȯ���մϴ�.
                        // ����� �ε���� �ʾҰų�, ViewID�� ��ġ�ϴ� ��쿡�� ���ε��մϴ�.
                        if (viewModel.ViewID.Value == view.ViewID || viewModel.IsLoaded.Value == false)
                        {

                            // ����� ��ġ�ϴ� ���, �ش� ����� �信 �����ϰ� ���� ���� ����Ʈ���� �����մϴ�.
                            // ����� �ε���� ���� ��쿡�� ���ε��մϴ�.
                            view.OnSetViewModel(viewModel);

                            index = Instance._bindings.FindIndex(x => x.ViewID == view.ViewID);
                            // ���ε� ������ ǥ���մϴ�
                            Instance._bindings[index].ViewModel = viewModel;

                            viewModel.IsLoaded.Value = true; // ����� �ε�Ǿ����� ǥ���մϴ�.

                            break;
                        }
                    }
                    // ã�� ����� ���� �� 
                    if (viewModel == null)
                    {
                        index = Instance._viewModelStroage.FindIndex(vm => vm.IsLoaded.Value == false);
                        if (index >= 0)
                        {
                            viewModel = Instance._viewModelStroage[index];
                            view.OnSetViewModel(viewModel);
                            Instance._bindings[index].ViewModel = viewModel;
                            viewModel.IsLoaded.Value = true; // ����� �ε�Ǿ����� ǥ���մϴ�.
                        }
                    }
                    return;

                }
                else
                {
                    // VIewID���� Default�� ���, ���� ���� ����Ʈ���� ù��° ����� ������ �信 �����մϴ�.
                    IViewModel viewModel = Instance._viewModelStroage[0];
                    view.OnSetViewModel(viewModel);
                    // ���ε� ������ ǥ���մϴ�
                    Instance._bindings[index].ViewModel = viewModel;

                }

            }
        }

        /// <summary>
        /// ���� �����ÿ� ���ε��� �����ϴ� ������Ʈ���Դϴ�.
        /// </summary>
        /// <param name="view"></param>
        public static void UnResister(IView view)
        {
            view.OnRemoveViewModel();
            // ��� ����
            RemoveViewFromBinding(view);
        }

        /// <summary>
        /// ����� �信 ���ε��մϴ�. ����� �̹� ���ε��Ǿ� ������ true�� ��ȯ�ϰ�, ���ο� �信 ���ε��Ǹ� true�� ��ȯ�մϴ�.
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
                return true;// �̹� ���ε��� ����̹Ƿ� true ��ȯ

            List<IView> targetViews = null;
            // ��𵨿� ���ε��� �並 ã���ϴ�.
            // ����� �ε���� ������쵵 ã���ϴ�
            if (viewModel.HasViewID.Value == false || viewModel.IsLoaded.Value == false)
            {
                // ����� ViewID�� ������ ���� ���� ���, ���ε����� ���� �並 ã���ϴ�.
                targetViews = Instance._bindings.FirstOrDefault(v => v.IsAvailable).Views;
                viewModel.IsLoaded.Value = true; // ����� �ε�Ǿ����� ǥ���մϴ�.
            }
            else
            {
                // ����� ViewID�� ������ �ִ� ���, �ش� ViewID�� ���� �並 ã���ϴ�.
                targetViews = Instance._bindings.FirstOrDefault(
                    v =>
                      v.HasViewID &&
                      v.ViewID == viewModel.ViewID.Value).Views;
            }
            if (targetViews == null) return false;

            // ����� ���ε��� �並 ã�����Ƿ�, �ش� �信 ����� �����մϴ�.
            SetViewModelToViews(targetViews, viewModel);

            int index = Instance._bindings.FindIndex(x => x.Views == targetViews);
            // �̹� ���ε��� �䰡 �ִ� ���, �ش� �信 ����� �ٽ� �����մϴ�.
            Instance._bindings[index].ViewModel = index >= 0 ? viewModel : null;

            return true;
        }

        /// <summary>
        /// ����� �信 ����ε��մϴ�. ����� ���ε��Ǿ� ������ �ش� �信 �ٽ� ���ε��ϰ� true�� ��ȯ�մϴ�.
        /// �������� ����� ������ �� ���˴ϴ�.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static bool TryRebind(IViewModel viewModel)
        {
            if (Instance._bindings == null)
                return false;

            // �ε�����ʾҰų� ��ID�� ���°��
            if (viewModel.HasViewID.Value == false || viewModel.IsLoaded.Value == false)
            {

                // �̹� ���ε� ���
                int index = Instance._bindings.FindIndex(v => v.ViewModel == viewModel);
                if (index >= 0)
                {
                    List<IView> targetViews = Instance._bindings[index].Views;
                    SetViewModelToViews(targetViews, viewModel);
                    return true;
                }

                // ����� ����� �ִ� ���
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
                // ���߿��� ã��
                // ��𵨿� ��ID�� �ִ°�� �˸��� �信 ���� 
                int index = Instance._bindings.FindIndex(v => v.IsMahch(viewModel));
                if (index >= 0)
                {
                    List<IView> targetViews = Instance._bindings[index].Views;

                    // ���� ���ε��ִ� ���� ��� ���� �� ���ε� �ȵ� ����Ʈ�οű�ϴ�.
                    int oldIndex = Instance._bindings.FindIndex(v => v.ViewModel == viewModel);
                    if (oldIndex >= 0)
                    {
                        // �̹� �ش� View�� ���ε��Ǿ� ����. �߰� �۾� ���ʿ�
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
        /// ��� �����Ͽ� ���ε�
        /// </summary>
        public static bool RemoveRebind(IView view)
        {
            if (Instance._bindings == null)
                return false;
            if (view == null)
                return false;

            // ���ε� �� �� ã��
            int index = Instance._bindings.FindIndex(x => x.ViewID == view.ViewID);
            if (index >= 0)
            {
                // ��� ã�Ƽ� ����� null�� ��ü
                view.OnRemoveViewModel();
                // ���� ��ID �丮��Ʈ�� ������ �߰��մϴ�
                RemoveViewFromBinding(view);
            }
            return true;
        }

        /// <summary>
        /// �� ���� ��ü�Ͽ� ���ε�
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
            // ���� ��A�� �ִ� ��� ����
            int AIndex = Instance._bindings.FindIndex(x => x.ViewID == viewA.ViewID);
            if (AIndex >= 0)
            {
                viewModelA = Instance._bindings[AIndex].ViewModel;
                viewA.OnRemoveViewModel();
                RemoveViewFromBinding(viewA);
            }
            // ���� ��B�� �ִ� ��� ����
            int BIndex = Instance._bindings.FindIndex(x => x.ViewID == viewB.ViewID);
            if (BIndex >= 0)
            {
                viewModelB = Instance._bindings[BIndex].ViewModel;
                viewB.OnRemoveViewModel();
                RemoveViewFromBinding(viewB);
            }

            // ���ο� ��� ����
            viewA.OnSetViewModel(viewModelB);
            AddViewToBinding(viewA);

            viewB.OnSetViewModel(viewModelA);
            AddViewToBinding(viewB);
            return true;
        }

        /// <summary>
        /// ���ε����� ���� �並 �����մϴ�. ���ε��� �䰡 ����, �䰡 �ִ� ��쿡�� ȣ��˴ϴ�.
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
            // ��� ����
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