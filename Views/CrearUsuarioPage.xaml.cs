using UsuariosRestMAUI.ViewModels;

namespace UsuariosRestMAUI.Views;

public partial class CrearUsuarioPage : ContentPage
{
    public CrearUsuarioPage(CrearUsuarioViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
