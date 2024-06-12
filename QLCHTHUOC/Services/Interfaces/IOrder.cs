using QLCHTHUOC.Model.DTO;

namespace QLCHTHUOC.Services.Interfaces
{
    public interface IOrder
    {
          List<OrderDTO> OrderDTOs(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true);
        OrderDTO GetOrder (int id);
        OrderAddDTO OrderAdd (OrderAddDTO dto);
       void  Update(OrderDTO OrderDTO);
        void DeleteOrder (int id);
    }
}
