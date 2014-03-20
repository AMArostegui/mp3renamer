using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Tags.Objects
{
    /// <summary>
    /// Provide a class to read and write ExContent obejct of ASF tags
    /// </summary>
    public class ExContentDescriptionOb : ASFObject, IEnumerable
    {
        /// <summary>
        /// GUID of Extended content description
        /// </summary>
        public const string GUIDst = "D2D0A440-E307-11D2-97F0-00A0C95EA850";
        private Dictionary<string, Descriptor> _Descriptors;

        /// <summary>
        /// Create new ExContentDescription from specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="BlockSize">Size of object to read</param>
        public ExContentDescriptionOb(TagStream rd, long BlockSize)
        {
            _Descriptors = new Dictionary<string, Descriptor>();

            Read(rd, BlockSize);
        }

        /// <summary>
        /// Create new ExContentDescription class
        /// </summary>
        public ExContentDescriptionOb()
        {
            _Descriptors = new Dictionary<string, Descriptor>();
        }

        /// <summary>
        /// return GUID value for Extended content description
        /// </summary>
        /// <returns></returns>
        protected override string OnGetGUID()
        { return GUIDst; }

        /// <summary>
        /// Read data from specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        /// <param name="ObjectSize">Maximum size of object to read</param>
        /// <returns>true if read successfull otherwise false</returns>
        protected override bool OnReadingData(TagStream rd, long ObjectSize)
        {
            int Contents = rd.AsBinaryReader.ReadInt16();

            Descriptor D;
            for (int i = 0; i < Contents; i++)
            {
                D = new Descriptor(rd);
                _Descriptors.Add(D.Name, D);
            }

            return true;
        }

        /// <summary>
        /// Write current tag to specific TagStream
        /// </summary>
        /// <param name="wr">Tag Stream to write</param>
        /// <returns>true if write successfull</returns>
        protected override bool OnWritingData(TagStream wr)
        {
            wr.WriteGUID(GUIDst);
            wr.AsBinaryWriter.Write((long)Length);
            wr.AsBinaryWriter.Write(Convert.ToInt16(Count));

            foreach (Descriptor D in _Descriptors.Values)
                D.Write(wr);

            return true;
        }

        /// <summary>
        /// Gets length of current frame
        /// </summary>
        /// <returns>System.Long contains length of current tag</returns>
        protected override long OnGetLength()
        {
            int L = 24;
            foreach (Descriptor D in _Descriptors.Values)
                L += D.Length;
            return 2 + L;
        }

        /// <summary>
        /// Add new descriptor to list
        /// </summary>
        /// <param name="Des">Descriptor to add</param>
        public void Add(Descriptor Des)
        {
            _Descriptors.Add(Des.Name, Des);
        }

        /// <summary>
        /// Remove specific descriptor from list
        /// </summary>
        /// <param name="Name">Name of descriptor to remove</param>
        public void Remove(string Name)
        {
            _Descriptors.Remove(Name);
        }

        /// <summary>
        /// Remove all descriptors except byte arrays
        /// </summary>
        public void RemoveNonArray()
        {
            foreach (Descriptor var in _Descriptors.Values)
                if (var.DataType != typeof(byte[]))
                    _Descriptors.Remove(var.Name);
        }

        /// <summary>
        /// Number of descriptor that current object contains
        /// </summary>
        public int Count
        {
            get
            { return _Descriptors.Count; }
        }

        /// <summary>
        /// Get value of descriptor according to it's name
        /// </summary>
        /// <param name="Name">Name of descriptor to return</param>
        /// <returns>Value of specific descriptor or null if name not found</returns>
        public object GetValue(string Name)
        {
            return _Descriptors[Name];
        }

        /// <summary>
        /// Set value of specific descriptor
        /// </summary>
        /// <param name="Name">Name of descriptor to set</param>
        /// <param name="Value">value of descriptor to set</param>
        public void SetValue(string Name, object Value)
        {
            _Descriptors.Remove(Name);

            _Descriptors.Add(Name, new Descriptor(Name, Value));
        }

        /// <summary>
        /// Get Enumerator for descriptors
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _Descriptors.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets or sets value of specific descriptor
        /// </summary>
        /// <param name="Name">Name of descriptor</param>
        /// <returns>Object contains descriptor value</returns>
        public object this[string Name]
        {
            get
            {
                Descriptor D;
                _Descriptors.TryGetValue(Name, out D);
                return (D != null) ? D.Value : null;
            }
            set
            {
                _Descriptors.Remove(Name);
                if (value != null && value.ToString() != "")
                {
                    Descriptor D = new Descriptor(Name, value);
                    _Descriptors.Add(Name, D);
                }
            }
        }
    }

    /// <summary>
    /// Provide descriptor class for reading and writing descriptors of ExContentDescription object
    /// </summary>
    public class Descriptor
    {
        private const short UnicodeStringIndex = 0;
        private const short ByteArrayIndex = 1;
        private const short BoolIndex = 2;
        private const short Int32Index = 3;
        private const short Int64Index = 4;
        private const short Int16Index = 5;

        private string _Name;
        private object _Value;
        private string _ExceptionMessage;
        private static Type[] _ValidTypes = { typeof(string), typeof(byte[]), typeof(bool), 
            typeof(Int32), typeof(Int64), typeof(Int16) };

        /// <summary>
        /// Read new Descriptor from specific TagStream
        /// </summary>
        /// <param name="rd"></param>
        public Descriptor(TagStream rd)
        {
            Read(rd);
        }

        /// <summary>
        /// Create new descriptor from specific values
        /// </summary>
        /// <param name="Name">Name of descriptor</param>
        /// <param name="Value">Value of descriptor</param>
        public Descriptor(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        /// <summary>
        /// Read Descriptor data from specific TagStream
        /// </summary>
        /// <param name="rd">TagStream to read data from</param>
        public void Read(TagStream rd)
        {
            int Length;
            Length = rd.AsBinaryReader.ReadInt16(); // Length of name
            Name = rd.ReadText(Length, Tags.ID3.TextEncodings.UTF_16, false); // reading name
            short DType = (short)rd.AsBinaryReader.ReadInt16(); // Data type

            Length = rd.AsBinaryReader.ReadInt16(); // Value Length

            if (DType >= _ValidTypes.Length)
            {
                _ExceptionMessage = "Unknown Datatype found, read this descriptor as ByteArray";
                DType = ByteArrayIndex; // Byte Array
            }

            switch (DType)
            {
                case UnicodeStringIndex: // string
                    Value = rd.ReadText(Length, Tags.ID3.TextEncodings.UTF_16);
                    break;
                case ByteArrayIndex: // Byte Array
                    byte[] Buffer = new byte[Length];
                    rd.Read(Buffer, 0, Length);
                    Value = Buffer;
                    break;
                case Int16Index: // Int16
                    Value = rd.AsBinaryReader.ReadInt16();
                    break;
                case Int32Index: // Int32
                    Value = rd.AsBinaryReader.ReadInt32();
                    break;
                case Int64Index: // Int64
                    Value = rd.AsBinaryReader.ReadInt64();
                    break;
                case BoolIndex: // bool
                    Value = (rd.AsBinaryReader.ReadInt32() == 0) ? false : true;
                    break;
            }
        }

        /// <summary>
        /// Write current descriptor to specific TagStream
        /// </summary>
        /// <param name="wr">TagStream to write data to</param>
        public void Write(TagStream wr)
        {
            wr.WriteText(Name, 2);
            short Index = GetTypeIndex;
            wr.AsBinaryWriter.Write((Int16)Index);
            wr.AsBinaryWriter.Write((Int16)ValueLength);

            byte[] Buffer;
            switch (Index)
            {
                case UnicodeStringIndex: // String
                    wr.WriteText((string)Value, Tags.ID3.TextEncodings.UTF_16, true);
                    break;
                case BoolIndex: // Bool
                    int Buff = (bool)Value == true ? 1 : 0;
                    wr.AsBinaryWriter.Write(Buff);
                    break;
                case ByteArrayIndex: // byte array
                    Buffer = (byte[])Value;
                    wr.Write(Buffer, 0, Buffer.Length);
                    break;
                case Int32Index: // Int32
                    wr.AsBinaryWriter.Write((int)Value);
                    break;
                case Int64Index: // Int64
                    wr.AsBinaryWriter.Write((Int64)Value);
                    break;
                case Int16Index: // Int16
                    wr.AsBinaryWriter.Write((Int16)Value);
                    break;
            }
        }

        /// <summary>
        /// Get or set name of current descriptor
        /// </summary>
        public string Name
        {
            get
            { return _Name; }
            set
            {
                if (value == string.Empty)
                    throw new ArgumentException("Name must be at least one character");
                _Name = value;
            }
        }

        /// <summary>
        /// Get or set DataType of current descriptor
        /// </summary>
        public Type DataType
        {
            get
            { return Value.GetType(); }
        }

        private short GetTypeIndex
        {
            get
            {
                for (short i = 0; i < _ValidTypes.Length; i++)
                    if (_ValidTypes[i] == DataType)
                        return i;
                return -1;
            }
        }

        /// <summary>
        /// Get or set value of current descriptor
        /// </summary>
        public object Value
        {
            get
            { return _Value; }
            set
            {
                if (!IsValidType(value.GetType()))
                    throw new ArgumentOutOfRangeException(value.GetType().ToString() +
                        " is not valid type for descriptor value");

                _Value = value;
            }
        }

        private static bool IsValidType(Type T)
        {
            foreach (Type t in _ValidTypes)
                if (t == T)
                    return true;

            return false;
        }

        /// <summary>
        /// Get Exception message of current descriptor
        /// </summary>
        public string ExceptionMessage
        {
            get
            { return _ExceptionMessage; }
        }

        private int ValueLength
        {
            get
            {
                switch (GetTypeIndex)
                {
                    case BoolIndex:
                    case Int32Index:
                        return 4;
                    case Int16Index:
                        return 2;
                    case Int64Index:
                        return 8;
                    case UnicodeStringIndex:
                        return ((string)Value).Length * 2 + 2; // 2 is seprator length
                    case ByteArrayIndex:
                        return ((byte[])Value).Length;
                    default:
                        return -1;
                }
            }
        }

        /// <summary>
        /// Get Length of current descriptor
        /// </summary>
        public int Length
        {
            get
            { return 6 + TagStream.StringLength(Name) + ValueLength; }
        }
    }
}
