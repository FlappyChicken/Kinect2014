using RootSystem = System;
using System.Linq;
using System.Collections.Generic;
namespace Windows.Kinect
{
    //
    // Windows.Kinect.BodyIndexFrame
    //
    public sealed partial class BodyIndexFrame : RootSystem.IDisposable, Helper.INativeWrapper

    {
        internal RootSystem.IntPtr _pNative;
        RootSystem.IntPtr Helper.INativeWrapper.nativePtr { get { return _pNative; } }

        // Constructors and Finalizers
        internal BodyIndexFrame(RootSystem.IntPtr pNative)
        {
            _pNative = pNative;
            Windows_Kinect_BodyIndexFrame_AddRefObject(ref _pNative);
        }

        ~BodyIndexFrame()
        {
            Dispose(false);
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Kinect_BodyIndexFrame_ReleaseObject(ref RootSystem.IntPtr pNative);
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Kinect_BodyIndexFrame_AddRefObject(ref RootSystem.IntPtr pNative);
        private void Dispose(bool disposing)
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                return;
            }

            Helper.NativeObjectCache.RemoveObject<BodyIndexFrame>(_pNative);

            if (disposing)
            {
                Windows_Kinect_BodyIndexFrame_Dispose(_pNative);
            }
                Windows_Kinect_BodyIndexFrame_ReleaseObject(ref _pNative);

            _pNative = RootSystem.IntPtr.Zero;
        }


        // Public Properties
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Windows_Kinect_BodyIndexFrame_get_BodyIndexFrameSource(RootSystem.IntPtr pNative);
        public  Windows.Kinect.BodyIndexFrameSource BodyIndexFrameSource
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("BodyIndexFrame");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_BodyIndexFrame_get_BodyIndexFrameSource(_pNative);
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.BodyIndexFrameSource>(objectPointer, n => new Windows.Kinect.BodyIndexFrameSource(n));
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Windows_Kinect_BodyIndexFrame_get_FrameDescription(RootSystem.IntPtr pNative);
        public  Windows.Kinect.FrameDescription FrameDescription
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("BodyIndexFrame");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_BodyIndexFrame_get_FrameDescription(_pNative);
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.FrameDescription>(objectPointer, n => new Windows.Kinect.FrameDescription(n));
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern long Windows_Kinect_BodyIndexFrame_get_RelativeTime(RootSystem.IntPtr pNative);
        public  RootSystem.TimeSpan RelativeTime
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("BodyIndexFrame");
                }

                return RootSystem.TimeSpan.FromMilliseconds(Windows_Kinect_BodyIndexFrame_get_RelativeTime(_pNative));
            }
        }


        // Public Methods
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray(RootSystem.IntPtr pNative, RootSystem.IntPtr frameData, int frameDataSize);
        public void CopyFrameDataToArray(byte[] frameData)
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                throw new RootSystem.ObjectDisposedException("BodyIndexFrame");
            }

            var frameDataSmartGCHandle = new Helper.SmartGCHandle(RootSystem.Runtime.InteropServices.GCHandle.Alloc(frameData, RootSystem.Runtime.InteropServices.GCHandleType.Pinned));
            var _frameData = frameDataSmartGCHandle.AddrOfPinnedObject();
            Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray(_pNative, _frameData, frameData.Length);
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Windows_Kinect_BodyIndexFrame_LockImageBuffer(RootSystem.IntPtr pNative);
        public Windows.Storage.Streams.IBuffer LockImageBuffer()
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                throw new RootSystem.ObjectDisposedException("BodyIndexFrame");
            }

            RootSystem.IntPtr objectPointer = Windows_Kinect_BodyIndexFrame_LockImageBuffer(_pNative);
            if (objectPointer == RootSystem.IntPtr.Zero)
            {
                return null;
            }

            return Helper.NativeObjectCache.CreateOrGetObject<Windows.Storage.Streams.IBuffer_Internal>(objectPointer, n => new Windows.Storage.Streams.IBuffer_Internal(n));
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Kinect_BodyIndexFrame_Dispose(RootSystem.IntPtr pNative);
        public void Dispose()
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                return;
            }

            Dispose(true);
            RootSystem.GC.SuppressFinalize(this);
        }

    }

}
