using Microsoft.EntityFrameworkCore;
using PetShopAgendamento.Data;
using PetShopAgendamento.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar suporte a sessão (para login)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();          // ← Primeiro o roteamento

app.UseSession();          // ← Depois a sessão (importante!)

app.UseAuthorization();    // ← Depois autorização (se houver)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Seed: criar admin padrão se não existir
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Usuarios.Any())
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var senhaHash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin123")));
        db.Usuarios.Add(new Usuario
        {
            Nome = "Gerente",
            Login = "admin",
            Senha = senhaHash,
            Perfil = Perfil.Admin,
            Email = "admin@pet.com",
            Cargo = "Gerente"
        });
        db.SaveChanges();
    }
}

app.Run();
