using ContactsAPI;
using ContactsAPI.Data;
using ContactsAPI.Repository;
using ContactsAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DeafultConnection")));

builder.Services.AddScoped<IContactRepository, ContactRepository>();//Contact Repository 

builder.Services.AddAutoMapper(typeof(MappingConfig));// Auto Mapper

builder.Services.AddControllers(option =>//idk
{

}).AddNewtonsoftJson();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
