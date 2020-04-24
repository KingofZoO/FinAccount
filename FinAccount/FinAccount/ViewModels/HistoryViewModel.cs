using FinAccount.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FinAccount.ViewModels {
    public class HistoryViewModel : INotifyPropertyChanged {
        public MainPageViewModel MainViewModel { get; private set; }

        private decimal totalPositiveSum = 0;
        private decimal totalNegativeSum = 0;
        private decimal totalDifferenceSum = 0;
        private string selectedNote;

        private IEnumerable<YearFinRecords> yearListRecords;
        private YearFinRecords yearRecord;
        private MonthFinRecords monthRecord;

        private string[] MonthsName = new[] {
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"
        };

        public ICommand DropHistoryCommand { get; private set; }

        public HistoryViewModel(MainPageViewModel vm) {
            MainViewModel = vm;
            DropHistoryCommand = new Command(DropHistory);

            CreateRecordTree(App.DataBase.GetRecords());
        }

        public IEnumerable<YearFinRecords> YearListRecords {
            get => yearListRecords;
            set {
                if (yearListRecords != value) {
                    yearListRecords = value;
                    OnPropertyChanged("YearListRecords");
                }
            }
        }

        public YearFinRecords YearRecord {
            get => yearRecord;
            set {
                if (yearRecord != value) {
                    yearRecord = value;
                    OnPropertyChanged("YearRecord");

                    MonthRecord = null;
                }
            }
        }

        public MonthFinRecords MonthRecord {
            get => monthRecord;
            set {
                if (monthRecord != value) {
                    monthRecord = value;
                    OnPropertyChanged("MonthRecord");
                }
            }
        }

        public decimal TotalPositiveSum {
            get => totalPositiveSum;
            set {
                if (totalPositiveSum != value) {
                    totalPositiveSum = decimal.Round(value);
                    OnPropertyChanged("TotalPositiveSum");
                }
            }
        }

        public decimal TotalNegativeSum {
            get => totalNegativeSum;
            set {
                if (totalNegativeSum != value) {
                    totalNegativeSum = decimal.Round(value);
                    OnPropertyChanged("TotalNegativeSum");
                }
            }
        }

        public decimal TotalDifferenceSum {
            get => totalDifferenceSum;
            set {
                if (totalDifferenceSum != value) {
                    totalDifferenceSum = decimal.Round(value);
                    OnPropertyChanged("TotalDifferenceSum");
                }
            }
        }

        public string SelectedNote {
            get => selectedNote;
            set {
                if (selectedNote != value) {
                    selectedNote = value;
                    OnPropertyChanged("SelectedNote");

                    if (string.IsNullOrEmpty(value))
                        CreateRecordTree(App.DataBase.GetRecords());
                    else
                        CreateRecordTree(App.DataBase.RecordsByNote(value));
                }
            }
        }

        private async void DropHistory() {
            bool result = await Application.Current.MainPage.DisplayAlert("Подтвердить действие", "Удалить историю?", "Да", "Нет");
            if (result) {
                ClearViewRecords(true);

                MainViewModel.MonthRecords.Clear();
                App.DataBase.DropHistory();
            }
        }

        private void CreateRecordTree(IEnumerable<FinRecord> records) {
            if (!records.Any()) {
                ClearViewRecords(true);
                return;
            }

            ClearViewRecords(false);
            List<YearFinRecords> tempYearRecords = new List<YearFinRecords>();

            int year = records.First().Date.Year;
            int month = records.First().Date.Month;
            List<FinRecord> monthList = new List<FinRecord>();
            List<MonthFinRecords> monthsList = new List<MonthFinRecords>();

            foreach(var record in records) {
                int tempYear = record.Date.Year;
                int tempMonth = record.Date.Month;

                if (month != tempMonth) {
                    monthsList.Add(new MonthFinRecords(monthList, MonthsName[month - 1]));
                    month = tempMonth;
                    monthList = new List<FinRecord>();
                }

                if (year != tempYear) {
                    tempYearRecords.Add(new YearFinRecords(monthsList, year.ToString()));
                    year = tempYear;
                    monthsList = new List<MonthFinRecords>();
                }

                monthList.Add(record);
            }

            if (monthList.Any())
                monthsList.Add(new MonthFinRecords(monthList, MonthsName[month - 1].ToString()));

            if (monthsList.Any())
                tempYearRecords.Add(new YearFinRecords(monthsList, year.ToString()));

            YearListRecords = tempYearRecords;

            CalculateSums();
        }

        private void CalculateSums() {
            if (YearListRecords == null)
                return;

            TotalPositiveSum = 0;
            TotalNegativeSum = 0;
            TotalDifferenceSum = 0;

            foreach (var record in YearListRecords) {
                TotalPositiveSum += record.PositiveSum;
                TotalNegativeSum += record.NegativeSum;
            }
            TotalDifferenceSum = TotalPositiveSum + TotalNegativeSum;
        }

        private void ClearViewRecords(bool isFullClear) {
            if (isFullClear) {
                YearListRecords = null;
                YearRecord = null;
                MonthRecord = null;

                TotalPositiveSum = 0;
                TotalNegativeSum = 0;
                TotalDifferenceSum = 0;
            }
            else {
                YearRecord = null;
                MonthRecord = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
