namespace Core.Tests;
public class OrderCreatedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.orderevents", "orderevents");
        return settings;
    }
    private OrderCreatedEventDTO SetEventData(Domain.Aggregates.Order.Order order)
    {
        return new OrderCreatedEventDTO()
        {ID = order.ID, CustomerName = order.CustomerName, CustomerTaxID = order.CustomerTaxID, Total = order.Total};
    }
    public OrderCreated Create_Order_Object_OK()
    {
        var order = OrderTest.Create_Order_Required_Properties_OK();
        var orderCreated = new OrderCreated();
        DpTest.SetDomainEventObject(orderCreated, order);
        return orderCreated;
    }
    [Fact]
    [Trait("EventHandler", "OrderCreatedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_OrderObjectFilled_Success()
    {
        //Arrange
        var settings = CustomSettings();
        var orderCreated = Create_Order_Object_OK();
        var order = DpTest.GetDomainEventObject<Domain.Aggregates.Order.Order>(orderCreated);
        var orderCreatedEventHandler = new Application.EventHandlers.Order.OrderCreatedEventHandler(null, DpTest.MockDp<IOrderState>(null));
        DpTest.SetupSettings(orderCreatedEventHandler.Dp, settings);
        DpTest.SetupStream(orderCreatedEventHandler.Dp);
        //Act
        var result = orderCreatedEventHandler.Handle(orderCreated);
        //Assert
        var sentEvents = DpTest.GetSentEvents(orderCreatedEventHandler.Dp);
        var orderCreatedEventDTO = SetEventData(order);
        Assert.Equal(sentEvents[0].Destination, settings["stream.orderevents"]);
        Assert.Equal("OrderCreated", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, orderCreatedEventDTO);
        Assert.Equal(result, true);
    }
}