using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using RedTechAPIChallenge.Models;
using RedTechAPIChallenge.Models.Repositories;
using RedTechAPIChallenge.Views;
using static RedTechAPIChallenge.Models.Enums;

namespace RedTechAPIChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [HttpGet]

        public IEnumerable<Views.Order> Get(string? orderType = null, string? customer = null)
        {
            int? id = null;
            if (orderType != null)
            {
                Enum.TryParse(orderType, true, out OrderType typeId);
                id = Convert.ToInt32(typeId);
            }
            List<Models.Order> orders = _orderRepository.GetOrders(id, customer);
            OrdersView ordersOutput = m_to_v(orders);
            return ordersOutput.orders;

        }

        private OrdersView m_to_v(List<Models.Order> orders)
        {
            OrdersView ordersOutput = new OrdersView { orders = new List<Views.Order>() };
            foreach (Models.Order order in orders)
            {
                ordersOutput.orders.Add(
                    new Views.Order
                    {
                        id = order.OrderID.ToString(),
                        orderType = ((OrderType)order.TypeID).ToString(),
                        customerName = order.CustomerName,
                        createdDate = order.CreatedDate.ToString(),
                        createdBy = order.CreatedByUsername
                    });
            }
            return ordersOutput;
        }

        [HttpPost]

        public NewOrderLogging Insert(OrderInput order)
        {
            NewOrderLogging ol = new NewOrderLogging();

            try
            {
                if (order.orderType == "" || order.customerName == "" || order.createdBy == "")
                {
                    throw new NullReferenceException();
                }
                Models.Order newOrder = v_to_m(order);
                int result = _orderRepository.InsertOrder(newOrder);
                if (result > 0)
                {
                    ol.status = "Success";
                    ol.recordId = result.ToString();
                    ol.message = "New order saved.";
                }
                else
                {
                    ol.status = "Failed";
                    ol.message = "Error during save.";
                }
            }
            catch (Exception ex)
            {

                ol.status = "Failed";
                ol.message = "Error during save.";
            }



            return ol;

        }

        [HttpPut]

        public NewOrderLogging Update(OrderInput order)
        {

            bool result = false;
            NewOrderLogging ol = new NewOrderLogging();
            try
            {
                if (order.orderType == "" && order.customerName == "" && order.createdBy == "")
                {
                    throw new NullReferenceException();
                }
                Models.Order newOrder = v_to_m(order);

                result = _orderRepository.UpdateOrder(newOrder, Convert.ToInt32(order.orderID));
                if (result)
                {
                    ol.status = "Success";
                    ol.recordId = order.orderID;
                    ol.message = "Order updated!";

                }
                else
                {
                    ol.status = "Failed";
                    ol.message = "Error during save.";
                }


            }
            catch (Exception ex)
            {
                ol.status = "Failed";
                ol.message = "Error during save.";

            }

            return ol;

        }

        private Models.Order v_to_m(OrderInput order)
        {
            Enum.TryParse(order.orderType, true, out OrderType orderType);
            return new Models.Order
            {
                TypeID = Convert.ToInt32(orderType),
                CustomerName = order.customerName,
                CreatedDate = DateTime.Now,
                CreatedByUsername = order.createdBy
            };
        }

        [HttpDelete]

        public IActionResult RemoveOrder(string[] orderIDs)
        {
            List<string> invalidIDs = new List<string>();

            foreach (string orderID in orderIDs)
            {
                if (!int.TryParse(orderID, out _))
                {
                    invalidIDs.Add(orderID);
                }
            }

            if (invalidIDs.Count > 0)
            {
                return new BadRequestObjectResult(new
                {
                    InvalidOrderIDs = invalidIDs
                });
            }

            List<string> notFoundIDs = _orderRepository.CheckOrder(orderIDs);

            if (notFoundIDs.Count > 0)
            {
                return new BadRequestObjectResult(new { InvalidOrderIDs = notFoundIDs });
            }

            int result = _orderRepository.DeleteOrder(orderIDs);

            if (orderIDs.Length == result)
            {
                return new NoContentResult();
            }

            return new BadRequestResult();
        }
    }

    
}
