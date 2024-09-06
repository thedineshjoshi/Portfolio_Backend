using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio_Backend.Data;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbString"));
});
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
var jwtSecret = builder.Configuration.GetValue<string>("JWT:Secret");
var keyBytes = Encoding.ASCII.GetBytes(jwtSecret);


builder.Services.AddAuthentication(O =>
{
    O.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    O.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    O.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    O.RequireAuthenticatedSignIn = false;
}).AddJwtBearer(Options =>
{
    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "localhost:7209",
        ValidAudience = "localhost:4200",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.NameIdentifier
    };
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI( c=> c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio_Backend v1"));
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
