namespace Ordering.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<Customer> Customers =>
        [
            Customer.Create(CustomerId.Of(new Guid("9c4b3ef1-560f-4f68-b4d8-6bbac7b25b88")), "John", "jsmith@test.com"),
            Customer.Create(CustomerId.Of(new Guid("2a83cbb0-b8d4-4e17-9b01-2627f22a8a03")), "Laura", "lsmith@test.com"),
        ];

        public static IEnumerable<Product> Products =>
        [
           Product.Create(ProductId.Of(new Guid("e6dde822-c848-4f07-82db-700a646b39cd")), "IPhone X", 1200),
           Product.Create(ProductId.Of(new Guid("48dbbb8c-bcef-4e42-b39a-dbc2e2dc4417")), "Samsumg 10", 950),
           Product.Create(ProductId.Of(new Guid("64f82bf4-3478-4a49-afd1-5b8e5b1cdee7")), "Huawei Plus", 825),
           Product.Create(ProductId.Of(new Guid("a2ccde71-c6d8-4212-8f81-cb931568003d")), "Xiomi Mi", 750),
        ];

        public static IEnumerable<Order> OrdersWithItems
        {
            get
            {
                var address1 = Address.Of("John", "Smith", "jsmith@test.com", "some street address", "Costa Rica", "San Jose", "12345");
                var address2 = Address.Of("Laura", "Smith", "lsmith@test.com", "Broadway No:0", "USA", "CA", "11010");

                var payment1 = Payment.Of("John", "555555555555555555555555", "12/28", "123", 1);
                var payment2 = Payment.Of("Laura", "111155555555555555555555", "06/30", "012", 2);

                var order1 = Order.Create(
                        OrderId.Of(Guid.NewGuid()),
                        CustomerId.Of(new Guid("9c4b3ef1-560f-4f68-b4d8-6bbac7b25b88")),
                        OrderName.Of("O0001"),
                        shippingAddress: address1,
                        billingAddress: address1,
                        payment1);

                order1.Add(ProductId.Of(new Guid("e6dde822-c848-4f07-82db-700a646b39cd")),
                    2, 1200);

                order1.Add(ProductId.Of(new Guid("48dbbb8c-bcef-4e42-b39a-dbc2e2dc4417")),
                    1, 950);

                var order2 = Order.Create(
                        OrderId.Of(Guid.NewGuid()),
                        CustomerId.Of(new Guid("2a83cbb0-b8d4-4e17-9b01-2627f22a8a03")),
                        OrderName.Of("O0002"),
                        shippingAddress: address2,
                        billingAddress: address2,
                        payment2);

                order2.Add(ProductId.Of(new Guid("e6dde822-c848-4f07-82db-700a646b39cd")),
                    1, 1200);

                order2.Add(ProductId.Of(new Guid("48dbbb8c-bcef-4e42-b39a-dbc2e2dc4417")),
                    1, 950);

                order2.Add(ProductId.Of(new Guid("a2ccde71-c6d8-4212-8f81-cb931568003d")),
                    1, 750);

                return [order1, order2];
            }
        }
    }
}
