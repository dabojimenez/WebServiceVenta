using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WSVenta.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Concepto = new HashSet<Concepto>();
        }

        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioUnitarioProducto { get; set; }
        public decimal CostoProducto { get; set; }

        public virtual ICollection<Concepto> Concepto { get; set; }
    }
}
