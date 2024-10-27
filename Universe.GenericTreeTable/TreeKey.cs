using System;
using System.Collections.Generic;
using System.Linq;

namespace Universe.GenericTreeTable
{
	public class TreeKey<TTreeKeyPart>
	{
		public TTreeKeyPart[] Path { get; }


		public TreeKey()
		{
		}

		public TreeKey(IEnumerable<TTreeKeyPart> path) : this()
		{
			Path = (path ?? throw new ArgumentNullException(nameof(path))).ToArray();
			for (int i = 0, l = Path.Length; i < l; i++)
				if (Path[i] == null)
					throw new ArgumentException($"path's element #{i} is null", nameof(path));
		}
		public TreeKey(params TTreeKeyPart[] path) : this((IEnumerable<TTreeKeyPart>) path)
		{
		}

	}
}
