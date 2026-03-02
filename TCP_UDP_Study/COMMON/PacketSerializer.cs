using System;
using System.Runtime.InteropServices;

namespace Common.Network
{
    public static class PacketSerializer
    {
        public static byte[] StructureToByteArray<T>(T obj) where T : struct
        {
            int size = Marshal.SizeOf(obj);
            byte[] array = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(obj, ptr, true);
                Marshal.Copy(ptr, array, 0, size);
            }
            finally { Marshal.FreeHGlobal(ptr); }
            return array;
        }

        public static T ByteArrayToStructure<T>(byte[] byteArray) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(byteArray, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }
    }
}