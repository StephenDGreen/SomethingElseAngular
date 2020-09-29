using Something.Domain.Models;
using System.Collections.Generic;

namespace Something.Application
{
    public interface ISomethingElseReadInteractor
    {
        List<SomethingElse> GetSomethingElseList();
    }
}