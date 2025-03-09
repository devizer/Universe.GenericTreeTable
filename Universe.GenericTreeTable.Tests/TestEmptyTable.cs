using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Universe.GenericTreeTable.Tests;

public class TestEmptyTable
{
	[Test]
	[TestCase(false)]
	[TestCase(true)]
	public void TestEmptyFull(bool needUnicode)
	{
		ConsoleTable table = new ConsoleTable();
		table.NeedUnicode = needUnicode;
		Console.WriteLine(table.ToString());
	}

	[Test]
	[TestCase(false)]
	[TestCase(true)]
	public void TestEmptyHeader1(bool needUnicode)
	{
		ConsoleTable table = new ConsoleTable("");
		table.NeedUnicode = needUnicode;
		Console.WriteLine(table.ToString());
	}

	[Test]
	[TestCase(false)]
	[TestCase(true)]
	public void TestEmptyHeader2(bool needUnicode)
	{
		ConsoleTable table = new ConsoleTable("", "");
		table.NeedUnicode = needUnicode;
		Console.WriteLine(table.ToString());
	}

	[Test]
	[TestCase(false)]
	[TestCase(true)]
	public void TestNullHeader1(bool needUnicode)
	{
		ConsoleTable table = new ConsoleTable(new string[] { null });
		table.NeedUnicode = needUnicode;
		Console.WriteLine(table.ToString());
	}
	[Test]
	[TestCase(false)]
	[TestCase(true)]
	public void TestNullHeader2(bool needUnicode)
	{
		ConsoleTable table = new ConsoleTable(new string[] { null, null });
		table.NeedUnicode = needUnicode;
		Console.WriteLine(table.ToString());
	}
	[Test]
	[TestCase(false)]
	[TestCase(true)]
	public void TestMultilineNullHeader2(bool needUnicode)
	{
		List<List<string>> header = new List<List<string>>();
		header.Add(null);
		header.Add(null);

		ConsoleTable table = new ConsoleTable(header);
		table.NeedUnicode = needUnicode;
		Console.WriteLine(table.ToString());
	}
}
