using API.Consumer;
using Mensajeria.Modelos;

CRUD<Rol>.EndPoint = "https://localhost:7073/api/Roles";
CRUD<EstadoMensaje>.EndPoint = "https://localhost:7073/api/EstadoMensajes";
CRUD<Conversacion>.EndPoint = "https://localhost:7073/api/Conversaciones";
CRUD<Usuario>.EndPoint = "https://localhost:7073/api/Usuarios";
CRUD<Mensaje>.EndPoint = "https://localhost:7073/api/Mensajes";
CRUD<ParticipanteConversacion>.EndPoint = "https://localhost:7073/api/ParticipantesConversacion";
CRUD<MensajeLectura>.EndPoint = "https://localhost:7073/api/MensajeLecturas";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
