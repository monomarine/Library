using Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Library.ViewModels
{
#pragma warning disable
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _author;
        private int yearOfPublication;
        private Book _selectedBook;

        public ObservableCollection<Book> Books { get; } = new();   
        public MainViewModel()
        {
            AddCommand = new RelayCommand(AddBook);
            DeleteCommand = new RelayCommand(DeleteBook);
        }

        #region properties
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }
        public int YearOfPublication
        {
            get => yearOfPublication;
            set
            {
                yearOfPublication = value;
                OnPropertyChanged(nameof(YearOfPublication));
            }
        }
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
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
        }
        private void DeleteBook(object parameter)
        {
            if (SelectedBook != null)
                Books.Remove(SelectedBook);
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
