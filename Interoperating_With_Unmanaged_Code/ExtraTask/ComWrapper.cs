using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ExtraTask
{
    [ComVisible(true)]
    [Guid("F13B0A43-8CA8-4289-A8B5-DDA1FF693B82")]
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class ComWrapper : IComWrapper
    {
        private readonly IDictionary<string, IntPtr> _libraries = new Dictionary<string, IntPtr>(StringComparer.OrdinalIgnoreCase);
        private bool _isDisposed;

        public Wrapper WrapProcedure(string library, string procedure, Type delegateType)
        {
            ThrowExceptionIfDisposed();
            IntPtr dllHandle = GetLibraryHandle(library);
            IntPtr procHandle = Win32Api.GetProcAddress(dllHandle, procedure);
            if (procHandle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            Delegate @delegate = Marshal.GetDelegateForFunctionPointer(procHandle, delegateType);
            return new Wrapper(library, procedure, @delegate);
        }

        private IntPtr GetLibraryHandle(string library)
        {
            if (_libraries.ContainsKey(library))
            {
                return _libraries[library];
            }

            IntPtr handle = Win32Api.LoadLibrary(library);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            _libraries.Add(library, handle);
            return handle;
        }

        private void ReleaseUnmanagedResources()
        {
            foreach (var library in _libraries)
            {
                Win32Api.FreeLibrary(library.Value);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
            _isDisposed = true;
        }

        ~ComWrapper()
        {
            ReleaseUnmanagedResources();
        }

        private void ThrowExceptionIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ComWrapper));
            }
        }
    }
}
