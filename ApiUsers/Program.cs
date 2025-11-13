using ApiUsers;
using ApiUsers.Data;
using ApiUsers.Repositories;
using ApiUsers.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


var geminiApiKey = builder.Configuration.GetSection("Gemini")["ApiKey"];
builder.Services.AddScoped<GeminiChatService>(_ =>
{
    if (string.IsNullOrEmpty(geminiApiKey))
    {
        throw new InvalidOperationException("Gemini:ApiKey não configurada. Verifique o Secret Manager ou appsettings.json.");
    }
    return new GeminiChatService(geminiApiKey);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//jwt
var key = Encoding.UTF8.GetBytes(Key.secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}
).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        //ValidateLifetime = true,

        //ValidIssuer = builder.Configuration["jwt:issuer"],
        //ValidAudience = ,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
// Registra a interface e implementação do Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeptRepository, DeptRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IStatususerRepository, StatususerRepository>();
builder.Services.AddScoped<IStatusTicketRepository, StatusTicketRepository>();
builder.Services.AddScoped<ICategoty, CategoryRepository>();
builder.Services.AddScoped<ITicket, TicketRepository>();
builder.Services.AddScoped<ITicketTransctionRepository, TicketTransctionRepository>();

builder.Services.AddSingleton<NotificationService>();




// Adiciona controllers (ASP.NET cria automaticamente os controllers)
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ================================
// Configura o pipeline HTTP
// ================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});


app.UseAuthentication();
app.UseAuthorization();
// Mapeia as rotas dos controllers da API
app.MapControllers();

app.Run();
