using QLCHTHUOC.Data;
using QLCHTHUOC.Model;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLCHTHUOC.Services.RePon
{
    public class MedicineRePon : IMedicine
    {
        private readonly AppDbContext _appDbContext;

        public MedicineRePon(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void DeleteMedicine(int id)
        {
            var d = _appDbContext.OrderDetails.SingleOrDefault(o => o.Id == id);
            if (d != null)
            {
                _appDbContext.OrderDetails.Remove(d);
                _appDbContext.SaveChanges();
                var del = _appDbContext.Medicines.SingleOrDefault(m => m.Id == id);
                if (del != null)
                {
                    _appDbContext.Medicines.Remove(del);
                    _appDbContext.SaveChanges();
                }
            }
            else
            {
                var del = _appDbContext.Medicines.SingleOrDefault(m => m.Id == id);
                if (del != null)
                {
                    _appDbContext.Medicines.Remove(del);
                    _appDbContext.SaveChanges();
                }
            }
        }
       

        public MedicineDTO GetMedicine(int id)
        {
            var i = _appDbContext.Medicines.SingleOrDefault(m => m.Id == id);
            if (i != null)
            {
                return new MedicineDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    Stock = i.Stock,
                    ImgURl = i.ImgURl
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
                ImgURl = dto.ImgURl
            };
            _appDbContext.Medicines.Add(m);
            _appDbContext.SaveChanges();

            return new MedicineAddDTO
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImgURl = dto.ImgURl
            };
        }

        public List<MedicineDTO> MedicineDTOs(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            var list = _appDbContext.Medicines.Select(a => new MedicineDTO
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                Stock = a.Stock,
                ImgURl = a.ImgURl
            });

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (filterOn.ToLower())
                {
                    case "name":
                        list = list.Where(x => x.Name.Contains(filterQuery, StringComparison.OrdinalIgnoreCase));
                        break;

                    default:

                        break;
                }
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                list = isAscending ? list.OrderBy(x => x.GetType().GetProperty(sortBy).GetValue(x)) : list.OrderByDescending(x => x.GetType().GetProperty(sortBy).GetValue(x));
            }

            return list.ToList();
        }


        public void Update(MedicineDTO medicineDTO)
        {
            var _medicine = _appDbContext.Medicines.SingleOrDefault(m => m.Id == medicineDTO.Id);
            if (_medicine != null)
            {
                _medicine.Name = medicineDTO.Name;
                _medicine.Description = medicineDTO.Description;
                _medicine.Price = medicineDTO.Price;
                _medicine.Stock = medicineDTO.Stock;
                _medicine.ImgURl = medicineDTO.ImgURl;
                _appDbContext.SaveChanges();
            }
        }
    }
}
