namespace Examen_PMO;

public partial class Lista : ContentPage
{
    string? des, foto;
    double lat, lon;
	public Lista()
	{
		InitializeComponent();
	}
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        //var page = new NavigationPage(new PageInit());
        var page = new MainPage();
        await Navigation.PushAsync(page);
    }
    private async void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            Models.sitios selectedSitio = e.CurrentSelection[0] as Models.sitios;
            des = selectedSitio.Descripcion;
            lat = selectedSitio.Latitud ?? 0.0;
            lon = selectedSitio.Longitud ?? 0.0;
            foto = selectedSitio.foto;
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listperson.ItemsSource = await App.Sitios.GetListPersons();
    }

    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        // Lógica para manejar el evento de clic del botón "Eliminar"
    }

    private async void Actualizar_Clicked(object sender, EventArgs e)
    {
        // Lógica para manejar el evento de clic del botón "Actualizar"
    }

    private async void Ver_Clicked(object sender, EventArgs e)
    {
        if (des == null)
        {
            await DisplayAlert("Aviso", "Selecccione un elemento de la lista", "OK");
        }
        else
        {
            await Navigation.PushAsync(new Mapa(des,foto,lat,lon));
        }
    }


}