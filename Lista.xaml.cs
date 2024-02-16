namespace Examen_PMO;

public partial class Lista : ContentPage
{
    string? des, foto;
    double lat, lon;
    int id;
	public Lista()
	{
		InitializeComponent();
        CheckGpsStatus();
	}

    private async void CheckGpsStatus(){
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                // Si los permisos de ubicaci�n no est�n otorgados, mostrar un mensaje y proporcionar un bot�n para ir a la configuraci�n
                bool result = await DisplayAlert("Alerta", "Los permisos de ubicaci�n no est�n otorgados. �Desea ir a la configuraci�n para activarlos?", "S�", "No");

                if (result)
                {
                    // Abrir la configuraci�n de la aplicaci�n
                    AppInfo.ShowSettingsUI();
                }
            }
            else
            {
                // Los permisos de ubicaci�n est�n otorgados, verificar si la configuraci�n de ubicaci�n est� activa
                var locationIsEnabled = await Geolocation.GetLastKnownLocationAsync();

                if (locationIsEnabled != null)
                {
                
                }
                else
                {
                    // La configuraci�n de ubicaci�n est� desactivada
                   await DisplayAlert("Alerta", "La configuraci�n de ubicaci�n est� desactivada. �Desea ir a la configuraci�n para activarla?", "Ok");
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar errores
            await DisplayAlert("Error", ex.Message, "OK");
        }
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
            id = selectedSitio.Id;
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listperson.ItemsSource = await App.Sitios.GetListPersons();
    }

    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        if (listperson.SelectedItem != null)
        {
            bool answer = await DisplayAlert("Confirmaci�n", "�Est� seguro de que desea eliminar este sitio?", "S�", "No");

            if (answer)
            {
                Models.sitios selectedSitio = listperson.SelectedItem as Models.sitios;

                // Eliminar el elemento seleccionado de la base de datos
                await App.Sitios.DeletePerson(selectedSitio);

                // Actualizar la lista despu�s de eliminar el elemento
                listperson.ItemsSource = await App.Sitios.GetListPersons();

                // Limpiar la selecci�n
                listperson.SelectedItem = null;
            }
        }
        else
        {
            await DisplayAlert("Aviso", "Seleccione un elemento de la lista para eliminar.", "OK");
        }
    }

    private async void Actualizar_Clicked(object sender, EventArgs e)
    {
        if (des == null)
        {
            await DisplayAlert("Aviso", "Selecccione un elemento de la lista", "OK");
        }
        else
        {
            await Navigation.PushAsync(new editar(des, foto, lat, lon,id));
        }
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