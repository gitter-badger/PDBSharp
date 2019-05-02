#region License
/*
 * Copyright (C) 2018 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Smx.PDBSharp.Leaves
{
	[LeafReader(LeafType.LF_PROCEDURE)]
	public class LF_PROCEDURE : TypeDataReader
	{
		public readonly UInt32 ReturnValueTypeIndex;
		public readonly CallingConvention CallingConvention;
		public readonly UInt16 NumberOfParameters;
		public readonly UInt32 ArgumentListTypeIndex;

		public LF_PROCEDURE(Stream stream) : base(stream) {
			ReturnValueTypeIndex = ReadUInt32();
			CallingConvention = ReadEnum<CallingConvention>();
			ReadByte(); //reserved
			NumberOfParameters = ReadUInt16();
			ArgumentListTypeIndex = ReadUInt32();
		}
	}
}
