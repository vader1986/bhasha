namespace Bhasha.Common.Extensions
{
    public static class ByteArrayExtensions
    {
        public static void Increment(this byte[] bytes, int index, byte max = byte.MaxValue)
        {
            if (bytes[index] != max)
            {
                bytes[index]++;
            }
        }
    }
}
