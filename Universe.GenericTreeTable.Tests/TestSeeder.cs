using System;
using System.Collections.Generic;

namespace Universe.GenericTreeTable.Tests
{
	class TestSeeder
	{
		public static IEnumerable<KeyValuePair<IEnumerable<string>, TestNodeData>> Seed(int depthMax)
		{
			var bottomPrefixes = new[] { "R&D", "Sales", "HR", "Marketing" };
			for (int org = 1; org <= 3; org++)
			{
				var orgName = $"Organization {org}";
				for (int dev = 1; dev <= 3; dev++)
				{
					var devName = $"Division {(char)(65 + dev - 1)}";
					for (int dep = 1; dep <= 2; dep++)
					{
						string[] key = new[] { orgName, devName, $"Department {dep}" };
						yield return new KeyValuePair<IEnumerable<string>, TestNodeData>(key, RandomData());
					}
				}
			}

		}

		private static Random R = new Random(42);
		static TestNodeData RandomData()
		{
			var statuses = new string[] { "AAA", "AA", "A", "B", "C", "D" };
			return new TestNodeData()
			{
				Count = R.Next(500, 5000),
				Date = new DateTime(2020, 1, 1).AddMinutes(R.Next(1, 5 * 365 * 24 * 60)),
				Revenue = R.Next(1, 10000) * 100,
				Status = statuses[R.Next(0, statuses.Length - 1)]
			};

		}
	}
}
