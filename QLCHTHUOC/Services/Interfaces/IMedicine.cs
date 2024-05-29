using QLCHTHUOC.Model.DTO;

namespace QLCHTHUOC.Services.Interfaces
{
    public interface IMedicine
    {
        List<MedicineDTO> MedicineDTOs(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        MedicineDTO GetMedicine (int id);
        MedicineAddDTO MedicineAdd (MedicineAddDTO dto);
       void  Update(MedicineDTO medicineDTO);
        void DeleteMedicine (int id);
    }
}
