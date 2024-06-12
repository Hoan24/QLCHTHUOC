using QLCHTHUOC.Model.DTO;

namespace QLCHTHUOC.Services.Interfaces
{
    public interface ICustomer
    {
        List<CustomerDTO> GetAll(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true);
        CustomerDTO GetCustomer(int id);
        CustomerAddDTO CustomerAdd(CustomerAddDTO dto);
        void Update(CustomerDTO CustomerDTO);
        void DeleteCustomer(int id);
    }
}
