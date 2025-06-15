namespace NSJ_MVVM
{
    public interface IView
    {
        bool HasViewID { get; set; }
        int ViewID { get; set; }

        bool HasViewModel { get; set; }

        void OnSetViewModel(IViewModel viewModel);
        void OnRemoveViewModel();
    }

    public interface IView<T> : IView
    {
        void OnSetViewModel(T viewModel);
    }
}