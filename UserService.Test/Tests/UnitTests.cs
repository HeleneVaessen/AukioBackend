using NUnit.Framework;
using System;

namespace UserService.Test
{
    [TestFixture]
    public class UnitTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAddNewUser()
        {
            MockDAL mockDAL = new MockDAL();
            Models.User user1 = new Models.User { Name = "Helene", Email = "mail@mail.com", School = "Fontys" };
            Services.IUserService userService = new Services.UserService(mockDAL);
            Assert.IsFalse(userService.GetAllUsers().Contains(user1));
            Assert.IsTrue(userService.CheckEmail("mail@mail.com"));
            userService.AddUser(user1);
            Assert.IsTrue(userService.GetAllUsers().Contains(user1));
            Assert.IsFalse(userService.CheckEmail("mail@mail.com"));

            Models.User user2 = new Models.User { Name = "NietHelene", Email = "m@mail.com", School = "Fontys" };
            userService.AddUser(user2);
            Assert.IsFalse(userService.CheckEmail("m@mail.com"));
            Assert.IsTrue(userService.GetAllUsers().Contains(user2));
        }
    }
}