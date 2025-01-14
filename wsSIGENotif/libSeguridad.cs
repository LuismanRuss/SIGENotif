using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace wsSIGENotif
{
    public static class Hash
    {
        public static string HashTextSHA1(string TextToHash)
        {
            SHA1CryptoServiceProvider SHA1 = null;
            byte[] bytValue = null;
            byte[] bytHash = null;

            SHA1 = new SHA1CryptoServiceProvider();
            bytValue = System.Text.Encoding.UTF8.GetBytes(TextToHash);
            bytHash = SHA1.ComputeHash(bytValue);
            SHA1.Clear();
            return Convert.ToBase64String(bytHash);
        }

        public static string HashTextMD5(string TextToHash)
        {
            MD5CryptoServiceProvider md5 = null;
            byte[] bytValue = null;
            byte[] bytHash = null;

            md5 = new MD5CryptoServiceProvider();
            bytValue = System.Text.Encoding.UTF8.GetBytes(TextToHash);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            return Convert.ToBase64String(bytHash);
        }
    }

    //CRIPTOGRAFÍA BIDIRECCIONAL
    public static class DES
    {
        public static string funDES_ToBase64(string sValue)
        {
            byte[] bytBuff = System.Text.Encoding.UTF8.GetBytes(sValue);
            return Convert.ToBase64String(bytBuff);
        }

        public static string funDES_FromBase64(string sValue)
        {
            byte[] bytBuff = Convert.FromBase64String(sValue);
            return System.Text.Encoding.UTF8.GetString(bytBuff);
        }
    }
}