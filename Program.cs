using cneProyectoVotacion.Services;   // 👈 importante

var builder = WebApplication.CreateBuilder(args);

// 👇 Registrar FirebaseServices como servicio Singleton
builder.Services.AddSingleton<FirebaseServices>();

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();