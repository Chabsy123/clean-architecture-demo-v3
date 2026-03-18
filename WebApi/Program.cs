using Application;
using Infrastructure;
using Persistance;
using Persistance.Seeds;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddInfrastructure();

var app = builder.Build();

//Add this block - it waits for the app to be ready before seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Call your seed methods here instead of in ServiceExtensions
    await DefaultRoles.SeedRolesAsync(services);
    await DefaultUsers.SeedUsersAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
