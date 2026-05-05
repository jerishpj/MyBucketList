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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
