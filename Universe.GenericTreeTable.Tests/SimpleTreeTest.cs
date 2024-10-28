using System;
using System.Linq;
using NUnit.Framework;

namespace Universe.GenericTreeTable.Tests
{
	public class SimpleTreeTest
	{
		[Test]
		public void TestUnique()
		{
			var plainRows = TestSeeder.Seed(3).ToArray();
			var plainRowsAsDebug = plainRows.Select(x => $"[{string.Join(",", x.Key.ToArray())}]").ToArray();
			var builder = new TreeTableBuilder<string, TestNodeData>(TestTreeConfiguration.Instance);
			var consoleTable = builder.Build(plainRows);
			Console.WriteLine(consoleTable);
		}

		[Test]
		public void TestDuplicateLastAndFirst()
		{
			var plainRows = TestSeeder.Seed(3).ToList();
			if (plainRows.Count > 2)
			{
				plainRows = plainRows.Concat(new[] { plainRows.First(), plainRows.Last() }).ToList();
			}

			var plainRowsAsDebug = plainRows.Select(x => $"[{string.Join(",", x.Key.ToArray())}]").ToArray();
			var builder = new TreeTableBuilder<string, TestNodeData>(TestTreeConfiguration.Instance);
			var consoleTable = builder.Build(plainRows);
			Console.WriteLine(consoleTable);
		}

	}
}
