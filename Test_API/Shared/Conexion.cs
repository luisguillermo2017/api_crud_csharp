
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Reflection;
using SR = System.Reflection.MethodBase;



public class Conexion
{

    //System.Data.SqlClient 4.8.5
    //------------------------------------------------------------------------------------------------------------------------------------------------
    public M_Respuesta SQL(string cnx, string sent, object? obj, config conf) //"SP_PROC_EJEMPLO @Usuario, @Password"
    {
        M_Respuesta M_Resp = new M_Respuesta
        {
            DATA = new List<DataTable>(),
            DATASTR = new List<Object>(),
            MSJ_ID = "18",
            MSJ_TIPO = "error",
            MSJ_TEXT = "Error: Excepcion no controlada"
        };
        SqlConnection? conn = null;
        try
        {
            conn = new SqlConnection(cnx);
            try { conn.Open(); }
            catch (SqlException ex)
            {
                Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + ex, conf); return M_Resp;
            }
            SqlCommand com = new SqlCommand(sent, conn);
            if (obj != null)
            {
                com = FindParams(com, sent, obj);
            }
            com.CommandTimeout = 90;
            SqlDataReader reader = com.ExecuteReader();
            DataTable dt;
            while (!reader.IsClosed)
            {
                dt = new DataTable();
                dt.Load(reader);
                dt = dt.Ajuste_DataTable();
                M_Resp.DATA.Add(dt);
                try
                {
                    M_Resp.DATASTR.Add(Utils.Table_To_JsonString(dt));
                }
                catch (Exception e)
                {
                    Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + e, conf);
                }
            }
            M_Resp.MSJ_ID = "0";
            M_Resp.MSJ_TIPO = "success";
            M_Resp.MSJ_TEXT = "Tablas: " + M_Resp.DATA.Count;
            if (M_Resp.DATA.Count > 0)
            { 

                for (int i = 0; i < M_Resp.DATA.Count; i++)
                {
                    DataColumnCollection columns = M_Resp.DATA[i].Columns;
                    if (columns.Contains("MSJ_TIPO")) { M_Resp.MSJ_TIPO = M_Resp.DATA[i].Rows[0]["MSJ_TIPO"].ToString(); }
                    if (columns.Contains("MSJ_TEXT")) { M_Resp.MSJ_TEXT = M_Resp.DATA[i].Rows[0]["MSJ_TEXT"].ToString(); }
                }
            }
            conn.Close();
            return M_Resp;
        }
        catch (SqlException ex)
        {
            Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + ex, conf);
            return M_Resp;
        }
        catch (Exception ex)
        {
            Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + ex, conf);
            return M_Resp;
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
    private SqlCommand FindParams(SqlCommand com, string query, Object obj)
    {
        Type t = obj.GetType();
        PropertyInfo[] props = t.GetProperties();
        string[] parametros = query.Replace("@", "").Split(",");
        foreach (var param in parametros)
        {
            string paramName = (param == parametros[0] ? param.Split(" ")[1] : param).Trim();
            foreach (var prop in props)
            {
                if (paramName.Trim().ToLower() == prop.Name.ToLower())
                {
                    if (prop.GetValue(obj) != null)
                    {
                        com.Parameters.AddWithValue(paramName.Trim(), prop.GetValue(obj));
                    }
                    else
                    {
                        com.Parameters.AddWithValue(paramName.Trim(), Convert.DBNull);
                    }
                    break;
                }
            }
        }
        return com;
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------







    /*
    //------------------------------------------------------------------------------------------------------------------------------------------------
    //System.Data.Odbc 7.0.0
    public M_Respuesta IFX(string cnx, string sent, object? obj, config conf) //EXECUTE PROCEDURE pr_ejemplo( ? , ? )
    {
        M_Respuesta M_Resp = new M_Respuesta
        {
            DATA = new List<DataTable>(),
            DATASTR = new List<Object>(),
            MSJ_ID = "18",
            MSJ_TIPO = "error",
            MSJ_TEXT = "Error: Excepcion no controlada"
        };
        OdbcConnection? conn = null;
        try
        {
            conn = new OdbcConnection(cnx);
            conn.Open();
            OdbcCommand com = new OdbcCommand(sent, conn);
            if (obj != null)
            {
                com = FindParams(com, obj);
            }
            com.CommandTimeout = 120;
            OdbcDataReader reader = com.ExecuteReader();
            DataTable dt;
            while (!reader.IsClosed)
            {
                dt = new DataTable();
                dt.Load(reader);
                dt = dt.Ajuste_DataTable();
                M_Resp.DATA.Add(dt);
                try
                {
                    M_Resp.DATASTR.Add(Utils.Table_To_JsonString(dt));
                }
                catch (Exception e)
                {
                    Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + e, conf);
                }
            }
            M_Resp.MSJ_ID = "0";
            M_Resp.MSJ_TIPO = "success";
            M_Resp.MSJ_TEXT = "Tablas: " + M_Resp.DATA.Count;
            if (M_Resp.DATA.Count > 0)
            {
                DataColumnCollection columns = M_Resp.DATA[0].Columns;
                if (columns.Contains("MSJ_TIPO")) { M_Resp.MSJ_TIPO = M_Resp.DATA[0].Rows[0]["MSJ_TIPO"].ToString(); }
                if (columns.Contains("MSJ_TEXT")) { M_Resp.MSJ_TEXT = M_Resp.DATA[0].Rows[0]["MSJ_TEXT"].ToString(); }
            }
            conn.Close();
            return M_Resp;
        }
        catch (SqlException ex)
        {
            Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + ex, conf);
            return M_Resp;
        }
        catch (Exception ex)
        {
            Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + ex, conf);
            return M_Resp;
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
    private OdbcCommand FindParams(OdbcCommand com, Object obj)
    {
        Type t = obj.GetType();
        PropertyInfo[] props = t.GetProperties();
        foreach (var prop in props)
        {
            com.Parameters.AddWithValue(prop.Name, prop.GetValue(obj));
        }
        return com;
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------
    */





    /*
    //------------------------------------------------------------------------------------------------------------------------------------------------
    //MySql.Data 8.0.32.1
    public M_Respuesta MySQL(string cnx, string sent, object? obj, config conf) //"CALL SP_PROC_EJEMPLO (?, ?)"
    {
        M_Respuesta M_Resp = new M_Respuesta
        {
            DATA = new List<DataTable>(),
            MSJ_ID = "18",
            MSJ_TIPO = "error",
            MSJ_TEXT = "Error: Excepcion no controlada"
        };
        MySqlConnection? conn = null;
        try
        {
            conn = new MySqlConnection(cnx);
            try { conn.Open(); }
            catch (MySqlException ex)
            {
                Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + ex, conf); return M_Resp;
            }
            MySqlCommand com = new MySqlCommand(sent, conn);
            if (obj != null)
            {
                com = FindParams(com, obj);
            }
            com.CommandTimeout = 90;
            MySqlDataReader reader = com.ExecuteReader();
            DataTable dt;
            while (!reader.IsClosed)
            {
                dt = new DataTable();
                dt.Load(reader);
                dt = dt.Ajuste_DataTable();
                M_Resp.DATA.Add(dt);
            }
            M_Resp.MSJ_ID = "0";
            M_Resp.MSJ_TIPO = "success";
            M_Resp.MSJ_TEXT = "Tablas: " + M_Resp.DATA.Count;
            if (M_Resp.DATA.Count > 0)
            {
                DataColumnCollection columns = M_Resp.DATA[0].Columns;
                if (columns.Contains("MSJ_TIPO")) { M_Resp.MSJ_TIPO = M_Resp.DATA[0].Rows[0]["MSJ_TIPO"].ToString(); }
                if (columns.Contains("MSJ_TEXT")) { M_Resp.MSJ_TEXT = M_Resp.DATA[0].Rows[0]["MSJ_TEXT"].ToString(); }
            }
            conn.Close();
            return M_Resp;
        }
        catch (MySqlException ex)
        {
            Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + ex, conf);
            return M_Resp;
        }
        catch (Exception ex)
        {
            Funciones.Error(cnx, this.GetType().FullName + "." + SR.GetCurrentMethod().Name + " " + ex, conf);
            return M_Resp;
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
    private MySqlCommand FindParams(MySqlCommand com, Object obj)
    {
        Type t = obj.GetType();
        PropertyInfo[] props = t.GetProperties();
        foreach (var prop in props)
        {
            com.Parameters.AddWithValue(prop.Name, prop.GetValue(obj));
        }
        return com;
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------
    */














}







public class M_Respuesta
{
    public List<DataTable> DATA { get; set; }
    public List<Object> DATASTR { get; set; }
    public string MSJ_ID { get; set; }
    public string MSJ_TIPO { get; set; }
    public string MSJ_TEXT { get; set; }

}