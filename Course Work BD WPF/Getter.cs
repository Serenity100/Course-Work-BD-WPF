using System.Windows.Controls;

namespace Course_Work_BD_WPF
{
    class Getter <T> where T : Page
    {
        protected T parent;
        public Getter(T page) => parent = page;
    }
}
