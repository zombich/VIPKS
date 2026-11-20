using KafkaLab;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<NotificationKafkaConsumer>();

var host = builder.Build();
host.Run();
