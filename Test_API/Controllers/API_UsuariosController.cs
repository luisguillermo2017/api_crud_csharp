using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test_API.Logic;
using Test_API.Models;

namespace Test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class API_UsuariosController : ControllerBase
    {

        #region IP y Token

        private string IP()
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
        }
        //------------------------------------------------------------------------------------------------------------------------------
        private string? Tk()
        {
            bool ok = Request.Headers.TryGetValue("Authorization", out var headerValue);
            return ok ? headerValue.ToString() : "";
        }

        #endregion

        #region Create y Update

        [HttpPost]
        [Route("Insertar_Actualizar")]

        public object Insertar_Actualizar(Insertar_Actualizar_Usuario In)
        {
            return new Logic_Insertar_Actualizar_Usuario().L_Logica_Insetar_Actualizar(In, Tk(), IP());
        }


        #endregion

        #region Read

        [HttpPost]
        [Route("Listar")]

        public object Listar()
        {
            return new Logic_Listar_Usuarios().L_Logica_Listar(Tk(), IP());
        }

        #endregion

        #region Read con parametro

        [HttpPost]
        [Route("Obtener")]

        public object Obtener(Obtener_Usuario_In In)
        {
            return new Logic_Obtener_Usuario().L_Logica_Obtener(In, Tk(), IP());
        }

        #endregion


    }
}
