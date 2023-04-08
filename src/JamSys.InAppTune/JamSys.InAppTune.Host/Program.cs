using JamSys.InAppTune.Host;
using JamSys.InAppTune.Host.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using JamSys.InAppTune.Host.Jobs;
using JamSys.InAppTune.Agent;
using JamSys.InAppTune.Host.Components;
using JamSys.InAppTune.Knobs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables()
        .AddJsonFile($"appSettings.json", true, reloadOnChange: true)
        .AddJsonFile($"appSettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, reloadOnChange: true)
        .AddJsonFile($"appSettings.{Environment.GetEnvironmentVariable("KNOBS")}.json", true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

string databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider");
var connString = builder.Configuration.GetConnectionString(databaseProvider + "Connection");


builder.Services.AddDbContext<TpccContext>(options =>
{
    if(databaseProvider.Equals("MySQL"))
        options.UseMySql(connString, ServerVersion.AutoDetect(connString));
    else if(databaseProvider.Equals("Postgres"))
        options.UseNpgsql(connString);
}, ServiceLifetime.Transient);


//We apply the same configuration to Tuning Agent as the main database
TuningAgent.Instance.ConfigureDatabase(options => 
{
    if(databaseProvider.Equals("MySQL"))
        options.UseMySql(connString, ServerVersion.AutoDetect(connString));
    else if(databaseProvider.Equals("Postgres"))
        options.UseNpgsql(connString);
});

var knobs =  builder.Configuration.GetSection(databaseProvider + "Knobs").Get<List<Knob>>();
TuningAgent.Instance.ConfigureKnobs(databaseProvider, knobs);

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddScoped<Stats>();
builder.Services.AddScoped<WarehouseCreationJob>();
builder.Services.AddScoped<ItemCreationJob>();
builder.Services.AddScoped<CustomerCreationJob>();
builder.Services.AddScoped<TrainNetworkJob>();
builder.Services.AddScoped<TuneJob>();

builder.Services.AddSingleton<Store>();

builder.Services.AddScoped<ReadOnlyWorkloadJob>();
builder.Services.AddScoped<ReadWriteWorkloadJob>();
//builder.Services.AddScoped<StockLevelTransaction>();


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

app.Run();
