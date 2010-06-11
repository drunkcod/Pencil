namespace Pencil.Core
{
    using System;

    public class ByteConverter
    {
        byte[] data;
        int position;

        public ByteConverter(byte[] data, int position)
        {
            this.data = data;
            this.position = position;
        }

		public int Position { get { return position; } }

        public bool HasData { get { return Position < data.Length; } }

        public byte ReadByte()
        {
            var value = data[position];
            ++position;
            return value;
        }

        public int ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public int ReadInt16()
        {
			return Combine(ReadByte(), ReadByte(), 8);
        }

        public Int32 ReadInt32()
        {
            CheckBounds(4);
			return Combine(ReadInt16(), ReadInt16(), 16);
        }

		static int Combine(int low, int high, int size)
		{
			return (high << size) | low;
		}

        public Int64 ReadInt64()
        {
            Int32 low = ReadInt32();
            Int64 hi = ReadInt32();
            hi <<= 32;
            return hi | (UInt32)low;
        }

        public float ReadSingle()
        {
            int start = position;
            position += 4;
            return BitConverter.ToSingle(data, start);
        }

        public double ReadDouble()
        {
            int start = position;
            position += 8;
            return BitConverter.ToDouble(data, start);
        }

        private void CheckBounds(int size)
        {
            if(position + size > data.Length)
                throw new InvalidOperationException(string.Format("Not enough data to read {0} bytes.", size));
        }

    }
}
