using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Universe.GenericTreeTable.Tests
{
	internal class TestNodeData {
		public string Status { get; set; }
		public int? Count { get; set; }
		public DateTime? Date { get; set; }
		public double Amount { get; set; }
	}

	class TestSeeder
	{
		public static IEnumerable<KeyValuePair<string, TestNodeData>> Seed()
		{
			throw new NotImplementedException();

		}
	}
}


