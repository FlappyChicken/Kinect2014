using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using RootSystem = System;

namespace Windows.Storage.Streams
{
    public interface IBufferNativeWrapper
    {
        RootSystem.IntPtr UnderlyingBuffer { get; }
    }
}