using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication()
    .AddCookie("path", op=>
    {
        op.Cookie.Name = "pathcookie";
        //the cookie will only be visible when we are on this path
        // the cookie will be visible when we are at localhost:{port}/path
        op.Cookie.Path = "/Path";

        // by default the browser will attach the cookie and send it on every request to the website
        // 1. None: it will allow the cookie to be attached always.. attacks can happen. The cookie will be sent with all requests 
        // 2. LAX: it will only allow get requests from external websites. if we want another application to redirect to our app and still be authenticated
        // 3. strict: The cookie will be sent with all requests. Only allow request from the same site
        op.Cookie.SameSite = SameSiteMode.None;
    })
    .AddCookie("local", op =>
    {
        // change the name of the cookie and not use the default scheme name
        op.Cookie.Name = "mycookie";

        // this will make the cookie available to be queried from the browser. so it we say
        // document.cookie then the cookie will be available. so never turn this on
        // else we will be able to extract the cookie from the browser
        //op.Cookie.HttpOnly = false;
    });

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
