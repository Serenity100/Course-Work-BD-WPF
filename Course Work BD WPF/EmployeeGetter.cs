using System;

namespace Course_Work_BD_WPF
{
    class EmployeeGetter : Getter <AddEmployeeToDBPage>
    {
        public EmployeeGetter(AddEmployeeToDBPage parent) : base(parent) { }

        public String GetPhone()
        {
            return parent.PhoneTB.Text;
        }

        public String GetName()
        {
            return parent.NameTB.Text;
        }

        public String GetSurname()
        {
            return parent.SurnameTB.Text;
        }

        public String GetBirthdayDate()
        {
            return parent.BirthdayDatePicker.Text;
        }

        public String GetEmploymentDate()
        {
            return parent.EmploymentDatePicker.Text;
        }
    }
}
