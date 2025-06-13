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
        /// ��𵨰� �並 ���ε��ϴ� ����Ʈ�Դϴ�.
        /// </summary>
        private List<BindingEntry> _bindings;

        /// <summary>
        /// ����� ���� �����ϱ� ���� ť�Դϴ�. �䰡 ���� �� ����� �����մϴ�.
        /// </summary>
        private List<TViewModel> _delayStroage;

        /// <summary>
        /// ��𵨰� �並 ���ε��ϴ� ������Ʈ���Դϴ�.
        /// </summary>
        /// <param name="view"></param>
        public static void Resister(IView<TViewModel> view)
        {

            if (Instance._bindings == null)
            {
                Instance._bindings = new();
            }


            int index = Instance._bindings.FindIndex(x => x.View == view);
            // ��𵨰� ���� ���ε��� Ȯ���մϴ�.
            if (index < 0)
            {
                Instance._bindings.Add(new BindingEntry(view, null));
            }


            // ����� ���� ���� ť�� �ִ��� Ȯ���մϴ�.
            if (Instance._delayStroage != null)
            {
                // ����� �̹� ���ε��Ǿ� �ִ��� Ȯ���մϴ�.
                if (Instance._delayStroage.Count <= 0)
                    return; // ���� ���� ť�� ��������� �ƹ� �۾��� ���� �ʽ��ϴ�.

                // ����� ViewID�� ������ �ִ��� Ȯ���մϴ�.
                if (view.HasViewID == true)
                {
                    // ViewID���� �����ϴ� ��� �ش� ����� ������ �信 �����մϴ�.
                    for (int i = 0; i < Instance._delayStroage.Count; i++)
                    {
                        TViewModel viewModel = Instance._delayStroage[i];
                        // ����� ViewID�� ���� ���� ViewID�� ��ġ�ϴ��� Ȯ���մϴ�.
                        // ����� �ε���� �ʾҰų�, ViewID�� ��ġ�ϴ� ��쿡�� ���ε��մϴ�.
                        if (viewModel.ViewID.Value == view.ViewID || viewModel.IsLoaded.Value == false)
                        {
                            // ����� ��ġ�ϴ� ���, �ش� ����� �信 �����ϰ� ���� ���� ����Ʈ���� �����մϴ�.
                            // ����� �ε���� ���� ��쿡�� ���ε��մϴ�.
                            view.OnSetViewModel(Instance._delayStroage[i]);
                            Instance._delayStroage.RemoveAt(i);

                            index = Instance._bindings.FindIndex(x => x.View == view);
                            // ���ε� ������ ǥ���մϴ�
                            Instance._bindings[index].ViewModel = index >= 0 ? viewModel : null;

                            viewModel.IsLoaded.Value = true; // ����� �ε�Ǿ����� ǥ���մϴ�.
                            return;
                        }
                    }
                }
                else
                {
                    // VIewID���� Default�� ���, ���� ���� ����Ʈ���� ù��° ����� ������ �信 �����մϴ�.
                    TViewModel viewModel = Instance._delayStroage[0];
                    view.OnSetViewModel(viewModel);
                    Instance._delayStroage.RemoveAt(0);

                    // ���ε� ������ ǥ���մϴ�
                    Instance._bindings[index].ViewModel = viewModel;
                }

            }
        }

        /// <summary>
        /// ��𵨰� ���� ���ε��� �����ϴ� ������Ʈ���Դϴ�.
        /// </summary>
        /// <param name="view"></param>
        public static void UnResister(IView<TViewModel> view)
        {
            // ��� ����
            int index = Instance._bindings.FindIndex(x => x.View == view);
            if (index >= 0)
            {
                Instance._bindings[index].ViewModel = null;
            }



        }

        /// <summary>
        /// ����� �信 ���ε��մϴ�. ����� �̹� ���ε��Ǿ� ������ true�� ��ȯ�ϰ�, ���ο� �信 ���ε��Ǹ� true�� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static bool TryBind(TViewModel viewModel)
        {
            if (Instance._bindings == null || Instance._bindings.Count <= 0)
            {
                if (Instance._delayStroage == null)
                    Instance._delayStroage = new List<TViewModel>();
                // �䰡 ���� ���� �� ����� ���� �����մϴ�.
                Instance._delayStroage.Add(viewModel);
                return false;
            }

            if (Instance._bindings.Any(x => x.ViewModel == viewModel))
                return true;// �̹� ���ε��� ����̹Ƿ� true ��ȯ

            IView<TViewModel> targetView = null;
            // ��𵨿� ���ε��� �並 ã���ϴ�.
            // ����� �ε���� ������쵵 ã���ϴ�
            if (viewModel.HasViewID.Value == false || viewModel.IsLoaded.Value == false)
            {
                // ����� ViewID�� ������ ���� ���� ���, ���ε����� ���� �並 ã���ϴ�.
                targetView = Instance._bindings.FirstOrDefault(v => v.IsAvailable).View;
                viewModel.IsLoaded.Value = true; // ����� �ε�Ǿ����� ǥ���մϴ�.
            }
            else
            {
                // ����� ViewID�� ������ �ִ� ���, �ش� ViewID�� ���� �並 ã���ϴ�.
                targetView = Instance._bindings.FirstOrDefault(
                    v =>
                      v.View.HasViewModel == false &&
                      v.View.HasViewID &&
                      v.View.ViewID == viewModel.ViewID.Value).View;
            }
            if (targetView == null) return false;

            // ����� ���ε��� �並 ã�����Ƿ�, �ش� �信 ����� �����մϴ�.
            targetView.OnSetViewModel(viewModel);

            int index = Instance._bindings.FindIndex(x => x.View == targetView);
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
                // ���߿��� ã��
                // ��𵨿� ��ID�� �ִ°�� �˸��� �信 ���� 
                int index = Instance._bindings.FindIndex(v => v.IsMahch(viewModel));
                if (index >= 0)
                {
                    IView<TViewModel> targetView = Instance._bindings[index].View;

                    // ���� ���ε��ִ� ���� ��� ���� �� ���ε� �ȵ� ����Ʈ�οű�ϴ�.
                    int oldIndex = Instance._bindings.FindIndex(v => v.ViewModel == viewModel);
                    if (oldIndex >= 0)
                    {
                        // �̹� �ش� View�� ���ε��Ǿ� ����. �߰� �۾� ���ʿ�
                        if (Instance._bindings[oldIndex].View == targetView)
                            return true;
                        Instance._bindings[oldIndex].View.OnRemoveViewModel();
                        Instance._bindings[oldIndex].ViewModel = null;
                    }

                    targetView.OnRemoveViewModel(); // ���� ����� �����մϴ�.
                    targetView.OnSetViewModel(viewModel);
                    Instance._bindings[index].ViewModel = viewModel;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��� �����Ͽ� ���ε�
        /// </summary>
        public static bool TryRebind(IView<TViewModel> view)
        {
            if (Instance._bindings == null)
                return false;
            if (view == null)
                return false;

            // ���ε� �� �� ã��
            int index = Instance._bindings.FindIndex(x => x.View == view);
            if (index >= 0) 
            {
                // ��� ã�Ƽ� ����� null�� ��ü
                view.OnRemoveViewModel();
                Instance._bindings[index].ViewModel = null;
            }
            return true;
        }

        /// <summary>
        /// �� ���� ��ü�Ͽ� ���ε�
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
            // ���� ��A�� �ִ� ��� ����
            int AIndex = Instance._bindings.FindIndex(x => x.View == viewA);
            if (AIndex >= 0)
            {
                viewModelA = Instance._bindings[AIndex].ViewModel;
                viewA.OnRemoveViewModel();
            }
            // ���� ��B�� �ִ� ��� ����
            int BIndex = Instance._bindings.FindIndex(x => x.View == viewB);
            if (BIndex >= 0)
            {
                viewModelB = Instance._bindings[BIndex].ViewModel;
                viewB.OnRemoveViewModel();
            }

            // ���ο� ��� ����
            viewA.OnSetViewModel(viewModelB);
            Instance._bindings[AIndex].ViewModel = viewModelB;

            viewB.OnSetViewModel(viewModelA);
            Instance._bindings[BIndex].ViewModel = viewModelA;
            return true;
        }

        /// <summary>
        /// ���ε����� ���� �並 �����մϴ�. ���ε��� �䰡 ����, �䰡 �ִ� ��쿡�� ȣ��˴ϴ�.
        /// </summary>
        public static void ClearUnUsed(Scene scene)
        {
            if (Instance._bindings != null) Instance._bindings.Clear();

            if (Instance._delayStroage != null) Instance._delayStroage.Clear();

        }
    }
}