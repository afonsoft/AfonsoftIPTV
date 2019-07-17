using AfonsoftIPTV.Helpers;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Prism;
using Prism.Ioc;
using System;

namespace AfonsoftIPTV.Droid
{
    [Activity(Label = "AfonsoftIPTV", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // Rollbar notifier configuartion
            RollbarHelper.ConfigureRollbarSingleton();

            // Registers for global exception handling.
            RollbarHelper.RegisterForGlobalExceptionHandling();

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
                var newExc = new ApplicationException("AndroidEnvironment_UnhandledExceptionRaiser", args.Exception);
                RollbarHelper.RollbarInstance.AsBlockingLogger(TimeSpan.FromSeconds(10)).Critical(newExc);
            };

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

