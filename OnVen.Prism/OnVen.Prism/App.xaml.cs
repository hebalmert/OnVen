using OnVen.Common.Services;
using OnVen.Prism.ViewModels;
using OnVen.Prism.Views;
using Prism;
using Prism.Ioc;
using Syncfusion.Licensing;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace OnVen.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("NTU0MTMwQDMxMzkyZTM0MmUzMEVXSVczelBCMHZtelQ4SExnejFScGR0RE5UaXVkU2FQclFkNTJGaGl1T0E9");

            InitializeComponent();

            await NavigationService.NavigateAsync($"NavigationPage/{nameof(ProductsPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            //Para Inyeccion de Servicio
            containerRegistry.Register<IApiService, ApiService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductsPage, ProductsPageViewModel>();
        }
    }
}
