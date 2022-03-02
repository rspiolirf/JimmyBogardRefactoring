using JimmyRefactoring.Domain;
using JimmyRefactoring.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("JimmyRefactoringDb"));
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    InitializeDatabase(services.GetRequiredService<AppDbContext>());
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

static void InitializeDatabase(AppDbContext appDbContext)
{
    appDbContext.Members.Add(new Member(1, "mario.bros@gmail.com", "Mario", "Bros"));
    appDbContext.Members.Add(new Member(2, "luigi.bros@gmail.com", "Luigi", "Bros"));
    appDbContext.Members.Add(new Member(3, "princesa.peach@gmail.com", "Princesa", "Peach"));
    appDbContext.Members.Add(new Member(4, "yoshi.dino@hotmail.com", "Yoshi", "Dino"));

    appDbContext.OfferTypes.Add(new OfferType(1, DateTime.Now, 5, ExpirationType.Assignment, "AliExpress.com"));

    appDbContext.SaveChanges();
}