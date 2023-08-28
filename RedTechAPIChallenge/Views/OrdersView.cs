namespace RedTechAPIChallenge.Views
{
    public class OrdersView
    {
        public List<Order> orders;
    }

    public class Order
    {
        public string id { get; set; }
        public string orderType { get; set; }
        public string customerName { get; set; }
        public string createdDate { get; set; }
        public string createdBy { get; set; }
    }
}
