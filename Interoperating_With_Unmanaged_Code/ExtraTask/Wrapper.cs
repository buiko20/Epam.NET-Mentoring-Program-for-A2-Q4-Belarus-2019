using System;

namespace ExtraTask
{
    public class Wrapper
    {
        public Wrapper(string library, string procedure, Delegate @delegate)
        {
            Library = library;
            Procedure = procedure;
            Delegate = @delegate;
        }

        public string Library { get; }

        public string Procedure { get; }

        public Delegate Delegate { get; }
    }
}
