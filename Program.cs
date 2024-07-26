using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using cityInfo.Api.DbContexts;
using cityInfo.Api.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/citiyinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
//builder.Services.AddControllers(opt =>
//{
//    opt.ReturnHttpNotAcceptable = true;
//}).AddXmlDataContractSerializerFormatters();
builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<CityInfoContext>(dbctxOpt => dbctxOpt.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
/*
    when use implenet working with AutoMapper
*/
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
/*
    when use implemet working with Bearer Auth
*/
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(opt => {
        var SecretKey = builder.Configuration["Authentication:SecretForKey"] ?? "";
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
               Convert.FromBase64String(SecretKey))
        };
    });
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
})
    .AddMvc()
    .AddApiExplorer(setupAction =>
    {
        setupAction.SubstituteApiVersionInUrl = true;
    });
var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
  .GetRequiredService<IApiVersionDescriptionProvider>();
builder.Services.AddSwaggerGen(setupAction =>
{
    foreach (var description in
        apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc(
            $"{description.GroupName}",
            new()
            {
                Title = "City Info API",
                Version = description.ApiVersion.ToString(),
                Description = "Through this API you can access cities and their points of interest."
            });
    }
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    setupAction.IncludeXmlComments(xmlCommentsFullPath);
    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });
    setupAction.AddSecurityRequirement(new()
    {
        {
            new ()
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth" }
            },
            new List<string>()
        }
    });
});
/*
 to start work with azure deploy
 */
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
    | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddProblemDetails(opt =>
{
    opt.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions.Add("problem info", "Additional info");
        ctx.ProblemDetails.Extensions.Add("Server", Environment.MachineName);
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            setupAction.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseForwardedHeaders();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});


app.Run();