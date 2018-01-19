using System;

namespace Course_Work_BD_WPF
{
    class AuthorGetter : Getter<AddAuthorToDBPage>
    {
        public AuthorGetter(AddAuthorToDBPage parent) : base(parent) { }

        public String GetName()
        {
            return parent.NameTB.Text;
        }

        public String GetSurname()
        {
            return parent.SurnameTB.Text;
        }

        public String GetPatronymic()
        {
            return parent.PatronymicTB.Text;
        }

        public String GetBirthdayYear()
        {
            return parent.BirthdayYearTB.Text;
        }
    }
}
