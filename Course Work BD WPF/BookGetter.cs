using System;

namespace Course_Work_BD_WPF
{
    class BookGetter : Getter <AddBookToDBPage>
    {
        public BookGetter(AddBookToDBPage parent) : base(parent) { }

        public int GetISBN()
        {
            Int32.TryParse(parent.ISBNTB.Text, out int result);
            return result;
        }

        public String GetTitle()
        {
            return parent.TitleTB.Text;
        }

        public String GetPublishYear()
        {
            return "01.01." + parent.YearTB.Text;
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

        public int GetPrice()
        {
            Int32.TryParse(parent.PriceTB.Text, out int result);
            return result;
        }
    }
}
