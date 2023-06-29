namespace Core.Tests;
public class PaymentServiceTest
{
    public Application.Services.Payment.Model.Payment SetupCommand(Action add, Action update, Action delete)
    {
        var domainPaymentMock = new Mock<Domain.Aggregates.Payment.Payment>();
        domainPaymentMock.Setup((o) => o.Add()).Callback(add);
        domainPaymentMock.Setup((o) => o.Update()).Callback(update);
        domainPaymentMock.Setup((o) => o.Delete()).Callback(delete);
        var payment = domainPaymentMock.Object;
        DpTest.MockDpDomain(payment);
        DpTest.Set<string>(payment, "CustomerName", Faker.Lorem.Sentence(1));
        var applicationPaymentMock = new Mock<Application.Services.Payment.Model.Payment>();
        applicationPaymentMock.Setup((o) => o.ToDomain()).Returns(payment);
        var applicationPayment = applicationPaymentMock.Object;
        return applicationPayment;
    }
    public IPaymentService SetupApplicationService()
    {
        var state = new Mock<IPaymentState>().Object;
        var paymentService = new Application.Services.Payment.PaymentService(state, DpTest.MockDp());
        return paymentService;
    }
    [Fact]
    [Trait("ApplicationService", "PaymentService")]
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
        var paymentService = SetupApplicationService();
        //Act
        paymentService.Add(command);
        //Assert
        Assert.True(addCalled);
    }
    [Fact]
    [Trait("ApplicationService", "PaymentService")]
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
        var paymentService = SetupApplicationService();
        //Act
        paymentService.Update(command);
        //Assert
        Assert.True(updateCalled);
    }
    [Fact]
    [Trait("ApplicationService", "PaymentService")]
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
        var paymentService = SetupApplicationService();
        //Act
        paymentService.Delete(command);
        //Assert
        Assert.True(deleteCalled);
    }
}