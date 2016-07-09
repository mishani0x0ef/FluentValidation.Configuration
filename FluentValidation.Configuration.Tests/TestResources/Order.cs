namespace FluentValidation.Configuration.Tests.TestResources
{
    public class Order : IOrder
    {
        public string ProductName { get; set; }
        public decimal Amount { get; set; }
    }
}
