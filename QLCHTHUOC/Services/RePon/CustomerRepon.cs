using QLCHTHUOC.Data;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Model;
using QLCHTHUOC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace QLCHTHUOC.Services.RePon
{
    
    public class CustomerRepon:ICustomer
    {
        private readonly AppDbContext _appDbContext;
        public CustomerRepon(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void DeleteCustomer(int id)
        {
            var customer = _appDbContext.Customers.SingleOrDefault(m => m.Id == id);
            if (customer != null)
            {
                var orders = _appDbContext.Orders.Where(o => o.CustomerId == id).ToList();
                foreach (var order in orders)
                {
                    var orderDetails = _appDbContext.OrderDetails.Where(od => od.OrderId == order.Id).ToList();
                    foreach (var orderDetail in orderDetails)
                    {
                        _appDbContext.OrderDetails.Remove(orderDetail);
                    }

                    _appDbContext.Orders.Remove(order);
                }

                _appDbContext.Customers.Remove(customer);

                _appDbContext.SaveChanges();
            }
        }

        public CustomerDTO GetCustomer(int id)
        {
            var i = _appDbContext.Customers.SingleOrDefault(m => m.Id == id);
            if (i != null)
            {
                return new CustomerDTO
                {
                    Name=i.Name,
                   Email = i.Email,
                   Phone= i.Phone,
                };
            }
            return null;
        }

        public CustomerAddDTO CustomerAdd(CustomerAddDTO dto)
        {
            var m = new Customer
            {
                Name= dto.Name,
                Email= dto.Email,
                Phone= dto.Phone,
            };
            _appDbContext.Customers.Add(m);
            _appDbContext.SaveChanges();
           
            return new CustomerAddDTO
            {
                Name = dto.Name,
               Email=dto.Email,
               Phone= dto.Phone,
            };
        }
        public List<CustomerDTO> GetAll(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true)
        {
            var list = _appDbContext.Customers.Select(a => new CustomerDTO
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                Phone = a.Phone,
            });
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.Where(x => x.Name.Contains(filterQuery));
                }
            }
            {
                if (!string.IsNullOrEmpty(sortBy))
                {
                    list = isAscending ? list.OrderBy(x => x.Name) :
list.OrderByDescending(x => x.Name);
                }
            }
            
            return list.ToList();
            
        }
        public void Update(CustomerDTO CustomerDTO)
        {
            var _Customer = _appDbContext.Customers.SingleOrDefault(m => m.Id == CustomerDTO.Id);
            if (_Customer != null)
            {
                _Customer.Name = CustomerDTO.Name;
                _Customer.Email = CustomerDTO.Email;
                _Customer.Phone = CustomerDTO.Phone;
            } 
            _appDbContext.SaveChanges();
        }
    }
}
