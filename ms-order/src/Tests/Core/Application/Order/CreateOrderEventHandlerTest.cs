namespace Core.Tests;
public class CreateOrderEventHandlerTest
{
    public CreateOrder Create_Order_Object_OK()
    {
        var order = OrderTest.Create_Order_Required_Properties_OK();
        var createOrder = new CreateOrder();
        DpTest.SetDomainEventObject(createOrder, order);
        return createOrder;
    }
    [Fact]
    [Trait("EventHandler", "CreateOrderEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_OrderObjectFilled_Success()
    {
        //Arrange
        object parameter = null;
        var createOrder = Create_Order_Object_OK();
        var order = DpTest.GetDomainEventObject<Domain.Aggregates.Order.Order>(createOrder);
        var repositoryMock = new Mock<IOrderRepository>();
        repositoryMock.Setup((o) => o.Add(order)).Returns(true).Callback(() =>
        {
            parameter = order;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IOrderState>();
        stateMock.SetupGet((o) => o.Order).Returns(repository);
        var state = stateMock.Object;
        var createOrderEventHandler = new Application.EventHandlers.Order.CreateOrderEventHandler(state, DpTest.MockDp<IOrderState>(state));
        //Act
        var result = createOrderEventHandler.Handle(createOrder);
        //Assert
        Assert.Equal(parameter, order);
        Assert.Equal(result, true);
    }
}