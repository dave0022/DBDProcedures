using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repository
{
    public interface IRepository
    {
        Department DepartmentCreate(Department obj);
        Department DepartmentGet(int id);
        List<Department> DepListGet();
        void UpdateDepName(Department obj);
        void UpdateDepManager(Department obj);
        void DepartmentDelete(int id);
    }
}
