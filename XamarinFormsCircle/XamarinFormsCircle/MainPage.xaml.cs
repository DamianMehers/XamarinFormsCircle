using Xamarin.Forms;
using XamarinFormsCircle.ViewModels;

namespace XamarinFormsCircle
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModelLocator.Main.Refresh = DoAnimation;
        }

        private void DoAnimation()
        {
            var animation = new Animation(v => TheCircle.Radius = (float)v);
            animation.Commit(this, "SimpleAnimation");
        }
    }
}
