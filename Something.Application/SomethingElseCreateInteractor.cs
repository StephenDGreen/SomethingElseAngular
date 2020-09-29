using Something.Domain;
using Something.Persistence;

namespace Something.Application
{
    public class SomethingElseCreateInteractor : ISomethingElseCreateInteractor
    {
        private ISomethingElseFactory factory;
        private ISomethingElsePersistence persistence;

        public SomethingElseCreateInteractor(ISomethingElseFactory factory, ISomethingElsePersistence persistence)
        {
            this.factory = factory;
            this.persistence = persistence;
        }

        public void CreateSomethingElse(string name)
        {
            var somethingElse = factory.Create(name);
            persistence.SaveSomethingElse(somethingElse);
        }
    }
}
