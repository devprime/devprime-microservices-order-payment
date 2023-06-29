namespace Core.Tests;
public class CreatePaymentEventHandlerTest
{
    public CreatePayment Create_Payment_Object_OK()
    {
        var payment = PaymentTest.Create_Payment_Required_Properties_OK();
        var createPayment = new CreatePayment();
        DpTest.SetDomainEventObject(createPayment, payment);
        return createPayment;
    }
    [Fact]
    [Trait("EventHandler", "CreatePaymentEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_PaymentObjectFilled_Success()
    {
        //Arrange
        object parameter = null;
        var createPayment = Create_Payment_Object_OK();
        var payment = DpTest.GetDomainEventObject<Domain.Aggregates.Payment.Payment>(createPayment);
        var repositoryMock = new Mock<IPaymentRepository>();
        repositoryMock.Setup((o) => o.Add(payment)).Returns(true).Callback(() =>
        {
            parameter = payment;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IPaymentState>();
        stateMock.SetupGet((o) => o.Payment).Returns(repository);
        var state = stateMock.Object;
        var createPaymentEventHandler = new Application.EventHandlers.Payment.CreatePaymentEventHandler(state, DpTest.MockDp<IPaymentState>(state));
        //Act
        var result = createPaymentEventHandler.Handle(createPayment);
        //Assert
        Assert.Equal(parameter, payment);
        Assert.Equal(result, true);
    }
}