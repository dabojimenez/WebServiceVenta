using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Services;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //De esta manera, con esta variable y la iterfaces creada estamos modularizando el proyecto, para en caso
        //de que se quiera cmabiar a un servicio de autenticacion externa no sea muy compliado
        private IUserService _userService;

        public UsuarioController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]//Con esto, en la url, no solo iria el nombre del controladro, sino que tambien la palabra login
        //[FromBody], indicamos que venga en el body, ya que s epuede enviar atos en el header, path
        public IActionResult Autenticar([FromBody] AuthRequest model)
        {
            Respuesta oRespuesta = new Respuesta();
            var userResponse = _userService.Auth(model);

            if (userResponse == null)
            {
                oRespuesta.Exito = 0;
                oRespuesta.Mensaje = "Usuario o clave Incorrecta";
                return BadRequest();//Nos envia un erro de navegador que es el eror (400)
            }
            oRespuesta.Exito = 1;
            oRespuesta.Data = userResponse;
            return Ok(oRespuesta);
        }
    }
}
