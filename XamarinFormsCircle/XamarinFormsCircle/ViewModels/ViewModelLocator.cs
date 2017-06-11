namespace XamarinFormsCircle.ViewModels
{
    public static class ViewModelLocator
    {
        private static MainViewModel _main;

        public static MainViewModel Main => _main ?? (_main = new MainViewModel());
    }
}
