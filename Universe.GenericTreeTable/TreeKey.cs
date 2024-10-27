using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Universe.GenericTreeTable
{
	public class TreeKey<TTreeKeyPart> where TTreeKeyPart : ITextable
	{
		private readonly Lazy<int> _HashCode;
		private readonly Lazy<string> _ToString;
		public TTreeKeyPart[] Path { get; }

		private IEqualityComparer<TTreeKeyPart> EqualityComparer;

		public TreeKey()
		{
			_ToString = new Lazy<string>(() =>
			{
				// const string arrow = " \x27a1 ";
				const string arrow = " \x2192 ";
				var formattedPath = Path?.Select(x => x.ToText()).ToArray();
				return   string.Join(arrow, formattedPath ?? new string[0]);
			}, LazyThreadSafetyMode.ExecutionAndPublication);

			_HashCode = new Lazy<int>(() =>
			{
				if (Path == null) return 0;
				int ret = 0;
				unchecked
				{
					foreach (var p in Path)
						ret = ret * 397 ^ (p?.GetHashCode() ?? 0);
				}

				return ret;

			}, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		public TreeKey(params TTreeKeyPart[] path) : this()
		{
			EqualityComparer = EqualityComparer<TTreeKeyPart>.Default;

			Path = path ?? throw new ArgumentNullException(nameof(path));
			for (int i = 0, l = path.Length; i < l; i++)
				if (path[i] == null)
					throw new ArgumentException($"path's element #{i} is null", nameof(path));
		}

		public TreeKey<TTreeKeyPart> Child(TTreeKeyPart childName)
		{
			if (childName == null) throw new ArgumentNullException(nameof(childName));
			var ret = new TreeKey<TTreeKeyPart>(Path.Concat(new[] { childName }).ToArray());
			ret.EqualityComparer = EqualityComparer;
			return ret;
		}

		public override string ToString() => _ToString.Value;

		protected bool Equals(TreeKey<TTreeKeyPart> other)
		{
			if (_HashCode.Value != other._HashCode.Value) return false;

			var len = Path.Length;
			var lenOther = other.Path.Length;
			if (len != lenOther) return false;
			for (int i = 0; i < len; i++)

				if (!EqualityComparer.Equals(Path[i], other.Path[i]))
					return false;

			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((TTreeKeyPart)obj);
		}

		public override int GetHashCode()
		{
			return _HashCode.Value;
		}

		class TempTree : Dictionary<TreeKey<TTreeKeyPart>, TempTree>
		{
			// null for sub tree
			public TTreeKeyPart Leaf;
		}

		public static List<Node<TreeKey<TTreeKeyPart>>> AsTree(IEnumerable<TreeKey<TTreeKeyPart>> plainList)
		{
			TempTree root = new TempTree();
			foreach (var plainItem in plainList)
			{
				var parent = root;
				for (int i = 0, l = plainItem.Path.Length; i < l; i++)
				{
					TreeKey<TTreeKeyPart> partial = new TreeKey<TTreeKeyPart>(plainItem.Path.Take(i + 1).ToArray());
					TempTree current = parent.GetOrAdd(partial, key => new TempTree());
					parent = current;
				}
			}

			if (plainList.Count() >= 33)
			{
				var breakHere = "ok";
			}

			List<Node<TreeKey<TTreeKeyPart>>> ret = new List<Node<TreeKey<TTreeKeyPart>>>();
			EnumSubTree(root, ret);
			return ret;
		}

		private static void EnumSubTree(TempTree treeNode, List<Node<TreeKey<TTreeKeyPart>>> nodes)
		{

			foreach (var pair in treeNode)
			{
				TreeKey<TTreeKeyPart> key = pair.Key;
				TempTree subTree = pair.Value;
				Node<TreeKey<TTreeKeyPart>> subNode = new Node<TreeKey<TTreeKeyPart>>()
				{
					State = key,
					Name = key.Path.Last().ToText(),
				};
				nodes.Add(subNode);
				EnumSubTree(subTree, subNode.Children);
			}

			var sorted = nodes.OrderBy(x => x.Name).ToList();
			nodes.Clear();
			nodes.AddRange(sorted);
		}
	}
}
