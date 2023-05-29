using System;
using System.Data;
using System.Data.SqlClient;
using EmployeesManagementApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmployeesManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var query = @"select DepartmentId, DepartmentName from dbo.Department";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                Console.Write(myCon);
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            var query = @"insert into dbo.Department values (@DepartmentName)";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            var query = @"update dbo.Department set DepartmentName= @DepartmentName where DepartmentId=@DepartmentId";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var query = @"delete from dbo.Department where DepartmentId=@DepartmentId";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);

                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}