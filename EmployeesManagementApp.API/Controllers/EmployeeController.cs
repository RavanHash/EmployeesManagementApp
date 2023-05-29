using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using EmployeesManagementApp.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmployeesManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            var query = @"select EmployeeId, EmployeeName,Department,
                            convert(varchar(10),DateOfJoining,120) as DateOfJoining,PhotoFileName from dbo.Employee";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
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
        public JsonResult Post(Employee emp)
        {
            var query = @"insert into dbo.Employee (EmployeeName,Department,DateOfJoining,PhotoFileName)
                    values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            var query = @"update dbo.Employee set EmployeeName= @EmployeeName, Department=@Department,
                            DateOfJoining=@DateOfJoining, PhotoFileName=@PhotoFileName where EmployeeId=@EmployeeId";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
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
            var query = @"delete from dbo.Employee where EmployeeId=@EmployeeId";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);

                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}