using log4net;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EasyProject.Util
{
    //회원 - 비밀번호 암호화 알고리즘
    public static class SHA256Hash
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public static string StringToHash(string userPW)
        {
            log.Info("StringToHash(string) invoked.");

            try
            {
                SHA256 sha = new SHA256Managed();
                string salt = CreateSalt(userPW);
                byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(userPW + salt));

                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hash)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }//foreach
                return stringBuilder.ToString();
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }//catch
            
        }//StringToHash(string)


        private static string CreateSalt(string userPW)
        {
            log.Info("CreateSalt(string) invoked.");

            try
            {
                byte[] userBytes;
                string salt;
                userBytes = Encoding.ASCII.GetBytes(userPW);
                long XORED = 0x00;

                foreach (int x in userBytes)
                    XORED = XORED ^ x;

                Random rand = new Random(Convert.ToInt32(XORED));
                salt = rand.Next().ToString();
                salt += rand.Next().ToString();
                salt += rand.Next().ToString();
                salt += rand.Next().ToString();
                return salt;
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }//catch

        }//CreateSalt


    }//class
}//namespace
