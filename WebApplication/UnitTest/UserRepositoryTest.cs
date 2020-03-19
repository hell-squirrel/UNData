using System;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repository;
using Repository.Implementation;

namespace UnitTest
{
    public class Tests
    {
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;
            var context = new Context(options);
            context.Users.Add(new User {Username = "user"});
            context.SaveChanges();
            _userRepository = new UserRepository(context);
        }

        [Test]
        public void UserCreateFailTest()
        {
            Assert.Throws<ApplicationException>(delegate { _userRepository.Create(new User {Username = "user"}); });
        }
        
        [Test]
        public void UserCreateSuccessTest()
        {
            Assert.DoesNotThrow(delegate { _userRepository.Create(new User {Username = "newuser"}); });
        }
    }
}