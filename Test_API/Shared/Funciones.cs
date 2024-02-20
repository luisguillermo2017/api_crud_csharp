using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Funciones
{
    //------------------------------------------------------------------------------------------------------------------------------------------------
    public static void Log(string Vars, string Error, config conf)
    {
        string r = "";
        try
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            string dtf = DateTime.Now.ToString("yyyyMMdd");
            string linea = dt + "|" + conf.ip + "|" + Vars + " | " + Error;
            string ruta = conf.ruta_logs + "/" + conf.app_nombre + "_" + dtf + ".log";
            File.AppendAllText(ruta, Environment.NewLine + linea, Encoding.UTF8);
        }
        catch (Exception e) { }
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------
    public static void Error(string Vars, string Error, config conf)
    {
        SqlConnection conn = null;
        try
        {
            if (Error != null) { Error = Error.Replace("'", "°"); }
            if (Vars != null) { Vars = Vars.Replace("'", "°"); }
            conn = new SqlConnection(conf.conexion);
            conn.Open();
            SqlCommand com = new SqlCommand("EXEC SP_BER_REGISTRA_ERROR '" + conf.ip + "'," + conf.app_nombre + ",'" + Vars + "','" + Error + "'", conn);
            SqlDataReader reader = com.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();
            if (dt != null && dt.Columns.Contains("MSJ_TIPO") && dt.Rows[0]["MSJ_TIPO"].ToString() == "success")
            {
            }
            else
            {
                Log(Vars, Error, conf);
            }
        }
        catch (SqlException ex)
        {
            Log(Vars, Error, conf);
        }
        catch (Exception ex)
        {
            Log(Vars, Error, conf);
        }
        finally
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------









}

