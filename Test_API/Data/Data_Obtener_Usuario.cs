using Newtonsoft.Json;
using System.Data;
using Test_API.Models;
using SR = System.Reflection.MethodBase;


namespace Test_API.Data
{
    public class Data_Obtener_Usuario
    {
        public Usuarios Get(config conf, int _id)
        { 
            try
            {
                Usuarios objReturn = new Usuarios();

                object parametros = new
                {
                    id = _id
                };

                M_Respuesta M_Resp = new Conexion().SQL(conf.conexion, "[dbo].[SP_Z_TEST_READ] @id", parametros, conf);

                if(M_Resp.MSJ_TIPO != "error" && M_Resp.DATA != null) 
                {
                    if(M_Resp.DATA.Count > 0 && M_Resp.DATA[0] != null && M_Resp.DATA[0].Rows.Count > 0) 
                    {
                        string JSON = String.Join("", M_Resp.DATA[0].AsEnumerable().Select(x => x[0].ToString()).ToArray());
                        objReturn = JsonConvert.DeserializeObject<Usuarios>(JSON);
                    }
                }

                return objReturn;
            }
            catch(Exception ex) 
            {
                Funciones.Error(this.GetType().FullName + "." + SR.GetCurrentMethod().Name, "" + ex, conf);
                return null;
            }
        }
    }
}
