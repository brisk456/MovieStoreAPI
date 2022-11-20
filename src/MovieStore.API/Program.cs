using Microsoft.EntityFrameworkCore;
using MoviesStore.Infrastructure.Context;
using MovieStore.API.Configuration;
using MovieStore.API.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJwtConfiguration(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MovieStoreDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers();

builder.Services.ResolveDependencies();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerConfiguration();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
