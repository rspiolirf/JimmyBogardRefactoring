using System.Text.Json.Serialization;
using JimmyRefactoring.Domain;
using JimmyRefactoring.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static JimmyRefactoring.Features.Offers.AssignOffer.AssignOfferRequest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.>
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

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

static void InitializeDatabase(AppDbContext appDbContext)
{
    appDbContext.Members.Add(new Member(1, "rspioli.rf@gmail.com", "Rafael", "Pioli"));
    appDbContext.Members.Add(new Member(2, "felipe@gmail.com", "Felipe", "Pioli"));
    appDbContext.Members.Add(new Member(3, "marcelo.pioli@gmail.com", "Marcelo", "Pioli"));
    appDbContext.Members.Add(new Member(4, "luandra@hotmail.com", "Luandra Pimenta", "Pioli"));

    appDbContext.OfferTypes.Add(new OfferType(1, DateTime.Now, 5, ExpirationType.Fixed, "AliExpress"));

    appDbContext.SaveChanges();
}