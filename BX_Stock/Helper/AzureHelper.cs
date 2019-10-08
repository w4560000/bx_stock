using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace BX_Stock
{
    public class AzureHelper
    {
        /// <summary>
        /// 取得Azure上的金鑰
        /// </summary>
        /// <param name="secretName">金鑰名稱</param>
        /// <returns></returns>
        public static string GetAzureKeyVault(string secretName)
        {
            KeyVaultClient keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));

            string connectionString = keyVaultClient.GetSecretAsync("https://bingxiangKeyvault.vault.azure.net/", secretName).Result.Value;

            return connectionString;
        }
    }
}