using NumberWordAnalyzer.Application.Interfaces;
using NumberWordAnalyzer.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NumberWordAnalyzer API", Version = "v1" });
});

// Register our services
builder.Services.AddScoped<INumberWordAnalyzerService, NumberWordAnalyzerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NumberWordAnalyzer API v1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NumberWordAnalyzer API v1");
        c.RoutePrefix = "swagger";
    });
}
//Disable Redirect

// Remove HTTPS redirection for Render
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();