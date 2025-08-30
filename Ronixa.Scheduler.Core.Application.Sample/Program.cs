var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Register Ronixa Services to Ioc
builder.Services.AddRonixaSchedulingJob<SampleJob>(c =>
{
    c.Duration = TimeSpan.FromSeconds(10);
    c.ForceExecuteInStartJob = true;
});

builder.Services.AddRonixaJobManagerService();
#endregion

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
