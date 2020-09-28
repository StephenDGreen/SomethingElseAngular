using Something.Domain.Models;
using System;

namespace Something.Domain
{
    public class SomethingElseFactory 
    {
        public SomethingElse Create(string name)
        {
            return SomethingElse.CreateNamedSomethingElse(name);
        }
    }
}