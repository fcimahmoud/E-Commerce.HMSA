﻿namespace Shared.BasketModels
{
    public record BasketItemDTO
    {
        public string Id { get; init; }
        public string ProductName { get; init; }
        public string PictureUrl { get; init; }
        [Range(1, double.MaxValue)]
        public decimal Price { get; init; }
        [Range(1, 99)]
        public int Quantity { get; init; }
    }
}
