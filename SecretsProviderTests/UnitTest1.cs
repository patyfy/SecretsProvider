using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretsProvider;

namespace SecretsProviderTests
{
    [TestClass]
    public class AzureKeyVaultSecretProviderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitSecretProvider_WithEmptyConnectionString_ThrowsAnException()
        {
            // Arrange && Act
            var provider = new AzureKeyVaultSecretProvider(null, null, null, null);
        }

        [TestMethod]
        public async Task GetSecretValue_Returns_String()
        {
            // Arrange
            var provider = new AzureKeyVaultSecretProvider();

            // Act
            var secret = await provider.GetSecretValueAsync("key");

            // Assert
            Assert.IsNotNull(secret);
        }
        
        [TestMethod]
        [TestCategory("E2E")]
        public async Task GetSecretValue_Returns_Secret()
        {
            // Arrange
            var expectedValue = "open sesame!";
            var provider = new AzureKeyVaultSecretProvider();

            // Act
            var secret = await provider.GetSecretValueAsync("key");

            // Assert
            Assert.IsNotNull(secret);
            Assert.AreEqual(expectedValue, secret.ToString());
        }

        [TestMethod]
        [TestCategory("E2E")]
        public async Task GetSecretValueUsingConnectionString_Returns_Secret()
        {
            // Arrange
            var expectedValue = "open sesame!";

            var uri = new Uri("");
            var tenantId = "";
            var clientId = "";
            var appKey = "";
            var provider = new AzureKeyVaultSecretProvider(uri, tenantId, clientId, appKey);

            // Act
            var secret = await provider.GetSecretValueAsync("key");

            // Assert
            Assert.IsNotNull(secret);
            Assert.AreEqual(expectedValue, secret.ToString());
        }
    }
}
