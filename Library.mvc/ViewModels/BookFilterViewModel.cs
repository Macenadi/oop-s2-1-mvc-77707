using System.Collections.Generic;
using Library.domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.MVC.ViewModels
{
    public class BookFilterViewModel
    {
        public string? SearchString { get; set; }
        public string? Category { get; set; }
        public string? Availability { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
        public SelectList? Categories { get; set; }
    }
}
