using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Universe.GenericTreeTable.Tests
{
	[TestFixture]
	public class TestWithoutBordersAndHeader
	{
		[Test]
		public void TestAll()
		{
			foreach (var unicode in new bool[] { false, true })
			{
				Console.WriteLine($"UNICODE: {unicode}");
				foreach (var hideHeader in new bool[] { true, false })
				foreach (var hideColumnBorders in new bool[] { true, false })
				{
					ConsoleTable table = new ConsoleTable("A", "-B", "C");
					table.HideColumnBorders = hideColumnBorders;
					table.HideHeader = hideHeader;
					table.NeedUnicode = unicode;
					for (int row = 1; row <= 3; row++)
					{
						table.AddRow(row, (long)Math.Pow(11, row), "*");
					}

					Console.WriteLine($"NO Headers: {hideHeader}, NO Borders: {hideColumnBorders}{Environment.NewLine}{table}");
				}
			}
		}

	}
}
