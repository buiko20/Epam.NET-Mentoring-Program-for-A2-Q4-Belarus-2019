namespace Task4
{
    internal class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: '{Name}', {nameof(Surname)}: '{Surname}', {nameof(Age)}: '{Age}'";
        }
    }
}
