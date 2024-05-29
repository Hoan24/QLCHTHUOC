using System.ComponentModel.DataAnnotations;

namespace QLCHTHUOC.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }

        public Customer Customer { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
