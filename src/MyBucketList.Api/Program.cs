using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using MyBucketList.Api.Features.BucketItem.Commands;
using MyBucketList.Api.Features.BucketItem.Queries;
using MyBucketList.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        );
    }));

// Enable detailed errors and sensitive data logging for development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.EnableDetailedErrors()
               .EnableSensitiveDataLogging());  
}

// Add services to the container.
builder.Services.AddControllers();

// Inject the services for the application
builder.Services.AddScoped<BucketItemCreateCommandHandler>();
builder.Services.AddScoped<GetAllBucketItemsQueryHandler>();

// Configure API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "MyBucketList API",
        Description = "API for managing bucket list items - Version 1",
    });
    
    // Add more versions as needed
    // options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    // {
    //     Version = "v2",
    //     Title = "MyBucketList API",
    //     Description = "API for managing bucket list items - Version 2",
    // });
});

var app = builder.Build();

// Auto-apply migrations in development (optional - use with caution)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Ensure database is created and migrations are applied
        dbContext.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBucketList API v1");
        // Add more versions as needed
        // options.SwaggerEndpoint("/swagger/v2/swagger.json", "MyBucketList API v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
