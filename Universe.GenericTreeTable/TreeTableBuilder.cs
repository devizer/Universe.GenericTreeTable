using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Universe.GenericTreeTable
{
	public class TreeTableBuilder<TTreeKeyPart,TData>
	{
		private readonly ITreeTableConfiguration<TTreeKeyPart, TData> Configuration;

		public TreeTableBuilder(ITreeTableConfiguration<TTreeKeyPart, TData> configuration)
		{
			Configuration = configuration;
		}

		public ConsoleTable Build(IEnumerable<KeyValuePair<TTreeKeyPart, TData>> plainNodes)
		{
			throw new NotImplementedException();
		}

		class TempTree : Dictionary<TreeKey<TTreeKeyPart>, TempTree>
		{
			// null for sub tree
			public TTreeKeyPart Leaf;
		}

		// public for tests only
		public List<Node<TreeKey<TTreeKeyPart>>> AsTree(IEnumerable<TreeKey<TTreeKeyPart>> plainList)
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

		private void EnumSubTree(TempTree treeNode, List<Node<TreeKey<TTreeKeyPart>>> nodes)
		{

			foreach (var pair in treeNode)
			{
				TreeKey<TTreeKeyPart> key = pair.Key;
				TempTree subTree = pair.Value;
				Node<TreeKey<TTreeKeyPart>> subNode = new Node<TreeKey<TTreeKeyPart>>()
				{
					State = key,
					Name = this.Configuration.KeyPartToText(key.Path.Last()),
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
