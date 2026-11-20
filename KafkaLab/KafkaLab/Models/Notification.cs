namespace KafkaLab.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EventType { get; set; }
        public string Message { get; set; }
    }

    public record OrderCreatedEvent(int Id, string Title, int Price, DateTime CreatedDate, bool IsDelivered);
    public record OrderCancelledEvent(int Id, string Reason, DateTime CancelledDate);
    public record PaymentCompletedEvent(int PaymentId, int OrderId, decimal Amount, string Status, string? FailureReason, DateTime ProcessedAt);
    public record PaymentFailedEvent(int PaymentId, int OrderId, decimal Amount, string Status, string? FailureReason, DateTime ProcessedAt);
}
