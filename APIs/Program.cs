using APIs;
using Infrastructures;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddWebAPIService();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
