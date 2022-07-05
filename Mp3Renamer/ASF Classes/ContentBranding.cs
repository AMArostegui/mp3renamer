using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace Tags.Objects
{
    /// <summary>
    /// Indicates diffrent types of Images for ASF tags
    /// </summary>
    public enum ImageTypes
    {
        /// <summary>
        /// none
        /// </summary>
        none = 0,
        /// <summary>
        /// bitmap image
        /// </summary>
        bmp,
        /// <summary>
        /// jpeg image
        /// </summary>
        jpeg,
        /// <summary>
        /// gif image
        /// </summary>
        gif
    }

    /// <summary>
    /// Provide a class for content branding object of ASF
    /// </summary>
    public class ContentBrandingOb : ASFObject
    {
        /// <summary>
        /// GUID of Content branding object
        /// </summary>
        public const string GUIDst = "2211B3FA-BD23-11D2-B4B7-00A0C955FC6E";
        private string _CopyrightURL;
        private string _ImageURL;
        private MemoryStream _Image;
        private ImageTypes _ImageType;

        /// <summary>
        /// Create new content branding object
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="BlockSize">Block Size of Branding object</param>
        public ContentBrandingOb(TagStream rd, long BlockSize)
        {
            Read(rd, BlockSize);
        }

        /// <summary>
        /// Create new content branding object
        /// </summary>
        /// <param name="CopyrightURL">Copyright URL</param>
        /// <param name="ImageURL">Image URL</param>
        /// <param name="Image">Image data</param>
        /// <param name="ImageType">Type of image</param>
        public ContentBrandingOb(string CopyrightURL, string ImageURL, MemoryStream Image, ImageTypes ImageType)
        {
            this.CopyrightURL = CopyrightURL;
            this.ImageURL = ImageURL;
            this.Image = Image;
            this.ImageType = ImageType;
        }

        /// <summary>
        /// Gets or sets ImageType of current Branding object
        /// </summary>
        public ImageTypes ImageType
        {
            get
            { return _ImageType; }
            set
            { _ImageType = value; }
        }

        /// <summary>
        /// CopyrightURL for current Branding Object
        /// </summary>
        public string CopyrightURL
        {
            get
            { return _CopyrightURL; }
            set
            { _CopyrightURL = value; }
        }

        /// <summary>
        /// Image URL that provide more information about image
        /// </summary>
        public string ImageURL
        {
            get { return _ImageURL; }
            set { _ImageURL = value; }
        }

        /// <summary>
        /// Image of current Branding Object
        /// </summary>
        public MemoryStream Image
        {
            get { return _Image; }
            set
            { _Image = value; }
        }

        /// <summary>
        /// Get System.Drawing.Image from Image stream
        /// </summary>
        /// <returns>System.Drawing.Image</returns>
        public Image GetImage()
        {
            return System.Drawing.Image.FromStream(Image);
        }

        /// <summary>
        /// Get GUID of current ASF object
        /// </summary>
        /// <returns></returns>
        protected override string OnGetGUID()
        { return GUIDst; }

        /// <summary>
        /// Read data of current frame from specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="ObjectSize">Maximum size of tag to read</param>
        /// <returns>true if read successfull otherwise false</returns>
        protected override bool OnReadingData(TagStream rd, long ObjectSize)
        {
            ImageType = (ImageTypes)rd.AsBinaryReader.ReadInt32();
            int Length = rd.AsBinaryReader.ReadInt32();
            if (ImageType != ImageTypes.none)
            {
                byte[] Buffer = new byte[Length];
                rd.Read(Buffer, 0, Length);
                Image = new MemoryStream();
                Image.Write(Buffer, 0, Length);
            }

            Length = rd.AsBinaryReader.ReadInt32();
            if (Length > 0)
                ImageURL = rd.ReadText(Length, Tags.ID3.TextEncodings.UTF_16);
            else
                ImageURL = "";

            Length = rd.AsBinaryReader.ReadInt32();
            if (Length > 0)
                CopyrightURL = rd.ReadText(Length, Tags.ID3.TextEncodings.UTF_16);
            else
                CopyrightURL = "";

            return true;
        }

        /// <summary>
        /// Call when frame need to write it's data to stream
        /// </summary>
        /// <param name="writer">TagStream to write data</param>
        protected override bool OnWritingData(TagStream writer)
        {
            writer.WriteGUID(GUIDst);
            writer.AsBinaryWriter.Write((long)Length);

            writer.AsBinaryWriter.Write((Int32)ImageType);
            writer.AsBinaryWriter.Write((Int32)Image.Length);
            if (ImageType != ImageTypes.none)
                Image.WriteTo(writer);

            writer.WriteText(ImageURL, 4);
            writer.WriteText(CopyrightURL, 4);

            return true;
        }

        /// <summary>
        /// Gets length of current frame in byte
        /// </summary>
        /// <returns>int contain length of current frame</returns>
        protected override long OnGetLength()
        {
            long L = 40;
            return L + _Image.Length + _ImageURL.Length + _CopyrightURL.Length;
        }
    }
}
