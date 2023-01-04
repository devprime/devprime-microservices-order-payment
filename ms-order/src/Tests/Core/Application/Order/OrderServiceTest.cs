namespace Core.Tests;
public class OrderServiceTest
{
    public Application.Services.Order.Model.Order SetupCommand(Action add, Action update, Action delete)
    {
        var domainOrderMock = new Mock<Domain.Aggregates.Order.Order>();
        domainOrderMock.Setup((o) => o.Add()).Callback(add);
        domainOrderMock.Setup((o) => o.Update()).Callback(update);
        domainOrderMock.Setup((o) => o.Delete()).Callback(delete);
        var order = domainOrderMock.Object;
        DpTest.MockDpDomain(order);
        DpTest.Set<string>(order, "CustomerName", Faker.Lorem.Sentence(1));
        DpTest.Set<string>(order, "CustomerTaxID", Faker.Lorem.Sentence(1));
        var applicationOrderMock = new Mock<Application.Services.Order.Model.Order>();
        applicationOrderMock.Setup((o) => o.ToDomain()).Returns(order);
        var applicationOrder = applicationOrderMock.Object;
        return applicationOrder;
    }

    public IOrderService SetupApplicationService()
    {
        var state = new Mock<IOrderState>().Object;
        var orderService = new Application.Services.Order.OrderService(state, DpTest.MockDp());
        return orderService;
    }
    [Fact]
    [Trait("ApplicationService", "OrderService")]
    [Trait("ApplicationService", "Success")]

    public void Add_CommandNotNull_Success()
    {
        //Arrange
        var addCalled = false;
        var add = () =>
        {
            addCalled = true;
        };
        var command = SetupCommand(add, () =>
        {
        }, () =>
        {
        });
        var orderService = SetupApplicationService();
        //Act
        orderService.Add(command);
        //Assert
        Assert.True(addCalled);
    }
    [Fact]
    [Trait("ApplicationService", "OrderService")]
    [Trait("ApplicationService", "Success")]

    public void Update_CommandFilled_Success()
    {
        //Arrange
        var updateCalled = false;
        var update = () =>
        {
            updateCalled = true;
        };
        var command = SetupCommand(() =>
        {
        }, update, () =>
        {
        });
        var orderService = SetupApplicationService();
        //Act
        orderService.Update(command);
        //Assert
        Assert.True(updateCalled);
    }
    [Fact]
    [Trait("ApplicationService", "OrderService")]
    [Trait("ApplicationService", "Success")]

    public void Delete_CommandFilled_Success()
    {
        //Arrange
        var deleteCalled = false;
        var delete = () =>
        {
            deleteCalled = true;
        };
        var command = SetupCommand(() =>
        {
        }, () =>
        {
        }, delete);
        var orderService = SetupApplicationService();
        //Act
        orderService.Delete(command);
        //Assert
        Assert.True(deleteCalled);
    }
}