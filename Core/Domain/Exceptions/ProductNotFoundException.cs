
namespace Domain.Exceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(string id) 
            : base($"Product with id : {id} Not Found.")
        {
        }
    }
}
