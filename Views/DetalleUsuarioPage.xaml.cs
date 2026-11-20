using UsuariosRestMAUI.ViewModels;

namespace UsuariosRestMAUI.Views;

public partial class DetalleUsuarioPage : ContentPage
{
    private readonly DetalleUsuarioViewModel viewModel;

    public DetalleUsuarioPage(DetalleUsuarioViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InicializarAsync();
    }
}
