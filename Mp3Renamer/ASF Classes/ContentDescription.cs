using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

using Tags.ID3;
using System.Collections;

namespace Tags.Objects
{
    /// <summary>
    /// Provide a class to read and write ASF Content Description object
    /// 75B22633-668E-11CF-A6D9-00AA0062CE6C
    /// </summary>
    public class ContentDescriptionOb : ASFObject
    {
        /// <summary>
        /// GUID of content descriptor
        /// </summary>
        public const string GUIDst = "75B22633-668E-11CF-A6D9-00AA0062CE6C";
        private string _Title;
        private string _Author;
        private string _Copyright;
        private string _Description;
        private ArrayList _RatingList;

        /// <summary>
        /// Create new content descriptor from TagStream
        /// </summary>
        /// <param name="rd">TagStream to read frame</param>
        /// <param name="ObjectSize">Object size</param>
        public ContentDescriptionOb(TagStream rd, long ObjectSize)
        {
            _RatingList = new ArrayList();
            Read(rd, ObjectSize);            
        }

        /// <summary>
        /// Create new Content Description object from specific information
        /// </summary>
        /// <param name="Title">Title of current file</param>
        /// <param name="Author">Author of current file</param>
        /// <param name="Copyright">Copyright notice of current file</param>
        /// <param name="Description">Description of current file</param>
        public ContentDescriptionOb(string Title, string Author, string Copyright,
            string Description)
        {
            this._Title = Title;
            this._Author = Author;
            this._Copyright = Copyright;
            this._Description = Description;
            this._RatingList = new ArrayList();
        }

        #region -> Properties <-

        /// <summary>
        /// Title of current file
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /// <summary>
        /// Author of current file
        /// </summary>
        public string Author
        {
            get { return _Author; }
            set { _Author = value; }
        }

        /// <summary>
        /// Copyright notice for this file
        /// </summary>
        public string Copyright
        {
            get { return _Copyright; }
            set { _Copyright = value; }
        }

        /// <summary>
        /// Description of current file
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        /// <summary>
        /// Gets or sets list of ratings
        /// </summary>
        public ArrayList Ratings
        {
            get
            { return _RatingList; }
        }

        #endregion

        /// <summary>
        /// Get Guid of current class
        /// </summary>
        /// <returns></returns>
        protected override string OnGetGUID()
        {
            return GUIDst;
        }

        /// <summary>
        /// Write this tag to specific TagStream
        /// </summary>
        /// <param name="wr">TagStream to write data</param>
        /// <returns>true if write successfull otherwise false</returns>
        protected override bool OnWritingData(TagStream wr)
        {
            wr.WriteGUID(GUIDst);
            wr.AsBinaryWriter.Write((long)Length);

            wr.AsBinaryWriter.Write(TagStream.StringLength(Title));
            wr.AsBinaryWriter.Write(TagStream.StringLength(Author));
            wr.AsBinaryWriter.Write(TagStream.StringLength(Copyright));
            wr.AsBinaryWriter.Write(TagStream.StringLength(Description));
            wr.AsBinaryWriter.Write(RatingLength);

            /* Some Applications like Jet Audio need string seprator to
             * show last character of text. unlike Media player
             */
            if (Title.Length > 0)
                wr.WriteText(Title, TextEncodings.UTF_16, true);
            if (Author.Length > 0)
                wr.WriteText(Author, TextEncodings.UTF_16, true);
            if (Copyright.Length > 0)
                wr.WriteText(Copyright, TextEncodings.UTF_16, true);
            if (Description.Length > 0)
                wr.WriteText(Description, TextEncodings.UTF_16, true);
            if (_RatingList.Count > 0)
            {
                foreach (string st in _RatingList)
                    wr.WriteText(st, TextEncodings.UTF_16, true);
            }

            return true;
        }

        /// <summary>
        /// Read tag dat afrom specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="ObjectSize">Maximum size of object</param>
        /// <returns>true if read successfull otherwise false</returns>
        protected override bool OnReadingData(TagStream rd, long ObjectSize)
        {
            int TitleLen, AuthorLen, CopyrightLen, DescriptionLen, RatingLen;
            _Title = _Author = _Copyright = _Description = string.Empty;

            TitleLen = rd.AsBinaryReader.ReadInt16();
            AuthorLen = rd.AsBinaryReader.ReadInt16();
            CopyrightLen = rd.AsBinaryReader.ReadInt16();
            DescriptionLen = rd.AsBinaryReader.ReadInt16();
            RatingLen = rd.AsBinaryReader.ReadInt16();

            if (TitleLen > 0)
                _Title = rd.ReadText(TitleLen * 2, TextEncodings.UTF_16);
            if (AuthorLen > 0)
                _Author = rd.ReadText(AuthorLen * 2, TextEncodings.UTF_16);
            if (CopyrightLen > 0)
                _Copyright = rd.ReadText(CopyrightLen * 2, TextEncodings.UTF_16);
            if (DescriptionLen > 0)
                _Description = rd.ReadText(DescriptionLen * 2, TextEncodings.UTF_16);

            string temp;
            while (RatingLen > 0)
            {
                temp = rd.ReadText(RatingLen, TextEncodings.UTF_16, ref RatingLen, false);
                _RatingList.Add(temp);
            }

            return true;
        }

        /// <summary>
        /// Gets length of current frame
        /// </summary>
        /// <returns>long contains current frame size</returns>
        protected override long OnGetLength()
        {
            int Length = 34; // 16(GUID) + 8(Size) + 10(Object Sizes)

            Length += TagStream.StringLength(Title);
            Length += TagStream.StringLength(Author);
            Length += TagStream.StringLength(Copyright);
            Length += TagStream.StringLength(Description);
            Length += RatingLength;

            return Length;
        }

        private Int16 RatingLength
        {
            get
            {
                Int16 Length = 0;
                foreach (string st in _RatingList)
                    Length += TagStream.StringLength(st);
                return Length;
            }
        }
    }
}
