using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinAccount.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage {
        private ViewCell lastTapedYear;
        private ViewCell lastTapedMonth;

        public HistoryPage() {
            InitializeComponent();
        }

        private void Year_ViewCell_Tapped(object sender, System.EventArgs e) {
            ChangeCellColor((ViewCell)sender, ref lastTapedYear);
        }

        private void Month_ViewCell_Tapped(object sender, System.EventArgs e) {
            ChangeCellColor((ViewCell)sender, ref lastTapedMonth);
        }

        private void ChangeCellColor(ViewCell senderCell, ref ViewCell tempCell) {
            if (tempCell != null)
                tempCell.View.BackgroundColor = Color.Transparent;

            tempCell = senderCell;
            tempCell.View.BackgroundColor = Color.FromHex("A5D6A7");
        }
    }
}