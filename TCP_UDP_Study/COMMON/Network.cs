using System;
using System.Runtime.InteropServices;

namespace Common.Network
{
    // ==========================================
    // 1. 통신용 데이터 구조체 (Interface 역할)
    // ==========================================

    /// <summary>
    /// 8바이트 단위로 분할(Fragment)되거나 전송될 수 있는 패킷 구조체
    /// C++ 서버/클라이언트와의 통신을 위해 Pack = 8로 메모리 정렬을 강제합니다.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct FragmentPacket
    {
        public byte FragmentId;          // 1바이트: 프래그먼트 고유 ID 또는 타입
        public short SequenceNumber;     // 2바이트: 순서 번호
        public int PayloadSize;          // 4바이트: 실제 데이터 크기
        public double Timestamp;         // 8바이트: 타임스탬프 (또는 long)

        // 10바이트 이상의 배열 (여기서는 16바이트로 설정)
        // C# 구조체 내에 고정 크기 배열을 넣기 위해 MarshalAs 속성을 사용합니다.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Data;              // 16바이트: 실제 데이터 파편
    }

    // ==========================================
    // 2. 엔디안(Endian) 변환 유틸리티 클래스
    // ==========================================

    /// <summary>
    /// 네트워크 바이트 오더(Big-Endian)와 호스트 바이트 오더(Little-Endian) 간의 변환을 담당합니다.
    /// </summary>
    public static class EndianConverter
    {
        // 네트워크 통신은 기본적으로 Big-Endian을 사용하고, 윈도우(x86)는 Little-Endian을 사용하므로
        // 전송 전 / 수신 후 항상 이 변환을 거쳐야 데이터가 깨지지 않습니다.

        /// <summary> 2바이트 (short/ushort) 엔디안 변환 </summary>
        public static short Swap(short value)
        {
            return System.Net.IPAddress.HostToNetworkOrder(value);
        }

        public static ushort Swap(ushort value)
        {
            return (ushort)System.Net.IPAddress.HostToNetworkOrder((short)value);
        }

        /// <summary> 4바이트 (int/uint) 엔디안 변환 </summary>
        public static int Swap(int value)
        {
            return System.Net.IPAddress.HostToNetworkOrder(value);
        }

        public static uint Swap(uint value)
        {
            return (uint)System.Net.IPAddress.HostToNetworkOrder((int)value);
        }

        /// <summary> 8바이트 (long/ulong/double) 엔디안 변환 </summary>
        public static long Swap(long value)
        {
            return System.Net.IPAddress.HostToNetworkOrder(value);
        }

        public static ulong Swap(ulong value)
        {
            return (ulong)System.Net.IPAddress.HostToNetworkOrder((long)value);
        }

        // double 타입은 비트 컨버터로 long으로 바꾼 뒤 변환해야 합니다.
        public static double Swap(double value)
        {
            long bits = BitConverter.DoubleToInt64Bits(value);
            long swappedBits = Swap(bits);
            return BitConverter.Int64BitsToDouble(swappedBits);
        }

        /// <summary> 
        /// 전체 구조체 엔디안 변환 
        /// 구조체 내부의 다중 바이트 필드들을 한 번에 변환하여 새로운 구조체를 반환합니다.
        /// </summary>
        public static FragmentPacket SwapStruct(FragmentPacket packet)
        {
            return new FragmentPacket
            {
                FragmentId = packet.FragmentId, // 1바이트는 엔디안 변환이 필요 없음
                SequenceNumber = Swap(packet.SequenceNumber),
                PayloadSize = Swap(packet.PayloadSize),
                Timestamp = Swap(packet.Timestamp),

                // byte 배열은 그 자체가 개별 바이트의 연속이므로 순서를 뒤집지 않습니다.
                // 단, 배열 안에 또 다른 4바이트 int 값들이 들어있다면 별도의 파싱이 필요합니다.
                Data = packet.Data
            };
        }
    }
    public static class PacketSerializer
    {
        /// <summary>
        /// 구조체를 바이트 배열(byte[])로 변환합니다. (전송용)
        /// </summary>
        public static byte[] StructureToByteArray<T>(T obj) where T : struct
        {
            int size = Marshal.SizeOf(obj);
            byte[] array = new byte[size];
            //C#의 가비지 컬렉터(GC)가 건드릴 수 없는, 운영체제(OS) 소관의 날것의 메모리 공간(Unmanaged Memory)을
            //지정된 크기만큼 임대
            IntPtr ptr = Marshal.AllocHGlobal(size); // 비관리형 메모리 할당


            try
            {
                Marshal.StructureToPtr(obj, ptr, true); // 구조체 데이터를 메모리로 복사
                Marshal.Copy(ptr, array, 0, size);      // 메모리 데이터를 바이트 배열로 복사
            }
            finally
            {
                Marshal.FreeHGlobal(ptr); // 메모리 누수 방지를 위해 반드시 해제
            }

            return array;
        }

        /// <summary>
        /// 바이트 배열(byte[])을 구조체로 변환합니다. (수신용)
        /// </summary>
        public static T ByteArrayToStructure<T>(byte[] byteArray) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(byteArray, 0, ptr, size); // 바이트 배열을 메모리로 복사
                return (T)Marshal.PtrToStructure(ptr, typeof(T)); // 메모리 데이터를 구조체로 캐스팅
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}