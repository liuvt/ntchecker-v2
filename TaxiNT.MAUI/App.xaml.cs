namespace TaxiNT.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        protected override void OnStart()
        {
            base.OnStart();

            // XÓA Preferences mỗi khi mở app
            Preferences.Remove("_loadDatas");
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Nếu muốn, bạn có thể xóa lại ở đây
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "TaxiNT.MAUI" };
        }
    }
}
