using APIs;
using Applications.Utils;
using Infrastructures;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddWebAPIService(builder.Configuration);
    builder.Services.Configure<MailSetting>(builder.Configuration.GetSection(nameof(MailSetting)));
    builder.Services.AddCors(cors => cors.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    }));
}

var app = builder.Build();
{
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
}
