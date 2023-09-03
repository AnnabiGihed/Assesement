using Newtonsoft.Json;
using Assessment.Application.Extensions;
using Assessment.Persistence.Extensions;
using Assessment.Infrastructure.Extensions;
using System.Globalization;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddControllers();

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.DefaultModelRendering(ModelRendering.Example);
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
		options.DefaultModelsExpandDepth(-1);
		options.DefaultModelRendering(ModelRendering.Example);
	});
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
