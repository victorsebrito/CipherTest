using System.Net.Security;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(kestrel =>
{
    kestrel.ConfigureHttpsDefaults(https =>
    {
        https.OnAuthenticate = (connContext, authOptions) =>
        {
            var ciphers = new List<TlsCipherSuite>()
                {
                  TlsCipherSuite.TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256,
                  TlsCipherSuite.TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384,
                };

            authOptions.EnabledSslProtocols = SslProtocols.Tls12;
            authOptions.CipherSuitesPolicy = new CipherSuitesPolicy(ciphers);
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
