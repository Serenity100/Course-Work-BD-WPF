using System;
using System.Windows.Controls;

namespace Course_Work_BD_WPF
{
    class OrderGetter : Getter <SellerPage>
    {
        public OrderGetter(SellerPage parent) : base(parent) { }

        public int GetCourierId()
        {
            if (parent.CourierId.Value.ToString().Length == 0)
                return -1;

            return (int)parent.CourierId.Value;
        }

        public int GetSellerId() => (int)parent.SellerId.Value;
        public int GetBuyerId() => (int)parent.BuyerId.Value;

        public String GetCompletionDate() => parent.CompletionDatePicker.Text;
        public String GetIssueDate() => parent.IssueDatePicker.Text;
        public String GetDeliveryMethod() => (parent.DeliveryMethodCmb.SelectedValue as ComboBoxItem).Content as String;
    }
}
