using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class config
{
    public static Microsoft.Extensions.Configuration.IConfiguration Configuration;
    public string ip { get; set; } = "";
    public string ip_cliente { get; set; } = "";
    public int pae_id { get; set; } = 0;
    public int bit_id { get; set; }
    public string perfil { get; set; } = "";
    public string app_nombre { get; set; } = "";
    public string ruta_logs { get; set; } = "";
    public string conexion { get; set; } = "";
    public string key { get; set; } = "";

    public void Configurar(string _ip, int _pae_id)
    {
        try
        {
            ip = _ip; 
            pae_id = _pae_id;
            perfil = Configuration["perfil"];
            app_nombre = Configuration["app_nombre"];
            ruta_logs = Configuration["ruta_logs"];
            conexion = Configuration[perfil + ":conexion"] + " password=" + Encripcion.DesEncriptar64(Configuration[perfil + ":conexion_pass"]) + ";";
            key = Encripcion.DesEncriptar64(Configuration[perfil + ":Key"]); 
        }
        catch (Exception ex) { }
    }
}





