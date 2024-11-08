﻿using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class EmployeeRepository: BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IQueryable<Employee> GetEmployees() 
        {
            return this.GetDbSet<Employee>();
        }
        public bool UserExists(int employee_ID)
        {
            return this.GetDbSet<Employee>().Any(x => x.Id == employee_ID);
        }

        public void AddEmployee(Employee employee)
        { 
            this.GetDbSet<Employee>().Add(employee);    
            UnitOfWork.SaveChanges();
        }

    }
}