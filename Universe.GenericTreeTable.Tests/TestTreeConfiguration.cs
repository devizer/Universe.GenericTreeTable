using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universe.GenericTreeTable.Tests
{
	internal class TestTreeConfiguration : ITreeTableConfiguration<string, TestNodeData>
	{
		public static readonly TestTreeConfiguration Instance = new TestTreeConfiguration();

		public IEqualityComparer<string> EqualityComparer { get; } = EqualityComparer<string>.Default;
		public string KeyPartToText(string keyPart) => keyPart;

		public string Separator => " \x2192 ";

		public ConsoleTable CreateColumns()
		{
			return new ConsoleTable("Department", "Status", "Date", "-Amount");
		}

		public void WriteColumns(ConsoleTable table, string renderedKey, TestNodeData info)
		{
			table.AddRow(renderedKey, info.Status, info.Date?.ToString("yyyy MM dd HH:mm:ss"), info.Revenue);
		}
	}
}
