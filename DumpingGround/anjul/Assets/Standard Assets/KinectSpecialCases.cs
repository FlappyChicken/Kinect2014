using RootSystem = System;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Windows.Kinect
{
	[RootSystem.Runtime.InteropServices.StructLayout(RootSystem.Runtime.InteropServices.LayoutKind.Sequential)]
	public struct PointF
	{
		public float X { get; set; }
		public float Y { get; set; }
		
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			if (!(obj is PointF))
			{
				return false;
			}
			
			return this.Equals((ColorSpacePoint)obj);
		}
		
		public bool Equals(ColorSpacePoint obj)
		{
			return (X == obj.X) && (Y == obj.Y);
		}
		
		public static bool operator ==(PointF a, PointF b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(PointF a, PointF b)
		{
			return !(a.Equals(b));
		}
	}
	
	internal sealed partial class AudioStream : RootSystem.IO.Stream
	{
		internal RootSystem.IntPtr _pNative;
		
		// Constructors and Finalizers
		internal AudioStream(RootSystem.IntPtr pNative)
		{
			_pNative = pNative;
			Windows_Kinect_IStream_AddRefObject(ref _pNative);
		}
		
		~AudioStream()
		{
			Dispose(false);
		}
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_IStream_ReleaseObject(ref RootSystem.IntPtr pNative);
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_IStream_AddRefObject(ref RootSystem.IntPtr pNative);
		protected override void Dispose(bool disposing)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				return;
			}
			
			Helper.NativeObjectCache.RemoveObject<AudioStream>(_pNative);
			Windows_Kinect_IStream_ReleaseObject(ref _pNative);
			
			if (disposing)
			{
				Windows_Kinect_IStream_Dispose(_pNative);
			}
			
			_pNative = RootSystem.IntPtr.Zero;
		}
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_IStream_Dispose(RootSystem.IntPtr pNative);
		#if UNITY_METRO
		public void Close()
			#else
			public override void Close()
				#endif
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				return;
			}
			
			Dispose(true);
			RootSystem.GC.SuppressFinalize(this);
		}
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_IStream_Read(
			RootSystem.IntPtr pNative,
			byte[] pBuffer,
			int offset,
			int bufferLength,
			RootSystem.IntPtr pBytesRead);
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("AudioStream");
			}
			
			IntPtr readPtr = Marshal.AllocCoTaskMem(sizeof(int));
			
			Windows_Kinect_IStream_Read(_pNative, buffer, offset, count, readPtr);
			
			int read = Marshal.ReadInt32(readPtr);
			Marshal.FreeCoTaskMem(readPtr);
			
			return read;
		}
		
		public override bool CanRead
		{
			get { return true; }
		}
		
		public override bool CanSeek
		{
			get { return false; }
		}
		
		public override bool CanWrite
		{
			get { return false; }
		}
		
		public override void Flush()
		{
			throw new NotImplementedException();
		}
		
		public override long Length
		{
			get { throw new NotImplementedException(); }
		}
		
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		
		public override long Seek(long offset, RootSystem.IO.SeekOrigin origin)
		{
			throw new NotImplementedException();
		}
		
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}
		
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}
	}
	
	public sealed partial class AudioBeam
	{
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern RootSystem.IntPtr Windows_Kinect_AudioBeam_OpenInputStream(RootSystem.IntPtr pNative);
		public RootSystem.IO.Stream OpenInputStream()
		{
			RootSystem.IntPtr objectPointer = Windows_Kinect_AudioBeam_OpenInputStream(_pNative);
			if (objectPointer == RootSystem.IntPtr.Zero)
			{
				return null;
			}
			
			var obj = Helper.NativeObjectCache.GetObject<Windows.Kinect.AudioStream>(objectPointer);
			if (obj == null)
			{
				obj = new Windows.Kinect.AudioStream(objectPointer);
				Helper.NativeObjectCache.AddObject<Windows.Kinect.AudioStream>(objectPointer, obj);
			}
			
			return obj;
		}
	}
	
	public sealed partial class AudioBeamSubFrame
	{
		[RootSystem.Runtime.InteropServices.DllImport(
			"XboxOneUnityAddin",
			EntryPoint = "Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToArray",
			CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToIntPtr(RootSystem.IntPtr pNative, RootSystem.IntPtr frameData, uint frameDataSize);
		public void CopyFrameDataToIntPtr(RootSystem.IntPtr frameData, uint size)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("AudioBeamSubFrame");
			}
			
			Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToIntPtr(_pNative, frameData, size);
		}
	}
	
	public sealed partial class AudioBeamFrame
	{
		private Windows.Kinect.AudioBeamSubFrame[] _subFrames = null;
		
		private void Dispose(bool disposing)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				return;
			}
			
			if (_subFrames != null)
			{
				foreach (var subFrame in _subFrames)
				{
					subFrame.Dispose();
				}
				
				_subFrames = null;
			}
			
			Helper.NativeObjectCache.RemoveObject<AudioBeamFrame>(_pNative);
			Windows_Kinect_AudioBeamFrame_ReleaseObject(ref _pNative);
			
			if (disposing)
			{
				Windows_Kinect_AudioBeamFrame_Dispose(_pNative);
			}
			
			_pNative = RootSystem.IntPtr.Zero;
		}
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_AudioBeamFrame_Dispose(RootSystem.IntPtr pNative);
		public void Dispose()
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				return;
			}
			
			Dispose(true);
			RootSystem.GC.SuppressFinalize(this);
		}
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern int Windows_Kinect_AudioBeamFrame_get_SubFrames(RootSystem.IntPtr pNative, [RootSystem.Runtime.InteropServices.Out] RootSystem.IntPtr[] outCollection, int outCollectionSize);
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern int Windows_Kinect_AudioBeamFrame_get_SubFrames_Length(RootSystem.IntPtr pNative);
		public RootSystem.Collections.Generic.IList<Windows.Kinect.AudioBeamSubFrame> SubFrames
		{
			get
			{
				if (_pNative == RootSystem.IntPtr.Zero)
				{
					throw new RootSystem.ObjectDisposedException("AudioBeamFrame");
				}
				
				if (_subFrames == null)
				{
					int collectionSize = Windows_Kinect_AudioBeamFrame_get_SubFrames_Length(_pNative);
					var outCollection = new RootSystem.IntPtr[collectionSize];
					_subFrames = new Windows.Kinect.AudioBeamSubFrame[collectionSize];
					
					collectionSize = Windows_Kinect_AudioBeamFrame_get_SubFrames(_pNative, outCollection, collectionSize);
					for (int i = 0; i < collectionSize; i++)
					{
						if (outCollection[i] == RootSystem.IntPtr.Zero)
						{
							continue;
						}
						
						var obj = Helper.NativeObjectCache.GetObject<Windows.Kinect.AudioBeamSubFrame>(outCollection[i]);
						if (obj == null)
						{
							obj = new Windows.Kinect.AudioBeamSubFrame(outCollection[i]);
							Helper.NativeObjectCache.AddObject<Windows.Kinect.AudioBeamSubFrame>(outCollection[i], obj);
						}
						
						_subFrames[i] = obj;
					}
				}
				
				return _subFrames;
			}
		}
	}
	
	public sealed partial class BodyFrame
	{
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_BodyFrame_GetAndRefreshBodyData(RootSystem.IntPtr pNative, [RootSystem.Runtime.InteropServices.Out] RootSystem.IntPtr[] bodies, int bodiesSize);
		public void GetAndRefreshBodyData(RootSystem.Collections.Generic.IList<Windows.Kinect.Body> bodies)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("BodyFrame");
			}
			
			int _bodies_idx = 0;
			var _bodies = new RootSystem.IntPtr[bodies.Count];
			for (int i = 0; i < bodies.Count; i++)
			{
				if (bodies[i] == null)
				{
					bodies[i] = new Body();
				}
				
				_bodies[_bodies_idx] = bodies[i].GetIntPtr();
				_bodies_idx++;
			}
			
			Windows_Kinect_BodyFrame_GetAndRefreshBodyData(_pNative, _bodies, bodies.Count);
			for (int i = 0; i < bodies.Count; i++)
			{
				bodies[i].SetIntPtr(_bodies[i]);
			}
		}
	}
	
	public sealed partial class Body
	{
		internal void SetIntPtr(RootSystem.IntPtr value) { _pNative = value; }
		internal RootSystem.IntPtr GetIntPtr() { return _pNative; }
		
		internal Body() { }
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern RootSystem.IntPtr Windows_Kinect_Body_get_Lean(RootSystem.IntPtr pNative);
		public Windows.Kinect.PointF Lean
		{
			get
			{
				if (_pNative == RootSystem.IntPtr.Zero)
				{
					throw new RootSystem.ObjectDisposedException("Body");
				}
				
				var objectPointer = Windows_Kinect_Body_get_Lean(_pNative);
				var obj = (Windows.Kinect.PointF)RootSystem.Runtime.InteropServices.Marshal.PtrToStructure(objectPointer, typeof(Windows.Kinect.PointF));
				RootSystem.Runtime.InteropServices.Marshal.FreeHGlobal(objectPointer);
				return obj;
			}
		}
	}
	
	public sealed partial class ColorFrame
	{
		[RootSystem.Runtime.InteropServices.DllImport(
			"XboxOneUnityAddin",
			EntryPoint = "Windows_Kinect_ColorFrame_CopyRawFrameDataToArray",
			CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_ColorFrame_CopyRawFrameDataToIntPtr(RootSystem.IntPtr pNative, IntPtr frameData, uint frameDataSize);
		public void CopyRawFrameDataToIntPtr(RootSystem.IntPtr frameData, uint size)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("ColorFrame");
			}
			
			Windows_Kinect_ColorFrame_CopyRawFrameDataToIntPtr(_pNative, frameData, size);
		}
		
		[RootSystem.Runtime.InteropServices.DllImport(
			"XboxOneUnityAddin",
			EntryPoint = "Windows_Kinect_ColorFrame_CopyConvertedFrameDataToArray",
			CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_ColorFrame_CopyConvertedFrameDataToIntPtr(RootSystem.IntPtr pNative, IntPtr frameData, uint frameDataSize, Windows.Kinect.ColorImageFormat colorFormat);
		public void CopyConvertedFrameDataToIntPtr(RootSystem.IntPtr frameData, uint size, Windows.Kinect.ColorImageFormat colorFormat)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("ColorFrame");
			}
			
			Windows_Kinect_ColorFrame_CopyConvertedFrameDataToIntPtr(_pNative, frameData, size, colorFormat);
		}
	}
	
	public sealed partial class DepthFrame
	{
		[RootSystem.Runtime.InteropServices.DllImport(
			"XboxOneUnityAddin",
			EntryPoint = "Windows_Kinect_DepthFrame_CopyFrameDataToArray",
			CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_DepthFrame_CopyFrameDataToIntPtr(RootSystem.IntPtr pNative, IntPtr frameData, uint frameDataSize);
		public void CopyFrameDataToIntPtr(RootSystem.IntPtr frameData, uint size)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("DepthFrame");
			}
			
			Windows_Kinect_DepthFrame_CopyFrameDataToIntPtr(_pNative, frameData, size / sizeof(ushort));
		}
	}
	
	public sealed partial class BodyIndexFrame
	{
		[RootSystem.Runtime.InteropServices.DllImport(
			"XboxOneUnityAddin",
			EntryPoint = "Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray",
			CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_BodyIndexFrame_CopyFrameDataToIntPtr(RootSystem.IntPtr pNative, IntPtr frameData, uint frameDataSize);
		public void CopyFrameDataToIntPtr(RootSystem.IntPtr frameData, uint size)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("BodyIndexFrame");
			}
			
			Windows_Kinect_BodyIndexFrame_CopyFrameDataToIntPtr(_pNative, frameData, size);
		}
	}
	
	public sealed partial class InfraredFrame
	{
		[RootSystem.Runtime.InteropServices.DllImport(
			"XboxOneUnityAddin",
			EntryPoint = "Windows_Kinect_InfraredFrame_CopyFrameDataToArray",
			CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_InfraredFrame_CopyFrameDataToIntPtr(RootSystem.IntPtr pNative, IntPtr frameData, uint frameDataSize);
		public void CopyFrameDataToIntPtr(RootSystem.IntPtr frameData, uint size)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("InfraredFrame");
			}
			
			Windows_Kinect_InfraredFrame_CopyFrameDataToIntPtr(_pNative, frameData, size / sizeof(ushort));
		}
	}
	
	public sealed partial class LongExposureInfraredFrame
	{
		[RootSystem.Runtime.InteropServices.DllImport(
			"XboxOneUnityAddin",
			EntryPoint = "Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToArray",
			CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToIntPtr(RootSystem.IntPtr pNative, IntPtr frameData, uint frameDataSize);
		public void CopyFrameDataToIntPtr(RootSystem.IntPtr frameData, uint size)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("LongExposureInfraredFrame");
			}
			
			Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToIntPtr(_pNative, frameData, size / sizeof(ushort));
		}
	}
	
	public sealed partial class CoordinateMapper
	{
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(
			RootSystem.IntPtr pNative,
			RootSystem.IntPtr depthFrameData,
			uint depthFrameDataSize,
			RootSystem.IntPtr depthSpacePoints,
			uint depthSpacePointsSize);
		public void MapColorFrameToDepthSpaceUsingIntPtr(RootSystem.IntPtr depthFrameData, uint depthFrameSize, RootSystem.IntPtr depthSpacePoints, uint depthSpacePointsSize)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("CoordinateMapper");
			}
			
			uint length = depthFrameSize / sizeof(UInt16);
			Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(_pNative, depthFrameData, length, depthSpacePoints, depthSpacePointsSize);
		}
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(
			RootSystem.IntPtr pNative,
			RootSystem.IntPtr depthFrameData,
			uint depthFrameDataSize,
			RootSystem.IntPtr cameraSpacePoints,
			uint cameraSpacePointsSize);
		public void MapColorFrameToCameraSpaceUsingIntPtr(RootSystem.IntPtr depthFrameData, int depthFrameSize, RootSystem.IntPtr cameraSpacePoints, uint cameraSpacePointsSize)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("CoordinateMapper");
			}
			
			uint length = (uint)depthFrameSize / sizeof(UInt16);
			Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(_pNative, depthFrameData, length, cameraSpacePoints, cameraSpacePointsSize);
		}
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(
			RootSystem.IntPtr pNative,
			RootSystem.IntPtr depthFrameData,
			uint depthFrameDataSize,
			RootSystem.IntPtr colorSpacePoints,
			uint colorSpacePointsSize);
		public void MapDepthFrameToColorSpaceUsingIntPtr(RootSystem.IntPtr depthFrameData, int depthFrameSize, RootSystem.IntPtr colorSpacePoints, uint colorSpacePointsSize)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("CoordinateMapper");
			}
			
			uint length = (uint)depthFrameSize / sizeof(UInt16);
			Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(_pNative, depthFrameData, length, colorSpacePoints, colorSpacePointsSize);
		}
		
		
		[RootSystem.Runtime.InteropServices.DllImport("XboxOneUnityAddin", CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(
			RootSystem.IntPtr pNative,
			IntPtr depthFrameData,
			uint depthFrameDataSize,
			RootSystem.IntPtr cameraSpacePoints,
			uint cameraSpacePointsSize);
		public void MapDepthFrameToCameraSpaceUsingIntPtr(RootSystem.IntPtr depthFrameData, int depthFrameSize, RootSystem.IntPtr cameraSpacePoints, uint cameraSpacePointsSize)
		{
			if (_pNative == RootSystem.IntPtr.Zero)
			{
				throw new RootSystem.ObjectDisposedException("CoordinateMapper");
			}
			
			uint length = (uint)depthFrameSize / sizeof(UInt16);
			Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(_pNative, depthFrameData, length, cameraSpacePoints, cameraSpacePointsSize);
		}
	}
}
