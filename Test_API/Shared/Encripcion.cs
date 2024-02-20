using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class Encripcion
{
    //-----------------------------------------------------------------------------------------
    public static string EncriptarN(this string cadena, string key)
    {
        byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.
        byte[] arreglo = UTF8Encoding.UTF8.GetBytes(cadena); //Arreglo donde guardaremos la cadena descifrada.
                                                             // Ciframos utilizando el Algoritmo MD5.
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        md5.Clear();
        //Ciframos utilizando el Algoritmo 3DES.
        TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
        tripledes.Key = llave;
        tripledes.Mode = CipherMode.ECB;
        tripledes.Padding = PaddingMode.PKCS7;
        ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
        byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
        tripledes.Clear();
        string salida = Convert.ToBase64String(resultado, 0, resultado.Length); // Convertimos la cadena y la regresamos.
        salida = salida.Replace("+", "-");
        salida = salida.Replace("/", "_");
        salida = salida.Replace("=", ":");
        return salida;
    }
    //-----------------------------------------------------------------------------------------
    public static string DesEncriptarN(this string cadena, string key)
    {
        string entrada = cadena.Replace("-", "+").Replace("_", "/").Replace(":", "=");
        byte[] llave;
        byte[] arreglo = Convert.FromBase64String(entrada); // Arreglo donde guardaremos la cadena descovertida.
                                                            // Ciframos utilizando el Algoritmo MD5.
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        md5.Clear();
        //Ciframos utilizando el Algoritmo 3DES.
        TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
        tripledes.Key = llave;
        tripledes.Mode = CipherMode.ECB;
        tripledes.Padding = PaddingMode.PKCS7;
        ICryptoTransform convertir = tripledes.CreateDecryptor();
        byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
        tripledes.Clear();
        string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado); // Obtenemos la cadena
        return cadena_descifrada; // Devolvemos la cadena
    }
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    public static string Encriptar64(this string _cadenaAencriptar)
    {
        string result = string.Empty;
        byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
        result = Convert.ToBase64String(encryted);
        return result;
    }
    //-----------------------------------------------------------------------------------------
    public static string DesEncriptar64(this string _cadenaAdesencriptar)
    {
        string result = string.Empty;
        byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
        result = System.Text.Encoding.Unicode.GetString(decryted);
        return result;
    }
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    public static string GetMD5(string str)
    {
        MD5 md5 = MD5CryptoServiceProvider.Create();
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] stream = null;
        StringBuilder sb = new StringBuilder();
        stream = md5.ComputeHash(encoding.GetBytes(str));
        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
        return sb.ToString();
    }
    //-----------------------------------------------------------------------------------------










}







