using System;
using System.Runtime.InteropServices;

namespace ExtraTask
{
    [ComVisible(true)]
    [Guid("DE457EAC-3304-4B6E-997E-F079749A288B")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IComWrapper : IDisposable
    {
        Wrapper WrapProcedure(string library, string procedure, Type delegateType);
    }
}
