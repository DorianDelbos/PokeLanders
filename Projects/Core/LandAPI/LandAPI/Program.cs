using LandAPI.API.Data;
using LandAPI.API.Services;
using LandAPI.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Add Razor Service
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/landopedia"));

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthorization();
app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();