using System.Data;
using System.Net;
using System.Net.Http.Headers;

public static class Utils
{
    //-----------------------------------------------------------------------------------------
    public static object Table_To_JsonString(DataTable table)
    {
        var list = new List<Dictionary<string, object>>();
        foreach (DataRow row in table.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in table.Columns)
            {
                dict[col.ColumnName] = (Convert.ToString(row[col]));
            }
            list.Add(dict);
        }
        return list;
    }
    //-----------------------------------------------------------------------------------------
    public static string Hoyddmmyyyy(int Num)
    {
        DateTime today = DateTime.Today;
        DateTime DaysEarlier = today.AddDays(Num);
        return DaysEarlier.ToString("dd/MM/yyyy");
    }
    //-------------------------------------------------------------------------------------------
    public static string Hoyyyyymmdd(int Num)
    {
        DateTime today = DateTime.Today;
        DateTime DaysEarlier = today.AddDays(Num);
        return DaysEarlier.ToString("yyyy-MM-dd");
    }
    //-------------------------------------------------------------------------------------------
    public static DataTable JsonStringToDataTable(string jsonString)
    {
        DataTable dt = new DataTable();
        string[] jsonStringArray = System.Text.RegularExpressions.Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
        List<string> ColumnsName = new List<string>();
        foreach (string jSA in jsonStringArray)
        {
            string[] jsonStringData = System.Text.RegularExpressions.Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
            foreach (string ColumnsNameData in jsonStringData)
            {
                try
                {
                    int idx = ColumnsNameData.IndexOf(":");
                    string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                    if (!ColumnsName.Contains(ColumnsNameString))
                    {
                        ColumnsName.Add(ColumnsNameString);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Parsing Column Name : {ColumnsNameData} {ex.Message}");
                }
            }
            break;
        }
        foreach (string AddColumnName in ColumnsName)
        {
            dt.Columns.Add(AddColumnName);
        }
        foreach (string jSA in jsonStringArray)
        {
            string[] RowData = System.Text.RegularExpressions.Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
            DataRow nr = dt.NewRow();
            foreach (string rowData in RowData)
            {
                try
                {
                    int idx = rowData.IndexOf(":");
                    string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                    string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                    nr[RowColumns] = RowDataString;
                }
                catch// (Exception ex)
                {
                    continue;
                }
            }
            dt.Rows.Add(nr);
        }
        return dt;
    }
    //-----------------------------------------------------------------------------------------
    public static object To_JsonDictionary(this DataTable table)
    {
        var list = new List<Dictionary<string, object>>();
        foreach (DataRow row in table.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in table.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        return list;
    }

    public static object Row_To_JsonDictionary(DataColumnCollection Columns, DataRow row)
    {
        var list = new List<Dictionary<string, object>>();
        var dict = new Dictionary<string, object>();

        foreach (DataColumn col in Columns)
        {
            dict[col.ColumnName] = row[col];
        }

        list.Add(dict);
        return list;
    }



    //-----------------------------------------------------------------------------------------
    public static M_Post_Out POST(string Link, List<M_Post_Header>? headers, Object Parametros)
    {
        M_Post_Out Out = new M_Post_Out() { is_success = false, message = "", data = "", status = System.Net.HttpStatusCode.BadRequest };
        try
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            if (headers != null)
            {
                foreach (M_Post_Header header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.key, header.value);
                }
            }

            var response = client.PostAsJsonAsync<Object>(Link, Parametros);
            var result = response.Result;
            Out.headers = result.Headers;

            Out.is_success = result.IsSuccessStatusCode;
            Out.status = result.StatusCode;

            var readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            Out.data = readTask.Result;

        }
        catch (Exception e)
        {
            return new M_Post_Out() { is_success = false, message = "Error POST: " + e, data = "", status = System.Net.HttpStatusCode.BadRequest };
        }
        return Out;
    }
    //-----------------------------------------------------------------------------------------
    public static string SinSaltos(this string cadena)
    {
        string result = cadena.Replace(System.Environment.NewLine, "");
        result = result.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
        return result;
    }
    //-----------------------------------------------------------------------------------------
    public static string SinComilla(this string _in)
    {
        string result = "";
        if (_in != null) { result = _in.Replace("'", ""); }
        return result;
    }
    //-----------------------------------------------------------------------------------------
    public static string SinComillas(this string _in)
    {
        string result = "";
        if (_in != null) { result = _in.Replace("'", "").Replace("\"", ""); }
        return result;
    }
    //-----------------------------------------------------------------------------------------
    public static string SinSQL(this string _in)
    {
        string _out = _in;
        if (_out != null)
        {
            _out.Replace("SELECT", "")
            .Replace("UPDATE", "")
            .Replace("INSERT", "")
            .Replace("DELETE", "")
            .Replace("REPLACE", "")
            .Replace("TRUNCATE", "")
            .Replace("CREATE", "")
            .Replace("DROP", "")
            .Replace("ALTER", "")
            .Replace("RENAME", "")
            .Replace("MERGE", "")
            .Replace(";", "")
            .Replace("'", "");
        }
        else { return ""; }
        return _out;
    }
    //-----------------------------------------------------------------------------------------
    public static string NomProp(this String persona)
    {
        string Npersona = "";
        for (int i = 0; i < persona.Length; i++)
        {
            if ((i == 0) | (i > 0 && (i + 1) < persona.Length && persona.Substring((i - 1), 1).Equals(" ")))
            {
                Npersona += (persona.Substring(i, 1)).ToUpper();
            }
            else
            {
                Npersona += persona.Substring(i, 1).ToLower();
            }
        }
        return Npersona;
    }
    //-----------------------------------------------------------------------------------------
    public static DataTable Ajuste_DataTable(this DataTable table)
    {
        foreach (DataColumn col in table.Columns)
        {
            col.ReadOnly = false;
            if (col.DataType == typeof(System.String))
            {
                col.MaxLength = 9999999;
            }
        }
        return table;
    }
    //-----------------------------------------------------------------------------------------





































}









//------------------------------------------------------------------------------------------------------
public class M_Post_Header
{
    public string key { get; set; } = "";
    public string value { get; set; } = "";
}
//------------------------------------------------------------------------------------------------------
public class M_Post_Out
{
    public bool is_success { get; set; }
    public System.Net.HttpStatusCode status { get; set; }
    public string? message { get; set; }
    public string? data { get; set; }
    public HttpResponseHeaders? headers { get; set; }
}
//------------------------------------------------------------------------------------------------------




