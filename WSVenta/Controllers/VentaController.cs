using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WSVenta.Models;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Services;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //como ya tenemos el token, agregamos la seguridad con []
    [Authorize]
    public class VentaController : ControllerBase
    {
        //asginamos la interface, de IVentaService, para que sea mas generico, puede q a futuro, venga otra clase, pero q implemente esta interface y es ganaci
        //ya que no tendriamos que cambiarla, por q si la implementa(MAS ESCALABLE)
        private IVentaService _venta;
        //cremaos el constructor, del controlador
        public VentaController(IVentaService venta)
        {
            this._venta = venta;
        }

        [HttpPost]
        public IActionResult Add(VentaRequest model)
        {
            Respuesta oRespuesta = new Respuesta();

            try
            {
                _venta.Add(model);
                oRespuesta.Exito = 1;
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(oRespuesta);
        }
    }
}
