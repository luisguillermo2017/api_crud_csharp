using System.Data;

public class Servicios
{
    public int SEV_ID { get; set; }
    public string SEV_DESCRIPCION_SERVICIO { get; set; }
    public string SEV_URL_SERVICIO { get; set; }
    public bool SEV_ESTADO { get; set; }


    #region FUNCIONES 
    public Servicios ObtenerSistemas(config conf, String Servicio)
    {
        Servicios Out = new Servicios();
        object parSP = new
        {
            SERV_NAME = Servicio
        };

        M_Respuesta M_Par = new Conexion().SQL(conf.conexion, "SP_OBTIENE_SERVICIO @SERV_NAME", parSP, conf);

        foreach (DataRow row in M_Par.DATA[0].Rows)
        {
            Out.SEV_ID = Convert.ToInt32(row["SEV_ID"].ToString());
            Out.SEV_DESCRIPCION_SERVICIO = row["SEV_DESCRIPCION_SERVICIO"].ToString(); 
            Out.SEV_URL_SERVICIO = row["SEV_URL_SERVICIO"].ToString();
            Out.SEV_ESTADO = Convert.ToBoolean(row["SEV_ESTADO"].ToString());
        }
        return Out;
    }
    #endregion
}