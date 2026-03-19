using Library.MVC.Controllers;
using Library.domain.Models;
using Library.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Library.Tests
{
    public class LoansControllerTests
    {
        [Fact]
        public async Task Create_Post_AddsLoanToDatabase()
        {
            using var context = TestDbContextFactory.Create();

            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                Isbn = "123",
                Category = "Fiction",
                IsAvailable = true
            };

            var member = new Member
            {
                FullName = "Alice Johnson",
                Email = "alice@test.com",
                Phone = "1111"
            };

            context.Books.Add(book);
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var controller = new LoansController(context);

            var loan = new Loan
            {
                BookId = book.Id,
                MemberId = member.Id,
                DueDate = DateTime.Now.AddDays(7)
            };

            var result = await controller.Create(loan);

            Assert.Equal(1, context.Loans.Count());

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Create_Loan_MakesBookUnavailable()
        {
            using var context = TestDbContextFactory.Create();

            var book = new Book
            {
                Title = "Design Patterns",
                Author = "Gamma",
                Isbn = "456",
                Category = "IT",
                IsAvailable = true
            };

            var member = new Member
            {
                FullName = "Bob Smith",
                Email = "bob@test.com",
                Phone = "2222"
            };

            context.Books.Add(book);
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var controller = new LoansController(context);

            var loan = new Loan
            {
                BookId = book.Id,
                MemberId = member.Id,
                DueDate = DateTime.Now.AddDays(7)
            };

            await controller.Create(loan);

            var updatedBook = context.Books.First(b => b.Id == book.Id);

            Assert.False(updatedBook.IsAvailable);
        }

        [Fact]
        public async Task Return_Loan_MakesBookAvailableAgain()
        {
            using var context = TestDbContextFactory.Create();

            var book = new Book
            {
                Title = "Clean Code",
                Author = "Robert Martin",
                Isbn = "789",
                Category = "Programming",
                IsAvailable = false
            };

            var member = new Member
            {
                FullName = "Charlie Brown",
                Email = "charlie@test.com",
                Phone = "3333"
            };

            context.Books.Add(book);
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var loan = new Loan
            {
                BookId = book.Id,
                MemberId = member.Id,
                LoanDate = DateTime.Now.AddDays(-2),
                DueDate = DateTime.Now.AddDays(5),
                ReturnedDate = null
            };

            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            var controller = new LoansController(context);

            var result = await controller.Return(loan.Id);

            var updatedLoan = context.Loans.First(l => l.Id == loan.Id);
            var updatedBook = context.Books.First(b => b.Id == book.Id);

            Assert.NotNull(updatedLoan.ReturnedDate);
            Assert.True(updatedBook.IsAvailable);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void Overdue_Logic_Is_True_When_DueDate_Passed_And_Not_Returned()
        {
            var loan = new Loan
            {
                LoanDate = DateTime.Now.AddDays(-10),
                DueDate = DateTime.Now.AddDays(-2),
                ReturnedDate = null
            };

            var isOverdue = loan.DueDate < DateTime.Now && loan.ReturnedDate == null;

            Assert.True(isOverdue);
        }
    }
}