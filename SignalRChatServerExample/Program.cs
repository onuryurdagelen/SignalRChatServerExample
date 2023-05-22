using SignalRChatServerExample.Hubs;

var builder = WebApplication.CreateBuilder(args);
//Cors politikasýný ayarlama
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(x => true)));

builder.Services.AddSignalR();


var app = builder.Build();

app.UseCors();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
});

app.Run();
