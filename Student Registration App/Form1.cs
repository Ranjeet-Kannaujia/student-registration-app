using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Configuration;
using System.Data.SqlClient;

namespace Student_Registration_App
{
    public partial class Form1 : Form
    {
        private List<Student> students = new List<Student>();
        //private int selectedStudentIndex = -1;
        private int selectedStudentId = -1;
        //private JsonService jsonService = new JsonService();
        private DatabaseService databaseService = new DatabaseService();
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentRegistrationDB"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = true;
            LoadStudents();
            btnAddStudent.Enabled = true;
            btnUpdateStudent.Enabled = false;
            cmbCourse.SelectedIndex = 0;
            btnViewStudent.Enabled = false;
            TestDataBaseConnection();
            //TestDataBase();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            selectedStudentId = Convert.ToInt32(dgvStudents.Rows[e.RowIndex].Cells["colId"].Value);
            //selectedStudentIndex = e.RowIndex;
            //selectedStudentId = students[selectedStudentIndex].Id;
            btnUpdateStudent.Enabled = true;
            btnAddStudent.Enabled = false;
            btnViewStudent.Enabled = true;
            //Student student = students[selectedStudentIndex];
            Student student = students.FirstOrDefault(s => s.Id == selectedStudentId);
            if (student == null)
            {
                MessageBox.Show("Student not found.");
                return;
            }

            txtName.Text = student.Name;
            txtAge.Text = student.Age.ToString();
            cmbCourse.Text = student.Course;
            if (student.Gender == "Male")
            {
                rbMale.Checked = true;
            }
            else if (student.Gender == "Female")
            {
                rbFemale.Checked = true;
            }
            else { rbMale.Checked = false; }
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            string name;
            int age;
            string course;
            string gender;
            if (!ValidateStudent(out name, out age, out course, out gender))
            {
                return;
            }
            Student student = new Student();
            //student.Id = GetNextStudentID();
            student.Name = name;
            student.Age = Convert.ToInt32(age);
            student.Course = course;
            student.Gender = gender;
            student.Id = databaseService.AddStudent(student);
            students.Add(student);

            //dgvStudents.Rows.Add(name, age, course, gender);
            dgvStudents.Rows.Add(student.Id, student.Name, student.Age, student.Course, student.Gender);
            //SaveStudentToJson();
            MessageBox.Show("Student Added SuccessFully!!");
            ClearForm();
        }

        /* private void btnDeleteStudent_Click(object sender, EventArgs e)
         {
             if (dgvStudents.CurrentRow == null)
             {
                 MessageBox.Show("Please select a student.");
                 return;
             }
             int rowIndex = dgvStudents.CurrentRow.Index;
             students.RemoveAt(rowIndex);
             dgvStudents.Rows.RemoveAt(rowIndex);
             SaveStudentToJson();
             MessageBox.Show("Student deleted successfully.");
             txtName.Clear();
             txtAge.Clear();
             cmbCourse.SelectedIndex = -1;
             rbMale.Checked = false;
             rbFemale.Checked = false;
             txtName.Focus();
         }
         private void btnDeleteStudent_Click(object sender, EventArgs e)
         {
             if (dgvStudents.CurrentRow == null)
             {
                 MessageBox.Show("Please select a student.");
                 return;
             }

             // Check if user selected the empty/new row
             if (dgvStudents.CurrentRow.IsNewRow)
             {
                 MessageBox.Show("Please select a valid student.");
                 return;
             }

             int rowIndex = dgvStudents.CurrentRow.Index;

             // Check whether index exists in students list
             if (rowIndex < 0 || rowIndex >= students.Count)
             {
                 MessageBox.Show("Invalid student selection.");
                 return;
             }

             students.RemoveAt(rowIndex);

             dgvStudents.Rows.RemoveAt(rowIndex);
             SaveStudentToJson();
             MessageBox.Show("Student deleted successfully.");

             txtName.Clear();
             txtAge.Clear();
             cmbCourse.SelectedIndex = -1;
             rbMale.Checked = false;
             rbFemale.Checked = false;

             txtName.Focus();
         }*/

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            if (selectedStudentId == -1)
            {
                MessageBox.Show("Please select a student to update...");
                return;
            }
            string name;
            int age;
            string course;
            string gender;
            if (!ValidateStudent(out name, out age, out course, out gender, selectedStudentId))
            {
                return;
            }
            Student student = students.FirstOrDefault(s => s.Id == selectedStudentId);
            if (student == null)
            {
                MessageBox.Show("Student not found.");
                return;
            }
            int rowIndex = -1;
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (!row.IsNewRow && Convert.ToInt32(row.Cells["colId"].Value) == selectedStudentId)
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            if (rowIndex == -1)
            {
                MessageBox.Show("Student row not found.");
                return;
            }
            /*int rowIndex = students.FindIndex(s => s.Id == selectedStudentId);
            if (rowIndex == -1)
            {
                MessageBox.Show("Student row not found.");
                return;
            }*/

            student.Name = name;
            student.Age = age;
            student.Course = course;
            student.Gender = gender;
            databaseService.UpdateStudent(student);
            //SaveStudentToJson();
            //Student student = students[selectedStudentIndex];
            dgvStudents.Rows[rowIndex].Cells["colName"].Value = student.Name;
            dgvStudents.Rows[rowIndex].Cells["colAge"].Value = student.Age;
            dgvStudents.Rows[rowIndex].Cells["colCourse"].Value = student.Course;
            dgvStudents.Rows[rowIndex].Cells["colGender"].Value = student.Gender;
            /*dgvStudents.Rows[selectedStudentIndex].Cells["colName"].Value = student.Name;
            dgvStudents.Rows[selectedStudentIndex].Cells["colAge"].Value = student.Age;
            dgvStudents.Rows[selectedStudentIndex].Cells["colCourse"].Value = student.Course;
            dgvStudents.Rows[selectedStudentIndex].Cells["colGender"].Value = student.Gender;*/
            MessageBox.Show("Student Updated successfully. ");
            ClearForm();
        }
        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one student.");
                return;
            }
            // Store selected row indexes
            List<int> selectedStudentIds = new List<int>();

            foreach (DataGridViewRow row in dgvStudents.SelectedRows)
            {
                // Ignore the empty/new row
                if (!row.IsNewRow)
                {
                    int studentId = Convert.ToInt32(row.Cells["colId"].Value);
                    selectedStudentIds.Add(studentId);
                }
            }
            if (selectedStudentIds.Count == 0)
            {
                MessageBox.Show("Please select a valid student.");
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure you want to delete " + selectedStudentIds.Count + " student(s)?", " Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }
            /*//foreach (int index in selectedStudentIds)
            //{
            //    if (index >= 0 && index < students.Count)
            //    {
            //        students.RemoveAt(index);
            //    }
            //    if (index >= 0 && index < dgvStudents.Rows.Count)
            //    {
            //        dgvStudents.Rows.RemoveAt(index);
            //    }
            //}*/

            int deletedCount = 0;
            foreach (int studentId in selectedStudentIds)
            {
                databaseService.DeleteStudent(studentId);
                int studentIndex = students.FindIndex(s => s.Id == studentId);
                if (studentIndex >= 0)
                {
                    students.RemoveAt(studentIndex);
                    deletedCount++;
                }
                DataGridViewRow rowToRemove = null;

                foreach (DataGridViewRow gridRow in dgvStudents.Rows)
                {
                    if (!gridRow.IsNewRow && Convert.ToInt32(gridRow.Cells["colId"].Value) == studentId)
                    {
                        rowToRemove = gridRow;
                        break;
                    }
                }
                if (rowToRemove != null)
                {
                    dgvStudents.Rows.Remove(rowToRemove);
                }
            }
            //SaveStudentToJson();
            MessageBox.Show(deletedCount + " student(s) deleted successfully.");
            ClearForm();
        }
        /*// Delete from last index to first index
        //    selectedIndexes.Sort();
        //selectedIndexes.Reverse();




        //SaveStudentToJson();

        //MessageBox.Show(selectedIndexes.Count + " students deleted successfully.");

        //ClearForm();*/
        private void lblSearch_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtSearch.Text.Trim();
            if (searchName == "")
            {
                MessageBox.Show("Please a name to search.");
                txtSearch.Focus();
                return;
            }
            List<Student> searchResult = databaseService.SearchStudents(searchName); ;
            dgvStudents.Rows.Clear();
            if (searchResult.Count == 0)
            {
                MessageBox.Show("Student not found.");
                return;
            }
            //students = searchResult;
            foreach (Student student in searchResult)
            {
                dgvStudents.Rows.Add(student.Id, student.Name, student.Age, student.Course, student.Gender);
            }
            selectedStudentId = searchResult[0].Id;
            btnViewStudent.Enabled = true;
            dgvStudents.Rows[0].Selected = true;
            dgvStudents.CurrentCell = dgvStudents.Rows[0].Cells["colId"];

            /*dgvStudents.ClearSelection();
            bool found = false;
            int firstMatchIndex = -1;
            for (int i = 0; i < students.Count; i++)
            {
                //if (students[i].Name.ToLower().Contains(searchName.ToLower()))
                if (students[i].Id.ToString().Contains(searchName)
                  || students[i].Name.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) >= 0
                  || students[i].Course.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) >= 0
                  || students[i].Gender.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    dgvStudents.Rows[i].Selected = true;
                    if (firstMatchIndex == -1)
                    {
                        firstMatchIndex = i;
                    }
                    found = true;
                    //break;
                }
            }
            if (found)
            {
                dgvStudents.CurrentCell = dgvStudents.Rows[firstMatchIndex].Cells[0];

            }
            else
            {
                MessageBox.Show("Student not found.");
            }*/

        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            dgvStudents.ClearSelection();
            ClearForm();
            LoadStudents();
        }

        /*  private void btnSave_Click(object sender, EventArgs e)
          {
              string name = txtName.Text.Trim();
              string ageText = txtAge.Text.Trim();
              string course = cmbCourse.Text;
              string gender = "";

              if (name == "")
              {
                  MessageBox.Show("Please enter student name. ");
                  txtName.Focus();
                  return;
              }
              if (!int.TryParse(ageText, out int age))
              {
                  MessageBox.Show("Please enter valid age. ");
                  txtAge.Focus();
                  return;
              }
              if (course == "")
              {
                  MessageBox.Show("Please select course. ");
                  cmbCourse.Focus();
                  return;
              }
              if (rbMale.Checked)
              {
                  gender = "Male";
              }
              else if (rbFemale.Checked)
              {
                  gender = "Female";
              }
              else
              {
                  MessageBox.Show("Please select gender. ");
                  return;
              }
              Student student = new Student();
              student.Name = name;
              student.Age = Convert.ToInt32(age);
              student.Course = course;
              student.Gender = gender;

              students.Add(student);

              //dgvStudents.Rows.Add(name, age, course, gender);
              dgvStudents.Rows.Add(student.Name, student.Age, student.Course, student.Gender);
              SaveStudentToJson();
              MessageBox.Show("Student saved successfully!");

              txtName.Clear();
              txtAge.Clear();
              cmbCourse.SelectedIndex = -1;
              rbMale.Checked = false;
              rbFemale.Checked = false;
              txtName.Focus();
              //string filePath = "students.json";
              //string json = JsonConvert.SerializeObject(students, Formatting.Indented);
              //File.WriteAllText(filePath, json);
              //MessageBox.Show("Student saved successfully!\n\nFile Location:\n" + Path.GetFullPath(filePath));
          }*/

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void LoadStudents()
        {
            students = databaseService.GetAllStudents();

            //students = jsonService.LoadStudents();

            /* //int nextId = 1;
             //foreach (Student student in students)
             //{
             //    if (student.Id == 0)
             //    {
             //        student.Id = nextId;
             //    }

             //    nextId = Math.Max(nextId, student.Id + 1);
             //}*/

            /*bool idNeedsFix = students.Any(s => s.Id == 0);

            if (idNeedsFix)
            {
                for (int i = 0; i < students.Count; i++)
                {
                    students[i].Id = i + 1;
                }
                //SaveStudentToJson();
            }*/

            dgvStudents.Rows.Clear();

            foreach (Student student in students)
            {
                dgvStudents.Rows.Add(student.Id, student.Name, student.Age, student.Course, student.Gender);
            }

            /* string filePath = "students.json";
             if (!File.Exists(filePath))
                 return;
             string json = File.ReadAllText(filePath);
             //students = JsonConvert.DeserializeObject<List<Student>>(json);
             students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
             dgvStudents.Rows.Clear();
             foreach (Student student in students)
             {
                 dgvStudents.Rows.Add(student.Name, student.Age, student.Course, student.Gender);
             }*/

        }

        private void ClearForm()
        {
            txtName.Clear();
            txtAge.Clear();
            //txtSearch.Clear();
            dgvStudents.ClearSelection();
            dgvStudents.CurrentCell = null;
            cmbCourse.SelectedIndex = 0;
            rbMale.Checked = false;
            rbFemale.Checked = false;
            //selectedStudentIndex = -1;
            txtName.Focus();
            selectedStudentId = -1;
            btnUpdateStudent.Enabled = false;
            btnAddStudent.Enabled = true;
            btnViewStudent.Enabled = false;
        }

        /* private int GetNextStudentID()
         {
             if (students.Count == 0)
             {
                 return 1;
             }
             return students.Max(s => s.Id) + 1;
         }*/

        private bool ValidateStudent(out string name, out int age, out string course, out string gender, int ignoreStudentId = -1)
        {
            name = txtName.Text.Trim();
            string ageText = txtAge.Text.Trim();
            course = cmbCourse.Text.Trim();
            gender = "";

            if (name == "")
            {
                MessageBox.Show("Please enter student name.");
                txtName.Focus();
                age = 0;
                return false;
            }

            foreach (char c in name)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Name Should contains only letters.");
                    txtName.Focus();
                    age = 0;
                    return false;
                }
            }
            if (!int.TryParse(ageText, out age))
            {
                MessageBox.Show("Please Enter a valid age.");
                txtAge.Focus();
                return false;
            }
            if (age < 1 || age > 100)
            {
                MessageBox.Show("Please enter age between 1 and 100.");
                txtAge.Focus();
                return false;
            }
            if (cmbCourse.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a course.");
                cmbCourse.Focus();
                return false;
            }
            if (rbMale.Checked)
            {
                gender = "Male";
            }
            else if (rbFemale.Checked)
            {
                gender = "Female";
            }
            else
            {
                MessageBox.Show("Please Select gender.");
                return false;
            }
            // Local variables for duplicate check
            string checkName = name;
            int checkAge = age;
            string checkCourse = course;
            string checkGender = gender;

            //bool isDuplicate = Enumerable.Range(0, students.Count)
            //    .Any(i => i != ignoreStudentId &&.
            bool isDuplicate = students.Any(s => s.Id != ignoreStudentId &&
                s.Name.Equals(checkName, StringComparison.OrdinalIgnoreCase) &&
                s.Age == checkAge &&
                s.Course.Equals(checkCourse, StringComparison.OrdinalIgnoreCase) &&
                s.Gender.Equals(checkGender, StringComparison.OrdinalIgnoreCase)
                );
            //students[i].Name.Equals(checkName, StringComparison.OrdinalIgnoreCase) &&
            // students[i].Age == checkAge &&
            // students[i].Course.Equals(checkCourse, StringComparison.OrdinalIgnoreCase) &&
            // students[i].Gender.Equals(checkGender, StringComparison.OrdinalIgnoreCase)
            // );

            /*bool isDuplicate = students.Any(s => s.Name.Equals(checkName, StringComparison.OrdinalIgnoreCase) && s.Age == checkAge && s.Course.Equals(checkCourse, StringComparison.OrdinalIgnoreCase) && s.Gender.Equals(checkGender, StringComparison.OrdinalIgnoreCase));*/

            if (isDuplicate)
            {
                MessageBox.Show("This Student is already exists.");
                return false;
            }
            return true;
        }

        /*private void SaveStudentToJson()
        {
            jsonService.SaveStudents(students);

           string filePath = "students.json";
            string json = JsonConvert.SerializeObject(students,Formatting.Indented);
            File.WriteAllText(filePath, json);
        }*/

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnViewStudent_Click(object sender, EventArgs e)
        {
            if (selectedStudentId == -1)
            {
                MessageBox.Show("Please select a student.");
                return;
            }

            Student student = students.FirstOrDefault(s => s.Id == selectedStudentId);
            if (student == null)
            {
                MessageBox.Show("Student not fount.");
                return;
            }

            StudentDetailsForm form = new StudentDetailsForm(student);
            form.ShowDialog();
            /* //if (selectedStudentId == -1)
             //{
             //    MessageBox.Show("Please select a student.");
             //    return;
             //}
             //Student student = students.FirstOrDefault(s => s.Id == selectedStudentId);
             //if (student == null)
             //{
             //    MessageBox.Show("Student not Found.");
             //    return;
             //}
             //string details = "STUDENT DETAILS\n" +
             //    "------------------------------\n" +
             //    "Id :" + student.Id + "\n" + "Name : " + student.Name + "\n" + "Age : " + student.Age + "\n" + "Course: " + student.Course + "\n" + "Gender :" + student.Gender + "\n" + "------------------------------";
             //MessageBox.Show(details, "Student Details", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
        }
        private void TestDataBaseConnection()
        {
            try
            {
                using (SqlConnection connection = databaseService.GetConnection())
                {
                    connection.Open();
                    MessageBox.Show("DataBase Connection Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DataBase Connection failed:\n" + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TestDataBase()
        {
            List<Student> dbStudents = databaseService.GetAllStudents();
            //MessageBox.Show("Database se "+dbStudents.Count+" students(s) mile.");
        }
    }
}
