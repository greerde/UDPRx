using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReaderAsync(15000).Wait();
        }

        private static async Task ReaderAsync (int port)
        {
            Console.WriteLine("Starting...");
            using (var client = new UdpClient(port))
            {
                do
                {
                    UdpReceiveResult r = await client.ReceiveAsync();

                    PDW p = Desserialize(r.Buffer);
                    Console.WriteLine("freq = " + p.startFrequencyMHz);
                }
                while (true);
            }
        }


      public static PDW Desserialize(byte[] data) {
      PDW result = new PDW();
      using (MemoryStream m = new MemoryStream(data)) {
         using (BinaryReader reader = new BinaryReader(m)) {
            		result.counter = reader.ReadInt32();
			result.receiverID = reader.ReadInt32();
			result.toa = reader.ReadInt64();
			result.startFrequencyMHz = reader.ReadSingle();
			result.stopFrequencyMHz = reader.ReadSingle();
			result.pulseWidth = reader.ReadDouble();
			result.snr = reader.ReadSingle();
			result.phase = reader.ReadSingle();
			result.phaseReferenceTime = reader.ReadSingle();
			result.errorFlags = reader.ReadInt32();
			result.modulationFlags = reader.ReadInt32();
			result.modulationParameters = reader.ReadSingle();
			result.angleOfArrival = reader.ReadSingle();
			result.riseTime = reader.ReadSingle();
			result.fallTime = reader.ReadSingle();
			result.pri = reader.ReadDouble();
         }
      }
      return result;
   }
}


   	public class PDW
	{
		public int counter = 0; // rolling counter 
		public int receiverID = 1; //!< ID of the receiver that generated the PDW
		
		public long toa; // time in ns

		public float startFrequencyMHz = 1000; //!< Start frequency in MHz (TOA)
		public float stopFrequencyMHz = 1000; //!< Stop frequency in MHz (TOA + PW)
		public double pulseWidth = 0.0005; //!< Pulse width in seconds
		public float pulseAmplitude = -5.4F; //! Pulse amplitude in dBm (verify not dBW)
		public float snr = 10; //!< Signal to noise ratio in dB
		public float phase = 0; //!< Phase in radians
		public float phaseReferenceTime = 0; //!< Offset from TOA of phase measurement in seconds
		public int errorFlags = 0;  // placeholder for error flags
		public int modulationFlags = 0;
		public float modulationParameters = 0; //!< in Hz or Hz/s as defined by EMD platinum
		public float angleOfArrival = 0; //!< in radians
		public float riseTime = 0; //!< Rise time in seconds
		public float fallTime = 0; //!< Rise time in seconds
		public double pri = 0; //!< Pulse repetition interval in seconds
	
	
	}
}
