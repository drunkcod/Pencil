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

        public int ReadInt16()
        {
            Int16 low = data[position];
            Int16 hi = data[position + 1];
            hi <<= 8;
            position += 2;
            return hi | low;
        }

        public Int32 ReadInt32()
        {
            CheckBounds(4);
            var low = ReadInt16();
            int hi = ReadInt16();
            hi <<= 16;
            return hi | low;
        }

        public Int64 ReadInt64()
        {
            Int32 low = ReadInt32();
            Int64 hi = ReadInt32();
            hi <<= 32;
            return hi | (UInt32)low;
        }

        public float ReadFloat32()
        {
            int start = position;
            position += 4;
            return BitConverter.ToSingle(data, start);
        }

        public double ReadFloat64()
        {
            int start = position;
            position += 4;
            return BitConverter.ToDouble(data, start);
        }

        private void CheckBounds(int size)
        {
            if(position + size > data.Length)
                throw new InvalidOperationException(string.Format("Not enough data to read {0} bytes.", size));
        }
        
    }
}
