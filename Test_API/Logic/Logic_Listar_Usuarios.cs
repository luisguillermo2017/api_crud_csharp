using Test_API.Models;
using Test_API.Data;

namespace Test_API.Logic
{
    public class Logic_Listar_Usuarios
    {
        public Servicio_Out L_Logica_Listar(string Token, string ip)
        {

            config conf = new config(); conf.Configurar(ip, 0);
            //conf.bit_id = new Bitacora().Bitacora_In(null, conf);

            Servicio_Out Out = new Servicio_Out()
            {
                respuesta_tipo = "error",
                respuesta_detalle = "Error: Excepcion no controlada"
            };

            Servicios servicio_valida_token = new Servicios().ObtenerSistemas(conf, "link_verifica_token");
            Token_Out TK_Resp = new Token().L_Valida_Token(Token, servicio_valida_token.SEV_URL_SERVICIO);

            try
            {
                if (TK_Resp.respuesta_tipo == "success") //***
                {
                    List<Usuarios> obj_Lista_Usuarios = new Data_Listar_Usuarios().Get_All(conf);

                    if(obj_Lista_Usuarios != null)
                    {
                        Out.respuesta_tipo = "success";
                        Out.respuesta_detalle = "Exito al realizar la acción";
                        Out.Data = obj_Lista_Usuarios;
                        Out.token = Token;//***
                    }
                    else 
                    {
                        Out.respuesta_tipo = "warning";
                        Out.respuesta_detalle = "Sin datos";
                        Out.Data = null;
                        Out.token = Token;//***
                    }

                }
                else 
                {
                    Out.respuesta_tipo = TK_Resp.respuesta_tipo; Out.respuesta_detalle = TK_Resp.respuesta_detalle; Out.token = TK_Resp.token; Out.Data = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Out;

        }
    }
}
