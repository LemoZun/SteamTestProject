namespace NSJ_MVVM
{
    public interface IView<T>
    {
        bool HasViewID { get; set; }
        int ViewID { get; set; }

        bool HasViewModel { get; set; }

        void SetViewModel(T viewModel);
    }
}