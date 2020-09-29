using Something.Domain.Models;
using System.Collections.Generic;

namespace Something.Persistence
{
    public interface ISomethingElsePersistence
    {
        List<SomethingElse> GetSomethingElseIncludingSomethingList();
        List<SomethingElse> GetSomethingElseList();
        void SaveSomethingElse(SomethingElse somethingElse);
    }
}