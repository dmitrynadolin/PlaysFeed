using MassTransit;
using OpenQA.Selenium.Chrome;
using PlaysFeed.Futbolme;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// Add MassTransit services
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
    });
});

// Default trivial implementation of IPageRegistry
builder.Services.AddSingleton<IPageRegistry, PageRegistry>();


var host = builder.Build();
host.Run();
