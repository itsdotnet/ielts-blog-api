using IELTSBlog.Api.Configurations;
using IELTSBlog.Api.Middlewares;
using IELTSBlog.Repository.Contexts;
using IELTSBlog.Service.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

//Custom services
builder.Services.AddServices();

//JWT
builder.Services.AddJwt(builder.Configuration);

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ConfigureSwagger();

//Lowercase route
builder.Services.AddRouting(options => options.LowercaseUrls = true);

//Web root path
PathHelper.WebRootPath = Path.GetFullPath("wwwroot");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
