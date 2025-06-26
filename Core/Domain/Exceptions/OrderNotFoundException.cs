﻿

namespace Domain.Exceptions
{
    public class OrderNotFoundException
        : NotFoundException
    {
        public OrderNotFoundException(Guid id)
            : base($"Order With Id {id} Not Found!.")
        {
        }
        public OrderNotFoundException(string message)
            : base(message)
        {
        }
    }
}
