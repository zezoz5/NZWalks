using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Controllers
{
    // https://localhost:7259/api/Student
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        // Get: https://localhost:7259/api/Student
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentNames = new string[] { "John", "Jane", "Mark", "Emily", "David" };

            return Ok(studentNames);
        }

    }
}