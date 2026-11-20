using UsuariosRestMAUI.ViewModels;
using UsuariosRestMAUI.Models;

namespace UsuariosRestMAUI.Views;

public partial class ListaUsuariosPage : ContentPage
{
    private readonly ListaUsuariosViewModel viewModel;

    public ListaUsuariosPage(ListaUsuariosViewModel viewModel)
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

    private async void AlSeleccionarUsuario(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is CollectionView lista && e.CurrentSelection.FirstOrDefault() is Usuario usuario)
        {
            lista.SelectedItem = null;
            await viewModel.AbrirDetalleAsync(usuario);
        }
    }
}
