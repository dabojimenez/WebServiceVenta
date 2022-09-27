using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WSVenta.Models
{
    public partial class Usuario
    {
        public int IdUsu { get; set; }
        public string EmailUsu { get; set; }
        public string PasswordUsu { get; set; }
        public string NombreUsu { get; set; }
    }
}
