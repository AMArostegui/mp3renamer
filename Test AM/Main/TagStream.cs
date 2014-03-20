using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Tags.ID3;
using Tags.ASF;
using Tags;

/*
 * An IO classes for Tag reading and writing
 */
namespace System.IO
{
    /// <summary>
    /// Provide a stream inherited from FileStream to read and write tags
    /// </summary>
    public class TagStream : FileStream
    {
        private BinaryReader _BReader;
        private BinaryWriter _BWriter;

        /// <summary>
        /// Create new TagStream
        /// </summary>
        /// <param name="path">Path of file to create tag on it</param>
        /// <param name="mode">FileMode for opening stream</param>
        public TagStream(string path, FileMode mode)
            : base(path, mode)
        {
            _BReader = new BinaryReader(this);
            _BWriter = new BinaryWriter(this);
        }

        #region -> Read Methods <-

        /// <summary>
        /// Read string from current TagStream
        /// </summary>
        /// <param name="MaxLength">Maximum length that can read from stream</param>
        /// <param name="TEncoding">TextEcoding to read from Stream</param>
        /// <param name="DetectEncoding">Determines wether the method must indicate encoding or not</param>
        /// <param name="ReadedLength">Indicate number of bytes readed from current stream</param>
        /// <returns>System.String contains readed string from current TagStream</returns>
        public string ReadText(int MaxLength, TextEncodings TEncoding, ref int ReadedLength, bool DetectEncoding)
        {
            if (MaxLength <= 0)
                return "";
            long Pos = this.Position; // store current position

            MemoryStream MStream = new MemoryStream();
            if (DetectEncoding && MaxLength >= 3)
            {
                byte[] Buffer = new byte[3];
                base.Read(Buffer, 0, Buffer.Length);
                if (Buffer[0] == 0xFF && Buffer[1] == 0xFE)
                {   // FF FE
                    TEncoding = TextEncodings.UTF_16;// UTF-16 (LE)
                    this.Position--;
                    MaxLength -= 2;
                }
                else if (Buffer[0] == 0xFE && Buffer[1] == 0xFF)
                {   // FE FF
                    TEncoding = TextEncodings.UTF_16BE;
                    this.Position--;
                    MaxLength -= 2;
                }
                else if (Buffer[0] == 0xEF && Buffer[1] == 0xBB && Buffer[2] == 0xBF)
                {
                    // EF BB BF
                    TEncoding = TextEncodings.UTF8;
                    MaxLength -= 3;
                }
                else
                    this.Position -= 3;
            }
            // Indicate text seprator type for current string encoding
            bool Is2ByteSeprator = (TEncoding == TextEncodings.UTF_16 || TEncoding == TextEncodings.UTF_16BE);

            byte Buf, Buf2;
            while (MaxLength > 0)
            {
                if (Is2ByteSeprator)
                {
                    Buf = ReadByte();
                    Buf2 = ReadByte();

                    if (Buf == 0 && Buf2 == 0)
                        break;
                    else
                    {
                        MStream.WriteByte(Buf);
                        MStream.WriteByte(Buf2);
                    }
                    
                    MaxLength--;
                }
                else
                {
                    Buf = ReadByte(); // Read First/Next byte from stream

                    if (Buf == 0)
                        break;
                    else
                        MStream.WriteByte(Buf);
    
                }

                MaxLength--;
            }

            if (MaxLength < 0)
                this.Position += MaxLength;

            ReadedLength -= Convert.ToInt32(this.Position - Pos);

            return StaticMethods.GetEncoding(TEncoding).GetString(MStream.ToArray());
        }

        /// <summary>
        /// Read string from current TagStream with automatic Encoding detection
        /// </summary>
        /// <param name="MaxLength">Maximum length that can read from stream</param>
        /// <param name="TEncoding">TextEcoding to read from Stream</param>
        /// <returns>System.String contains readed string from current TagStream</returns>
        public string ReadText(int MaxLength, TextEncodings TEncoding)
        {
            int i = 0;
            return ReadText(MaxLength, TEncoding, ref i, true);
        }

        /// <summary>
        /// Read string from current TagStream
        /// </summary>
        /// <param name="MaxLength">Maximum length that can read from stream</param>
        /// <param name="TEncoding">TextEcoding to read from Stream</param>
        /// <param name="DetectEncoding">Determines wether the method must indicate encoding or not</param>
        /// <returns>System.String contains readed string from current TagStream</returns>
        public string ReadText(int MaxLength, TextEncodings TEncoding, bool DetectEncoding)
        {
            int i = 0;
            return ReadText(MaxLength, TEncoding, ref i, DetectEncoding);
        }

        /// <summary>
        /// Read a byte from current TagStream
        /// </summary>
        /// <returns>Readed byte</returns>
        public new byte ReadByte()
        {
            return Convert.ToByte(base.ReadByte());
        }

        /// <summary>
        /// Read specified length of byte from current TagStream and returns it as UInt
        /// </summary>
        /// <param name="Length">Length of bytes to read</param>
        /// <returns>UInt represent readed number</returns>
        public uint ReadUInt(int Length)
        {
            if (Length > 4 || Length < 1)
                throw (new ArgumentOutOfRangeException("ReadUInt method can read 1-4 byte(s)"));

            byte[] Buf = new byte[Length];
            byte[] RBuf = new byte[4];
            base.Read(Buf, 0, Length);
            Buf.CopyTo(RBuf, 4 - Buf.Length);
            Array.Reverse(RBuf);
            return BitConverter.ToUInt32(RBuf, 0);
        }

        /// <summary>
        /// Read data from current TagStream and returns it as MemoryStream
        /// </summary>
        /// <param name="Length">Length to read</param>
        /// <returns>MemoryStream contains data readed</returns>
        public MemoryStream ReadData(int Length)
        {
            MemoryStream ms;
            byte[] Buf = new byte[Length];
            base.Read(Buf, 0, Length);
            ms = new MemoryStream();
            ms.Write(Buf, 0, Length);

            return ms;
        }

        /// <summary>
        /// Read a Guid from current TagStream
        /// </summary>
        /// <returns>Readed guid or Guid.Empty for not valid Guids</returns>
        public Guid ReadGuid()
        {
            // Guid stores Little Endian for each part
            int Part1;
            short Part2, Part3;
            byte[] Part4;

            Part1 = AsBinaryReader.ReadInt32();
            Part2 = AsBinaryReader.ReadInt16();
            Part3 = AsBinaryReader.ReadInt16();
            Part4 = AsBinaryReader.ReadBytes(8);

            Guid RGuid;
            try
            {
                RGuid = new Guid(Part1, Part2, Part3, Part4);
                return RGuid;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        #endregion

        #region -> Write Methods <-

        /// <summary>
        /// Write specified string to current TagStream
        /// </summary>
        /// <param name="Text">Text to write in TagStream</param>
        /// <param name="TEncoding">TextEncoding use for writing</param>
        /// <param name="AddNullCharacter">Indicate if need to add null character at end of string</param>
        public void WriteText(string Text, TextEncodings TEncoding, bool AddNullCharacter)
        {
            byte[] Buf;
            Buf = StaticMethods.GetEncoding(TEncoding).GetBytes(Text);
            this.Write(Buf, 0, Buf.Length);
            if (AddNullCharacter)
            {
                this.WriteByte(0);
                if (TEncoding == TextEncodings.UTF_16 || TEncoding == TextEncodings.UTF_16BE)
                    this.WriteByte(0);
            }
        }

        /// <summary>
        /// Writes size of text then write text use UTF_16
        /// </summary>
        /// <param name="Text">Text to write</param>
        /// <param name="Length">Length of size in byte(2, 4 or 8)</param>
        public void WriteText(string Text, int Length)
        {
            switch (Length)
            {
                case 2:
                    this.AsBinaryWriter.Write(TagStream.StringLength(Text));
                    break;
                case 4:
                    this.AsBinaryWriter.Write((int)TagStream.StringLength(Text));
                    break;
                case 8:
                    this.AsBinaryWriter.Write((long)TagStream.StringLength(Text));
                    break;
                default:
                    throw new ArgumentException("Length must be 2, 4 or 8");
            }

            if (Text.Length > 0)
                WriteText(Text, TextEncodings.UTF_16, true);
        }

        /// <summary>
        /// Write specified GUID to current TagStream
        /// </summary>
        /// <param name="GUID">GUID to write</param>
        public void WriteGUID(Guid GUID)
        {
            byte[] Buf;
            Buf = GUID.ToByteArray();

            this.Write(Buf, 0, Buf.Length);
        }

        /// <summary>
        /// Write Specific GUID to current TagStream
        /// </summary>
        /// <param name="GUID">GUID to write to TagStream</param>
        public void WriteGUID(string GUID)
        {
            WriteGUID(new Guid(GUID));
        }

        #endregion

        /// <summary>
        /// Gets Current TagStream as Binary Reader
        /// </summary>
        public BinaryReader AsBinaryReader
        {
            get
            { return _BReader; }
        }

        /// <summary>
        /// Gets Current TagStream as Binary Writer
        /// </summary>
        public BinaryWriter AsBinaryWriter
        {
            get
            { return _BWriter; }
        }

        /// <summary>
        /// Indicate if current TagStream contains ID3v1 Information
        /// </summary>
        public bool HaveID3v1()
        {
            if (this.Length < 128)
                return false;
            this.Seek(-128, SeekOrigin.End);
            string Tag = ReadText(3, TextEncodings.Ascii);
            return (Tag == "TAG");
        }

        /// <summary>
        /// Get Length of specified string in byte and use Unicode encoding
        /// </summary>
        /// <param name="st">String to get length</param>
        /// <returns>0 if string length be zero otherwise calculated length</returns>
        public static Int16 StringLength(string st)
        {
            return Convert.ToInt16(st.Length > 0 ? st.Length * 2 + 2 : 0);
        }

        /// <summary>
        /// Delete original file and rename temporary file
        /// </summary>
        /// <param name="OriginalPath">Original file path</param>
        /// <param name="TempPath">Address of temp file</param>
        /// <param name="NewPath">Name of file after saving</param>
        public static void DeleteRename(string OriginalPath, string TempPath, string NewPath)
        {
            try
            {
                FileInfo F = new FileInfo(OriginalPath);
                F.OpenWrite().Close(); // Identify if file is in use by another process

                File.Delete(OriginalPath);
            }
            catch (Exception Ex)
            {
                // If application couldn't delete original file
                // it will delete temporary file and throw exception
                File.Delete(TempPath);
                throw Ex;
            }

            try
            {
                File.Move(TempPath, NewPath);
            }
            catch (Exception Ex)
            {
                // If application couldn't rename file
                File.Move(TempPath, OriginalPath);
                throw Ex;
            }
        }
    }
}
