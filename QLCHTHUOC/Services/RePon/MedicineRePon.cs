using QLCHTHUOC.Data;
using QLCHTHUOC.Model;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Services.Interfaces;

namespace QLCHTHUOC.Services.RePon
{
    public class MedicineRePon : IMedicine
    {
        private readonly AppDbContext _appDbContext;
        public MedicineRePon(AppDbContext appDbContext) { 
        _appDbContext = appDbContext;
        }
        public void DeleteMedicine(int id)
        {
            var del=_appDbContext.Medicines.SingleOrDefault(m => m.Id == id);
            if (del != null)
            {
                _appDbContext.Medicines.Remove(del);
                _appDbContext.SaveChanges();
            }
            
        }

        public MedicineDTO GetMedicine(int id)
        {
            var i=_appDbContext.Medicines.SingleOrDefault(m=> m.Id == id);
            if (i != null) 
            {
                return new MedicineDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    Stock = i.Stock,
                }; 
            }
            return null;
        }

        public MedicineAddDTO MedicineAdd(MedicineAddDTO dto)
        {
            var m = new Medicine
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
            };
            _appDbContext.Medicines.Add(m);
            _appDbContext.SaveChanges();
           
            return new MedicineAddDTO
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
            };
        }

        public List<MedicineDTO> MedicineDTOs(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var list = _appDbContext.Medicines.Select(a => new MedicineDTO
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                Stock = a.Stock,
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
            var skipResults = (pageNumber - 1) * pageSize;
            return list.Skip(skipResults).Take(pageSize).ToList();
        }

        public void Update(MedicineDTO medicineDTO)
        {
           var _medicine=_appDbContext.Medicines.SingleOrDefault(m=>m.Id==medicineDTO.Id);
            _medicine.Name=medicineDTO.Name;
            _medicine.Description=medicineDTO.Description;
            _medicine.Price=medicineDTO.Price;
            _medicine.Stock=medicineDTO.Stock;
            _appDbContext.SaveChanges();
        }
    }
}
