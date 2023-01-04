namespace Core.Tests;
public class DeleteOrderEventHandlerTest
{
    public DeleteOrder Create_Order_Object_OK()
    {
        var order = OrderTest.Create_Order_Required_Properties_OK();
        var deleteOrder = new DeleteOrder();
        DpTest.SetDomainEventObject(deleteOrder, order);
        return deleteOrder;
    }
    [Fact]
    [Trait("EventHandler", "DeleteOrderEventHandler")]
    [Trait("EventHandler", "Success")]

    public void Handle_OrderObjectFilled_Success()
    {
        //Arrange
        object parameter = null;
        var deleteOrder = Create_Order_Object_OK();
        var order = DpTest.GetDomainEventObject<Domain.Aggregates.Order.Order>(deleteOrder);
        var repositoryMock = new Mock<IOrderRepository>();
        repositoryMock.Setup((o) => o.Delete(order.ID)).Returns(true).Callback(() =>
        {
            parameter = order;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IOrderState>();
        stateMock.SetupGet((o) => o.Order).Returns(repository);
        var state = stateMock.Object;
        var deleteOrderEventHandler = new Application.EventHandlers.Order.DeleteOrderEventHandler(state, DpTest.MockDp<IOrderState>(state));
        //Act
        var result = deleteOrderEventHandler.Handle(deleteOrder);
        //Assert
        Assert.Equal(parameter, order);
        Assert.Equal(result, true);
    }
}