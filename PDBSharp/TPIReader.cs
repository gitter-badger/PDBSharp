#region License
/*
 * Copyright (C) 2019 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using Smx.PDBSharp.Leaves;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices;

namespace Smx.PDBSharp
{
	public struct TPISlice
	{
		public UInt32 Offset;
		public UInt32 Size;
	}

	public struct TPIHash
	{
		public Int16 StreamNumber;
		public UInt16 AuxHashStreamNumber;

		public UInt32 HashKeySize;
		public UInt32 NumHashBuckets;

		/// <summary>
		/// offset,size pairs of hash values
		/// </summary>
		public TPISlice HashValues;
		/// <summary>
		/// offset,size pairs of type indices
		/// </summary>
		public TPISlice TypeOffsets;
		/// <summary>
		/// offset,size pairs of hash head list
		/// </summary>
		public TPISlice HashHeadList;
	}

	public struct TPIHeader
	{
		public TPIVersion Version;
		public UInt32 HeaderSize;

		public UInt32 MinTypeIndex;
		public UInt32 MaxTypeIndex;

		public UInt32 GpRecSize;
		public TPIHash Hash;
	}

	public enum TPIVersion : UInt32
	{
		V40 = 19950410,
		V41 = 19951122,
		V50Beta = 19960307,
		V50 = 19961031,
		V70 = 19990903,
		V80 = 20040203
	}

	public delegate void OnLeafDataDelegate(byte[] data);

	public class TPIReader : ReaderBase
	{
		public readonly TPIHeader Header;

		private readonly IServiceContainer ctx;

		public event OnLeafDataDelegate OnLeafData;

		public Lazy<IEnumerable<ILeafContainer>> lazyLeafContainers;

		public IEnumerable<ILeafContainer> Types => lazyLeafContainers.Value;

		public uint GetLeafSize(UInt32 offset) {
			return PerformAt(Header.HeaderSize + offset, () => {
				return (uint)ReadUInt16() + sizeof(UInt16);
			});
		}

		public bool HasTi(UInt32 TypeIndex) {
			return TypeIndex >= Header.MinTypeIndex && TypeIndex < Header.MaxTypeIndex;
		}

		public bool IsBuiltinTi(UInt32 TypeIndex) {
			return TypeIndex <= ((uint)SpecialTypeMode.NearPointer128 | 0xFF);
		}

		public TPIReader(IServiceContainer ctx, Stream stream) : base(stream) {
			this.ctx = ctx;

			Header = ReadStruct<TPIHeader>();
			if (Header.HeaderSize != Marshal.SizeOf<TPIHeader>()) {
				throw new InvalidDataException();
			}

			if (!Enum.IsDefined(typeof(TPIVersion), Header.Version)) {
				throw new InvalidDataException();
			}


#if false
			if(Header.Version != TPIVersion.V80) {
				throw new NotImplementedException($"TPI Version {Header.Version} not supported yet");
			}
#endif

			lazyLeafContainers = new Lazy<IEnumerable<ILeafContainer>>(ReadTypes);
		}

		public ILeafContainer ReadType(uint typeOffset) {
			return PerformAt(typeOffset, () => ReadType());
		}

		private ILeafContainer ReadType() {
			UInt16 length = ReadUInt16();
			if (length == 0) {
				return null;
			}

			int dataSize = length + sizeof(UInt16);
			byte[] leafDataBuf = new byte[dataSize];

			{
				UInt32 leafHash = HasherV2.HashBufferV8(leafDataBuf, 0xFFFFFFFF);
				UInt32 hash = HasherV2.HashData(leafDataBuf, Header.Hash.NumHashBuckets);
			}

			MemoryStream stream = new MemoryStream(leafDataBuf);
			BinaryWriter wr = new BinaryWriter(stream);
			wr.Write(length);
			wr.Write(ReadBytes(length));

			OnLeafData?.Invoke(leafDataBuf);

			stream.Position = 0;
			TypeDataReader rdr = new TypeDataReader(ctx, stream);
			return rdr.ReadTypeLazy();
		}

		private IEnumerable<ILeafContainer> ReadTypes() {
			long savedPos = Stream.Position;
			long processed = 0;
			while (processed < Header.GpRecSize) {
				yield return ReadType();
				processed += Stream.Position - savedPos;
				savedPos = Stream.Position;
			}
		}
	}
}
