﻿
namespace Domain.Entities
{
    public class BasketItem
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; } 
        public int Quantity { get; set; }
    }
}
