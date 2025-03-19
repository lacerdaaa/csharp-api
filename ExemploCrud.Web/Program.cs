using ExemploCrud.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//adicionando o db context
builder.Services.AddDbContext<AppDb>(
    options => options.UseSqlite("Data Source=employee.db")
    );

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.EnsureCreated();
};

app.MapGet("/funcionarios", async (AppDb db) => await db.Funcionarios.ToListAsync());
app.MapGet("/funcionarios/{id}", async (int id, AppDb db) =>
    await db.Funcionarios.FindAsync(id) is Funcionario funcionario ? Results.Ok(funcionario) : Results.NotFound());
app.MapPost("/funcionarios", async (Funcionario funcionario, AppDb db) =>
{
    db.Funcionarios.Add(funcionario);
    await db.SaveChangesAsync();
    return Results.Created($"/funcionarios/{funcionario.Id}", funcionario);
});
app.MapPut("/funcionarios/{id}", async (int id, Funcionario inputFuncionario, AppDb db) =>
{
    var funcionario = await db.Funcionarios.FindAsync(id);
    if (funcionario is null) return Results.NotFound();

    funcionario.Nome = inputFuncionario.Nome;
    funcionario.Cargo = inputFuncionario.Cargo;

    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/funcionarios/{id}", async (int id, AppDb db) =>
{
    if (await db.Funcionarios.FindAsync(id) is Funcionario funcionario)
    {
        db.Funcionarios.Remove(funcionario);
        await db.SaveChangesAsync();
        return Results.Ok(funcionario);
    }

    return Results.NotFound();
});

app.Run();

public class Funcionario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; }
}

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
}
