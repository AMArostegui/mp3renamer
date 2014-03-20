using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using Tags;
using Tags.Objects;
using System.ComponentModel;

namespace Tags.ASF
{
    /// <summary>
    /// Provide a class to read and write WMA tags
    /// </summary>
    public class ASFTagInfo : ITagInfo
    {
        /// <summary>
        /// GUID for WMA file
        /// </summary>
        public const string GUIDst = "75B22630-668E-11CF-A6D9-00AA0062CE6C";
        private Hashtable _List;
        private ExceptionCollection _Exceptions;
        private ArrayList _Unknown;
        private string _FilePath;
        private long _DataPosition;

        /// <summary>
        /// Create new ASFTagInfo from specific file and load it's data
        /// </summary>
        /// <param name="FilePath">File path to read ASF</param>
        /// <param name="LoadData">Indicate Loading ASF data or not</param>
        public ASFTagInfo(string FilePath, bool LoadData)
        {
            _FilePath = FilePath;
            _List = new Hashtable();
            _Exceptions = new ExceptionCollection();
            _Unknown = new ArrayList();
            _IsTemplate = false;
            
            if (LoadData)
                Load();
        }

        /// <summary>
        /// Load ASF information from file
        /// </summary>
        /// <returns>True if read successfull</returns>
        public bool Load()
        {
            Exceptions.Clear();

            TagStream Data = new TagStream(_FilePath, FileMode.Open);

            Guid g;
            long BlockSize;
            long MaxSize;
            int Headers;
            
            g = Data.ReadGuid();
            if (g.ToString().ToUpper() != GUIDst)
                throw new ASFException("This file is not a valid ASF file",
                    g.ToString(), ExceptionLevels.Error);

            MaxSize = Data.AsBinaryReader.ReadInt64();
            Headers = Data.AsBinaryReader.ReadInt32();

            Data.ReadByte();
            if (Data.ReadByte() != 2)
                Exceptions.Add(new ASFException("Second reserved byte usually is 2 in this file this is not",
                    g.ToString(), ExceptionLevels.Warning));
            _DataPosition = MaxSize;

            while (Data.CanRead && MaxSize - Data.Position > 24) // GUID(16 Byte) + QWORD(Size of object) = 24 Bytes
            {
                g = Data.ReadGuid();
                if (g == Guid.Empty)
                    throw new ASFException("Not Valid GUID found", g.ToString(), ExceptionLevels.Repaired);

                BlockSize = Data.AsBinaryReader.ReadInt64() - 24;
                if (BlockSize < 0)
                    throw new ASFException("Block size is negative, file have problem or not readed successfully",
                        g.ToString(), ExceptionLevels.Error);

                ASFObject Object = null;
                switch (g.ToString().ToUpper())
                {
                    case ContentDescriptionOb.GUIDst:
                        Object = new ContentDescriptionOb(Data, BlockSize);
                        break;
                    case ExContentDescriptionOb.GUIDst:
                        Object = new ExContentDescriptionOb(Data, BlockSize);
                        break;
                    case ContentBrandingOb.GUIDst:
                        Object = new ContentBrandingOb(Data, BlockSize);
                        break;
                    default:
                        Object = new GeneralObject(Data, BlockSize, g.ToString());
                        _Unknown.Add(Object);
                        break;
                }

                foreach (Exception Ob in Object.Exceptions)
                    _Exceptions.Add(Ob);

                if (Object.GetType() != typeof(GeneralObject))
                    _List.Add(Object.GUID, Object);
            }

            Data.Close();
            return true;
        }

        /// <summary>
        /// Save file on specific path
        /// </summary>
        /// <param name="Path">Path to save file</param>
        /// <returns>True if saved successfull</returns>
        public bool SaveAs(string Path)
        {
            TagStream TempFile = new TagStream(TempFilePath, FileMode.Create);

            WriteHeader(TempFile);

            foreach (GeneralObject GO in UnknowsObjects)
                GO.WriteData(TempFile);

            foreach (ASFObject Ob in _List.Values)
                Ob.WriteData(TempFile);

            if (!IsTemplate)
            {
                TagStream OriginalFile = new TagStream(FilePath, FileMode.Open);
                
                OriginalFile.ReadGuid();
                long L = OriginalFile.AsBinaryReader.ReadInt64();
                OriginalFile.Seek(L, SeekOrigin.Begin);

                L = OriginalFile.Length - L;
                byte[] Buf = new byte[L];
                OriginalFile.Read(Buf, 0, Buf.Length);
                TempFile.Write(Buf, 0, Buf.Length);

                OriginalFile.Close();
            }

            TempFile.Close();
           

            TagStream.DeleteRename(FilePath, TempFile.Name, Path);
            FilePath = Path;

            return true;
        }

        /// <summary>
        /// Save file in the same path
        /// </summary>
        /// <returns>true if saved successfull</returns>
        public bool Save()
        {
            return SaveAs(_FilePath);
        }

        /// <summary>
        /// Save current ASFTagInfo with specific formula of filename
        /// </summary>
        /// <param name="Formula">Formula to make filename</param>
        /// <returns>True if savfe successfull otherwise false</returns>
        public bool Save(string Formula)
        {
            return SaveAs(GetFilepath(Formula));
        }

        string GetFilepath(string Formula)
        {
            if (Formula == "")
                return FilePath;

            return Path.Combine(Path.GetDirectoryName(FilePath), MakeFileName(Formula));
        }

        /// <summary>
        /// Make file name according to formula
        /// </summary>
        /// <param name="Formula">Formula to make filename</param>
        public string MakeFileName(string Formula)
        {
            string FileName = "";

            Formula = Formula.Replace("<", "<;");
            string ID;
            foreach (string St in Formula.Split('>', '<'))
            {
                if (St.StartsWith(";"))
                {
                    ID = St.Remove(0, 1);
                    if (ID.StartsWith("WM/TrackNumber"))
                    {
                        string TRCK = ExContentDescription["WM/TrackNumber"].ToString();
                        if (TRCK == null)
                            TRCK = "";
                        else
                            TRCK = TRCK.Split('/')[0];

                        if (ID.Length == 15)
                        {
                            int Digits = int.Parse(ID[14].ToString());

                            while (Digits-- > TRCK.Length)
                            {
                                FileName += "0";
                            }
                        }
                        FileName += TRCK;
                    }
                    else
                    {
                        object Val = ExContentDescription[ID];
                        if (Val != null)
                            FileName += Val.ToString();
                    }
                }
                else
                    FileName += St;
            }

            return FileName + ".wma";
        }

        private void WriteHeader(TagStream wr)
        {
            wr.WriteGUID(GUIDst);
            wr.AsBinaryWriter.Write((Int64)HeaderLength);
            wr.AsBinaryWriter.Write((Int32)_List.Count + UnknowsObjects.Length);
            wr.WriteByte(1);
            wr.WriteByte(2);
        }

        private long HeaderLength
        {
            get
            {
                long Length = 30; // 24 GUID + Size + 4Count + 2Reserved

                foreach (ASFObject var in _List.Values)
                    Length += var.Length;

                foreach (GeneralObject var in UnknowsObjects)
                    Length += var.Length;

                return Length;
            }
        }

        /// <summary>
        /// Get file path of current ASFTag
        /// </summary>
        public string FilePath
        {
            get
            { return _FilePath; }
            private set
            { _FilePath = value; }
        }

        /// <summary>
        /// Filename of current ASFTagInfo
        /// </summary>
        public string FileName
        {
            get
            { return Path.GetFileName(FilePath); }
        }

        /// <summary>
        /// Path of file to use as temp while saving
        /// </summary>
        private string TempFilePath
        {
            get
            {
                return FilePath + "~Temp";
            }
        }

        /// <summary>
        /// Get Content Description of current ASFTagInfo
        /// </summary>
        public ContentDescriptionOb ContentDescription
        {
            get
            { return (ContentDescriptionOb)_List[ContentDescriptionOb.GUIDst]; }
            set
            { SetValue(ContentDescriptionOb.GUIDst, value); }
        }

        /// <summary>
        /// Extended Content Description for current TagInfo
        /// </summary>
        public ExContentDescriptionOb ExContentDescription
        {
            get
            { return (ExContentDescriptionOb)_List[ExContentDescriptionOb.GUIDst]; }
            set
            { SetValue(ExContentDescriptionOb.GUIDst, value); }
        }

        /// <summary>
        /// Content Branding Obejct
        /// </summary>
        public ContentBrandingOb ContentBranding
        {
            get
            { return (ContentBrandingOb)_List[ContentBrandingOb.GUIDst]; }
            set
            { SetValue(ContentBrandingOb.GUIDst, value); }
        }

        private void SetValue(string GUID, ASFObject Object)
        {
            if (_List.ContainsKey(GUID))
                _List[GUID] = Object;
            else
                _List.Add(GUID, Object);
        }

        /// <summary>
        /// Exceptions that occured while reading the file
        /// </summary>
        public ExceptionCollection Exceptions
        {
            get
            { return _Exceptions; }
        }

        /// <summary>
        /// Get array of unknown readed objects
        /// </summary>
        public GeneralObject[] UnknowsObjects
        {
            get
            { return (GeneralObject[])_Unknown.ToArray(typeof(GeneralObject)); }
        }

        /// <summary>
        /// Indicate if current tag contains Excpetion
        /// </summary>
        public bool HaveException
        {
            get { return (_Exceptions.Count > 0); }
        }

        private bool _IsTemplate;
        /// <summary>
        /// Indicate if current tag is template
        /// </summary>
        public bool IsTemplate
        {
            get { return _IsTemplate; }
            set { _IsTemplate = value; }
        }

    }
}