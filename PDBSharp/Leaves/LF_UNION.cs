#region License
/*
 * Copyright (C) 2019 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using System;
using System.ComponentModel.Design;
using System.IO;

namespace Smx.PDBSharp.Leaves
{
	public class LF_UNION : LeafBase, ILeaf
	{
		public readonly UInt16 NumberOfElements;
		public readonly TypeProperties Properties;
		public readonly ILeafContainer FieldType;

		public readonly ILeafContainer StructSize;

		public readonly string Name;

		public override bool IsDefnUdt {
			get {
				return Properties.HasFlag(TypeProperties.IsForwardReference);
			}
		}

		public override bool IsGlobalDefnUdt {
			get {
				return (
					Properties.HasFlag(TypeProperties.IsForwardReference) &&
					Properties.HasFlag(TypeProperties.IsScoped) &&
					!IsUdtAnon
				);
			}
		}

		public override bool IsLocalDefnUdtWithUniqueName {
			get {
				return (
					!Properties.HasFlag(TypeProperties.IsForwardReference) &&
					Properties.HasFlag(TypeProperties.IsScoped) &&
					Properties.HasFlag(TypeProperties.HasUniqueName) &&
					!IsUdtAnon
				);
			}
		}

		public override string UdtName => Name;

		public LF_UNION(IServiceContainer pdb, Stream stream) {
			TypeDataReader r = new TypeDataReader(pdb, stream);

			NumberOfElements = r.ReadUInt16();
			Properties = r.ReadFlagsEnum<TypeProperties>();
			FieldType = r.ReadIndexedTypeLazy();

			StructSize = r.ReadVaryingType(out uint dataSize);
			Name = r.ReadCString();
		}

		public void Write(PDBFile pdb, Stream stream) {
			TypeDataWriter w = new TypeDataWriter(pdb, stream, LeafType.LF_UNION);
			w.WriteUInt16(NumberOfElements);
			w.WriteEnum<TypeProperties>(Properties);
			w.WriteIndexedType(FieldType);
			w.WriteVaryingType(StructSize);
			w.WriteCString(Name);
		}
	}
}
