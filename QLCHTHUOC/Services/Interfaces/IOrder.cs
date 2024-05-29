using QLCHTHUOC.Model.DTO;

namespace QLCHTHUOC.Services.Interfaces
{
    public interface IOrder
    {
          List<OrderDTO> OrderDTOs(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        OrderDTO GetOrder (int id);
        OrderAddDTO OrderAdd (OrderDTO dto);
       void  Update(OrderDTO OrderDTO);
        void DeleteOrder (int id);
    }
}
