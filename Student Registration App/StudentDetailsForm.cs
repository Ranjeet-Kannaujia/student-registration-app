using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Registration_App
{
    public partial class StudentDetailsForm : Form
    {
        public StudentDetailsForm(Student student)
        {
            InitializeComponent();
            lblIdValue.Text = student.Id.ToString();
            lblNameValue.Text = student.Name;
            lblAgeValue.Text = student.Age.ToString();
            lblCourseValue.Text = student.Course;
            lblGenderValue.Text = student.Gender;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
