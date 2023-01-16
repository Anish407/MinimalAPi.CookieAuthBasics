using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDataProtection();

builder.Services.AddAuthentication()
    .AddCookie("mycookie");

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

// similar to what app.useAuthentication() works
app.Use(async (context, next) =>
{
    var idp = context.RequestServices.GetRequiredService<IDataProtectionProvider>();
    var protector = idp.CreateProtector("cookie1");

    // read the data in the cookie from the headers
    var cookie = context.Request.Headers.Cookie.FirstOrDefault(i => i.StartsWith("name="));
    if (!string.IsNullOrEmpty(cookie))
    {
        var encryptedData = cookie.Split("=").Last();
        var data = protector.Unprotect(encryptedData);

        var claims = new List<Claim>()
        {
            // assign the value read from the cookie to a claim
            new Claim("name",data),
            new Claim("age","34"),
            new Claim("email","a@b.com"),
        };
        var identity = new ClaimsIdentity(claims);

        //the schema name will let us know which schema to use and how to
        // decrypt the cookie
        //var identity = new ClaimsIdentity(claims, "schema_name");
        context.User = new ClaimsPrincipal(identity);
    }


    await next(context);

});

app.MapControllers();

app.Run();
