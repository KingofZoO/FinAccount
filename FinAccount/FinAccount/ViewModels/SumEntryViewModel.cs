using System;
using System.Windows.Input;
using System.ComponentModel;
using Xamarin.Forms;
using FinAccount.Models;
using System.Linq;

namespace FinAccount.ViewModels {
    public class SumEntryViewModel : INotifyPropertyChanged {
        public bool IsPositive { get; private set; }
        public MainPageViewModel MainViewModel { get; private set; }

        private string enteredSum;
        private string enteredNote;

        public ICommand AddSymbolCommand { get; private set; }
        public ICommand RemoveSymbolCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand AddNoteCommand { get; private set; }
        public ICommand RemoveNoteCommand { get; private set; }

        public SumEntryViewModel(MainPageViewModel vm, bool isPositive) {
            MainViewModel = vm;
            IsPositive = isPositive;

            AddSymbolCommand = new Command<string>(AddSymbol);
            RemoveSymbolCommand = new Command(RemoveSymbol);
            ApplyCommand = new Command(ApplySum);
            AddNoteCommand = new Command(AddNote);
            RemoveNoteCommand = new Command(RemoveNote);
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

        public string EnteredNote {
            get => enteredNote;
            set {
                if (enteredNote != value) {
                    enteredNote = value;
                    OnPropertyChanged("EnteredNote");
                }
            }
        }

        private void AddSymbol(string symbol) {
            EnteredSum += symbol;
        }

        private void RemoveSymbol() {
            if (string.IsNullOrEmpty(EnteredSum))
                return;

            EnteredSum = EnteredSum.Remove(EnteredSum.Length - 1);
        }

        private void ApplySum() {
            if (string.IsNullOrEmpty(EnteredSum))
                return;

            RecordAdded();
        }

        private void RecordAdded() {
            decimal tempSum = decimal.Parse(EnteredSum) * (IsPositive ? 1 : -1);

            FinRecord record = new FinRecord {
                Sum = tempSum,
                Note = EnteredNote,
                Date = DateTime.Now,
            };

            MainViewModel.MonthRecords.Add(record);
            App.DataBase.SaveRecord(record);

            MainViewModel.TotalSum += tempSum;

            EnteredSum = string.Empty;
            EnteredNote = string.Empty;
        }

        private async void AddNote() {
            string note = await Application.Current.MainPage.DisplayPromptAsync("Добавить категорию", "Введите название категории", "Ок", "Отмена");

            if (!string.IsNullOrEmpty(note) && !MainViewModel.NoteList.Contains(note))
                MainViewModel.NoteList.Add(note.ToLower());
        }

        private async void RemoveNote() {
            string note = await Application.Current.MainPage.DisplayActionSheet("Выбрать категорию для удаления", "Отмена", null, MainViewModel.NoteList.ToArray());

            if (!string.IsNullOrEmpty(note))
                MainViewModel.NoteList.Remove(note);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
