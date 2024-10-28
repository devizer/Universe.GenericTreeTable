using System;
using NUnit.Framework;

namespace Universe.GenericTreeTable.Tests
{
	public class SimpleTreeTest
	{

		[Test]
		public void Test1()
		{
			var plainRows = TestSeeder.Seed(3);
			var builder = new TreeTableBuilder<string, TestNodeData>(TestTreeConfiguration.Instance);
			var consoleTable = builder.Build(plainRows);
			Console.WriteLine(consoleTable);
		}
	}
}
