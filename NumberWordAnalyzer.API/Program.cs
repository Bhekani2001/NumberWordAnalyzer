using NumberWordAnalyzer.Application.Interfaces;
using NumberWordAnalyzer.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NumberWordAnalyzer API", Version = "v1" });
});


builder.Services.AddScoped<INumberWordAnalyzerService, NumberWordAnalyzerService>();
builder.Services.AddScoped<ILogStorageService, LogStorageService>();

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

//I Disabled Redirect for render deployment

// Remove HTTPS redirection for Render
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();