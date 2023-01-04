namespace Core.Tests;
public class PaymentDeletedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.paymentevents", "paymentevents");
        return settings;
    }
    private PaymentDeletedEventDTO SetEventData(Domain.Aggregates.Payment.Payment payment)
    {
        return new PaymentDeletedEventDTO()
        {ID = payment.ID, CustomerName = payment.CustomerName, OrderID = payment.OrderID, Value = payment.Value};
    }

    public PaymentDeleted Create_Payment_Object_OK()
    {
        var payment = PaymentTest.Create_Payment_Required_Properties_OK();
        var paymentDeleted = new PaymentDeleted();
        DpTest.SetDomainEventObject(paymentDeleted, payment);
        return paymentDeleted;
    }
    [Fact]
    [Trait("EventHandler", "PaymentDeletedEventHandler")]
    [Trait("EventHandler", "Success")]

    public void Handle_PaymentObjectFilled_Success()
    {
        //Arrange
        var settings = CustomSettings();
        var paymentDeleted = Create_Payment_Object_OK();
        var payment = DpTest.GetDomainEventObject<Domain.Aggregates.Payment.Payment>(paymentDeleted);
        var paymentDeletedEventHandler = new Application.EventHandlers.Payment.PaymentDeletedEventHandler(null, DpTest.MockDp<IPaymentState>(null));
        DpTest.SetupSettings(paymentDeletedEventHandler.Dp, settings);
        DpTest.SetupStream(paymentDeletedEventHandler.Dp);
        //Act
        var result = paymentDeletedEventHandler.Handle(paymentDeleted);
        //Assert
        var sentEvents = DpTest.GetSentEvents(paymentDeletedEventHandler.Dp);
        var paymentDeletedEventDTO = SetEventData(payment);
        Assert.Equal(sentEvents[0].Destination, settings["stream.paymentevents"]);
        Assert.Equal("PaymentDeleted", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, paymentDeletedEventDTO);
        Assert.Equal(result, true);
    }
}