using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Servicios.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(string username, string password);

        Task<bool> Register(
            string nombre,
            string email,
            string password
        );
    }
}
