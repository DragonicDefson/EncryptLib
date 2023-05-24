using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Configuration;
using System.Text;

/*
                                                        @@@                                                                     
                                                      @@@@@@@@@@@@@@@@@@@@@@,                                                   
                                                  %@@@@@@@@,           #@@@@@@@@                                                
                                               %@@@@@@@@@@@&                 @@@@@@    #                                        
                                             @@@@@@@@@@@@@@@                    @@@@@@                                          
                                            @@@@*@@@@@@@@@@@                   @@@@@@@@                                         
                                          @@@@@  @@@@@@@@@@@@@           %@@@@@@@   @@@@&                                       
                                         %@@@@  @@@@@@@@@@@@@@@@   @@@@@@@@@@@       @@@@%                                      
                                         @@@@     @@@@@@@@@@@@@@@@@@@@@@@@@@          @@@@                                      
                                        &@@@        #@@@@@@@@@@@@@@@@@@@@@            @@@@@                                     
                                        @@@@          @@@@@@@@@@@@@@@@@@               @@@@                                     
                                        @@@@      (  @@@@@@@@@@@@@@@@@             &@&.@@@@                                     
                                         @@@@     @@@@@@@@@@@@@@@@@@@            @@@@@@@@@                                      
                                         @@@@    @@@@@@@  #@@@@@@@@@@@@@    @@/      @@@@@                                      
                                          @@@@    @@@@@@          #@@@@@@@          @@@@@                                       
                                           @@@@@    @@@@@          @@@@@@@@@       @@@@@                                        
                                             @@@@@      @@@    &@  @@@@@@@@@@    @@@@@                                          
                                               @@@@@&     @@       @@@     @@@@@@@@@                                            
                                                 %@@@@@@@   %@     @      @@@@@@@&                                              
                                                 #@  @@@@@@@@@@@@@@@@@@@@@@@@@  *@                                              
                                            @@@@           %@@@@@@@@@@@#                                                                                                                                                                                                                                                                                                                  
*/


namespace EncryptLib
{
    public class Security
    {
        private static byte[] PrivateKey;
        private static string Algorithm;
        private static int IVLength;
        private static char IVSeperator;

        public static string AES256Encrypt(string data)
        {
            IBufferedCipher cipherEncrypt = CipherUtilities.GetCipher(Algorithm);
            byte[] vector = new SecureRandom().GenerateSeed(IVLength);
            ICipherParameters parameters = new ParametersWithIV(new KeyParameter(PrivateKey), vector);
            cipherEncrypt.Init(true, parameters);
            byte[] ciphered = cipherEncrypt.DoFinal(Encoding.UTF8.GetBytes(data));
            return $"{Convert.ToBase64String(ciphered)}{IVSeperator}{Convert.ToBase64String(vector)}";
        }

        public static string AES256Decrypt(string data)
        {
            IBufferedCipher cipherDecrypt = CipherUtilities.GetCipher(Algorithm);
            string[] splitData = data.Split(IVSeperator);
            byte[] vector = Convert.FromBase64String(splitData[1]);
            byte[] ciphered = Convert.FromBase64String(splitData[0]);
            ICipherParameters parameters = new ParametersWithIV(new KeyParameter(PrivateKey), vector);
            cipherDecrypt.Init(false, parameters);
            return Encoding.UTF8.GetString(cipherDecrypt.DoFinal(ciphered));
        }

        public static void SetWebConfig (IConfiguration configuration)
        {
            PrivateKey = Encoding.UTF8.GetBytes(configuration["EncryptLib.PrivateKey"]);
            Algorithm = configuration["EncryptLib.Algorithm"];
            IVLength = int.Parse(configuration["EncryptLib.IVLength"]);
            IVSeperator = configuration["EncryptLib.IVSeperator"].ToCharArray()[0];
        }

        public static void SetAppConfig()
        {
            PrivateKey = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings.Get("EncryptLib.PrivateKey"));
            Algorithm = ConfigurationManager.AppSettings.Get("EncryptLib.Algorithm");
            IVLength = int.Parse(ConfigurationManager.AppSettings.Get("EncryptLib.IVLength"));
            IVSeperator = ConfigurationManager.AppSettings.Get("EncryptLib.IVSeperator").ToCharArray()[0];
        }
    }
}
