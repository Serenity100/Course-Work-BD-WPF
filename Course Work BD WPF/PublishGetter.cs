using System;

namespace Course_Work_BD_WPF
{
    class PublishGetter : Getter <AddPublishToDBPage>
    {
        public PublishGetter(AddPublishToDBPage parent) : base(parent) { }

        public String GetTitle()
        {
            return parent.TitleTB.Text;
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
