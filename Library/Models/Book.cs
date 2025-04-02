using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    internal class Book
    {

        [Required(ErrorMessage = "название книги обязательно!")]
        [MinLength(5, ErrorMessage ="название - минимум 5 символов")]
        public string Title { get; set; }
        [Required(ErrorMessage ="автор обязателен")]
        public string Author { get; set; }
        [Range(1900, 2025, ErrorMessage ="Год издания в пределах 1900 - 2025")]
        public int YearOfPublication { get; set; }
    }
}
