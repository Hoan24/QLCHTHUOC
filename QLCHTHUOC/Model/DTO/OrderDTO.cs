﻿namespace QLCHTHUOC.Model.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public string CustomerName { get; set; }

    }
}
