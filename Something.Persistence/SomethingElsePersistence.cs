using Something.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Something.Persistence
{
    public class SomethingElsePersistence : ISomethingElsePersistence
    {
        private AppDbContext ctx;

        public SomethingElsePersistence(AppDbContext ctx)
        {
            this.ctx = ctx;
        }

        public void SaveSomethingElse(SomethingElse somethingElse)
        {
            ctx.SomethingElses.Add(somethingElse);
            ctx.SaveChanges();
        }

        public List<Domain.Models.SomethingElse> GetSomethingElseList()
        {
            return ctx.SomethingElses.ToList();
        }
    }
}
