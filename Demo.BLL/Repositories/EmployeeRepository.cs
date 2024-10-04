using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext _dbContext;
        public EmployeeRepository(MVCAppDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeesByName(string SearchValue)
        {
            return _dbContext.Employees.Include(E => E.Department).Where(E =>E.Name.ToLower().Contains(SearchValue.ToLower()));
        }
    }
}
