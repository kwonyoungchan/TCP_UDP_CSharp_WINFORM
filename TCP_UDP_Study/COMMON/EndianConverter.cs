using System;
using System.Net;

namespace Common.Network
{
    public static class EndianConverter
    {
        public static short Swap(short value) => IPAddress.HostToNetworkOrder(value);
        public static int Swap(int value) => IPAddress.HostToNetworkOrder(value);

        // float(4바이트 실수) 엔디안 변환
        public static float Swap(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }

        public static PacketHeader SwapHeader(PacketHeader header)
        {
            return new PacketHeader
            {
                SrcID = Swap(header.SrcID),
                DstID = Swap(header.DstID),
                MsgID = Swap(header.MsgID)
            };
        }

        public static FragmentPacket SwapStruct(FragmentPacket packet)
        {
            return new FragmentPacket
            {
                Header = SwapHeader(packet.Header),
                SequenceNumber = Swap(packet.SequenceNumber),
                PayloadSize = Swap(packet.PayloadSize),
                Data = packet.Data
            };
        }

        public static FlightDataPacket SwapStruct(FlightDataPacket packet)
        {
            return new FlightDataPacket
            {
                Header = SwapHeader(packet.Header),
                Pitch = Swap(packet.Pitch),
                Roll = Swap(packet.Roll),
                Yaw = Swap(packet.Yaw),
                MainRotorRpm = Swap(packet.MainRotorRpm),
                TailRotorRpm = Swap(packet.TailRotorRpm)
            };
        }
    }
}