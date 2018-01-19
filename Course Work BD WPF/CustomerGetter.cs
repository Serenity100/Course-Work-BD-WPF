using System;

namespace Course_Work_BD_WPF
{
    class CustomerGetter : Getter <AddCustomerToDBPage>
    {
        public CustomerGetter(AddCustomerToDBPage parent) : base(parent) { }

        public String GetName()
        {
            return parent.NameTB.Text;
        }

        public String GetSurname()
        {
            return parent.SurnameTB.Text;
        }

        public String GetPhone()
        {
            return parent.PhoneTB.Text;
        }

        private String GetCity()
        {
            return parent.CityTB.Text;
        }

        private String GetStreet()
        {
            return parent.StreetTB.Text;
        }

        private String GetHouse()
        {
            return parent.HouseTB.Text;
        }

        private String GetFlat()
        {
            return parent.FlatTB.Text;
        }

        public String GetAddress()
        {
            return "г. " + GetCity() + ", ул. " + GetStreet() + ", д. " + GetHouse() + ", кв. " + GetFlat();
        }
    }
}
