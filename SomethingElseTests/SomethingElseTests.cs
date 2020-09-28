using Something.Domain;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Domain = Something.Domain.Models;

namespace SomethingElseTests
{
    [ExcludeFromCodeCoverage]
    public class SomethingElseTests
    {
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
    }
}
