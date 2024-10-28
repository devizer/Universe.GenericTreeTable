using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using NUnit.Framework;

namespace Universe.GenericTreeTable.Tests
{
	public class SimpleTreeTest
	{

		[Test]
		public void Test1()
		{
			var plainRows = TestSeeder.Seed(3).ToArray();
			IEnumerable<string>[] zz = plainRows.Select(x => x.Key).ToArray();
			var plainRowsAsDebug = plainRows.Select(x => $"[{string.Join(",", x.Key.ToArray())}]").ToArray();
			var builder = new TreeTableBuilder<string, TestNodeData>(TestTreeConfiguration.Instance);
			var consoleTable = builder.Build(plainRows);
			Console.WriteLine(consoleTable);
		}
	}
}
