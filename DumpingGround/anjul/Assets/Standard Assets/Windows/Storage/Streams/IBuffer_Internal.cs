using RootSystem = System;
using System.Linq;
using System.Collections.Generic;
namespace Windows.Storage.Streams
{
    //
    // Windows.Storage.Streams.IBuffer
    //
    internal sealed partial class IBuffer_Internal : IBuffer, Helper.INativeWrapper
    {
        internal RootSystem.IntPtr _pNative;
        RootSystem.IntPtr Helper.INativeWrapper.nativePtr { get { return _pNative; } }

        // Constructors and Finalizers
        internal IBuffer_Internal(RootSystem.IntPtr pNative)
        {
            _pNative = pNative;
            Windows_Storage_Streams_IBuffer_AddRefObject(ref _pNative);
        }

        ~IBuffer_Internal()
        {
            Dispose(false);
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Storage_Streams_IBuffer_ReleaseObject(ref RootSystem.IntPtr pNative);
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Storage_Streams_IBuffer_AddRefObject(ref RootSystem.IntPtr pNative);
        private void Dispose(bool disposing)
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                return;
            }

            Helper.NativeObjectCache.RemoveObject<IBuffer_Internal>(_pNative);
                Windows_Storage_Streams_IBuffer_ReleaseObject(ref _pNative);

            _pNative = RootSystem.IntPtr.Zero;
        }


        // Public Properties
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern uint Windows_Storage_Streams_IBuffer_get_Capacity(RootSystem.IntPtr pNative);
        public  uint Capacity
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("IBuffer");
                }

                return Windows_Storage_Streams_IBuffer_get_Capacity(_pNative);
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern uint Windows_Storage_Streams_IBuffer_get_Length(RootSystem.IntPtr pNative);
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Storage_Streams_IBuffer_put_Length(RootSystem.IntPtr pNative, uint value);
        public  uint Length
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("IBuffer");
                }

                return Windows_Storage_Streams_IBuffer_get_Length(_pNative);
            }
            set
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("IBuffer");
                }

                Windows_Storage_Streams_IBuffer_put_Length(_pNative, value);
            }
        }

    }

}
