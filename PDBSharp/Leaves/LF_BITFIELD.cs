#region License
/*
 * Copyright (C) 2019 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using System.ComponentModel.Design;
using System.IO;

namespace Smx.PDBSharp.Leaves
{
	public class LF_BITFIELD : ILeaf
	{
		public readonly ILeafContainer Type;
		public readonly byte Length;
		public readonly byte Position;

		public LF_BITFIELD(IServiceContainer pdb, SpanReader stream) {
			TypeDataReader r = new TypeDataReader(pdb, stream);
			Type = r.ReadIndexedTypeLazy();
			Length = r.ReadByte();
			Position = r.ReadByte();
		}

		public void Write(PDBFile pdb, Stream stream) {
			TypeDataWriter w = new TypeDataWriter(pdb, stream, LeafType.LF_BITFIELD);
			w.WriteIndexedType(Type);
			w.WriteByte(Length);
			w.WriteByte(Position);
			w.WriteLeafHeader();
		}
	}
}
