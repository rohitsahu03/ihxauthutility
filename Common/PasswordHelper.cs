using System;
using System.Security.Cryptography;
using System.Text;

namespace AuthUtility.Common {
    public class PasswordHelper {
        public static byte[] StrToByteArray (string strValue) {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding ();
            return encoding.GetBytes (strValue);
        }
        public static string GenerateMD5 (string plainphrase) {
            byte[] tmpSource;
            byte[] tmpHash;
            //Create a byte array from source data.
            tmpSource = ASCIIEncoding.ASCII.GetBytes (plainphrase);
            tmpHash = new MD5CryptoServiceProvider ().ComputeHash (tmpSource);
            // and then convert tmpHash to string...
            string encoded = BitConverter.ToString (tmpHash).Replace ("-", string.Empty).ToLower ();
            return encoded;
        }
    }
}