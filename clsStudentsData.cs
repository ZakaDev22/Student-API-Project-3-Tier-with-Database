using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentsAPIDataAccessLayer
{
    public class StudentDTO
    {
        public StudentDTO(int id, string name, byte age, decimal grade)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Grade = grade;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public byte Age { get; set; }
        public decimal Grade { get; set; }
    }


    public class clsStudentsData
    {
        static string _connectionString = "Server=localhost;Database=StudentDB;Integrated Security=True;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;";


        public static async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            List<StudentDTO> studentDTOs = new List<StudentDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Students_GetAll", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                studentDTOs.Add(new StudentDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("ID")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetByte(reader.GetOrdinal("Age")),
                                    reader.GetDecimal(reader.GetOrdinal("Grade"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log sqlEx.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
            catch (Exception ex)
            {
                // Log ex.Message or use a logging framework
                throw; // rethrow or handle as needed
            }

            return studentDTOs;
        }

        public static async Task<List<StudentDTO>> GetAllPassedStudentsAsync()
        {
            List<StudentDTO> studentDTOs = new List<StudentDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Students_GetPassedStudent", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                studentDTOs.Add(new StudentDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("ID")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetByte(reader.GetOrdinal("Age")),
                                    reader.GetDecimal(reader.GetOrdinal("Grade"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log sqlEx.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
            catch (Exception ex)
            {
                // Log ex.Message or use a logging framework
                throw; // rethrow or handle as needed
            }

            return studentDTOs;
        }


        public static async Task<double> GetStudentsAVGGradeAsync()
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Students_GetStudentsAVGGrade", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();

                        return await cmd.ExecuteScalarAsync() != DBNull.Value ? Convert.ToDouble(cmd.ExecuteScalar()) : 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log sqlEx.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
            catch (Exception ex)
            {
                // Log ex.Message or use a logging framework
                throw; // rethrow or handle as needed
            }

        }

        public static async Task<StudentDTO> GetStudentByIdAsync(int studentId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_Students_GetStudentByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentID", studentId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetByte(reader.GetOrdinal("Age")),
                                reader.GetDecimal(reader.GetOrdinal("Grade"))
                            );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log sqlEx.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
            catch (Exception ex)
            {
                // Log ex.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
        }

        public static async Task<int> AddStudentAsync(StudentDTO StudentDTO)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_Students_AddNewStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                    command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                    command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);

                    var outputIdParam = new SqlParameter("@ID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(outputIdParam);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    return (int)outputIdParam.Value;
                }
            }
            catch (SqlException sqlEx)
            {
                // Log sqlEx.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
            catch (Exception ex)
            {
                // Log ex.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
        }



        public static async Task<bool> UpdateStudentAsync(StudentDTO StudentDTO)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_Students_UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", StudentDTO.Id);
                    command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                    command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                    command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
            catch (SqlException sqlEx)
            {
                // Log sqlEx.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
            catch (Exception ex)
            {
                // Log ex.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
        }

        public static async Task<bool> DeleteStudentAsync(int studentId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_Students_DeleteStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", studentId);

                    await connection.OpenAsync();

                    return Convert.ToBoolean(command.ExecuteScalar());


                }
            }
            catch (SqlException sqlEx)
            {
                // Log sqlEx.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
            catch (Exception ex)
            {
                // Log ex.Message or use a logging framework
                throw; // rethrow or handle as needed
            }
        }



    }
}
