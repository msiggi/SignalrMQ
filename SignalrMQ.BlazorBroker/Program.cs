using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SignalrMQ.Broker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseEndpoints(endpoints => endpoints.MapHub<SignalrMqBroker>("signalrmqbrokerhub",
    configureOptions =>
    {
     //           configureOptions.TransportMaxBufferSize = 10 * 1024 * 1024; 
    }));

app.Run();
