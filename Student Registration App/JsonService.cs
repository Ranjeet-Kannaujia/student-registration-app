using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Student_Registration_App
{
    public class JsonService
    {
        private string filePath = "students.json";

        public void SaveStudents(List<Student> students)
        {
            try
            {
                string json = JsonConvert.SerializeObject(students, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error while saving students:\n" + ex.Message, "Save Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error
                );
            }
        }
        public List<Student> LoadStudents()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<Student>();
                }
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Student>();
                }

                return JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading students:\n" + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return new List<Student>();
        }
    }
}
