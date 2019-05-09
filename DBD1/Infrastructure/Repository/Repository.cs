using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Infrastructure.Repository
{
    public class Repository : IRepository
    {
        private string _cs;
        public Repository()
        {
            _cs = new DB().GetCS();
        }
           
        public Department DepartmentGet(int id)
        {
            Department temp = null;
            using (SqlConnection SqlCONNEC = new SqlConnection(_cs))
            {
                using (SqlCommand SqlCMD = new SqlCommand("usp_GetDepartment", SqlCONNEC))
                {
                    
                    SqlCMD.CommandType = CommandType.StoredProcedure;

                    SqlCMD.Parameters.Add("@DNumber", SqlDbType.VarChar).Value = id;

                    SqlCONNEC.Open();
                    SqlDataReader reader = SqlCMD.ExecuteReader();

                    while (reader.Read())
                    {
						var empCount = String.Format("{0}", reader["EmpCount"]);
						var noe = (string.IsNullOrWhiteSpace(empCount)) ? 0 : int.Parse(empCount);

						temp = (new Department() {
							FirstName = String.Format("{0}", reader["DName"]),
							Id = int.Parse(String.Format("{0}", reader["DNumber"])),
							MgrSSN = int.Parse(String.Format("{0}", reader["MgrSSN"])),
							StartDateMgr = String.Format("{0}", reader["MgrStartDate"]),
							Employee = noe
						});
                    }
                }
            }
            return temp;
        }

        public void UpdateDepName(Department o)
        {
            using (SqlConnection SqlCONNEC = new SqlConnection(_cs))
            {
                using (SqlCommand SqlCMD = new SqlCommand("usp_UpdateDepartmentName", SqlCONNEC))
                {
                    SqlCMD.CommandType = CommandType.StoredProcedure;
                    SqlCMD.Parameters.Add("@DName", SqlDbType.VarChar).Value = o.FirstName;
                    SqlCMD.Parameters.Add("@DNumber", SqlDbType.VarChar).Value = o.Id;
                    SqlCONNEC.Open();
                    SqlCMD.ExecuteNonQuery();
                }
            }
        }

        public List<Department> DepListGet()
        {
            var temp = new List<Department>();
            using (SqlConnection SqlCONNEC = new SqlConnection(_cs))
            {
                using (SqlCommand SqlCMD = new SqlCommand("usp_GetAllDepartments ", SqlCONNEC))
                {
                    SqlCMD.CommandType = CommandType.StoredProcedure;

                    SqlCONNEC.Open();
                    SqlDataReader reader = SqlCMD.ExecuteReader();

                    while (reader.Read())
                    {
						var empCount = String.Format("{0}", reader["EmpCount"]);
						var noe = (string.IsNullOrWhiteSpace(empCount)) ? 0 : int.Parse(empCount);


						temp.Add(new Department() {
							FirstName = String.Format("{0}", reader["DName"]),
							Id = int.Parse(String.Format("{0}", reader["DNumber"])),
							MgrSSN = int.Parse(String.Format("{0}", reader["MgrSSN"])),
							StartDateMgr = String.Format("{0}", reader["MgrStartDate"]),
							Employee = noe
                        });
                    }

                }
            }
            return temp;
        }

        public void UpdateDepManager(Department o)
        {
            using (SqlConnection SqlCONNEC = new SqlConnection(_cs))
            {
                using (SqlCommand SqlCMD = new SqlCommand("usp_UpdateDepartmentManager", SqlCONNEC))
                {
                    SqlCMD.CommandType = CommandType.StoredProcedure;
                    SqlCMD.Parameters.Add("@DNumber", SqlDbType.VarChar).Value = o.Id;
                    SqlCMD.Parameters.Add("@MgrSSN", SqlDbType.VarChar).Value = o.MgrSSN;
                    SqlCONNEC.Open();
                    SqlCMD.ExecuteNonQuery();
                }
            }
        }
        public Department DepartmentCreate(Department obj)
        {
            using (SqlConnection SqlCONNEC = new SqlConnection(_cs))
            {
                using (SqlCommand SqlCMD = new SqlCommand("usp_CreateDepartment", SqlCONNEC))
                {
                    SqlCMD.CommandType = CommandType.StoredProcedure;
                    SqlParameter output = new SqlParameter("@Dnum", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlCMD.Parameters.Add("@MgrSSN", SqlDbType.VarChar).Value = obj.MgrSSN;
                    SqlCMD.Parameters.Add("@DName", SqlDbType.VarChar).Value = obj.FirstName;                 
                    SqlCMD.Parameters.Add(output);
                    SqlCONNEC.Open();
                    SqlCMD.ExecuteNonQuery();
                    return DepartmentGet(int.Parse((string)SqlCMD.Parameters["@Dnum"].Value));
                }
            }
        }
        public void DepartmentDelete(int id)
        {
            using (SqlConnection SqlCONNEC = new SqlConnection(_cs))
            {
                using (SqlCommand SqlCMD = new SqlCommand("usp_DeleteDepartment", SqlCONNEC))
                {                   
                    SqlCMD.Parameters.Add("@DNumber", SqlDbType.VarChar).Value = id;
                    SqlCMD.CommandType = CommandType.StoredProcedure;
                    SqlCONNEC.Open();
                    SqlCMD.ExecuteNonQuery();
                }
            }
        }
    }
}
