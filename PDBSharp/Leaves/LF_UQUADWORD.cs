#region License
/*
 * Copyright (C) 2018 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smx.PDBSharp.Leaves
{
	[LeafReader(LeafType.LF_UQUADWORD)]
	public class LF_UQUADWORD: SymbolDataReader, ILeaf<ulong>
	{
		public LF_UQUADWORD(PDBFile pdb, Stream stream) : base(pdb, stream) {
			Value = ReadUInt64();
		}

		public ulong Value { get; }
	}
}
