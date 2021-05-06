using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _appEnvironment;

        public DepartmentController(IConfiguration configuration, IWebHostEnvironment appEnvironment)
        {
            _configuration = configuration;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public JsonResult Get(int page, string depName ="")
        {
            try
            {
                using (AngTestContext db = new AngTestContext())
                {
                    var deps = db.Department.Where(x => x.DepartmentName.ToLower().Contains(depName.ToLower())).Skip(page * 5).Take(5).ToList();
                    //var deps = db.Department.ToList();
                    return Json(deps);
                }
            }
            catch (Exception)
            {
                return new JsonResult("failed");
            }
        }


        [HttpPost]
        public JsonResult Post(Department dep)
        {
            try
            {
                using (AngTestContext db = new AngTestContext())
                {
                    Department dep1 = new Department();
                    if (dep.DepartmentName != "")
                    {
                        dep1.DepartmentName = dep.DepartmentName;
                    }
                    else
                    {
                        return new JsonResult("Enter name of the department!");
                    }
                    db.Department.Add(dep1);
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
        public JsonResult Put(Department dep)
        {
            try
            {
                using (AngTestContext db = new AngTestContext())
                {
                    var id = dep.DepartmentId;
                    var dep1 = db.Department.Find(id);
                    dep1.DepartmentName = dep.DepartmentName;
                    db.SaveChanges();
                }
                return new JsonResult("Updated Successfully!");
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
                    var dep1 = db.Department.Find(id);
                    db.Department.Remove(dep1);
                    db.SaveChanges();
                }
                return new JsonResult("Deleted Successfully!");
            }
            catch (Exception)
            {
                return new JsonResult("Failed to Delete !");
            }
        }
    }
}
