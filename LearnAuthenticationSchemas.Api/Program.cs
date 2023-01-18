using LearnAuthenticationSchemas.Api.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// A scheme will specify the way authentication takes place,
// if i have a scheme that configures OAuth then when I use a challenge and specify the scheme name
//.. it will use the configured scheme to challenge the user. determines which authentication scheme should be used to authenticate.
// Add services to the container.
builder.Services.AddAuthentication("local")
    //.AddScheme<CookieAuthenticationOptions, CookieAuthHandler>("local", o => { })
    .AddCookie("local", op =>
    {
        op.LoginPath = new PathString("/api/auth/LoginLocal");
    })
    .AddCookie("Customer", op =>
    {
        op.LoginPath = new PathString("/api/auth/LoginCustomer");
    });

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("customer", p =>
    //{
    //    p.AddAuthenticationSchemes("local", "Customer").RequireAuthenticatedUser();
    //});

    options.AddPolicy("onlyCustomer", p =>
    {
        p.RequireClaim("role", "Customer");
    });

    options.AddPolicy("onlylocal", p =>
    {
        p.RequireClaim("role", "local");
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



