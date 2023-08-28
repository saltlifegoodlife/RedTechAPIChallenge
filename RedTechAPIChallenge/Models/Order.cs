namespace RedTechAPIChallenge.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int TypeID { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUsername { get; set; }
    }
}
