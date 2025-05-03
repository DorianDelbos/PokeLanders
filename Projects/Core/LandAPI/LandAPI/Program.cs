using LandAPI.API;
using LandAPI.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(5000); });

// Add services to the container.
builder.Services.AddControllers();

// Add Razor Service
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Add services to the container.
builder.Services.AddScoped<LanderRepository>();
builder.Services.AddScoped<LanderService>();
builder.Services.AddScoped<TypeRepository>();
builder.Services.AddScoped<TypeService>();
builder.Services.AddScoped<StatRepository>();
builder.Services.AddScoped<StatService>();
builder.Services.AddScoped<EvolutionChainRepository>();
builder.Services.AddScoped<EvolutionChainService>();
builder.Services.AddScoped<AilmentRepository>();
builder.Services.AddScoped<AilmentService>();
builder.Services.AddScoped<MoveRepository>();
builder.Services.AddScoped<MoveService>();
builder.Services.AddScoped<NatureRepository>();
builder.Services.AddScoped<NatureService>();

WebApplication app = builder.Build();

app.MapGet("/", () => Results.Redirect("/landopedia"));

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthorization();
app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
