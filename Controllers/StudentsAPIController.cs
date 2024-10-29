using Microsoft.AspNetCore.Mvc;
using StudentsAPIBusinessLayer;
using StudentsAPIDataAccessLayer;

namespace Student_API_Project_3_Tier_with_Database.Controllers
{
    [Route("api/StudentsAPI")]
    [ApiController]
    public class StudentsAPIController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //here we used StudentDTO
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudentsAsync() // Define a method to get all students.
        {

            List<StudentDTO> students = await StudentsAPIBusinessLayer.clsStudents.GetAllStudentsAsync();
            if (students.Count == 0)
            {
                return NotFound("No Students Found!");
            }
            return Ok(students); // Returns the list of students.

        }

        [HttpGet("Passed", Name = "GetAllPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudentPassedAsync()
        {
            List<StudentDTO> students = await StudentsAPIBusinessLayer.clsStudents.GetAllPassedStudentsAsync();
            if (students.Count == 0)
            {
                return NotFound("No Students Found!");
            }
            return Ok(students); // Returns the list of students.
        }

        [HttpGet("AVG", Name = "GetStudentsAVGGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<double>> GetStudentsAVGGradeAsync()
        {
            var AverageGrade = await clsStudentsData.GetStudentsAVGGradeAsync();

            return Ok(AverageGrade);
        }

        [HttpGet("GetStudentByID/{ID}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDTO>> GetStudentByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                return BadRequest($"Bad Request With ID {ID}!");
            }

            clsStudents student = await clsStudents.GetStudentByIDAsync(ID);
            if (student == null)
            {
                return NotFound($"Not Student With ID {ID} Has Ben Fount ");
            }

            StudentDTO studentDTO = student.SDTO;

            return Ok(studentDTO);
        }

        [HttpPost("AddNewStudent", Name = "AddNewStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDTO>> AddNewStudentAsync(StudentDTO NewStudent)
        {
            if (NewStudent == null || string.IsNullOrEmpty(NewStudent.Name) || NewStudent.Age < 0 || NewStudent.Grade < 0)
            {
                return BadRequest("Bad Request Informations");
            }

            clsStudents student = new clsStudents(new StudentDTO(NewStudent.Id, NewStudent.Name, NewStudent.Age, NewStudent.Grade));

            await student.SaveAsync();

            NewStudent.Id = student.ID;

            return CreatedAtRoute("GetStudentByID", new { ID = NewStudent.Id }, NewStudent);

        }

        [HttpDelete("DeleteStudentByID/{ID}", Name = "DeleteStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteStudentByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                return BadRequest($"Bad Request With ID {ID}!");
            }

            if (await clsStudents.DeleteStudentByIDAsync(ID))

                return Ok($"Student with ID {ID} has been deleted.");
            else
                return NotFound($"Student with ID {ID} not found. no rows deleted!");

        }

        [HttpPut("UpdateStudentByID/{ID}", Name = "UpdateStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<clsStudents>> UpdateStudentByID(int ID, StudentDTO UpdatedStudent)
        {
            if (ID < 1 || UpdatedStudent == null || string.IsNullOrEmpty(UpdatedStudent.Name) || UpdatedStudent.Age < 0 || UpdatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);

            clsStudents student = await clsStudents.GetStudentByIDAsync(ID);


            if (student == null)
            {
                return NotFound($"Student with ID {ID} not found.");
            }


            student.Name = UpdatedStudent.Name;
            student.Age = UpdatedStudent.Age;
            student.Grade = UpdatedStudent.Grade;

            if (await student.SaveAsync())
                //we return the DTO not the full student object.
                return Ok(student.SDTO);
            else
                return StatusCode(500, new { Message = "Error, Student was Not Updating" });

        }


    }
}
