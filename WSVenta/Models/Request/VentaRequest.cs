using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WSVenta.Models.Request
{
    public class VentaRequest
    {
        //Campo requerido
        [Required]
        //Validar que sea mayor a cero, y enviamso un mensaje personalizado si existe un 400, BadRequest
        [Range(1,double.MaxValue, ErrorMessage = "El valor de IdCliente debe ser mayor a cero")]
        //la validacion perzonalizada que hicimos ne la aprte inferior, lo colo sin Attribute, ya que lo detecta como un atributo , podmeos o no colocarle el nombre completo, a gusto de
        //cada quien
        [ExisteCliente(ErrorMessage = "El cliente no existe")]
        public int IdCliente { get; set; }

        [Required]
        //Que venga por lomneos un conecpeto, una compra
        //(MinLength), funciona tambien con arreglos, y en Core
        [MinLength(1, ErrorMessage = "Deben existir conceptos")]
        public List<Concepto> Conceptos { get; set; }

        //creamos un constructor, en caso de que algun valor sea nulo
        public VentaRequest()
        {
            //que se incialice aun q no tenga valores
            this.Conceptos = new List<Concepto>();
        }
    }

    public class Concepto
    {
        public int IdProducto { get; set; }
        public int CatidadConcepto { get; set; }
        public decimal PrecioUnitarioConcepto { get; set; }
        public decimal ImporteConcepto { get; set; }
    }

    //Crearemos nuestro propio validation
    //estas (regiones), son secciones de codigo dond epodemos comprimir o agrupar, masomenos como los comentarios
    #region Validaciones
    public class ExisteClienteAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int IdCliente = (int)value;
            using (VentaRealContext db =  new VentaRealContext())
            {
                if (db.Cliente.Find(IdCliente) == null) return false;
            }
            return true;
        }
    }
    #endregion
}
