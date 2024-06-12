using QLCHTHUOC.Data;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Model;
using QLCHTHUOC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace QLCHTHUOC.Services.RePon
{
    public class OrderRePon:IOrder
    {
        private readonly AppDbContext _appDbContext;
        public OrderRePon(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void DeleteOrder(int id)
        {
            var d = _appDbContext.OrderDetails.SingleOrDefault(o => o.Id == id);
            if (d != null)
            {
                _appDbContext.OrderDetails.Remove(d);
                _appDbContext.SaveChanges();
                var del = _appDbContext.Orders.SingleOrDefault(m => m.Id == id);
                if (del != null)
                {
                    _appDbContext.Orders.Remove(del);
                    _appDbContext.SaveChanges();
                }
            }
            else
            {
                var del = _appDbContext.Orders.SingleOrDefault(m => m.Id == id);
                if (del != null)
                {
                    _appDbContext.Orders.Remove(del);
                    _appDbContext.SaveChanges();
                }
            }
        }

        public OrderDTO GetOrder(int id)
        {
            var order = _appDbContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Medicine)
                .SingleOrDefault(o => o.Id == id);

            if (order != null)
            {
                var orderDTO = new OrderDTO
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    ImgURls = order.OrderDetails.Select(od => od.Medicine.ImgURl).ToList()
                };

                return orderDTO;
            }

            return null;
        }


        public OrderAddDTO OrderAdd(OrderAddDTO dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = dto.OrderDate,
                Quantity=dto.Quantity,
            };

            _appDbContext.Orders.Add(order);
            _appDbContext.SaveChanges();

            decimal totalPrice = 0;

            foreach (var id in dto.MedicineId)
            {
                var medicine = _appDbContext.Medicines.SingleOrDefault(m => m.Id == id);
                if (medicine != null && medicine.Stock > 0)
                {
                    var _orderdetail = new OrderDetail()
                    {
                        OrderId = order.Id,
                        MedicineId = id,
                    };
                    totalPrice += medicine.Price * order.Quantity; 

                    medicine.Stock -= order.Quantity;

                    _appDbContext.OrderDetails.Add(_orderdetail);
                }
            }

            _appDbContext.SaveChanges();
             dto.TotalPrice = totalPrice;
        
            return dto;
        }

        public List<OrderDTO> OrderDTOs(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            var query = _appDbContext.Orders
                                      .Include(o => o.Customer)
                                      .Include(o => o.OrderDetails)
                                      .ThenInclude(od => od.Medicine)
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("OrderDate", StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.TryParse(filterQuery, out var orderDate))
                    {
                        query = query.Where(o => o.OrderDate.Date == orderDate.Date);
                    }
                }
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isAscending ? query.OrderBy(o => o.OrderDate) : query.OrderByDescending(o => o.OrderDate);
            }

            var orders = query.ToList();
            var orderDTOs = orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.Name,
                OrderDate = o.OrderDate,
                MedicineName = o.OrderDetails.Select(od => od.Medicine.Name).ToList(),
                MedicineId = o.OrderDetails.Select(od => od.Medicine.Id).ToList(),
                ImgURls = o.OrderDetails.Select(od => od.Medicine.ImgURl).ToList()
            }).ToList();

            return orderDTOs;
        }

        public void Update(OrderDTO OrderDTO)
        {
            var _Order = _appDbContext.Orders.SingleOrDefault(m => m.Id == OrderDTO.Id);
            if (_Order != null)
            {
                _Order.OrderDate = OrderDTO.OrderDate;
                _Order.CustomerId = OrderDTO.CustomerId;
               
                _appDbContext.SaveChanges();
            }
        }
    }
}
