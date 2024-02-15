using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Examen_PMO.Models
{
    [Table("Sitios")]
    public class sitios
    {
       
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            
            public double? Longitud { get; set; }
    
            public double? Latitud { get; set; }

            [MaxLength(100)]
            public string? Descripcion { get; set; }

            public string? foto { get; set; }

        }
    
}
