using System.Net.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cipherSuites = builder.Configuration.GetSection("TlsCipherSuites").Get<List<TlsCipherSuite>>();
var cipherSuitesPolicy = new CipherSuitesPolicy(cipherSuites);

builder.WebHost.ConfigureKestrel(kestrel =>
{
    kestrel.ConfigureHttpsDefaults(https =>
    {
        https.OnAuthenticate = (connContext, authOptions) =>
        {
            authOptions.CipherSuitesPolicy = cipherSuitesPolicy;
        };
    });
});

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
