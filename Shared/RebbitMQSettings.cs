
namespace Shared;

public static class RebbitMQSettings // metinsel  degerleri birkez tutacagımızdan dolayı static yaptık instance olmadan cagiracagimiz için
{
    public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";
    public const string Payment_StockReservedEventQueue = "payment-stock-reserved-event-queue";
    public const string Order_StockNotReservedEventQueue = "order-stock-Not-reserved-event-queue";

    public const string Order_PaymentCompletedEventQueue = "order-payment-completed-event-queue";


     public const string Stock_PaymentFailedEventQueue = "stock-payment-failed-event-queue";
    public const string Order_PaymentFailedEventQueue = "order-payment-failed-event-queue";





}