using Dapper;
using System.Data;

namespace RedTechAPIChallenge.Models.Mappers
{
    public class OrderRepository : IOrderRepository
    {

        private readonly IDbConnection _connection;

        public OrderRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public List<Order> GetOrders(int? id, string? customer)
        {
            var list = new List<string> { "SELECT * FROM orders" };
            if (id != null || customer != null)
            {
                list.Add("WHERE");
                if (id != null)
                {
                    list.Add("TypeID = @typeID");
                }
                if (id != null && customer != null) {
                    list.Add("AND");
                }
                if (customer != null)
                {
                    list.Add("CustomerName = @customer");
                }
            }
            string query = string.Join(" ", list) + ";";
            return (List<Order>)_connection.Query<Order>(query, new { typeID = id, customer = customer });
        }

        public int InsertOrder(Order order)
        {
            string query = "INSERT INTO orders (`TypeID`, `CustomerName`, `CreatedDate`, `CreatedByUsername`) VALUES (@typeId, @customerName, @createdDate, @createdByUser); SELECT LAST_INSERT_ID();";
            return _connection.QuerySingle<int>(query,
                new { typeId = order.TypeID, customerName = order.CustomerName, createdDate = order.CreatedDate, createdByUser = order.CreatedByUsername });
        }

        public bool UpdateOrder(Order order, int orderId)
        {

            var queryArr = new List<string>();
            if (order.TypeID != 0)
            {
                queryArr.Add("TypeID = @typeId");
            }
            if (!string.IsNullOrEmpty(order.CustomerName))
            {
                queryArr.Add("CustomerName = @customerName");
            }
            if (!string.IsNullOrEmpty(order.CreatedByUsername))
            {
                queryArr.Add("CreatedByUsername = @createdByUser");
            }
            var add = string.Join(", ", queryArr);
            string query = $"UPDATE orders SET {add} WHERE OrderID = @orderId;";
            return _connection.Execute(query, new { orderId = orderId, typeId = order.TypeID, customerName = order.CustomerName, createdDate = order.CreatedDate, createdByUser = order.CreatedByUsername }) > 0;
        }
        public int DeleteOrder(string[] orderIDs)
        {
            int result = 0;
            foreach (var id in orderIDs)
            {
                result += _connection.Execute("DELETE FROM orders WHERE OrderID = @orderID;", new { orderID = id });
            }
            return result;
        }

    }
}
