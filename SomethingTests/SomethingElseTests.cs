using Microsoft.EntityFrameworkCore;
using Moq;
using Something.Application;
using Something.Domain;
using Something.Persistence;
using SomethingTests.Infrastructure.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Domain = Something.Domain.Models;

namespace SomethingTests
{
    public class SomethingElseTests
    {
        private readonly Domain.SomethingElse somethingElse = Domain.SomethingElse.CreateNamedSomethingElse("Fred Bloggs") ;
        private readonly Domain.Something something = new Domain.Something() { Name = "Alice Bloggs" };

        public SomethingElseTests()
        {
            somethingElse.Somethings.Add(something);
        }

        [Fact]
        public void SomethingElse_HasAName()
        {
            var expected = "Fred Bloggs";
            var something1 = Domain.SomethingElse.CreateNamedSomethingElse(expected);
            
            string actual = something1.Name;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SomethingElseFactory_Create_CreatesSomethingElseWithName()
        {
            SomethingElseFactory factory = new SomethingElseFactory();
            string expected = "Fred Bloggs";

            Domain.SomethingElse actual = factory.Create(expected);

            Assert.IsType<Domain.SomethingElse>(actual);
            Assert.Equal(expected, actual.Name);
        }
        [Fact]
        public void SomethingElse_CreateNamedSomethingElse_ThrowsArgumentExceptionWithoutName()
        {
            string name = null;

            var exception = Assert.Throws<ArgumentException>(() => Domain.SomethingElse.CreateNamedSomethingElse(name));
            Assert.Equal("name", exception.ParamName);
        }
        [Fact]
        public void SomethingElseFactory_Create_ThrowsArgumentExceptionWithoutName()
        {
            SomethingElseFactory factory = new SomethingElseFactory();
            string name = null;

            var exception = Assert.Throws<ArgumentException>(() => factory.Create(name));
            Assert.Equal("name", exception.ParamName);
        }
        [Fact]
        public void DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesIt()
        {

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesIt)))
            {
                ctx.SomethingElses.Add(somethingElse);
                ctx.SaveChanges();
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesIt)))
            {
                var savedSomethingElse = ctx.SomethingElses.Single();
                Assert.Equal(somethingElse.Name, savedSomethingElse.Name);
            };
        }

        [Fact]
        public void SomethingElsePersistence__SaveSomethingElse__SavesSomethingElseToDatabase()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__SaveSomethingElse__SavesSomethingElseToDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__SaveSomethingElse__SavesSomethingElseToDatabase)))
            {
                var savedSomethingElse = ctx.SomethingElses.Single();
                Assert.Equal(somethingElse.Name, savedSomethingElse.Name);
            };
        }

        [Fact]
        public void SomethingElsePersistence__GetSomethingElseList__RetrievesSomethingElseListFromDatabase()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__GetSomethingElseList__RetrievesSomethingElseListFromDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__GetSomethingElseList__RetrievesSomethingElseListFromDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                var savedSomethingElse = persistence.GetSomethingElseList();
                Assert.Equal(somethingElse.Name, savedSomethingElse.Single().Name);
            };
        }

        [Fact]
        public void SomethingElseCreateInteractor_CreateSomethingElse_PersistsSomethingElseWithName()
        {
            Mock<ISomethingElseFactory> mockSomethingElseFactory = new Mock<ISomethingElseFactory>();
            mockSomethingElseFactory.Setup(x => x.Create(somethingElse.Name)).Returns(somethingElse);
            Mock<ISomethingElsePersistence> mockPersistence = new Mock<ISomethingElsePersistence>();
            SomethingElseCreateInteractor somethingElseInteractor = new SomethingElseCreateInteractor(mockSomethingElseFactory.Object, mockPersistence.Object);

            somethingElseInteractor.CreateSomethingElse(somethingElse.Name);

            mockPersistence.Verify(x => x.SaveSomethingElse(somethingElse));
        }

        [Fact]
        public void SomethingElseReadInteractor_ReadSomethingElseList_RetrievesSomethingElseListFromPersistence()
        {
            var somethingElseList = new List<Domain.SomethingElse>();
            somethingElseList.Add(somethingElse);
            var mockPersistence = new Mock<ISomethingElsePersistence>();
            mockPersistence.Setup(x => x.GetSomethingElseList()).Returns(somethingElseList);
            SomethingElseReadInteractor interactor = new SomethingElseReadInteractor(mockPersistence.Object);

            List<Domain.SomethingElse> somethingElseList1 = interactor.GetSomethingElseList();

            Assert.Equal(somethingElseList.Count, somethingElseList1.Count);
            Assert.Equal(somethingElseList[somethingElseList.Count - 1].Name, somethingElseList1[somethingElseList1.Count - 1].Name);
        }
        [Fact]
        public void SomethingElse_HasAListOfSomethings()
        {
            var name = "Fred Bloggs";
            var somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            int expected = 0;

            int actual = somethingElse1.Somethings.Count;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SomethingElse_AddSomething_AddsSomethingToSomethings()
        {
            var name = "Fred Bloggs";
            var somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            int expected = 1;

            somethingElse1.Somethings.Add(something);
            int actual = somethingElse1.Somethings.Count;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void DbContextFactory_CreateAppDbContext_SavesSomethingElseWithSomethingToDatabaseAndRetrievesIt()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseWithSomethingToDatabaseAndRetrievesIt)))
            {
                ctx.SomethingElses.Add(somethingElse);
                ctx.SaveChanges();
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseWithSomethingToDatabaseAndRetrievesIt)))
            {
                var savedSomethingElse = ctx.SomethingElses.Include(s => s.Somethings).Single();
                Assert.Equal(somethingElse.Somethings[0].Name, savedSomethingElse.Somethings[0].Name);
            };
        }
    }
}
