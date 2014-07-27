using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using RootSystem = System;

namespace Windows.Storage.Streams
{
    internal sealed partial class IBuffer_Internal : RootSystem.IDisposable, IBufferNativeWrapper
    {
        // Constructors and Finalizers
        public void Dispose()
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                throw new RootSystem.ObjectDisposedException("Buffer");
            }

            Dispose(true);
            RootSystem.GC.SuppressFinalize(this);
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Windows_Storage_Streams_IBuffer_get_UnderlyingBuffer(RootSystem.IntPtr pNative);
        public IntPtr UnderlyingBuffer
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("Buffer");
                }

                return Windows_Storage_Streams_IBuffer_get_UnderlyingBuffer(_pNative);
            }
        }
    }
}