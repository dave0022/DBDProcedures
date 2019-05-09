using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Department : Entity
    {
        public int Employee { get; set; }
        public string FirstName { get; set; }
        public string StartDateMgr { get; set; }
        public int MgrSSN { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + " MgrSSN: " + MgrSSN +
                    " StartDateMgr: " + StartDateMgr + " Empoyee: " + Employee + " FirstName: " + FirstName;
        }
        public Department()
        {
            Employee = -1;
            FirstName = "Error";          
            StartDateMgr = "Error";
            MgrSSN = -1;
        }
        
    }
}