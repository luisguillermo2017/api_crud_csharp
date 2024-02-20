using Newtonsoft.Json;

public class Bitacora
{
    //----------------------------------------------------------------------------------------------------------------------------
    public int Bitacora_In(Object model, config conf)
    {
        int Id = 0;
        string JsonIn = ""; try { JsonIn = JsonConvert.SerializeObject(model); } catch (Exception e) { }
        try
        {
            var sql = "SP_BITACORA_IN @Ip, @Servicio , @JsonIn";

            object bitacora = new
            {
                Ip = conf.ip,
                Ip_Cliente = conf.ip_cliente,
                Servicio = conf.app_nombre,
                JsonIn = JsonIn
            };

            M_Respuesta M_Resp = new Conexion().SQL(conf.conexion, sql, bitacora, conf);
            Id = Int32.Parse(M_Resp.DATA[0].Rows[0]["BIT_ID"].ToString());
        }
        catch (Exception e)
        {
            Funciones.Error("Bitacora.Bitacora_In " + conf.ip, "" + e, conf);
        }
        return Id;
    }
    //----------------------------------------------------------------------------------------------------------------------------
    public void Bitacora_Trazabilidad(string texto, config conf)
    {
        try
        {
            string sql = "SP_BITACORA_TRAZABILIDAD @id , @texto";
            object bitacora = new
            {
                id = conf.bit_id,
                texto = texto
            };
            new Conexion().SQL(conf.conexion, sql, bitacora, conf);
        }
        catch (Exception e)
        {
            Funciones.Error("Bitacora_Trazabilidad " + conf.bit_id, "" + e, conf);
        }
    }
    //----------------------------------------------------------------------------------------------------------------------------
    public void Bitacora_Out(object model, config conf)
    {
        string JsonIn = ""; try { JsonIn = JsonConvert.SerializeObject(model); } catch (Exception e) { }
        try
        {
            string sql = "SP_BITACORA_OUT @Id , @JsonOut";
            object bitacora = new
            {
                Id = conf.bit_id,
                JsonOut = JsonIn
            };
            M_Respuesta M_Resp = new Conexion().SQL(conf.conexion, sql, bitacora, conf);
        }
        catch (Exception e)
        {
            Funciones.Error("Bitacora.Bitacora_Out Id " + conf.bit_id, "" + e, conf);
        }
    }
    //----------------------------------------------------------------------------------------------------------------------------







}






