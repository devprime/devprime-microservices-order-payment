namespace Application.EventHandlers.Payment;
public class UpdatePaymentEventHandler : EventHandler<UpdatePayment, IPaymentState>
{
    public UpdatePaymentEventHandler(IPaymentState state, IDp dp) : base(state, dp)
    {
    }

    public override dynamic Handle(UpdatePayment updatePayment)
    {
        var payment = updatePayment.Get<Domain.Aggregates.Payment.Payment>();
        return Dp.State.Payment.Update(payment);
    }
}