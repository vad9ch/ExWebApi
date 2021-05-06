using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _appEnvironment;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment appEnvironment)
        {
            _configuration = configuration;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public JsonResult Get(int page, string empName = "")
        {
            try
            {
                using (AngTestContext db = new AngTestContext())
                {
                    var epms = db.Employee.Where(x => x.EmployeeName.ToLower().Contains(empName.ToLower())).Skip(page * 5).Take(5).ToList();
                    //var epms = db.Employee.ToList();
                    return Json(epms);
                }
            }
            catch (Exception)
            {
                return new JsonResult(new { error = "failed" });
            }
        }



        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            try
            {
                using (AngTestContext db = new AngTestContext())
                {
                    DateTime thisDay = DateTime.Today;
                    Employee emp1 = new Employee();
                    if (emp.EmployeeName != "")
                    {
                        emp1.EmployeeName = emp.EmployeeName;
                    }
                    else
                    {
                        return new JsonResult("enter the name of the employee!");
                    }
                    emp1.Department = emp.Department;

                    if (emp.DateOfJoining != null & emp.DateOfJoining <= thisDay)
                    {
                        emp1.DateOfJoining = emp.DateOfJoining;
                    }
                    else
                    {
                        return new JsonResult("enter date of joining correctly!");
                    }
                    emp1.PhotoFileName = emp.PhotoFileName;
                    db.Employee.Add(emp1);
                    db.SaveChanges();
                }
                return new JsonResult("Added Successfully!");
            }
            catch (Exception)
            {
                return new JsonResult("Failed to Add!");
            }
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            try
            {
                using (AngTestContext db = new AngTestContext())
                {

                    var id = emp.EmployeeId;
                    var emp1 = db.Employee.Find(id);
                    emp1.EmployeeName = emp.EmployeeName;
                    emp1.Department = emp.Department;
                    emp1.DateOfJoining = emp.DateOfJoining;
                    emp1.PhotoFileName = emp.PhotoFileName;
                    db.SaveChanges();
                }
                return new JsonResult("Updated successfully!");
            }
            catch (Exception)
            {
                return new JsonResult("Failed to Update!");
            }
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                using (AngTestContext db = new AngTestContext())
                {
                    var emp1 = db.Employee.Find(id);
                    db.Employee.Remove(emp1);
                    db.SaveChanges();
                }
                return new JsonResult("Deleted successfully!");
            }
            catch (Exception)
            {
                return new JsonResult("Failed to Delete!");
            }
        }


        [Route("SaveFile")]
        [HttpPost]
        public IActionResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _appEnvironment.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return Json(filename);
            }
            catch (Exception)
            {

                return Json("anonymous.png");
            }
        }
        [Route("GetAllDepartmentNames")]
        public IActionResult GetAllDepartmentNames()
        {
            using (AngTestContext db = new AngTestContext())
            {
                var depNames = db.Department.ToList();
                return Json(depNames);
            }
        }
    }
}
