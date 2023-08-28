namespace RedTechAPIChallenge.Models.Mappers
{
    public interface IOrderRepository
    {
        public List<Order> GetOrders(int? id, string? customer);

        public int InsertOrder(Order order);

        public bool UpdateOrder(Order order, int orderId);

        public int DeleteOrder(string[] orderIDs);

    }
}
