using api.Data;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api.Service;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Add this for the controller
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Stock Management API",
        Version = "V1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat  = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });

});
builder.Services.AddControllers().AddNewtonsoftJson(Options => {

    Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});


//Add this for the database
builder.Services.AddDbContext<ApplicationDBContext>(options => {
     
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//JWT and Identity Core
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme = 
    options.DefaultScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});


//Add this to map interfaces, dependency injection
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<IFMPService, FMPService>();
builder.Services.AddHttpClient<IFMPService, FMPService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>{
          options.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock Management API v1");

    });
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    //.WithOrigins("https://localhost:5005")
    .SetIsOriginAllowed(origin => true)
);

//Add this for the jwt and identity
app.UseAuthentication();
app.UseAuthorization();
//Add this to map the controller
app.MapControllers();

app.Run();

