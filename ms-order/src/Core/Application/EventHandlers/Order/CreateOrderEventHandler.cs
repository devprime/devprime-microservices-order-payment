namespace Application.EventHandlers.Order;
public class CreateOrderEventHandler : EventHandler<CreateOrder, IOrderState>
{
    public CreateOrderEventHandler(IOrderState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(CreateOrder createOrder)
    {
        var order = createOrder.Get<Domain.Aggregates.Order.Order>();
        return Dp.State.Order.Add(order);
    }
}