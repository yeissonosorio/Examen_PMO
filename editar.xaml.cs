namespace Examen_PMO;

public partial class editar : ContentPage

{
    FileResult photo;
    string Des, Fot;
    double Lat, Lon;
    int Id;
    public editar(string des,string fot, double lat,double lon, int id)
	{
		InitializeComponent();
        Des = des;
        Fot = fot;
        Lat = lat;
        Lon = lon;
        Id = id;

        latitud.Text = Lat+"";
        longitud.Text = Lon + "";
        descripcion.Text = Des + "";

        byte[] imageBytes = Convert.FromBase64String(Fot);
        Foto.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(imageBytes));
        latitud.IsEnabled = false;
        longitud.IsEnabled=false;

    }
    private async void btntomare_Clicked(object sender, EventArgs e)
    {

        string[] options = { "Tomar foto", "Seleccionar de la galería" };
        string action = await DisplayActionSheet("Seleccionar imagen", "Cancelar", null, options);

        if (action == "Tomar foto")
        {
            photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                string path = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using Stream sourcephoto = await photo.OpenReadAsync();
                using FileStream Stringlocal = File.OpenWrite(path);

                Foto.Source = ImageSource.FromStream(() => photo.OpenReadAsync().Result);

                await sourcephoto.CopyToAsync(Stringlocal);
            }
        }
        else if (action == "Seleccionar de la galería")
        {
            photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                Foto.Source = ImageSource.FromStream(() => photo.OpenReadAsync().Result);
            }
        }
    }

    public string GetImage64()
    {
        if (photo != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Stream stream = photo.OpenReadAsync().Result;
                stream.CopyTo(ms);
                byte[] data = ms.ToArray();
                string Base64 = Convert.ToBase64String(data);

                return Base64;
            }
        }
        return null;
    }
    private async void btneditar_Clicked(object sender, EventArgs e)
    {
        try
        {

            double lati = double.Parse(latitud.Text);
            double longi = double.Parse(longitud.Text);

            string Foto = GetImage64();

            if (descripcion.Text == null )
            {
                if (longi == 0)
                {
                    await DisplayAlert("Aviso", "acepte los permisos de ubicacion o active la ubicacion", "OK");
                }
                await DisplayAlert("Aviso", "LLene todos los campos", "OK");
            }
            else
            {
                try
                {
                    Models.sitios sitioExistente = await App.Sitios.GePerson(Id);
                    sitioExistente.Descripcion = descripcion.Text;
                    if (Foto == null)
                    {
                        sitioExistente.foto = Fot;
                    }
                    else
                    {
                        sitioExistente.foto = Foto;
                    }

                    if (await App.Sitios.StoreSitios(sitioExistente) > 0)
                    {
                        await DisplayAlert("Aviso", "Registro Actualizado con exito!!", "OK");
                        await Navigation.PushAsync(new Lista());
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "" + ex, "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "active la ubicación o acepte los permisos de ubicación", "Ok");
        }
    }

}