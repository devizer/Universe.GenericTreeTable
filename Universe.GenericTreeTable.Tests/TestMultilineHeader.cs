using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Universe.GenericTreeTable.Tests;

public class TestMultilineHeader
{

	[Test]
	[TestCase(false)]
	[TestCase(true)]
	public void Test1(bool needUnicode)
	{
		List<string[]> header = new List<string[]>();
		header.Add(new[] { "Title" });
		header.Add(new[] { "Category" });
		List<string[]> dataColumns = MultilineColumnsForTest.Columns;
		dataColumns = dataColumns.Take(9).ToList();
		dataColumns[1][0] = "-" + dataColumns[1][0];

		header.AddRange(dataColumns);
		ConsoleTable table = new ConsoleTable(header.Select(x => (IEnumerable<string>)x));
		table.NeedUnicode = needUnicode;
		int counter = 0;
		for (int i = 0; i < 9; i++)
		{
			var row = new object[] { TestSeeder.GetRandomStatus(), -2999 + counter++, null, 7000 + counter++ };
			table.AddRow(row);
		}

		Console.WriteLine(table.ToString());
	}
}
