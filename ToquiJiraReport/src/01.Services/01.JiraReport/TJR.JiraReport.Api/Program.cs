using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Poc.Jira.Doc.Api;
using System.Reflection;
using TJR.JiraReport.Services.Proxies;
using TJR.JiraReport.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// apiurls
builder.Services.Configure<ApiUrls>(options => builder.Configuration.GetSection("ApiUrls").Bind(options));

// HttpContextAccessor to make calls to proxies and get claims principal
builder.Services.AddHttpContextAccessor();

// Proxies
builder.Services.AddHttpClient<IJiraProxy, JiraProxy>();

// add mediatr para el patron mediador
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.Load("TJR.JiraReport.Services")));

builder.Services.AddScoped<IJiraRepository, JiraRepository>();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5173/").AllowAnyHeader().AllowAnyMethod();
    });
});


builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
