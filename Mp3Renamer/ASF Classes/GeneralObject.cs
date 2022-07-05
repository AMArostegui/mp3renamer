using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tags.Objects
{
    /// <summary>
    /// Provide an object to read and write unknown objects of ASF
    /// </summary>
    public class GeneralObject : ASFObject
    {
        byte[] _Data;
        string _GUID;

        /// <summary>
        /// Create new GeneralObject from specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="ObjectSize">Object size to read from TagStream</param>
        /// <param name="GUID">GUID of object</param>
        public GeneralObject(TagStream rd, long ObjectSize, string GUID)
        {
            Read(rd, ObjectSize);
            _GUID = GUID;
        }

        /// <summary>
        /// Read data from specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="ObjectSize">Object size in tag stream</param>
        /// <returns>true if readed successfully otherwise false</returns>
        protected override bool OnReadingData(TagStream rd, long ObjectSize)
        {
            _Data = new byte[ObjectSize];
            rd.Read(_Data, 0, (int)ObjectSize);
            return true;
        }

        /// <summary>
        /// Call when frame need to write it's data to stream
        /// </summary>
        /// <param name="wr">TagStream to write data</param>
        protected override bool OnWritingData(TagStream wr)
        {
            wr.WriteGUID(this.GUID);
            wr.AsBinaryWriter.Write((long)Length);

            wr.Write(_Data, 0, _Data.Length);
            return true;
        }

         /// <summary>
        /// Gets length of current frame in byte
        /// </summary>
        /// <returns>int contain length of current frame</returns>
        protected override long OnGetLength()
        {
            return _Data.Length + 24;
        }

        /// <summary>
        /// Gets GUID of current object
        /// </summary>
        /// <returns>String contain GUID of current object</returns>
        protected override string OnGetGUID()
        {
            return _GUID;
        }
    }
}
