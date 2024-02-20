using Test_API.Models;
using Test_API.Data;
using SR = System.Reflection.MethodBase;


namespace Test_API.Logic
{
    public class Logic_Obtener_Usuario
    {
        public Servicio_Out L_Logica_Obtener( Obtener_Usuario_In In, string Token, string ip)
        {
            config conf = new config(); conf.Configurar(ip, 0);

            Servicio_Out Out = new Servicio_Out()
            {
                respuesta_tipo = "error",
                respuesta_detalle = "Error: Excepcion no controlada"
            };

            Servicios servicio_valida_token = new Servicios().ObtenerSistemas(conf, "link_verifica_token");
            Token_Out TK_Resp = new Token().L_Valida_Token(Token, servicio_valida_token.SEV_URL_SERVICIO);

            try
            {
                if (TK_Resp.respuesta_tipo == "success")
                {
                    
                    if (In.id > 0)
                    {
                        Usuarios obj_Usuario = new Data_Obtener_Usuario().Get(conf, In.id);


                        if (obj_Usuario != null)
                        {

                            Out.respuesta_tipo = "success";
                            Out.respuesta_detalle = "Exito al Realizar la Accion";
                            Out.Data = obj_Usuario;
                            Out.token = TK_Resp.token;
                        }
                        else
                        {
                            Out.respuesta_tipo = "warning";
                            Out.respuesta_detalle = "No se encontró el Registro.";
                            Out.Data = null;
                            Out.token = TK_Resp.token;
                        }
                    }
                    else
                    {
                        Out.respuesta_tipo = "warning";
                        Out.respuesta_detalle = "Debe enviar el Id del usuario a consultar.";
                        Out.Data = null;
                        Out.token = TK_Resp.token;
                    }
                    
                }
                else
                {
                    Out.respuesta_tipo = TK_Resp.respuesta_tipo; Out.respuesta_detalle = TK_Resp.respuesta_detalle; Out.token = TK_Resp.token; Out.Data = null;
                }
            }
            catch (Exception e)
            {
                Funciones.Error(this.GetType().FullName + "." + SR.GetCurrentMethod().Name, "" + e, conf);
                Out.respuesta_tipo = "error";
                Out.respuesta_detalle = "Error: Excepcion no controlada";
                Out.Data = null;
                Out.token = TK_Resp.token;
            }


            new Bitacora().Bitacora_Out(Out, conf);
            return Out;
        }
    }
    
}
