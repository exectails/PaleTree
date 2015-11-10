using System;
using System.Text;

namespace PaleTree.Shared
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
		/// Packet op's table size
		/// </summary>
		/// <remarks>
		/// -1 if loaded from an outdated log file.
		/// </remarks>
		public int OpSize { get; private set; }

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
		/// <param name="opSize"></param>
		/// <param name="packet"></param>
		/// <param name="time"></param>
		/// <param name="received"></param>
		public PalePacket(string opName, int opSize, Packet packet, DateTime time, bool received)
		{
			OpName = opName;
			OpSize = opSize;
			Packet = packet;
			Time = time;
			Received = received;
		}

		/// <summary>
		/// Returns info about packet as string.
		/// </summary>
		/// <returns></returns>
		public string GetPacketInfo()
		{
			var sb = new StringBuilder();

			var length = this.Packet.Length;
			var opName = this.OpName;
			var opSize = this.OpSize;

			sb.AppendFormat("Op: {0:X4} {1}", this.Op, opName);
			sb.AppendFormat(", Size: {0}", length);
			if (opSize == -1)
				sb.AppendFormat(" (Table: ?)");
			else if (opSize > 0)
				sb.AppendFormat(" (Table: {0}, Garbage: {1})", opSize, length - opSize);

			return sb.ToString();
		}
	}
}
