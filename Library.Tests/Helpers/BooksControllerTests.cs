using Library.MVC.Controllers;
using Library.MVC.ViewModels;
using Library.domain.Models;
using Library.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Library.Tests
{
    public class BooksControllerTests
    {
        [Fact]
        public async Task Create_AddsBook()
        {
            using var context = TestDbContextFactory.Create();
            var controller = new BooksController(context);

            var book = new Book
            {
                Title = "Clean Code",
                Author = "Robert Martin",
                Isbn = "999",
                Category = "Tech",
                IsAvailable = true
            };

            await controller.Create(book);

            Assert.Equal(1, context.Books.Count());
        }

        [Fact]
        public async Task Book_Search_Returns_Expected_Matches()
        {
            using var context = TestDbContextFactory.Create();

            context.Books.AddRange(
                new Book
                {
                    Title = "Clean Code",
                    Author = "Robert Martin",
                    Isbn = "111",
                    Category = "Programming",
                    IsAvailable = true
                },
                new Book
                {
                    Title = "Deep Learning",
                    Author = "Ian Goodfellow",
                    Isbn = "112",
                    Category = "AI",
                    IsAvailable = true
                }
            );

            await context.SaveChangesAsync();

            var controller = new BooksController(context);

            var result = await controller.Index("Clean", null, null);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BookFilterViewModel>(viewResult.Model);

            Assert.Single(model.Books);
            Assert.Equal("Clean Code", model.Books.First().Title);
        }
    }
}
