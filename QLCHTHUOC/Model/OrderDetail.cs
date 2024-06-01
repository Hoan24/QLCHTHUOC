using System.ComponentModel.DataAnnotations;

namespace QLCHTHUOC.Model
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MedicineId { get; set; }
        public Order? Order { get; set; }
        public Medicine? Medicine { get; set; }
    }
}
