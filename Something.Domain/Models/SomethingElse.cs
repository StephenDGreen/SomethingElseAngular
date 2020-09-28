namespace Something.Domain.Models
{
    public class SomethingElse
    {
        private SomethingElse(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public static SomethingElse CreateNamedSomethingElse(string name)
        {
            if (name == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "name");
            }
            return new SomethingElse(name);
        }
    }
}
