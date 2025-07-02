using Compunnel.Multichannel.Messaging.Application;
using Compunnel.Multichannel.Messaging.Application.Interface;
using Compunnel.Multichannel.Messaging.BackgroudJob.Host_RCS;
using Compunnel.Multichannel.Messaging.Domain;
using Compunnel.Multichannel.Messaging.Infrastructure.Data;
using Compunnel.Multichannel.Messaging.Infrastructure.DatContextContext;
using Compunnel.Multichannel.Messaging.Infrastructure.MessageBroker;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.Channels;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build())
    .CreateLogger();

try
{
    Log.Information("Starting Rcs Worker Service...");
    IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog() // 2. Plug Serilog into the host builder
    .ConfigureServices((hostContext, services) =>
    {

        services.AddTransient<IListnerService, ListnerService>();
        services.AddTransient<IMessageBrokerService, RabbitMQMessageBrokerService>();

        services.AddSingleton<IMessageProcessor, MessageProcessor>();

        services.AddSingleton<IMessageRepository, MessageRepository>();
        services.AddSingleton<ITokenRepository, TokenRepository>();

        var configuration = hostContext.Configuration;
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));

        string dbsetting = configuration["DBSettings:connection"] + "Password=" + configuration["DBSettings:Pass"];

        services.AddDbContext<ApplicationContext>(optionsBuilder => optionsBuilder.UseNpgsql(dbsetting));

        var _channelSubscribedExchangeMessage = Channel.CreateUnbounded<MessageData>();

        services.AddSingleton(_channelSubscribedExchangeMessage);

        services.AddHostedService<RcsWorker>();
    })
    .Build();



    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Rcs Worker Service terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}