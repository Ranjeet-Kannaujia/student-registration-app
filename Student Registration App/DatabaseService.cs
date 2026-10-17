using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Registration_App
{
    public class DatabaseService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentRegistrationDB"].ConnectionString;
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            string query = "Select Id, Name,Age,Course,Gender from Students";
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student();
                            student.Id = Convert.ToInt32(reader["ID"]);
                            student.Name = reader["Name"].ToString();
                            student.Age = Convert.ToInt32(reader["Age"]);
                            student.Course = reader["Course"].ToString();
                            student.Gender = reader["Gender"].ToString();
                            students.Add(student);
                        }
                    }
                }
            }
            return students;
        }
        public List<Student> SearchStudents(string searchText)
        {
            List<Student> students = new List<Student>();
            string query = @"Select Id,Name,Age,Course,Gender from Students where CONVERT(NVARCHAR(20),Id) LIKE @Search OR Name Like @Search Or Course Like @Search Or Gender Like @Search";
            using (SqlConnection con = GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Search", "%" + searchText + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student();

                            student.Id = Convert.ToInt32(reader["ID"]);
                            student.Name = reader["Name"].ToString();
                            student.Age = Convert.ToInt32(reader["Age"]);
                            student.Course = reader["Course"].ToString();
                            student.Gender = reader["Gender"].ToString();
                            students.Add(student);
                        }
                    }
                }
            }
            return students;
        }
        public int AddStudent(Student student)
        {
            string query = @"Insert into Students (Name, Age, Course, Gender) Output Inserted.Id Values(@Name, @Age,@Course, @Gender)";
            using (SqlConnection con = GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    cmd.Parameters.AddWithValue("@Course", student.Course);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public void UpdateStudent(Student student)
        {
            string query = @"Update Students SET Name = @Name,Age=@Age,Course=@Course,Gender=@Gender where Id=@Id";
            using (SqlConnection con = GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", student.Id);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    cmd.Parameters.AddWithValue("@Course", student.Course);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);

                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void DeleteStudent(int studentId)
        {
            string query = @"Delete from Students where Id=@Id";
            using (SqlConnection con = GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", studentId);
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
