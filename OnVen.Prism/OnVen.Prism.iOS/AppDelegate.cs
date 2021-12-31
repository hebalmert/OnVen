using Foundation;
using Prism;
using Prism.Ioc;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.SfRotator.XForms.iOS;
using Syncfusion.XForms.iOS.TextInputLayout;
using UIKit;


namespace OnVen.Prism.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            //Indicador de Actividad o Spining
            new SfBusyIndicatorRenderer();
            new SfRotatorRenderer();
            SfTextInputLayoutRenderer.Init();

            //Manejo de Imagenes
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
