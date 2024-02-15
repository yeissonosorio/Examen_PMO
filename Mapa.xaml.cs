using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace Examen_PMO;

public partial class Mapa : ContentPage
{
	string des, fot;
	double lat, lon;
    Pin pin;
    public Mapa(string Des,string Fot,double Lat, double Lon)
	{
		InitializeComponent();
		des = Des;
		fot = Fot;
		lat = Lat; 
		lon = Lon;
        var map = new Microsoft.Maui.Controls.Maps.Map(MapSpan.FromCenterAndRadius(new Location(lat, lon), Distance.FromMiles(1)));

        // Crear el pin cona ubicación actual
        pin = new Pin
        {
            Label = "Sitio",
            Address = ""+des,
            Type = PinType.Place,
            Location = new Location(lat, lon)
        };

        // Agregar el pin al mapa
        map.Pins.Add(pin);

        // Establecer el contenido de la página como el mapa
        Content = map;
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        byte[] imageBytes = Convert.FromBase64String(fot);

        // Guardar la imagen decodificada como un archivo temporal
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp_image.png");
        File.WriteAllBytes(filePath, imageBytes);

        // Compartir la imagen como un archivo temporal
        await Share.RequestAsync(new ShareFileRequest
        {
            Title = "Compartir foto",
            File = new ShareFile(filePath)
            {
                ContentType = "image/png" // Ajusta el tipo de contenido según el formato de la foto
            }
        });
    }


}