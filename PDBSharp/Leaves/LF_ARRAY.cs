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
	public class LF_ARRAY : LeafBase
	{
		public ILeafContainer ElementType { get; set; }
		public ILeafContainer IndexingType { get; set; }

		public ILeafContainer Size { get; set; }

		public string Name { get; set; }

		public LF_ARRAY(IServiceContainer ctx, SpanStream stream) : base(ctx, stream) {
		}
		
		public override void Read() {
			TypeDataReader r = CreateReader();

			ElementType = r.ReadIndexedTypeLazy();
			IndexingType = r.ReadIndexedTypeLazy();

			Size = r.ReadVaryingType(out uint dataSize);

			Name = r.ReadCString();
		}

		public override void Write() {
			TypeDataWriter w = CreateWriter(Leaves.LeafType.LF_ARRAY);
			w.WriteIndexedType(ElementType);
			w.WriteIndexedType(IndexingType);
			w.WriteVaryingType(Size);
			w.WriteCString(Name);
			w.WriteHeader();
		}
	}
}
