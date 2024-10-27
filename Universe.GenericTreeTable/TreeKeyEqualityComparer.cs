using System;
using System.Collections.Generic;
using System.IO;

namespace Universe.GenericTreeTable
{
	class TreeKeyEqualityComparer<TTreeKeyPart> : IEqualityComparer<TreeKey<TTreeKeyPart>>
	{
		public readonly IEqualityComparer<TTreeKeyPart> PartEqualityComparer;

		public TreeKeyEqualityComparer(IEqualityComparer<TTreeKeyPart> partEqualityComparer)
		{
			PartEqualityComparer = partEqualityComparer ?? throw new ArgumentNullException(nameof(partEqualityComparer));
		}

		public bool Equals(TreeKey<TTreeKeyPart> x, TreeKey<TTreeKeyPart> y)
		{
			if (x == null && y == null) return true;
			if (x == null || y == null) return false;

			var xLen = x.Path.Length;
			var yLen = y.Path.Length;
			if (xLen != yLen) return false;
			for (int i = 0; i < xLen; i++)
				if (!PartEqualityComparer.Equals(x.Path[i], y.Path[i]))
					return false;

			return true;
		}

		public int GetHashCode(TreeKey<TTreeKeyPart> obj)
		{
			if (obj.Path == null) return 0;
			int ret = 0;
			unchecked
			{
				foreach (var p in obj.Path)
					ret = ret * 397 ^ (p == null ? 13 : PartEqualityComparer.GetHashCode(p));
			}

			return ret;
		}
	}
}
