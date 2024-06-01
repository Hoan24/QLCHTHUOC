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
          
        }

        public OrderDTO GetOrder(int id)
        {
            var i = _appDbContext.Orders.SingleOrDefault(m => m.Id == id);
            if (i != null)
            {
                return new OrderDTO
                {
                    Id = i.Id,
                    CustomerId = i.CustomerId,
                    OrderDate = i.OrderDate,
                };
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

            // Save changes after all operations
            _appDbContext.SaveChanges();
             dto.TotalPrice = totalPrice;
        
            return dto;
        }

        public List<OrderDTO> OrderDTOs(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
                                         bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var list = _appDbContext.Orders.Select(a => new OrderDTO
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                CustomerName=a.Customer.Name,
                MedicineName = _appDbContext.OrderDetails.Select(c => c.Medicine.Name).ToList(),
                OrderDate = a.OrderDate,

            }); 

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("OrderDate", StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.TryParse(filterQuery, out var orderDate))
                    {
                        list = list.Where(x => x.OrderDate.Date == orderDate.Date);
                    }
                }
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                list = isAscending ? list.OrderBy(x => x.OrderDate) : list.OrderByDescending(x => x.OrderDate);
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return list.Skip(skipResults).Take(pageSize).ToList();
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
