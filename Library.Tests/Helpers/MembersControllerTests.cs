using Library.MVC.Controllers;
using Library.domain.Models;
using Library.Tests.Helpers;
using Xunit;

namespace Library.Tests
{
    public class MembersControllerTests
    {
        [Fact]
        public async Task Create_AddsMember()
        {
            using var context = TestDbContextFactory.Create();
            var controller = new MembersController(context);

            var member = new Member
            {
                FullName = "Nadia",
                Email = "test@test.com",
                Phone = "123456789"
            };

            await controller.Create(member);

            Assert.Equal(1, context.Members.Count());
        }
    }
}