using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tags
{
    /// <summary>
    /// Base object for ASF objects
    /// </summary>
    public abstract class ASFObject
    {
        private ExceptionCollection _Exceptions = new ExceptionCollection();

        /// <summary>
        /// Write current object data to specific TagStream
        /// </summary>
        /// <param name="wr">TagStream to write data to</param>
        public bool WriteData(TagStream wr)
        { return OnWritingData(wr); }

        /// <summary>
        /// Read object from specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="ObjectSize">Maximum size of object</param>
        public bool Read(TagStream rd, long ObjectSize)
        { return OnReadingData(rd, ObjectSize); }

        /// <summary>
        /// Get GUID of current Object
        /// </summary>
        public string GUID
        {
            get
            { return OnGetGUID(); }
        }

        /// <summary>
        /// Get Length of current object
        /// </summary>
        public long Length
        {
            get
            { return OnGetLength(); }
        }

        /// <summary>
        /// Add specific exception to list of occured exceptions
        /// </summary>
        /// <param name="Ex">Exception to add</param>
        protected void AddException(Exception Ex)
        {
            _Exceptions.Add(Ex);
        }

        /// <summary>
        /// Exceptions that occured while reading this object
        /// </summary>
        public ExceptionCollection Exceptions
        {
            get
            { return _Exceptions; }
        }

        /// <summary>
        /// Call when frame need to write it's data to stream
        /// </summary>
        /// <param name="wr">TagStream to write data</param>
        protected virtual bool OnWritingData(TagStream wr)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Read data from specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="ObjectSize">Object size in tag stream</param>
        /// <returns>true if readed successfully otherwise false</returns>
        protected virtual bool OnReadingData(TagStream rd, long ObjectSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets GUID of current frame 
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetGUID()
        {
            throw new Exception("The method or operation is not implemented.");
        }

         /// <summary>
        /// Gets length of current frame in byte
        /// </summary>
        /// <returns>int contain length of current frame</returns>
        protected virtual long OnGetLength()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
