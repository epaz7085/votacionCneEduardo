using votacionCneEduardo.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddSingleton<FirebaseServices>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
