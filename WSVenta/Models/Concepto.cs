using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WSVenta.Models
{
    public partial class Concepto
    {
        public long IdConcepto { get; set; }
        public long IdVenta { get; set; }
        public int CatidadConcepto { get; set; }
        public decimal PrecioUnitarioConcepto { get; set; }
        public decimal ImporteConcepto { get; set; }
        public int IdProducto { get; set; }

        public virtual Producto IdProductoNavigation { get; set; }
        public virtual Venta IdVentaNavigation { get; set; }
    }
}
