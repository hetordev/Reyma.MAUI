using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

using NavbarAnimation.Maui.ViewModels.Base;
using NavbarAnimation.Maui.ViewModels.Pages;

namespace NavbarAnimation.Maui.Views.Pages;

public partial class RibbonPage : ContentPage
{

    public RibbonPage(RibbonViewModel viewModel)
    {
        InitializeComponent();

        On<iOS>().SetUseSafeArea(false);

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        if (BindingContext is not BaseViewModel baseViewModel)
            return;

        baseViewModel.Load();
    }
}