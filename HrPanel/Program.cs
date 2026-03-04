using HrPanel.Components;
using HrPanel.Data;
using HrPanel.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<LeaveService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<PositionService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddCascadingAuthenticationState(); // Kimlik bilgisini tüm sayfalara yayar
builder.Services.AddAuthenticationCore(); // Temel auth altyapısını kurar
builder.Services.AddAuthentication("BoşŞema")
    .AddCookie("BoşŞema", options =>
    {
        options.LoginPath = "/login";
    });
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();