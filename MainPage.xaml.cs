
namespace Examen_PMO
{
    public partial class MainPage : ContentPage
    {

        FileResult photo;


        public MainPage()
        {
            InitializeComponent();
            GetLocation();
            longitud.IsEnabled = false;
            latitud.IsEnabled = false;
        }
        async void GetLocation()
        {
            try
            {
                // Solicitar permiso de acceso a la ubicación
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    // Si se obtiene la ubicación, puedes usarla  desees
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;

                    latitud.Text = latitude.ToString();
                    longitud.Text = longitude.ToString();
                    

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // La geolocalización no es compatible en este dispositivo/platforma
                Console.WriteLine($"Geolocalización no es soportada: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // No se otorgó permiso para acceder a la ubicación
                Console.WriteLine($"Permiso de ubicación no otorgado: {pEx.Message}");
            }
            catch (Exception ex)
            {
                // Otras excepciones
                Console.WriteLine($"Error al obtener la ubicación: {ex.Message}");
            }
        }

        private async void btnprocesar_Clicked(object sender, EventArgs e)
        {
            try
            {

                double lati = double.Parse(latitud.Text);
                double longi = double.Parse(longitud.Text);
                string Foto = GetImage64();

                if (Foto == null || descripcion.Text == null || longi == 0)
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
                        var sitios = new Models.sitios
                        {
                            Longitud = longi,
                            Latitud = lati,
                            Descripcion = descripcion.Text,
                            foto = Foto
                        };

                        if (await App.Sitios.StoreSitios(sitios) > 0)
                        {
                            await DisplayAlert("Aviso", "Registro ingresado con exito!!", "OK");
                            await Navigation.PushAsync(new Lista());
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", "" + ex, "OK");
                    }
                }
            }
            catch (Exception ex) {
                await DisplayAlert("Error", "active la ubicación o acepte los permisos de ubicación","Ok");
            }
        }

        private async void btntomar_Clicked(object sender, EventArgs e)
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
    }

}
