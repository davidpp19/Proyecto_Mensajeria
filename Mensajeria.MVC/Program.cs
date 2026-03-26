using API.Consumer;
using Mensajeria.Modelos;
using Mensajeria.Servicios;
using Mensajeria.Servicios.Interfaces;

CRUD<Rol>.EndPoint = "https://proyecto-mensajeria.onrender.com/api/Roles";
CRUD<EstadoMensaje>.EndPoint = "https://proyecto-mensajeria.onrender.com/api/EstadoMensajes";
CRUD<Conversacion>.EndPoint = "https://proyecto-mensajeria.onrender.com/api/Conversaciones";
CRUD<Usuario>.EndPoint = "https://proyecto-mensajeria.onrender.com/api/Usuarios";
CRUD<Mensaje>.EndPoint = "https://proyecto-mensajeria.onrender.com/api/Mensajes";
CRUD<ParticipanteConversacion>.EndPoint = "https://proyecto-mensajeria.onrender.com/api/ParticipantesConversacion";
CRUD<MensajeLectura>.EndPoint = "https://proyecto-mensajeria.onrender.com/api/MensajeLecturas";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddAuthentication("Cookies") //cokies
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Account/Index"; // Ruta de inicio de sesión


                });
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();
