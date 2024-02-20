using Newtonsoft.Json;

public class Token
{
    //------------------------------------------------------------------------------------------------------------------------------------------------
    public Token_Out L_Valida_Token(string token, string link)
    {
        Token_Out Out = new Token_Out() { respuesta_tipo = "error" };
        try
        {

            if (token != "")
            {
                object obj = new
                {
                    token = token.Replace("\"", "")
                };
                M_Post_Out Post_Resp = Utils.POST(link, null, obj);
                if (Post_Resp.is_success)
                {
                    Out = JsonConvert.DeserializeObject<Token_Out>(Post_Resp.data);
                }
            }
            else
            {
                Out.respuesta_tipo = "warning"; Out.respuesta_detalle = "El token contiene un formato incorrecto";
            }
        }
        catch (Exception ex) { Out.respuesta_tipo = "error"; Out.respuesta_detalle = "El token contiene un formato incorrecto"; }
        return Out;
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------
    public bool Verifica_Permiso(string sis_nombre, string per_nombre, token_data data_tk)
    {
        bool Out = false;
        for (int i = 0; i < data_tk.sistemas.Count; i++)
        {
            if (string.Equals(sis_nombre, data_tk.sistemas[i].sis_nombre))
            {
                for (int j = 0; j < data_tk.sistemas[i].permisos.Count; j++)
                {
                    if (string.Equals(per_nombre, data_tk.sistemas[i].permisos[j].per_nombre))
                    {
                        Out = true;
                    }
                }
            }
        }
        return Out;
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------


}




public class Token_Out
{
    public string respuesta_tipo { get; set; } = "";
    public string respuesta_detalle { get; set; } = "";
    public string token { get; set; } = "";
    public token_data? data { get; set; } = new token_data();
}







public class token_data
{
    public string usu_usuario { get; set; } = "";
    public string usu_correo { get; set; } = "";
    public List<token_data_sistemas> sistemas { get; set; } = new List<token_data_sistemas> { };
}


public class token_data_sistemas
{
    public string sis_nombre { get; set; } = "";
    public List<token_data_permisos> permisos { get; set; } = new List<token_data_permisos> { };
}


public class token_data_permisos
{
    public string per_nombre { get; set; } = "";
    public List<int> pae_id_s { get; set; } = new List<int> { };
}

