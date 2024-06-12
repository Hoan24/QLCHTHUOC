namespace MVCQLCHTHUOC.Models
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public List<string> MedicineName { get; set; }
        public List<int> MedicineId
        { get; set; }
        public List<string> ImgURls { get; set; }
    }
}
