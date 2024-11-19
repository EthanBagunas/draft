using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ICustomerRepository
    {
        void CreateCustomer(Customer customer);
        Customer GetCustomerById(int id);
    }
}