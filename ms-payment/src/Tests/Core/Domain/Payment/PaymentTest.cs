namespace Core.Tests;
public class PaymentTest
{
    public static Guid FixedID = new Guid("99afa025-4e6d-4847-88e0-8293cda8dd0b");

    public static Guid IDFixedID = new Guid("b4d6ad71-1ea8-4e2f-b8c1-434a6dce431b");

    public static Guid OrderIDFixedID = new Guid("65f26c75-ad77-4555-b530-4fed91a5bc04");

#region fixtures

    public static Domain.Aggregates.Payment.Payment Create_Payment_Required_Properties_OK()
    {
        var payment = new Domain.Aggregates.Payment.Payment();
        DpTest.MockDpDomain(payment);
        DpTest.Set<Guid>(payment, "ID", FixedID);
        DpTest.Set<Guid>(payment, "ID", IDFixedID);
        DpTest.Set<string>(payment, "CustomerName", Faker.Lorem.Sentence(1));
        DpTest.Set<Guid>(payment, "OrderID", OrderIDFixedID);
        return payment;
    }

    public static Domain.Aggregates.Payment.Payment Create_Payment_With_CustomerName_Required_Property_Missing()
    {
        var payment = new Domain.Aggregates.Payment.Payment();
        DpTest.MockDpDomain(payment);
        DpTest.Set<Guid>(payment, "ID", FixedID);
        DpTest.Set<Guid>(payment, "ID", IDFixedID);
        DpTest.Set<Guid>(payment, "OrderID", OrderIDFixedID);
        return payment;
    }

#endregion fixtures

#region add
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Success")]

    public void Add_Required_properties_filled_Success()
    {
        //Arrange
        var payment = Create_Payment_Required_Properties_OK();
        DpTest.MockDpProcessEvent<bool>(payment, "CreatePayment", true);
        DpTest.MockDpProcessEvent<bool>(payment, "PaymentCreated", true);
        //Act
        payment.Add();
        //Assert
        var domainevents = DpTest.GetDomainEvents(payment);
        Assert.True(domainevents[0] is CreatePayment);
        Assert.True(domainevents[1] is PaymentCreated);
        Assert.NotEqual(payment.ID, Guid.Empty);
        Assert.True(payment.IsNew);
        Assert.True(payment.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Fail")]

    public void Add_CustomerName_Missing_Fail()
    {
        //Arrange
        var payment = Create_Payment_With_CustomerName_Required_Property_Missing();
        //Act and Assert
        var ex = Assert.Throws<PublicException>(payment.Add);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("CustomerName is required", i));
        Assert.False(payment.Dp.Notifications.IsValid);
    }

#endregion add

#region update
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Success")]

    public void Update_Required_properties_filled_Success()
    {
        //Arrange
        var payment = Create_Payment_Required_Properties_OK();
        DpTest.MockDpProcessEvent<bool>(payment, "UpdatePayment", true);
        DpTest.MockDpProcessEvent<bool>(payment, "PaymentUpdated", true);
        //Act
        payment.Update();
        //Assert
        var domainevents = DpTest.GetDomainEvents(payment);
        Assert.True(domainevents[0] is UpdatePayment);
        Assert.True(domainevents[1] is PaymentUpdated);
        Assert.NotEqual(payment.ID, Guid.Empty);
        Assert.True(payment.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Fail")]

    public void Update_CustomerName_Missing_Fail()
    {
        //Arrange
        var payment = Create_Payment_With_CustomerName_Required_Property_Missing();
        //Act and Assert
        var ex = Assert.Throws<PublicException>(payment.Update);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("CustomerName is required", i));
        Assert.False(payment.Dp.Notifications.IsValid);
    }

#endregion update

#region delete
    [Fact]
    [Trait("Aggregate", "Delete")]
    [Trait("Aggregate", "Success")]

    public void Delete_IDFilled_Success()
    {
        //Arrange
        var payment = Create_Payment_Required_Properties_OK();
        DpTest.MockDpProcessEvent<bool>(payment, "DeletePayment", true);
        //Act
        payment.Delete();
        //Assert
        var domainevents = DpTest.GetDomainEvents(payment);
        Assert.True(domainevents[0] is DeletePayment);
        Assert.True(domainevents[1] is PaymentDeleted);
    }

#endregion delete
}