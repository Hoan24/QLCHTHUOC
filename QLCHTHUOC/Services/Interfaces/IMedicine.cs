using QLCHTHUOC.Model.DTO;

namespace QLCHTHUOC.Services.Interfaces
{
    public interface IMedicine
    {
        List<MedicineDTO> MedicineDTOs(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true);
        MedicineDTO GetMedicine(int id);
      
        MedicineAddDTO MedicineAdd(MedicineAddDTO dto);
        void Update(MedicineDTO medicineDTO);
        void DeleteMedicine(int id);
    }
}
