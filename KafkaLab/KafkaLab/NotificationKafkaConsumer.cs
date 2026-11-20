using Confluent.Kafka;
using KafkaLab.Models;
using System.Text.Json;

namespace KafkaLab
{
    public class NotificationKafkaConsumer : BackgroundService
    {
        public static string FilePath = "notifications.txt";
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => Consume());
        }

        private static void Consume()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "10.9.2.80:9092",
                GroupId = "notification-service-group1",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(new[]
            {
                "order.created",
                "payment.completed",
                "payment.failed",
                "order.cancelled"
            });

            while (true)
            {
                try
                {
                    var result = consumer.Consume();
                    var topic = result.Topic;
                    var json = result.Message.Value;

                    Notification? notification = null;

                    switch (topic)
                    {
                        case "order.created":
                            var created = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

                            notification = new()
                            {
                                EventType = topic,
                                Message = $"Создан заказ с номером - {created.Id}, название - {created.Title}, статус доставки заказа - {created.IsDelivered}, цена - {created.Price} [{created.CreatedDate.ToString()}]"
                            };
                            break;

                        case "order.cancelled":
                            var cancelled = JsonSerializer.Deserialize<OrderCancelledEvent>(json);

                            notification = new()
                            {
                                EventType = topic,
                                Message = $"Отменен заказ с номером - {cancelled.Id}, по причине - {cancelled.Reason} [{cancelled.CancelledDate.ToString()}]"
                            };
                            break;

                        case "payment.failed":
                            var failed = JsonSerializer.Deserialize<PaymentFailedEvent>(json);

                            notification = new()
                            {
                                EventType = topic,
                                Message = $"Не удалось выполнить платеж с номером {failed.PaymentId} для заказа {failed.OrderId} на сумму {failed.Amount}, по причине - {failed.FailureReason} [{failed.ProcessedAt.ToString()}]"
                            };
                            break;

                        case "payment.completed":
                            var completed = JsonSerializer.Deserialize<PaymentCompletedEvent>(json);

                            notification = new()
                            {
                                EventType = topic,
                                Message = $"Выполнен платеж с номером {completed.PaymentId} для заказа {completed.OrderId} на сумму {completed.Amount} [{completed.ProcessedAt.ToString()}]"
                            };
                            break;
                    }
                    if (notification is not null)
                    {
                        Console.WriteLine(notification.Message);
                        Console.WriteLine();

                        var text = $"--------\nId - {notification.Id}\nEvent - {notification.EventType}\nMessage - {notification.Message}\n--------\n\n";
                        File.AppendAllText(FilePath, text);
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
