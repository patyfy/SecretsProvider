using Common;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Threading.Tasks;

namespace SecretsProvider
{
    public class AzureKeyVaultSecretProvider : IKeyVaultSecretProvider
    {
        private Uri _vaultUri;
        private KeyVaultClient _vaultClient;
        private AzureServiceTokenProvider _azureServiceTokenProvider;

        public AzureKeyVaultSecretProvider(Uri uri)
        {
            this.InitializeSecretProvider(uri);
        }

        public AzureKeyVaultSecretProvider(Uri uri, string tenantId, string clientId, string secret)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            if (string.IsNullOrEmpty(tenantId)) throw new ArgumentNullException("tenantId");
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException("clientId");
            if (string.IsNullOrEmpty(secret)) throw new ArgumentNullException("secret");

            var connectionString = $"tenantId={tenantId};appId={clientId};appKey={secret};RunAs=App";

            this.InitializeSecretProvider(uri, connectionString);
        }

        private void InitializeSecretProvider(Uri uri, string connectionString = null)
        {
            _vaultUri = uri;

            _azureServiceTokenProvider = new AzureServiceTokenProvider(connectionString);

            _vaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(_azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        public async Task<string> GetSecretValueAsync(string name)
        {
            var secret = await _vaultClient.GetSecretAsync($"{_vaultUri}/{name}")
                .ConfigureAwait(false);

            return secret.Value;
        }
    }
}
