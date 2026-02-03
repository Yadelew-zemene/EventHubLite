using EventHub.Api.Data;
using Microsoft.EntityFrameworkCore;
using EventHub.Api.Dtos;
using EventHub.Api.Entities;
using EventHub.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;





var builder = WebApplication.CreateBuilder(args);

// ---------- Database ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });
builder.Services.AddSingleton<JwtService>();

// ---------- Authentication & Authorization ----------
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// ---------- Build App ----------
var app = builder.Build();

// ---------- Middleware ----------
app.UseAuthentication();
app.UseAuthorization();

// ---------- Minimal Test Endpoint ----------
app.MapGet("/", () => "EventHub Lite API is running!");
app.MapGet("/test-users", async (AppDbContext db) =>
{
    var users = await db.Users.ToListAsync();
    return users;
});
//Post for registration
app.MapPost("/auth/register", async (RegisterDto dto, AppDbContext db, JwtService jwtServices) =>
{
    if (await db.Users.AnyAsync(u => u.Email == dto.Email))
    {
        Results.BadRequest(new { message="Email already registered" });
    };
    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
    var newUser = new User
    {
        FullName = dto.FullName,
        Email = dto.Email,
        PasswordHashed = hashedPassword

    };
    db.Users.Add(newUser);
    await db.SaveChangesAsync();

    var token = jwtServices.GenerateToken(newUser);
    return Results.Ok(new
    {
        newUser.FullName,
        newUser.Email,
        token

} );

});
//post for ligin
app.MapPost("/auth/login", async (LoginDto dto, AppDbContext db, JwtService jwtServices) =>
{
    var loginUser = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
    if (loginUser == null)
    {
        Results.BadRequest("Invalid Email or Password");
    }
    var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, loginUser.PasswordHashed);
    if (!validPassword)
    {
        return Results.BadRequest("Invalid Password");

    }
    var token = jwtServices.GenerateToken(loginUser);
    return Results.Ok(
   new
   {
       loginUser.Id,
       loginUser.FullName,
       loginUser.Email,
       token
   }
    );



});
app.MapGet("/me", [Authorize] (ClaimsPrincipal user) =>
{
    return new
    {
        Email = user.FindFirst(ClaimTypes.Email)?.Value,
        Role = user.FindFirst(ClaimTypes.Role)?.Value
    };
});
app.MapPost("/admin/only",
    [Authorize(Roles = "Admin")] () =>
{
    return "Welcome Admin";
});
app.MapPost("/events",
[Authorize(Roles = "Organizer,Admin")]
async (CreateEventDto dto, AppDbContext db, ClaimsPrincipal user) =>
{
    var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var ev = new Event
    {
        Title = dto.Title,
        Description = dto.Description,
        Date = dto.Date,
        OrganizerId = userId
    };

    db.Events.Add(ev);
    await db.SaveChangesAsync();

    return Results.Ok(ev);
});



app.UseAuthentication();
app.UseAuthorization();
// ---------- Run ----------
app.Run();
