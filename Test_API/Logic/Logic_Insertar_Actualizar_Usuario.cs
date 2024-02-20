using Test_API.Models;
using Test_API.Data;
using SR = System.Reflection.MethodBase;

namespace Test_API.Logic
{
    public class Logic_Insertar_Actualizar_Usuario
    {
        public Servicio_Out L_Logica_Insetar_Actualizar(Insertar_Actualizar_Usuario In, string Token, string ip)
        {
            config conf = new config(); conf.Configurar(ip, 0);
            conf.bit_id = new Bitacora().Bitacora_In(In, conf);

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
                        if (In.id != null && In.id > -1)
                        {


                            object parametros = new
                            {
                                id = In.id,
                                nombre = In.nombre,
                                apellido = In.apellido,
                                edad = In.edad,
                                fecha_nacimiento = In.fecha_nacimiento,
                                fecha_hora_registro = In.fecha_hora_registro,
                                estado = In.estado,
                                COM_USUARIO_REGISTRA = TK_Resp.data.usu_usuario
                            };

                            M_Respuesta M_Resp = new Conexion().SQL(conf.conexion, "[dbo].[SP_Z_TEST_CREATE_UPDATE] @id, @nombre, @apellido, @edad, @fecha_nacimiento, @fecha_hora_registro, @estado", parametros, conf);

                            List<Usuarios> obj_Lista_Usuario = null;

                            Out.respuesta_tipo = M_Resp.MSJ_TIPO;
                            Out.respuesta_detalle = M_Resp.MSJ_TEXT;

                            if (M_Resp.MSJ_TIPO != "error" && M_Resp.DATA != null)
                            {
                                obj_Lista_Usuario = new Data_Listar_Usuarios().Get_All(conf);
                            }

                            Out.Data = obj_Lista_Usuario;
                            Out.token = TK_Resp.token;




                        }
                        else
                        {
                            Out.respuesta_tipo = "warning"; Out.respuesta_detalle = "Debe enviar el ID del Usuario."; Out.token = TK_Resp.token; Out.Data = null;
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
