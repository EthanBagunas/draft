using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Manager;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ASI.Basecode.Services.Services
{
    public class EmpService: IEmpService
    {

        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        public EmpService(IEmployeeRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }
        public void AddEmp(EmpViewModel model) 
        {
            var emp = new Employee();
            try
            {
                // connects the viewmodel and the repo model  
                _mapper.Map(model, emp);

                emp.Password = PasswordManager.EncryptPassword(model.Password);
                emp.CreatedTime = DateTime.Now;
                emp.UpdatedTime = DateTime.Now;
                _repository.AddEmployee(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
