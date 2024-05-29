using System.ComponentModel.DataAnnotations;

namespace QLCHTHUOC.Model
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
