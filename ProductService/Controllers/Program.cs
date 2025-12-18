using Controllers.Services;

var builder = WebApplication.CreateBuilder(args);

var redisConfig = builder.Configuration.GetSection("Redis")["Configuration"]
                 ?? builder.Configuration["REDIS_CONNECTION"]
                 ?? throw new InvalidOperationException("Redis configuration is missing.");

// Add services to the container.

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisConfig);
builder.Services.AddSingleton<ICacheService, DistributedCacheService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
