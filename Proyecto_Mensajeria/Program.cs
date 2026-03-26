using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mensajeria.API.Data;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MensajeriaAPIContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MensajeriaBDD") ?? throw new InvalidOperationException("Connection string 'MensajeriaBDD' not found.")));

// Add services to the container.
//Indicamos que en el AddController vamos a serializar.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
