using System;

namespace PostSharpCodeRewriting.Logic
{
    public interface IService
    {
        Guid CreateRandom();

        BusinessEntity Get(Guid id);
    }
}
