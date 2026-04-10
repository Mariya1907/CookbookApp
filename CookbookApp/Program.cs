using CookbookApp.Services;
using CookbookApp.Components;  // ← App.razor в Components!

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IRecipeService, RecipeService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()  // ← ПРОСТО App (из Components)
    .AddInteractiveServerRenderMode();

Console.WriteLine("🚀 http://localhost:5000");
app.Run("http://localhost:5000");