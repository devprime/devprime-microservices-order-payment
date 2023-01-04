namespace Core.Tests;
public class DeletePaymentEventHandlerTest
{
    public DeletePayment Create_Payment_Object_OK()
    {
        var payment = PaymentTest.Create_Payment_Required_Properties_OK();
        var deletePayment = new DeletePayment();
        DpTest.SetDomainEventObject(deletePayment, payment);
        return deletePayment;
    }
    [Fact]
    [Trait("EventHandler", "DeletePaymentEventHandler")]
    [Trait("EventHandler", "Success")]

    public void Handle_PaymentObjectFilled_Success()
    {
        //Arrange
        object parameter = null;
        var deletePayment = Create_Payment_Object_OK();
        var payment = DpTest.GetDomainEventObject<Domain.Aggregates.Payment.Payment>(deletePayment);
        var repositoryMock = new Mock<IPaymentRepository>();
        repositoryMock.Setup((o) => o.Delete(payment.ID)).Returns(true).Callback(() =>
        {
            parameter = payment;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IPaymentState>();
        stateMock.SetupGet((o) => o.Payment).Returns(repository);
        var state = stateMock.Object;
        var deletePaymentEventHandler = new Application.EventHandlers.Payment.DeletePaymentEventHandler(state, DpTest.MockDp<IPaymentState>(state));
        //Act
        var result = deletePaymentEventHandler.Handle(deletePayment);
        //Assert
        Assert.Equal(parameter, payment);
        Assert.Equal(result, true);
    }
}