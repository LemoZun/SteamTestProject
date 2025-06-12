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
        /// ��𵨰� �並 ���ε��ϴ� ��ųʸ��Դϴ�.
        /// </summary>
        private Dictionary<IView<TViewModel>, TViewModel> _bindings;
        /// <summary>
        /// ��𵨰� ���ε��� �� �ִ� ���� ����Դϴ�.
        /// </summary>
        private List<IView<TViewModel>> _views;

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
                Instance._bindings = new Dictionary<IView<TViewModel>, TViewModel>();
            }

            if (Instance._views == null)
            {
                Instance._views = new List<IView<TViewModel>>();
            }

            // ��𵨰� ���� ���ε��� Ȯ���մϴ�.
            if (Instance._views.Contains(view) == false)
                Instance._views.Add(view);

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
                            view.SetViewModel(Instance._delayStroage[i]);
                            Instance._delayStroage.RemoveAt(i);
                            viewModel.IsLoaded.Value = true; // ����� �ε�Ǿ����� ǥ���մϴ�.
                            return;
                        }
                    }
                }
                else
                {
                    // VIewID���� Default�� ���, ���� ���� ����Ʈ���� ù��° ����� ������ �信 �����մϴ�.
                    view.SetViewModel(Instance._delayStroage[0]);
                    Instance._delayStroage.RemoveAt(0);
                }

            }
        }

        /// <summary>
        /// ��𵨰� ���� ���ε��� �����ϴ� ������Ʈ���Դϴ�.
        /// </summary>
        /// <param name="view"></param>
        public static void UnResister(IView<TViewModel> view)
        {
            Instance._views.Remove(view);

            Instance._bindings[view] = null;

        }

        /// <summary>
        /// ����� �信 ���ε��մϴ�. ����� �̹� ���ε��Ǿ� ������ true�� ��ȯ�ϰ�, ���ο� �信 ���ε��Ǹ� true�� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static bool TryBind(TViewModel viewModel)
        {
            if (Instance._bindings == null)
            {
                if (Instance._delayStroage == null)
                    Instance._delayStroage = new List<TViewModel>();
                // �䰡 ���� ���� �� ����� ���� �����մϴ�.
                Instance._delayStroage.Add(viewModel);
                return false;
            }

            if (Instance._bindings.ContainsValue(viewModel))
                return true;// �̹� ���ε��� ����̹Ƿ� true ��ȯ

            IView<TViewModel> targetView = null;
            // ��𵨿� ���ε��� �並 ã���ϴ�.
            // ����� �ε���� ������쵵 ã���ϴ�
            if (viewModel.HasViewID.Value == false || viewModel.IsLoaded.Value == false)
            {
                // ����� ViewID�� ������ ���� ���� ���, ���ε����� ���� �並 ã���ϴ�.
                targetView = Instance._views.FirstOrDefault(v => v.HasViewModel == false);
                viewModel.IsLoaded.Value = true; // ����� �ε�Ǿ����� ǥ���մϴ�.
            }
            else
            {
                // ����� ViewID�� ������ �ִ� ���, �ش� ViewID�� ���� �並 ã���ϴ�.
                targetView = Instance._views.FirstOrDefault(v => v.HasViewModel == false && v.HasViewID && v.ViewID == viewModel.ViewID.Value);
            }
            if (targetView == null) return false;

            // ����� ���ε��� �並 ã�����Ƿ�, �ش� �信 ����� �����մϴ�.
            targetView.SetViewModel(viewModel);
            if (Instance._bindings.ContainsKey(targetView))
            {
                // �̹� ���ε��� �䰡 �ִ� ���, �ش� �信 ����� �ٽ� �����մϴ�.
                Instance._bindings[targetView] = viewModel;
            }
            else
            {
                // ��𵨰� ���� ���ε��� �߰��մϴ�.
                Instance._bindings.Add(targetView, viewModel);
            }

            Instance._views.Remove(targetView);

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

            KeyValuePair<IView<TViewModel>, TViewModel> targetEntry = Instance._bindings.FirstOrDefault(x => x.Value.Equals(viewModel));

            if (targetEntry.Equals(default(KeyValuePair<IView<TViewModel>, TViewModel>)))
                return false;

            targetEntry.Key.RemoveViewModel(); // ���� ����� �����մϴ�.
            targetEntry.Key.SetViewModel(viewModel);

            Instance._bindings[targetEntry.Key] = viewModel;

            return true;
        }

        /// <summary>
        /// ���ε����� ���� �並 �����մϴ�. ���ε��� �䰡 ����, �䰡 �ִ� ��쿡�� ȣ��˴ϴ�.
        /// </summary>
        public static void ClearUnUsed(Scene scene)
        {
            Instance._views = null;
            Instance._bindings = null;
            Instance._delayStroage = null;
        }
    }
}