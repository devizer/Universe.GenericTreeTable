using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

		public ConsoleTable Build(IEnumerable<KeyValuePair<IEnumerable<TTreeKeyPart>, TData>> plainNodes)
		{
			KeyValuePair<TreeKey<TTreeKeyPart>, TData>[] plainWithTreeKey = plainNodes
				.Select(x => new KeyValuePair<TreeKey<TTreeKeyPart>, TData>(new TreeKey<TTreeKeyPart>(x.Key), x.Value))
				.ToArray();

			var reportCopyRaw = plainWithTreeKey;
			reportCopyRaw = reportCopyRaw.OrderBy(x => x.Key.ToString()).ToArray();
			TreeKeyEqualityComparer<TTreeKeyPart> treeKeyEqualityComparer = new TreeKeyEqualityComparer<TTreeKeyPart>(this.Configuration.EqualityComparer);
			var reportCopy = reportCopyRaw.ToDictionary(x => x.Key, x => x.Value, treeKeyEqualityComparer);

			List<Node<TreeKey<TTreeKeyPart>>> rootKeys = AsTree(reportCopyRaw.Select(x => x.Key));
			List<KeyValuePair<TreeKey<TTreeKeyPart>, string>> orderedKeys = new List<KeyValuePair<TreeKey<TTreeKeyPart>, string>>();

			void Enum1(List<Node<TreeKey<TTreeKeyPart>>> nodes)
			{
				foreach (var node in nodes)
				{
					orderedKeys.Add(new KeyValuePair<TreeKey<TTreeKeyPart>, string>(node.State, node.AscII));
					Enum1(node.Children);
				}
			}
			AscIITreeDiagram<TreeKey<TTreeKeyPart>>.PopulateAscII(rootKeys);
			Enum1(rootKeys);

			var letsDebug = "ok";


			ConsoleTable ct = this.Configuration.CreateColumns();
			StringBuilder debugTree = new StringBuilder();
			foreach (var pair in orderedKeys)
			{
				TreeKey<TTreeKeyPart> path = pair.Key;
				string pathAsString = pair.Value;
				// var total = reportCopyRaw.FirstOrDefault(x => x.Key.Equals(path)).Value ?? zeroMetrics;
				debugTree.AppendLine($"{path,-125} {pathAsString}");
				reportCopy.TryGetValue(path, out TData total);
				var detail = total;
				if (total == null) ct.AddRow(pathAsString);
				else
				{
					this.Configuration.WriteColumns(ct, pathAsString, total);
				}
			}

			return ct;
		}
		class TempTree : Dictionary<TreeKey<TTreeKeyPart>, TempTree>
		{
			public TempTree(IEqualityComparer<TreeKey<TTreeKeyPart>> comparer) : base(comparer)
			{
			}
			// null for sub tree
			// public TTreeKeyPart Leaf;
		}

		// public for tests only
		public List<Node<TreeKey<TTreeKeyPart>>> AsTree(IEnumerable<TreeKey<TTreeKeyPart>> plainList)
		{
			TreeKeyEqualityComparer<TTreeKeyPart> treeKeyEqualityComparer = new TreeKeyEqualityComparer<TTreeKeyPart>(this.Configuration.EqualityComparer);
			TempTree root = new TempTree(treeKeyEqualityComparer);
			foreach (var plainItem in plainList)
			{
				var parent = root;
				for (int i = 0, l = plainItem.Path.Length; i < l; i++)
				{
					TreeKey<TTreeKeyPart> partial = new TreeKey<TTreeKeyPart>(plainItem.Path.Take(i + 1).ToArray());
					TempTree current = parent.GetOrAdd(partial, key => new TempTree(treeKeyEqualityComparer));
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
