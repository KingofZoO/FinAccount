using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using FinAccount.Models;
using FinAccount.Views;
using System.Collections.Generic;
using System.Text;

namespace FinAccount.ViewModels {
    public class MainPageViewModel : INotifyPropertyChanged {
        private decimal totalSum;
        private readonly string totalSumPropName = "TotalSumProp";
        private string noteListPropName = "NoteListProp";

        private decimal positiveSum = 0;
        private decimal negativeSum = 0;
        private decimal differenceSum = 0;

        public INavigation Navigation { get; set; }

        public ObservableCollection<FinRecord> MonthRecords { get; private set; }
        public ObservableCollection<string> NoteList { get; private set; }

        public ICommand AddCommand { get; private set; }
        public ICommand SubtractCommand { get; private set; }
        public ICommand RemoveRecordCommand { get; private set; }

        public ICommand EditTotalSumCommand { get; private set; }
        public ICommand ShowHistoryCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        public MainPageViewModel() {
            LoadAppProperties();

            //---------------------------FillDataBase();

            MonthRecords = new ObservableCollection<FinRecord>(App.DataBase.MonthRecords());
            MonthRecords.CollectionChanged += (s, e) => CalculateSums();
            CalculateSums();

            NoteList.CollectionChanged += (s, e) => {
                StringBuilder sb = new StringBuilder();
                foreach (string note in NoteList) {
                    if (!string.IsNullOrEmpty(note))
                        sb.Append(note + '?');
                }
                App.Current.Properties[noteListPropName] = sb.ToString();
            };

            AddCommand = new Command(AddSum);
            SubtractCommand = new Command(SubtractSum);
            RemoveRecordCommand = new Command<FinRecord>(RemoveRecord);
            EditTotalSumCommand = new Command(EditTotalSum);
            ShowHistoryCommand = new Command(ShowHistory);
            BackCommand = new Command(Back);
        }

        private void LoadAppProperties() {
            if (App.Current.Properties.TryGetValue(totalSumPropName, out object sum))
                totalSum = (decimal)sum;
            else
                totalSum = 0;

            if (App.Current.Properties.TryGetValue(noteListPropName, out object listString)) {
                NoteList = new ObservableCollection<string>(listString.ToString().Split(new char[] { '?' }));
            }
            else
                NoteList = new ObservableCollection<string>();
        }

        public decimal TotalSum {
            get => totalSum;
            set {
                if(totalSum != value) {
                    totalSum = decimal.Round(value);
                    OnPropertyChanged("TotalSum");
                    App.Current.Properties[totalSumPropName] = value;
                }
            }
        }

        public decimal PositiveSum {
            get => positiveSum;
            set {
                if (positiveSum != value) {
                    positiveSum = decimal.Round(value);
                    OnPropertyChanged("PositiveSum");
                }
            }
        }

        public decimal NegativeSum {
            get => negativeSum;
            set {
                if (negativeSum != value) {
                    negativeSum = decimal.Round(value);
                    OnPropertyChanged("NegativeSum");
                }
            }
        }

        public decimal DifferenceSum {
            get => differenceSum;
            set {
                if (differenceSum != value) {
                    differenceSum = decimal.Round(value);
                    OnPropertyChanged("DifferenceSum");
                }
            }
        }

        private void AddSum() {
            Navigation.PushAsync(new SumEntryPage {
                BindingContext = new SumEntryViewModel(this, true)
            });
        }

        private void SubtractSum() {
            Navigation.PushAsync(new SumEntryPage {
                BindingContext = new SumEntryViewModel(this, false)
            });
        }

        private void EditTotalSum() {
            Navigation.PushAsync(new TotalSumPage {
                BindingContext = new TotalSumViewModel(this)
            });
        }

        private void ShowHistory() {
            Navigation.PushAsync(new HistoryPage {
                BindingContext = new HistoryViewModel(this)
            });
        }

        private void Back() => Navigation.PopAsync();

        private void RemoveRecord(FinRecord cell) {
            MonthRecords.Remove(cell);
            App.DataBase.DeleteRecord(cell);
            TotalSum -= cell.Sum;
        }

        private void CalculateSums() {
            PositiveSum = 0;
            NegativeSum = 0;

            foreach (var record in MonthRecords) {
                if (record.Sum >= 0)
                    PositiveSum += record.Sum;
                else
                    NegativeSum += record.Sum;
            }
            DifferenceSum = PositiveSum + NegativeSum;
        }

        private void FillDataBase() {
            var rnd = new Random();
            int count = NoteList.Count;
            int num = 0;

            for(int y = 2018; y < 2021; y++) {
                for(int m = 1; m < 13; m++) {
                    for(int d = 1; d < 26; d++) {
                        if (y == 2020 && m == 4 && d == 21)
                            return;

                        App.DataBase.SaveRecord(new FinRecord {
                            Sum = rnd.Next(-1000, 1000),
                            Date = new DateTime(y, m, d),
                            Note = NoteList[num++ % count]
                        });
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
