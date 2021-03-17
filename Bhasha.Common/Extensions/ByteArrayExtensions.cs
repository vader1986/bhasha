namespace Bhasha.Common.Extensions
{
    public static class ByteArrayExtensions
    {
        public static void Inc(this byte[] bytes, int index, byte max = byte.MaxValue)
        {
            if (bytes[index] != max)
            {
                bytes[index]++;
            }
        }
    }
}
