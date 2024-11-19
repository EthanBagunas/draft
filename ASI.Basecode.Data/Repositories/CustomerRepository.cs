using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void CreateCustomer(Customer customer)
        {
            this.GetDbSet<Customer>().Add(customer);
            UnitOfWork.SaveChanges();
        }

        public Customer GetCustomerById(int id)
        {
            return this.GetDbSet<Customer>().FirstOrDefault(c => c.Id == id);
        }
    }
}