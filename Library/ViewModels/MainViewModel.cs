using Library.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Library.ViewModels
{
#pragma warning disable
    internal class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string _title;
        private string _author;
        private int yearOfPublication;
        private Book _selectedBook;

        public ObservableCollection<Book> Books { get; } = new();  
        private readonly Dictionary<string, List<string>> _errors = new();
        public MainViewModel()
        {
            AddCommand = new RelayCommand(AddBook, CanAddBook);
            DeleteCommand = new RelayCommand(DeleteBook, CanDeleteBook);
            ValidateProperty(nameof(Title), "");
            ValidateProperty(nameof(Author), "");
            ValidateProperty(nameof(YearOfPublication), 0);
        }

        #region properties
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
                ValidateProperty(nameof(Title), value); 
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
                ValidateProperty(nameof(Author), value);
                AddCommand.RaiseCanExecuteChanged();
            }
        }
        public int YearOfPublication
        {
            get => yearOfPublication;
            set
            {
                yearOfPublication = value;
                OnPropertyChanged(nameof(YearOfPublication));
                ValidateProperty(nameof(YearOfPublication), value); 
                AddCommand.RaiseCanExecuteChanged();
            }
        }
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region ICommand
        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }

        private void AddBook(object parameter)
        {
            Books.Add(new Book
            {
                Title = Title,
                Author = Author,
                YearOfPublication = yearOfPublication,
            }
            );
            Title = Author = "";
            YearOfPublication = 1900;
        }
        private bool CanAddBook(object parameter) => !HasErrors;
        private void DeleteBook(object parameter)
        {
            if (SelectedBook != null)
                Books.Remove(SelectedBook);
        }
        private bool CanDeleteBook(object parameter) => SelectedBook is Book;

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region INotifyDataErrorInfo
        public bool HasErrors => _errors.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private void OnErrorsChanged(string propertyName) =>
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        public IEnumerable GetErrors(string? propertyName)=>
            _errors.TryGetValue(propertyName, out var errors) ? errors : null;

        private void ValidateProperty<T>(string propertyName, T value)
        {
            _errors.Remove(propertyName);

            var context = new ValidationContext(new Book())
                            { MemberName =  propertyName };
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(value, context, results);
            if (results.Count > 0)
                _errors[propertyName] = results
                    .Select(x => x.ErrorMessage)
                    .ToList();

            OnErrorsChanged(propertyName);
        }

        #endregion
    }
}
