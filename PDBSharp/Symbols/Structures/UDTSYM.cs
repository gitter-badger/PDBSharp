#region License
/*
 * Copyright (C) 2018 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using System;
using System.IO;

namespace Smx.PDBSharp.Symbols.Structures
{
	public class UdtSym : SymbolDataReader
	{
		public readonly UInt32 TypeIndex;
		public readonly string Name;

		public UdtSym(Stream stream) : base(stream) {
			TypeIndex = ReadUInt32();
			Name = ReadSymbolString();
		}
	}
}
