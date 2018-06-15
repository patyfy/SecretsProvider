using System.Threading.Tasks;

namespace Common
{
    public interface IKeyVaultSecretProvider
    {
        Task<string> GetSecretValueAsync(string name);
    }
}
