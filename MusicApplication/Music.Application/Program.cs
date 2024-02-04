using MusicApplication.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterDI()
    .AddAutoMapper()
    .AddMvcService()
    .AddDBContextService();

var app = builder.Build();

// Configure the HTTP request pipeline.3
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();