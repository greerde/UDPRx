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
            using (var client = new UdpClient(port))
            {
                do
                {
                    Console.WriteLine("Starting...");
                    UdpReceiveResult r = await client.ReceiveAsync();
                    byte [] dg = r.Buffer;
                    //string rx = Encoding.UTF8.GetString(dg);

                    PDW p = new PDW();
                     p = Desserialize(r.Buffer);


                    Console.WriteLine("freq = " + p.startFrequencyMHz);

                }
                while (true);
            }
        }


        public static PDW Desserialize(byte[] data) {
      PDW result = new PDW();
      using (MemoryStream m = new MemoryStream(data)) {
         using (BinaryReader reader = new BinaryReader(m)) {
            result.Header = reader.ReadInt32();
			result.dt = DateTime.FromBinary(reader.ReadInt64());
			result.dtNanoSec = reader.ReadInt64();
			result.startFrequencyMHz = reader.ReadSingle();
			result.stopFrequencyMHz = reader.ReadSingle();
			result.receiverID = reader.ReadInt32();
			result.pulseWidth = reader.ReadDouble();
			result.snr = reader.ReadSingle();
			result.phase = reader.ReadSingle();
			result.phaseReferenceTime = reader.ReadSingle();
			result.errorFlags = reader.ReadInt16();
			result.modulationFlags = reader.ReadInt16();
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
		public int Header = 0; // using 32 bit int as header placeholder
						//REPLACED: HeaderFlags recordHeader; //!< Record header

		public DateTime dt;
		public long dtNanoSec;
		//REPLACED: HighResolutionTime toa; //!< UTC seconds with picosecond precision

		public float startFrequencyMHz = 0; //!< Start frequency in MHz (TOA)
		public float stopFrequencyMHz = 0; //!< Stop frequency in MHz (TOA + PW)
		public int receiverID = 0; //!< ID of the receiver that generated the PDW
		public double pulseWidth = 0; //!< Pulse width in seconds
		public float pulseAmplitude = 0; //! Pulse amplitude in dBm (verify not dBW)
		public float snr = 0; //!< Signal to noise ratio in dB
		public float phase = 0; //!< Phase in radians
		public float phaseReferenceTime = 0; //!< Offset from TOA of phase measurement in seconds
		public short errorFlags = 0;  // placeholder for error flags
							   // REPLACED: ErrorFlags errorFlags; //!< Error conditions that occured during generation of this PDW
		public short modulationFlags = 0;
		//REPLACED: ModulationFlags modulationFlags; //!< Modulation types detected during generation of this PDW
		public float modulationParameters = 0; //!< in Hz or Hz/s as defined by EMD platinum
		public float angleOfArrival = 0; //!< in radians
		public float riseTime = 0; //!< Rise time in seconds
		public float fallTime = 0; //!< Rise time in seconds
		public double pri = 0; //!< Pulse repetition interval in seconds
	


	public byte[] Serialize()
	{
		using (MemoryStream m = new MemoryStream())
		{
			using (BinaryWriter writer = new BinaryWriter(m))
			{
				writer.Write(Header);
				writer.Write(dt.ToBinary());
				writer.Write(dtNanoSec);
				writer.Write(startFrequencyMHz);
				writer.Write(stopFrequencyMHz);
				writer.Write(receiverID);
				writer.Write(pulseWidth);
				writer.Write(pulseAmplitude);
				writer.Write(snr);
				writer.Write(phase);
				writer.Write(phaseReferenceTime);
				writer.Write(errorFlags);
				writer.Write(modulationFlags);
				writer.Write(modulationParameters);
				writer.Write(angleOfArrival);
				writer.Write(riseTime);
				writer.Write(fallTime);
				writer.Write(pri);

			}
			return m.ToArray();
		}
	}
}
	


}
