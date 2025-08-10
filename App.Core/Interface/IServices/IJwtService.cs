
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IJwtService
    {
        string Authenticate(int userId, string userName, string Email, string Role);
    }
}
