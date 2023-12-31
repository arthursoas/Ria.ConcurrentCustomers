using Ria.ConcurrentCustomers.API.Managers;
using Ria.ConcurrentCustomers.API.Storage;
using Ria.ConcurrentCustomers.API.Storage.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICustomerManager, CustomerManager>();
builder.Services.AddSingleton<ITextCustomerStorage, TextCustomerStorage>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddHttpClient();

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
