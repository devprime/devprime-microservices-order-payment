namespace Core.Tests;
public class PaymentUpdatedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.paymentevents", "paymentevents");
        return settings;
    }
    private PaymentUpdatedEventDTO SetEventData(Domain.Aggregates.Payment.Payment payment)
    {
        return new PaymentUpdatedEventDTO()
        {ID = payment.ID, CustomerName = payment.CustomerName, OrderID = payment.OrderID, Value = payment.Value};
    }

    public PaymentUpdated Create_Payment_Object_OK()
    {
        var payment = PaymentTest.Create_Payment_Required_Properties_OK();
        var paymentUpdated = new PaymentUpdated();
        DpTest.SetDomainEventObject(paymentUpdated, payment);
        return paymentUpdated;
    }
    [Fact]
    [Trait("EventHandler", "PaymentUpdatedEventHandler")]
    [Trait("EventHandler", "Success")]

    public void Handle_PaymentObjectFilled_Success()
    {
        //Arrange
        var settings = CustomSettings();
        var paymentUpdated = Create_Payment_Object_OK();
        var payment = DpTest.GetDomainEventObject<Domain.Aggregates.Payment.Payment>(paymentUpdated);
        var paymentUpdatedEventHandler = new Application.EventHandlers.Payment.PaymentUpdatedEventHandler(null, DpTest.MockDp<IPaymentState>(null));
        DpTest.SetupSettings(paymentUpdatedEventHandler.Dp, settings);
        DpTest.SetupStream(paymentUpdatedEventHandler.Dp);
        //Act
        var result = paymentUpdatedEventHandler.Handle(paymentUpdated);
        //Assert
        var sentEvents = DpTest.GetSentEvents(paymentUpdatedEventHandler.Dp);
        var paymentUpdatedEventDTO = SetEventData(payment);
        Assert.Equal(sentEvents[0].Destination, settings["stream.paymentevents"]);
        Assert.Equal("PaymentUpdated", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, paymentUpdatedEventDTO);
        Assert.Equal(result, true);
    }
}