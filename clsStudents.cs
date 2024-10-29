using StudentsAPIDataAccessLayer;

namespace StudentsAPIBusinessLayer
{
    public class clsStudents
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public StudentDTO SDTO
        {
            get { return new StudentDTO(this.ID, this.Name, this.Age, this.Grade); }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public byte Age { get; set; }
        public decimal Grade { get; set; }

        public bool Gendor { get; set; } = false;

        public clsStudents(StudentDTO SDTO, enMode cMode = enMode.AddNew)

        {
            this.ID = SDTO.Id;
            this.Name = SDTO.Name;
            this.Age = SDTO.Age;
            this.Grade = SDTO.Grade;

            Mode = cMode;
        }
        private async Task<bool> _AddNewStudent()
        {
            //call DataAccess Layer 

            this.ID = await clsStudentsData.AddStudentAsync(SDTO);

            return (this.ID != -1);
        }

        private async Task<bool> _UpdateStudent()
        {
            return await clsStudentsData.UpdateStudentAsync(SDTO);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewStudent())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:

                    return await _UpdateStudent();
            }

            return false;
        }


        public static async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            return await clsStudentsData.GetAllStudentsAsync();
        }

        public static async Task<List<StudentDTO>> GetAllPassedStudentsAsync()
        {
            return await clsStudentsData.GetAllPassedStudentsAsync();
        }

        public static async Task<double> GetStudentsAverageGradeAsync()
        {
            return await clsStudentsData.GetStudentsAVGGradeAsync();
        }

        public static async Task<clsStudents> GetStudentByIDAsync(int ID)
        {
            StudentDTO studentDto = await clsStudentsData.GetStudentByIdAsync(ID);

            if (studentDto != null)
            {
                return new clsStudents(studentDto, enMode.Update);
            }
            else
                return null;
        }

        public static async Task<bool> DeleteStudentByIDAsync(int ID)
        {
            return await clsStudentsData.DeleteStudentAsync(ID);
        }
    }
}
