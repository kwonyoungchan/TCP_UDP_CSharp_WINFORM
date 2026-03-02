using Common.Network;

namespace UdpCommunicationExample
{
    public class DataManager
    {
        private static readonly DataManager _instance = new DataManager();
        public static DataManager Instance => _instance;

        private FragmentPacket _latestFragment;
        private FlightDataPacket _latestFlight;
        private readonly object _lock = new object();

        private DataManager() { }

        public FragmentPacket LatestFragment
        {
            get { lock (_lock) { return _latestFragment; } }
            set { lock (_lock) { _latestFragment = value; } }
        }

        public FlightDataPacket LatestFlight
        {
            get { lock (_lock) { return _latestFlight; } }
            set { lock (_lock) { _latestFlight = value; } }
        }
    }
}