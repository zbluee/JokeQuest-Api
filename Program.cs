using JokeApi.Configs;
using JokeApi.Services;
using JokeApi.MiddleWares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using Bearer Scheme (\"bearer {token}\")",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,

    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
// builder.Services.AddSingleton<UserServices>();
// builder.Services.AddSingleton<JokeServices>();
builder.Services.AddSingleton<IJokeServices, JokeServices>();
builder.Services.AddSingleton<IUserServices, UserServices>();
builder.Services.AddSingleton<IJwtUtils, JwtUtils>();
// builder.Services.AddSingleton<JwtUtils>();
builder.Services.AddTransient<JwtMiddleware>();
builder.Services.AddTransient<ErrorHandlerMiddleware>();

//configuring jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var Signingkey = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Jwt:Secret").Value);
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Signingkey),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.ConfigureCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();
// add jwt middleware for specific path.
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/Joke"), appBuilder => {
    appBuilder.UseMiddleware<JwtMiddleware>();
});

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
