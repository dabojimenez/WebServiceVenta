using System.ComponentModel.DataAnnotations;

namespace WSVenta.Models.Request
{
    public class AuthRequest
    {
        [Required]//Indicamos que son valores requeridos
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
