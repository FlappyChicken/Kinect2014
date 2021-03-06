using RootSystem = System;
using System.Linq;
using System.Collections.Generic;
namespace Windows.Kinect
{
    //
    // Windows.Kinect.ColorFrameSource
    //
    public sealed partial class ColorFrameSource : Helper.INativeWrapper

    {
        internal RootSystem.IntPtr _pNative;
        RootSystem.IntPtr Helper.INativeWrapper.nativePtr { get { return _pNative; } }

        // Constructors and Finalizers
        internal ColorFrameSource(RootSystem.IntPtr pNative)
        {
            _pNative = pNative;
            Windows_Kinect_ColorFrameSource_AddRefObject(ref _pNative);
        }

        ~ColorFrameSource()
        {
            Dispose(false);
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Kinect_ColorFrameSource_ReleaseObject(ref RootSystem.IntPtr pNative);
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Kinect_ColorFrameSource_AddRefObject(ref RootSystem.IntPtr pNative);
        private void Dispose(bool disposing)
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                return;
            }

            Helper.NativeObjectCache.RemoveObject<ColorFrameSource>(_pNative);
                Windows_Kinect_ColorFrameSource_ReleaseObject(ref _pNative);

            _pNative = RootSystem.IntPtr.Zero;
        }


        // Public Properties
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Windows_Kinect_ColorFrameSource_get_FrameDescription(RootSystem.IntPtr pNative);
        public  Windows.Kinect.FrameDescription FrameDescription
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("ColorFrameSource");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_ColorFrameSource_get_FrameDescription(_pNative);
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.FrameDescription>(objectPointer, n => new Windows.Kinect.FrameDescription(n));
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern bool Windows_Kinect_ColorFrameSource_get_IsActive(RootSystem.IntPtr pNative);
        public  bool IsActive
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("ColorFrameSource");
                }

                return Windows_Kinect_ColorFrameSource_get_IsActive(_pNative);
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Windows_Kinect_ColorFrameSource_get_KinectSensor(RootSystem.IntPtr pNative);
        public  Windows.Kinect.KinectSensor KinectSensor
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("ColorFrameSource");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_ColorFrameSource_get_KinectSensor(_pNative);
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.KinectSensor>(objectPointer, n => new Windows.Kinect.KinectSensor(n));
            }
        }


        // Events
        private static RootSystem.Runtime.InteropServices.GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;
        [RootSystem.Runtime.InteropServices.UnmanagedFunctionPointer(RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(RootSystem.IntPtr args, RootSystem.IntPtr pNative);
        private static Helper.CollectionMap<RootSystem.IntPtr, List<RootSystem.EventHandler<Windows.Kinect.FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new Helper.CollectionMap<RootSystem.IntPtr, List<RootSystem.EventHandler<Windows.Kinect.FrameCapturedEventArgs>>>();
        [AOT.MonoPInvokeCallbackAttribute(typeof(_Windows_Kinect_FrameCapturedEventArgs_Delegate))]
        private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(RootSystem.IntPtr result, RootSystem.IntPtr pNative)
        {
            List<RootSystem.EventHandler<Windows.Kinect.FrameCapturedEventArgs>> callbackList = null;
            Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out callbackList);
            lock(callbackList)
            {
                var objThis = Helper.NativeObjectCache.GetObject<ColorFrameSource>(pNative);
                var args = new Windows.Kinect.FrameCapturedEventArgs(result);
                foreach(var func in callbackList)
                {
#if UNITY_METRO || UNITY_XBOXONE
                    UnityEngine.WSA.Application.InvokeOnAppThread(() => { try { func(objThis, args); } catch { } }, true);
#else
                    Helper.EventPump.Instance.Enqueue(() => { try { func(objThis, args); } catch { } });
#endif
                }
            }
        }
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Kinect_ColorFrameSource_add_FrameCaptured(RootSystem.IntPtr pNative, _Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);
        public  event RootSystem.EventHandler<Windows.Kinect.FrameCapturedEventArgs> FrameCaptured
        {
            add
            {
#if !UNITY_METRO && !UNITY_XBOXONE
                Helper.EventPump.EnsureInitialized();
#endif

                Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    callbackList.Add(value);
                    if(callbackList.Count == 1)
                    {
                        var del = new _Windows_Kinect_FrameCapturedEventArgs_Delegate(Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
                        _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = RootSystem.Runtime.InteropServices.GCHandle.Alloc(del);
                        Windows_Kinect_ColorFrameSource_add_FrameCaptured(_pNative, del, false);
                    }
                }
            }
            remove
            {
                Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    callbackList.Remove(value);
                    if(callbackList.Count == 0)
                    {
                        Windows_Kinect_ColorFrameSource_add_FrameCaptured(_pNative, Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler, true);
                        _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
                    }
                }
            }
        }

        private static RootSystem.Runtime.InteropServices.GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;
        [RootSystem.Runtime.InteropServices.UnmanagedFunctionPointer(RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(RootSystem.IntPtr args, RootSystem.IntPtr pNative);
        private static Helper.CollectionMap<RootSystem.IntPtr, List<RootSystem.EventHandler<Windows.Data.PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new Helper.CollectionMap<RootSystem.IntPtr, List<RootSystem.EventHandler<Windows.Data.PropertyChangedEventArgs>>>();
        [AOT.MonoPInvokeCallbackAttribute(typeof(_Windows_Data_PropertyChangedEventArgs_Delegate))]
        private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(RootSystem.IntPtr result, RootSystem.IntPtr pNative)
        {
            List<RootSystem.EventHandler<Windows.Data.PropertyChangedEventArgs>> callbackList = null;
            Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out callbackList);
            lock(callbackList)
            {
                var objThis = Helper.NativeObjectCache.GetObject<ColorFrameSource>(pNative);
                var args = new Windows.Data.PropertyChangedEventArgs(result);
                foreach(var func in callbackList)
                {
#if UNITY_METRO || UNITY_XBOXONE
                    UnityEngine.WSA.Application.InvokeOnAppThread(() => { try { func(objThis, args); } catch { } }, true);
#else
                    Helper.EventPump.Instance.Enqueue(() => { try { func(objThis, args); } catch { } });
#endif
                }
            }
        }
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern void Windows_Kinect_ColorFrameSource_add_PropertyChanged(RootSystem.IntPtr pNative, _Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);
        public  event RootSystem.EventHandler<Windows.Data.PropertyChangedEventArgs> PropertyChanged
        {
            add
            {
#if !UNITY_METRO && !UNITY_XBOXONE
                Helper.EventPump.EnsureInitialized();
#endif

                Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    callbackList.Add(value);
                    if(callbackList.Count == 1)
                    {
                        var del = new _Windows_Data_PropertyChangedEventArgs_Delegate(Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
                        _Windows_Data_PropertyChangedEventArgs_Delegate_Handle = RootSystem.Runtime.InteropServices.GCHandle.Alloc(del);
                        Windows_Kinect_ColorFrameSource_add_PropertyChanged(_pNative, del, false);
                    }
                }
            }
            remove
            {
                Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    callbackList.Remove(value);
                    if(callbackList.Count == 0)
                    {
                        Windows_Kinect_ColorFrameSource_add_PropertyChanged(_pNative, Windows_Data_PropertyChangedEventArgs_Delegate_Handler, true);
                        _Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
                    }
                }
            }
        }


        // Public Methods
        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Windows_Kinect_ColorFrameSource_OpenReader(RootSystem.IntPtr pNative);
        public Windows.Kinect.ColorFrameReader OpenReader()
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                throw new RootSystem.ObjectDisposedException("ColorFrameSource");
            }

            RootSystem.IntPtr objectPointer = Windows_Kinect_ColorFrameSource_OpenReader(_pNative);
            if (objectPointer == RootSystem.IntPtr.Zero)
            {
                return null;
            }

            return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.ColorFrameReader>(objectPointer, n => new Windows.Kinect.ColorFrameReader(n));
        }

        [RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Windows_Kinect_ColorFrameSource_CreateFrameDescription(RootSystem.IntPtr pNative, Windows.Kinect.ColorImageFormat format);
        public Windows.Kinect.FrameDescription CreateFrameDescription(Windows.Kinect.ColorImageFormat format)
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                throw new RootSystem.ObjectDisposedException("ColorFrameSource");
            }

            RootSystem.IntPtr objectPointer = Windows_Kinect_ColorFrameSource_CreateFrameDescription(_pNative, format);
            if (objectPointer == RootSystem.IntPtr.Zero)
            {
                return null;
            }

            return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.FrameDescription>(objectPointer, n => new Windows.Kinect.FrameDescription(n));
        }

    }

}
