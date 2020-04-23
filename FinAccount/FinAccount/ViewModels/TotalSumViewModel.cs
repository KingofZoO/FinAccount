using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FinAccount.ViewModels {
    public class TotalSumViewModel : INotifyPropertyChanged {
        public MainPageViewModel MainViewModel { get; private set; }
        private string enteredSum = "";

        public ICommand SetTotalSumCommand { get; private set; }
        public ICommand FocusEntryCommand { get; private set; }

        public TotalSumViewModel(MainPageViewModel vm) {
            MainViewModel = vm;
            SetTotalSumCommand = new Command(SetTotalSum);
            FocusEntryCommand = new Command<Entry>((entry) => entry.Focus());
        }

        public string EnteredSum {
            get => enteredSum;
            set {
                if (enteredSum != value) {
                    enteredSum = value;
                    OnPropertyChanged("EnteredSum");
                }
            }
        }

        private async void SetTotalSum() {
            if (decimal.TryParse(EnteredSum.ToString(), out decimal res)) {
                bool result = await Application.Current.MainPage.DisplayAlert("Подтвердить действие", "Изменить баланс?", "Да", "Нет");
                if (result) {
                    MainViewModel.TotalSum = res;
                    EnteredSum = string.Empty;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
