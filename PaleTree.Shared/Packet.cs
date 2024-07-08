﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PaleTree.Shared
{
	public enum PacketType
	{
		ClientServer,
		ServerClient,
	}

	public class Packet
	{
		private const int DefaultSize = 1024;

		protected byte[] _buffer;
		protected int _ptr;
		protected Packet _zlibPacket;
		protected int _bodyStart;

		/// <summary>
		/// Length of the packet.
		/// </summary>
		public int Length { get; protected set; }

		/// <summary>
		/// Packet's op.
		/// </summary>
		public int Op { get; protected set; }

		/// <summary>
		/// Packet's type.
		/// </summary>
		public PacketType Type { get; protected set; }

		/// <summary>
		/// Creates packet from buffer coming from client.
		/// </summary>
		/// <param name="buffer"></param>
		public Packet(byte[] buffer, PacketType type)
		{
			_buffer = buffer;
			this.Type = type;

			this.Length = buffer.Length;
			this.Op = this.GetShort();

			this.GetInt(); // sequence

			// The checksum was originally client-server only, but it's now also
			// used in server-client packets. For compatibility with empty packets
			// in older logs, we'll check the buffer length, as we get an exception
			// otherwise.
			//if (type == PacketType.ClientServer)
			if (buffer.Length >= 10)
				this.GetInt(); // checksum

			_bodyStart = _ptr;
		}

		/// <summary>
		/// Returns copy of buffer.
		/// </summary>
		public byte[] GetBuffer()
		{
			return _buffer.ToArray();
		}

		/// <summary>
		/// Sets pointer back to start of data.
		/// </summary>
		public void Rewind()
		{
			_ptr = _bodyStart;
		}

		/// <summary>
		/// Throws if not enough bytes are left to read a value with the given length.
		/// </summary>
		/// <param name="needed"></param>
		/// <returns></returns>
		private void AssertGotEnough(int needed)
		{
			if (_ptr + needed > this.Length)
				throw new InvalidOperationException("Not enough bytes left to read a '" + needed + "' byte value.");
		}

		/// <summary>
		/// Increases buffer size if more space is needed to fit the given
		/// amount of bytes.
		/// </summary>
		/// <param name="needed"></param>
		private void EnsureSpace(int needed)
		{
			if (_ptr + needed > this.Length)
				Array.Resize(ref _buffer, _buffer.Length + DefaultSize);
		}

		/// <summary>
		/// Reads byte from buffer.
		/// </summary>
		/// <returns></returns>
		public byte GetByte()
		{
			this.AssertGotEnough(1);

			var val = _buffer[_ptr];
			_ptr += sizeof(byte);

			return val;
		}

		/// <summary>
		/// Reads short from buffer.
		/// </summary>
		/// <returns></returns>
		public short GetShort()
		{
			this.AssertGotEnough(2);

			var val = BitConverter.ToInt16(_buffer, _ptr);
			_ptr += sizeof(short);

			return val;
		}

		/// <summary>
		/// Reads int from buffer.
		/// </summary>
		/// <returns></returns>
		public int GetInt()
		{
			this.AssertGotEnough(4);

			var val = BitConverter.ToInt32(_buffer, _ptr);
			_ptr += sizeof(int);

			return val;
		}

		/// <summary>
		/// Reads long from buffer.
		/// </summary>
		/// <returns></returns>
		public long GetLong()
		{
			this.AssertGotEnough(8);

			var val = BitConverter.ToInt64(_buffer, _ptr);
			_ptr += sizeof(long);

			return val;
		}

		/// <summary>
		/// Reads float from buffer.
		/// </summary>
		/// <returns></returns>
		public float GetFloat()
		{
			this.AssertGotEnough(4);

			var val = BitConverter.ToSingle(_buffer, _ptr);
			_ptr += sizeof(float);

			return val;
		}

		/// <summary>
		/// Reads given amount of bytes from buffer and returns them as UTF8 string.
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public string GetString(int length)
		{
			this.AssertGotEnough(length);

			var val = Encoding.UTF8.GetString(_buffer, _ptr, length);

			// Relatively fast way to get rid of null bytes.
			var nullIndex = val.IndexOf((char)0);
			if (nullIndex != -1)
				val = val.Substring(0, nullIndex);

			_ptr += length;

			return val;
		}

		/// <summary>
		/// Reads null-terminated string from buffer and returns it as UTF8.
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public string GetString()
		{
			for (int i = _ptr; i < _buffer.Length; ++i)
			{
				if (_buffer[i] == 0)
				{
					var val = Encoding.UTF8.GetString(_buffer, _ptr, i - _ptr);
					_ptr += val.Length + 1;
					return val;
				}
			}

			throw new Exception("String not null-terminated.");
		}

		/// <summary>
		/// Reads struct from buffer.
		/// </summary>
		/// <typeparam name="TStruct"></typeparam>
		/// <returns></returns>
		public TStruct GetStruct<TStruct>() where TStruct : new()
		{
			var type = typeof(TStruct);
			if (!type.IsValueType || type.IsPrimitive)
				throw new Exception("GetObj can only marshal to structs.");

			var size = Marshal.SizeOf(typeof(TStruct));
			var buffer = this.GetBin(size);

			IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, intPtr, buffer.Length);
			var val = Marshal.PtrToStructure(intPtr, typeof(TStruct));
			Marshal.FreeHGlobal(intPtr);

			return (TStruct)val;
		}

		/// <summary>
		/// Reads given amount of bytes from buffer.
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public byte[] GetBin(int length)
		{
			this.AssertGotEnough(length);

			var val = new byte[length];
			Buffer.BlockCopy(_buffer, _ptr, val, 0, length);
			_ptr += length;

			return val;
		}

		/// <summary>
		/// Reads given amount of bytes from buffer and returns them as hex string.
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public string GetBinAsHex(int length)
		{
			return BitConverter.ToString(this.GetBin(length)).ToString().Replace("-", "").ToUpper();
		}

		/// <summary>
		/// Writes byte to buffer.
		/// </summary>
		/// <param name="val"></param>
		public void PutByte(byte val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutByte(val);
				return;
			}

			this.EnsureSpace(1);

			_buffer[_ptr++] = (byte)(val);
			this.Length += sizeof(byte);
		}

		/// <summary>
		/// Writes value as byte (0|1) to buffer.
		/// </summary>
		/// <param name="val"></param>
		public void PutByte(bool val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutByte(val);
				return;
			}

			this.PutByte(val ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Writes short to buffer.
		/// </summary>
		/// <param name="val"></param>
		public void PutShort(int val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutShort(val);
				return;
			}

			this.EnsureSpace(2);

			_buffer[_ptr++] = (byte)(val >> (8 * 0));
			_buffer[_ptr++] = (byte)(val >> (8 * 1));
			this.Length += sizeof(short);
		}

		/// <summary>
		/// Writes int to buffer.
		/// </summary>
		/// <param name="val"></param>
		public void PutInt(int val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutInt(val);
				return;
			}

			this.EnsureSpace(4);

			_buffer[_ptr++] = (byte)(val >> (8 * 0));
			_buffer[_ptr++] = (byte)(val >> (8 * 1));
			_buffer[_ptr++] = (byte)(val >> (8 * 2));
			_buffer[_ptr++] = (byte)(val >> (8 * 3));
			this.Length += sizeof(int);
		}

		public void PutLong(long val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutLong(val);
				return;
			}

			this.EnsureSpace(8);

			_buffer[_ptr++] = (byte)(val >> (8 * 0));
			_buffer[_ptr++] = (byte)(val >> (8 * 1));
			_buffer[_ptr++] = (byte)(val >> (8 * 2));
			_buffer[_ptr++] = (byte)(val >> (8 * 3));
			_buffer[_ptr++] = (byte)(val >> (8 * 4));
			_buffer[_ptr++] = (byte)(val >> (8 * 5));
			_buffer[_ptr++] = (byte)(val >> (8 * 6));
			_buffer[_ptr++] = (byte)(val >> (8 * 7));
			this.Length += sizeof(long);
		}

		/// <summary>
		/// Writes float to buffer.
		/// </summary>
		/// <param name="val"></param>
		public void PutFloat(float val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutFloat(val);
				return;
			}

			this.EnsureSpace(4);

			var bVal = BitConverter.GetBytes(val);
			_buffer[_ptr++] = bVal[0];
			_buffer[_ptr++] = bVal[1];
			_buffer[_ptr++] = bVal[2];
			_buffer[_ptr++] = bVal[3];
			this.Length += sizeof(float);
		}

		/// <summary>
		/// Writes string of given length to buffer.
		/// </summary>
		/// <remarks>
		/// If length is greater than string's length it's filled with null bytes.
		/// </remarks>
		/// <param name="val"></param>
		/// <param name="length"></param>
		public void PutString(string val, int length)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutString(val, length);
				return;
			}

			this.EnsureSpace(length);

			if (val == null)
				val = "";

			var bytes = Encoding.UTF8.GetBytes(val);
			Buffer.BlockCopy(bytes, 0, _buffer, _ptr, Math.Min(bytes.Length, length));
			_ptr += length;
			this.Length += length;
		}

		/// <summary>
		/// Writes null-terminated string to buffer.
		/// </summary>
		/// <param name="val"></param>
		public void PutString(string val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutString(val);
				return;
			}

			if (val == null)
				val = "";

			// Append terminator
			if (val == "" || (val.Length > 0 && val[val.Length - 1] != '\0'))
				val += '\0';

			this.PutBin(Encoding.UTF8.GetBytes(val));
		}

		/// <summary>
		/// Writes string to buffer, without null terminator, but prefixed
		/// with a short describing its length.
		/// </summary>
		/// <param name="val"></param>
		public void PutLpString(string val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutLpString(val);
				return;
			}

			if (val == null)
				val = "";

			// Append terminator
			if (val == "" || (val.Length > 0 && val[val.Length - 1] != '\0'))
				val += '\0';

			var bytes = Encoding.UTF8.GetBytes(val);
			this.PutShort(bytes.Length);
			this.PutBin(bytes);
		}

		/// <summary>
		/// Writes bytes to buffer.
		/// </summary>
		/// <param name="val"></param>
		public void PutBin(params byte[] val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutBin(val);
				return;
			}

			this.EnsureSpace(val.Length);

			Buffer.BlockCopy(val, 0, _buffer, _ptr, val.Length);
			_ptr += val.Length;
			this.Length += val.Length;
		}

		/// <summary>
		/// Writes bytes parsed from given hex string to buffer.
		/// </summary>
		/// <param name="hex"></param>
		public void PutBinFromHex(string hex)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutBinFromHex(hex);
				return;
			}

			if (hex == null)
				throw new ArgumentNullException("hex");

			hex = hex.Trim().Replace(" ", "").Replace("-", "");

			if (hex == "")
				return;

			var val =
				Enumerable.Range(0, hex.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
				.ToArray();

			this.PutBin(val);
		}

		/// <summary>
		/// Writes the given amount of bytes to the buffer.
		/// </summary>
		/// <param name="amount"></param>
		public void PutEmptyBin(int amount)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutEmptyBin(amount);
				return;
			}

			if (amount <= 0)
				return;

			this.PutBin(new byte[amount]);
		}

		/// <summary>
		/// Writes struct to buffer.
		/// </summary>
		/// <param name="val"></param>
		public void PutBin(object val)
		{
			if (_zlibPacket != null)
			{
				_zlibPacket.PutBin(val);
				return;
			}

			var type = val.GetType();
			if (!type.IsValueType || type.IsPrimitive)
				throw new Exception("PutBin only takes byte[] and structs.");

			var size = Marshal.SizeOf(val);
			var arr = new byte[size];
			var ptr = Marshal.AllocHGlobal(size);

			Marshal.StructureToPtr(val, ptr, true);
			Marshal.Copy(ptr, arr, 0, size);
			Marshal.FreeHGlobal(ptr);

			this.PutBin(arr);
		}

		/// <summary>
		/// Copies packet data to given buffer at offset.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public byte[] Build(ref byte[] buffer, int offset)
		{
			Buffer.BlockCopy(_buffer, 0, buffer, offset, this.Length);

			return _buffer;
		}
	}
}
