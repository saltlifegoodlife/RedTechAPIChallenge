namespace RedTechAPIChallenge.Models.Repositories
{
    public interface IOrderRepository
    {
        public List<Order> GetOrders(int? id, string? customer);

        public int InsertOrder(Order order);

        public bool UpdateOrder(Order order, int orderId);

        public List<string> CheckOrder(string[] orderIds);
        public int DeleteOrder(string[] orderIDs);

    }
}
