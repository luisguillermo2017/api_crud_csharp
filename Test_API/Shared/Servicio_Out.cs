public class Servicio_Out
{
    /// <summary>
    /// TIPO DE RESPUESTA (SUCCESS, WARNGING, ERRRO)
    /// </summary>
    public string respuesta_tipo { get; set; } = "";
    /// <summary>
    /// MENSAJE DE LA RESPUESTA
    /// </summary>
    public string respuesta_detalle { get; set; } = "";
    /// <summary>
    /// TOKEN REFRESCADO
    /// </summary>
    public string token { get; set; } = "";

    /// <summary>
    /// DATOS RETORNADOR DE LA EJECUCION.
    /// </summary>
    public Object Data { get; set; }
}
