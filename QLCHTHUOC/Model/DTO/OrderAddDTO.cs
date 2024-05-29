namespace QLCHTHUOC.Model.DTO
{
    public class OrderAddDTO
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }

    }
}
