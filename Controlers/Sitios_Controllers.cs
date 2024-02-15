using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Examen_PMO.Models;

namespace Examen_PMO.Controlers
{
    public class Sitios_Controllers
    {
        readonly SQLiteAsyncConnection _connection;

        public Sitios_Controllers()
        {
            SQLite.SQLiteOpenFlags extensiones = SQLite.SQLiteOpenFlags.ReadWrite |
                                                SQLite.SQLiteOpenFlags.Create |
                                                SQLite.SQLiteOpenFlags.SharedCache;

            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "DBSitios.db3"), extensiones);
            _connection.CreateTableAsync<sitios>().Wait();

        }
        //Insertar
        public async Task<int> StoreSitios(sitios sites)
        {

            if (sites.Id == 0)
            {
                return await _connection.InsertAsync(sites);
            }
            else
            {
                return await _connection.UpdateAsync(sites);
            }
        }
        //Obtener Lista
        public async Task<List<Models.sitios>> GetListPersons()
        {

            return await _connection.Table<sitios>().ToListAsync();
        }

        public async Task<Models.sitios> GePerson(int pid)
        {

            return await _connection.Table<sitios>().Where(i => i.Id == pid).FirstOrDefaultAsync();
        }

        // Delete Element
        public async Task<int> DeletePerson(sitios sites)
        {

            return await _connection.DeleteAsync(sites);
        }
    }
}
