using Something.Domain;
using Something.Persistence;
using System;

namespace Something.Application
{
    public class SomethingElseCreateInteractor : ISomethingElseCreateInteractor
    {
        private readonly ISomethingFactory somethingFactory;
        private ISomethingElseFactory factory;
        private ISomethingElsePersistence persistence;

        public SomethingElseCreateInteractor(ISomethingFactory somethingFactory, ISomethingElseFactory factory, ISomethingElsePersistence persistence)
        {
            this.somethingFactory = somethingFactory;
            this.factory = factory;
            this.persistence = persistence;
        }

        public void CreateSomethingElse(string name)
        {
            var somethingElse = factory.Create(name);
            persistence.SaveSomethingElse(somethingElse);
        }

        public void CreateSomethingElse(string name, string[] othernames)
        {
            var somethingElse = factory.Create(name);
            foreach (var nm in othernames)
            {
                var something = somethingFactory.Create(nm);
                somethingElse.Somethings.Add(something);
            }
            persistence.SaveSomethingElse(somethingElse);
        }
    }
}
