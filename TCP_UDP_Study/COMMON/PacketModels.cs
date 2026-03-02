using System;
using System.Runtime.InteropServices;

namespace Common.Network
{
    // ==========================================
    // 1. 통신용 데이터 구조체 (Interface 역할)
    // ==========================================

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct PacketHeader
    {
        public short SrcID;
        public short DstID;
        public short MsgID;
    }

    /// <summary>
    /// 8바이트 단위로 분할(Fragment)되거나 전송될 수 있는 패킷 구조체
    /// C++ 서버/클라이언트와의 통신을 위해 Pack = 8로 메모리 정렬을 강제합니다.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct FragmentPacket
    {
        public PacketHeader Header;

        public byte FragmentId;          // 1바이트: 프래그먼트 고유 ID 또는 타입
        public short SequenceNumber;     // 2바이트: 순서 번호
        public int PayloadSize;          // 4바이트: 실제 데이터 크기
        public double Timestamp;         // 8바이트: 타임스탬프 (또는 long)

        // 10바이트 이상의 배열 (여기서는 16바이트로 설정)
        // C# 구조체 내에 고정 크기 배열을 넣기 위해 MarshalAs 속성을 사용합니다.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Data;              // 16바이트: 실제 데이터 파편
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct FlightDataPacket
    {
        public PacketHeader Header;
        public float Pitch;
        public float Roll;
        public float Yaw;
        public int MainRotorRpm;
        public int TailRotorRpm;
    }
}