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
}

    app.MapGet("/", () => "Hello World!");

app.Run();

