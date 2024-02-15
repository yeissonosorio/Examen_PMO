namespace Examen_PMO
{
    public partial class App : Application
    {
        static Controlers.Sitios_Controllers sitios;

        // Create the database connection as a singleton.
        public static Controlers.Sitios_Controllers Sitios
        {
            get
            {
                if (sitios == null)
                {
                    sitios = new Controlers.Sitios_Controllers();
                }
                return sitios;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            MainPage = new NavigationPage(new Lista());
        }

    }
}
