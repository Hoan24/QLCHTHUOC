namespace MVCQLCHTHUOC.Model.DTO
{
    public class OrderAddDTO
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<int> MedicineId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
