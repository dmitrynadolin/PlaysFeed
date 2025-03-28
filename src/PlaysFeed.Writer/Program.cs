using MassTransit;
using Microsoft.EntityFrameworkCore;
using PlaysFeed.DataAccess;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IDeduplicationWriter, DeduplicationWriter>();

// Register Redis connection multiplexer as a singleton
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));


// Connect to the rabbitmq server via MassTransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // These settings match rabbitmq service settings in the compose file.
        // Normally, we would use a configuration file to store the connection settings
        cfg.Host("localhost", 5681, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Register the consumer
        cfg.ReceiveEndpoint("match-results", e =>
        {
            e.ConfigureConsumer<MatchResultsConsumer>(context);
        });
    });

    x.AddConsumer<MatchResultsConsumer>(consumer =>
    {
        consumer.Options<BatchOptions>(o =>
        {
            o.SetConcurrencyLimit(1).SetMessageLimit(100).SetTimeLimit(s: 10);
        });
    });
});


// Register a DbContext
var connectionString = builder.Configuration.GetConnectionString(nameof(GamesDbContext));

builder.Services.AddDbContext<GamesDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


var host = builder.Build();

// Ensure the database is created. In the real production scenario, we would use migrations.
using(var scope = host.Services.CreateScope())
{
    using var db = scope.ServiceProvider.GetRequiredService<GamesDbContext>();
    db.Database.EnsureCreated();
}

// Seed the database with initial data
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GamesDbContext>();
    var seeder = new DataSeeder(db);
    seeder.Seed();
}

host.Run();