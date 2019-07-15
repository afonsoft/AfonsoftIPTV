﻿using Prism;
using Prism.Ioc;
using AfonsoftIPTV.ViewModels;
using AfonsoftIPTV.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AfonsoftIPTV
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { } 

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/PrismMasterDetailPageIPTV");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<PrismMasterDetailPageIPTV, PrismMasterDetailPageIPTVViewModel>();
            containerRegistry.RegisterForNavigation<PrismContentPageVideo, PrismContentPageVideoViewModel>();
            containerRegistry.RegisterForNavigation<PrismTabbedPageList, PrismTabbedPageListViewModel>();
        }
    }
}
