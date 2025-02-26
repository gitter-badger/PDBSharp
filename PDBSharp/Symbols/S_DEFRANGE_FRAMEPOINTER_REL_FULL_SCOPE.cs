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

namespace Smx.PDBSharp.Symbols
{
	public class S_DEFRANGE_FRAMEPOINTER_REL_FULL_SCOPE : SymbolBase
	{
		public S_DEFRANGE_FRAMEPOINTER_REL_FULL_SCOPE(IServiceContainer ctx, IModule mod, SpanStream stream) : base(ctx, mod, stream){

		}

		public void Write(PDBFile pdb, Stream stream) {
			SymbolDataWriter w = CreateWriter(SymbolType.S_DEFRANGE_FRAMEPOINTER_REL_FULL_SCOPE);
			w.WriteHeader();
		}
	}
}
