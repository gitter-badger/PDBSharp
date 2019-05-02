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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smx.PDBSharp.Symbols.Structures
{
	public class CV_LVAR_ADDR_RANGE : ReaderBase
	{
		public readonly UInt32 OffsetStart;
		public readonly UInt16 IndexSectionStart;
		public readonly UInt16 Length;

		public CV_LVAR_ADDR_RANGE(Stream stream) : base(stream) {
			OffsetStart = ReadUInt32();
			IndexSectionStart = ReadUInt16();
			Length = ReadUInt16();
		}
	}
}
