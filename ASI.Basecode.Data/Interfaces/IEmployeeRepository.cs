using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;
using NetTopologySuite.Noding;

namespace ASI.Basecode.Data.Interfaces
{
    public  interface IEmployeeRepository
    {
        bool UserExists(int employee_ID);
        void AddEmployee(Employee employee);

    }
    
}
