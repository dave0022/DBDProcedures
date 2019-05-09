using Domain.Entities;
using Domain.Repository;
using Infrastructure.Repository;
using System;

namespace DBD1
{
	class Program
	{
		static void Main(string[] args)
		{
			Program prog = new Program();
			prog.RepInit();
			prog.Run();
		}
        private void RepInit()
        {
            repo = new Repository();
            this.Working = true;
        }
        private IRepository repo;
        private bool Working = false; 
		

        private void UpdateDepartmentName()
        {
            Console.WriteLine("Enter the Id");
            int id;
            var success = Int32.TryParse(Console.ReadLine(), out id);

            Console.WriteLine("Enter the Name");
            var name = Console.ReadLine();

            var dep = new Department()
            {
                FirstName = name,
                Id = id
            };

            try
            {
                repo.UpdateDepName(dep);
                Console.WriteLine("Succesfully Updated");
                Console.WriteLine(repo.DepartmentGet(dep.Id).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void CreateDepartment()
        {
            Console.WriteLine("Enter the name of the department");
            var name = Console.ReadLine();

            Console.WriteLine("Enter the MgrSSN");
            int mgrSSN;
            var success = Int32.TryParse(Console.ReadLine(), out mgrSSN);

            var dep = new Department()
            {
                FirstName = name,
                MgrSSN = mgrSSN
            };

            try
            {
                Console.WriteLine(repo.DepartmentCreate(dep));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void GetDepartmentInformation()
        {
            string result;
            Console.WriteLine("Choose your department by writting its Id");
            int DNumber;
            var success = Int32.TryParse(Console.ReadLine(), out DNumber);
            try
            {
                result = repo.DepartmentGet(DNumber).ToString();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            Console.WriteLine(result);
        }

        private void UpdateDepartmentManager()
        {
            Console.WriteLine("Enter the id of the department");
            int id;
            Int32.TryParse(Console.ReadLine(), out id);

            Console.WriteLine("Enter the MgrSSN");
            int mgrSSN;
            Int32.TryParse(Console.ReadLine(), out mgrSSN);

            var dep = new Department()
            {
                MgrSSN = mgrSSN,
                Id = id
            };

            try
            {
                repo.UpdateDepManager(dep);
                Console.WriteLine("Succesfully Updated");
                Console.WriteLine(repo.DepartmentGet(dep.Id).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void DeleteDepartment()
        {
            Console.WriteLine("Delete the department by writing its Id");
            try
            {
                int DNumber;
                var success = Int32.TryParse(Console.ReadLine(), out DNumber);
                Console.WriteLine(repo.DepartmentGet(DNumber).ToString());
                repo.DepartmentDelete(DNumber);
                Console.WriteLine("Succesfully Deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void GetAllDepartments()
        {
            foreach (var element in repo.DepListGet())
            {
                Console.WriteLine(element);
            }
        }

        private void Run()
        {
            while (this.Working)
            {
                Console.WriteLine("\nTo change department name press 1");
                Console.WriteLine("\nTo create new department press 2");
                Console.WriteLine("\nTo update department manager press 3");
                Console.WriteLine("\nTo delete department press 4");
                Console.WriteLine("\nTo get department information press 5");
                Console.WriteLine("\nTo get all departments press 6");
                Console.WriteLine("\nTo quit the program press 7");

                var key = Console.ReadKey();
                Console.WriteLine("\n\n\n");
                switch (char.ToLower(key.KeyChar))
                {
                    case '1':
                        UpdateDepartmentName();
                        break;
                    case '2':
                        CreateDepartment();

                        break;
                    case '3':
                        UpdateDepartmentManager();
                        break;
                    case '4':
                        DeleteDepartment();
                        break;

                    case '5':
                        GetDepartmentInformation();
                        break;
                    case '6':
                        GetAllDepartments();

                        break;
                    case '7':
                        Working = false;
                        break;
                }

                Console.ReadKey();
            }
        }
    }
}
