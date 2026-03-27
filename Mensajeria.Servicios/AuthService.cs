using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using API.Consumer;
using Mensajeria.Modelos;
using Mensajeria.Servicios.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;


namespace Mensajeria.Servicios
{
    public class AuthService : Interfaces.IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Login(string email, string password)
        {
            var usuarios = CRUD<Usuario>.GetAll();

            foreach (var usuario in usuarios)
            {
                if (usuario.Correo_Usuario == email)
                {
                    //BCrypt compara el texto plano con el Hash almacenado en la base de datos
                    if (BCrypt.Net.BCrypt.Verify(password, usuario.Contrasena_Usuario))
                    {
                        var datosUsuario = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.Nombre_Usuario),
                            new Claim(ClaimTypes.Email, usuario.Correo_Usuario),
                            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                            // Añadimos el rol del usuario como claim para autorización basada en roles
                            new Claim(ClaimTypes.Role, usuario.Rol?.Nombre_Rol ?? (usuario.RolId == 1 ? "Administrador" : "Usuario Estándar"))
                        };

                        var credenciaDigital = new ClaimsIdentity(datosUsuario, "Cookies");
                        var usuarioAutenticado = new ClaimsPrincipal(credenciaDigital);
                        await _httpContextAccessor.HttpContext.SignInAsync("Cookies", usuarioAutenticado);
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> Register(
         string nombre,
         string email,
         string password,
         int? rolId)
        {
            //Verificamos duplicados con endpoints específicos
            var usuarioExistente = CRUD<Usuario>.GetAll()
                 .FirstOrDefault(u => u.Correo_Usuario == email);

            if (usuarioExistente != null)
            {
                Console.WriteLine("Error: El correo ya está registrado.");
                return false;
            }

            try
            {
                //CREACIÓN DEL OBJETO USUARIO CON HASH SEGURIDAD
                var nuevoUsuario = new Usuario
                {
                    Id = 0,
                    Nombre_Usuario = nombre,
                    Correo_Usuario = email,
                    Contrasena_Usuario = BCrypt.Net.BCrypt.HashPassword(password),
                    RolId = rolId ?? 2, 

                    
                    Estado_Usuario = true,
                    Fecha_Creacion_Usuario = DateTime.Now
                };

                CRUD<Usuario>.Create(nuevoUsuario);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                return false;
            }
        }
    }
}
