using System;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repository;
using Repository.Implementation;

namespace UnitTest
{
    public class AnalyticsRepositoryTest
    {
        private AnalyticsRepository _analyticsRepository;
        private Context _context;
        
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;
            _context = new Context(options);
            _analyticsRepository = new AnalyticsRepository(_context);
        }

        [Test]
        public void LocationAddFail1Test()
        {
            _context.Locations.Add(new Location(){Name = "location", LocationId = 1});
            _context.SaveChanges();
            Assert.Throws<ApplicationException>(delegate
            {
                _analyticsRepository.SaveLocation(4,"location");
            });
        }
        
        [Test]
        public void LocationAddFail2Test()
        {
            Assert.Throws<ApplicationException>(delegate
            {
                _analyticsRepository.SaveLocation(2,null);
            });
        }
        
        [Test]
        public void LocationAddFail3Test()
        {
            Assert.Throws<ApplicationException>(delegate
            {
                _analyticsRepository.SaveLocation(2,"");
            });
        }
        
        [Test]
        public void LocationAddFail4Test()
        {
            _context.Locations.Add(new Location(){Name = "location1", LocationId = 2});
            _context.SaveChanges();
            Assert.Throws<ApplicationException>(delegate
            {
                _analyticsRepository.SaveLocation(2,"location1");
            });
        }
        [Test]
        public void LocationAddTest()
        {
            Assert.DoesNotThrow(delegate
            {
                _analyticsRepository.SaveLocation(3,"location3");
            });
        }
    }
}