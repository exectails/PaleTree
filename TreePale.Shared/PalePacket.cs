using System;

namespace TreePale.Shared
{
	/// <summary>
	/// Wrapper around Packet.
	/// </summary>
	public class PalePacket
	{
		private string cache;

		/// <summary>
		/// The underlying packet.
		/// </summary>
		public Packet Packet { get; private set; }

		/// <summary>
		/// Packet's op
		/// </summary>
		public int Op { get { return Packet.Op; } }

		/// <summary>
		/// Packet's op
		/// </summary>
		public string OpName { get; private set; }

		/// <summary>
		/// Time at which the packet was sent/received
		/// (MinValue if no data is available)
		/// </summary>
		public DateTime Time { get; private set; }

		/// <summary>
		/// True if packet was received instead of sent.
		/// </summary>
		public bool Received { get; private set; }

		/// <summary>
		/// Creates new PalePacket.
		/// </summary>
		/// <param name="opName"></param>
		/// <param name="packet"></param>
		/// <param name="time"></param>
		/// <param name="received"></param>
		public PalePacket(string opName, Packet packet, DateTime time, bool received)
		{
			OpName = opName;
			Packet = packet;
			Time = time;
			Received = received;
		}

		/// <summary>
		/// Creates new PalePacket, with date being MinValue (no data).
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="received"></param>
		public PalePacket(string opName, Packet packet, bool received)
			: this(opName, packet, DateTime.MinValue, received)
		{
		}

		/// <summary>
		/// Returns packet data as string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (cache != null)
				return cache;

			return (cache = Packet.ToString());
		}
	}
}
