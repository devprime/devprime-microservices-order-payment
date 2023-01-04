namespace Core.Tests;
public class OrderTest
{
    public static Guid FixedID = new Guid("f816599f-9e93-4dc1-b34f-793a6bfb6401");

    public static Guid IDFixedID = new Guid("6a1289b1-e4a3-4d8e-a9cc-a754b57df615");

#region fixtures

    public static Domain.Aggregates.Order.Order Create_Order_Required_Properties_OK()
    {
        var order = new Domain.Aggregates.Order.Order();
        DpTest.MockDpDomain(order);
        DpTest.Set<Guid>(order, "ID", FixedID);
        DpTest.Set<Guid>(order, "ID", IDFixedID);
        DpTest.Set<string>(order, "CustomerName", Faker.Lorem.Sentence(1));
        DpTest.Set<string>(order, "CustomerTaxID", Faker.Lorem.Sentence(1));
        return order;
    }

    public static Domain.Aggregates.Order.Order Create_Order_With_CustomerName_Required_Property_Missing()
    {
        var order = new Domain.Aggregates.Order.Order();
        DpTest.MockDpDomain(order);
        DpTest.Set<Guid>(order, "ID", FixedID);
        DpTest.Set<Guid>(order, "ID", IDFixedID);
        DpTest.Set<string>(order, "CustomerTaxID", Faker.Lorem.Sentence(1));
        return order;
    }

    public static Domain.Aggregates.Order.Order Create_Order_With_CustomerTaxID_Required_Property_Missing()
    {
        var order = new Domain.Aggregates.Order.Order();
        DpTest.MockDpDomain(order);
        DpTest.Set<Guid>(order, "ID", FixedID);
        DpTest.Set<Guid>(order, "ID", IDFixedID);
        DpTest.Set<string>(order, "CustomerName", Faker.Lorem.Sentence(1));
        return order;
    }

#endregion fixtures

#region add
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Success")]

    public void Add_Required_properties_filled_Success()
    {
        //Arrange
        var order = Create_Order_Required_Properties_OK();
        DpTest.MockDpProcessEvent<bool>(order, "CreateOrder", true);
        DpTest.MockDpProcessEvent<bool>(order, "OrderCreated", true);
        //Act
        order.Add();
        //Assert
        var domainevents = DpTest.GetDomainEvents(order);
        Assert.True(domainevents[0] is CreateOrder);
        Assert.True(domainevents[1] is OrderCreated);
        Assert.NotEqual(order.ID, Guid.Empty);
        Assert.True(order.IsNew);
        Assert.True(order.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Fail")]

    public void Add_CustomerName_Missing_Fail()
    {
        //Arrange
        var order = Create_Order_With_CustomerName_Required_Property_Missing();
        //Act and Assert
        var ex = Assert.Throws<PublicException>(order.Add);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("CustomerName is required", i));
        Assert.False(order.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Fail")]

    public void Add_CustomerTaxID_Missing_Fail()
    {
        //Arrange
        var order = Create_Order_With_CustomerTaxID_Required_Property_Missing();
        //Act and Assert
        var ex = Assert.Throws<PublicException>(order.Add);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("CustomerTaxID is required", i));
        Assert.False(order.Dp.Notifications.IsValid);
    }

#endregion add

#region update
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Success")]

    public void Update_Required_properties_filled_Success()
    {
        //Arrange
        var order = Create_Order_Required_Properties_OK();
        DpTest.MockDpProcessEvent<bool>(order, "UpdateOrder", true);
        DpTest.MockDpProcessEvent<bool>(order, "OrderUpdated", true);
        //Act
        order.Update();
        //Assert
        var domainevents = DpTest.GetDomainEvents(order);
        Assert.True(domainevents[0] is UpdateOrder);
        Assert.True(domainevents[1] is OrderUpdated);
        Assert.NotEqual(order.ID, Guid.Empty);
        Assert.True(order.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Fail")]

    public void Update_CustomerName_Missing_Fail()
    {
        //Arrange
        var order = Create_Order_With_CustomerName_Required_Property_Missing();
        //Act and Assert
        var ex = Assert.Throws<PublicException>(order.Update);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("CustomerName is required", i));
        Assert.False(order.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Fail")]

    public void Update_CustomerTaxID_Missing_Fail()
    {
        //Arrange
        var order = Create_Order_With_CustomerTaxID_Required_Property_Missing();
        //Act and Assert
        var ex = Assert.Throws<PublicException>(order.Update);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("CustomerTaxID is required", i));
        Assert.False(order.Dp.Notifications.IsValid);
    }

#endregion update

#region delete
    [Fact]
    [Trait("Aggregate", "Delete")]
    [Trait("Aggregate", "Success")]

    public void Delete_IDFilled_Success()
    {
        //Arrange
        var order = Create_Order_Required_Properties_OK();
        DpTest.MockDpProcessEvent<bool>(order, "DeleteOrder", true);
        //Act
        order.Delete();
        //Assert
        var domainevents = DpTest.GetDomainEvents(order);
        Assert.True(domainevents[0] is DeleteOrder);
        Assert.True(domainevents[1] is OrderDeleted);
    }

#endregion delete
}