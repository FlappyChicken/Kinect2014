using RootSystem = System;
using System.Linq;
using System.Collections.Generic;
namespace Windows.Storage.Streams
{
    //
    // Windows.Storage.Streams.IBuffer
    //
    public interface IBuffer
    {

        // Public Properties
         uint Capacity
        {
            get;
        }

         uint Length
        {
            get;
            set;
        }

    }

}
