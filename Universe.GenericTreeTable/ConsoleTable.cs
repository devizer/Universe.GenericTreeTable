using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Universe.GenericTreeTable
{
    public class ConsoleTable
    {
        private readonly List<List<string>> content = new List<List<string>>();
        private readonly List<string[]> header = new List<string[]>();
        private readonly List<bool> rightAlignment = new List<bool>();

        public bool NeedUnicode = false;

        public ConsoleTable(List<List<string>> multilineColumns) : this((IEnumerable<IEnumerable<string>>)multilineColumns)
        {
        }
        public ConsoleTable(List<string[]> multilineColumns) : this((IEnumerable<IEnumerable<string>>)multilineColumns)
        {
        }

		public ConsoleTable(IEnumerable<IEnumerable<string>> multilineColumns)
        {
	        foreach (IEnumerable<string> column in multilineColumns)
	        {
		        var lines = (column ?? new string[]{string.Empty}).ToArray();
		        var firstLine = lines.FirstOrDefault();
		        var isRightAlignment = firstLine?.StartsWith("-") == true;
		        rightAlignment.Add(isRightAlignment);
		        if (isRightAlignment) lines[0] = lines[0].TrimStart('-');
				header.Add(lines);
			}
        }

		public ConsoleTable(params string[] singlelineColumns)
        {
            foreach (var column in singlelineColumns)
            {
	            var columnNormalized = column ?? string.Empty;
				rightAlignment.Add(columnNormalized.StartsWith("-"));
                header.Add(new string[] { columnNormalized.TrimStart('-') });
            }
        }

        public void AddRow(params object[] values)
        {
            var row = new List<string>();
            foreach (var v in values)
            {
	            if (v is double?)
	            {
		            var d = (double?)v;
		            row.Add(!d.HasValue ? "-" : d.Value.ToString("n2"));
	            }
	            else if (v is int?)
	            {
		            var d = (int?)v;
		            row.Add(!d.HasValue ? "-" : d.Value.ToString("n0"));
	            }
	            else
				{
		            row.Add(Convert.ToString(v));
	            }
            }

            content.Add(row);
        }

        public override string ToString()
        {
            var copy = new List<List<string>>();
            // If single line header (removed)
            // copy.Add(header.Select(x => Convert.ToString(x)).ToList());
			// If Multiline header
			var headerHeight = header.Count == 0 ? 0 : header.Select(x => x.Length).Max(); // rows in header
			for (int headerRowIndex = 0; headerRowIndex < headerHeight; headerRowIndex++)
			{
				List<string> headerRow = new List<string>();
				foreach (string[] strings in header)
				{
					var padTop = headerHeight - strings.Length;
					var yIndex = headerRowIndex - padTop;
					string headerCell = yIndex >= 0 && strings.Length > 0 && yIndex < strings.Length ? strings[yIndex] : "";
					headerRow.Add(headerCell);
				}
				copy.Add(headerRow);
			}


            copy.AddRange(content);
            var cols = copy.Count == 0 ? 0 : copy.Max(x => x.Count);
            var width = Enumerable.Repeat(1, cols).ToList();
            for (var y = 0; y < copy.Count; y++)
            {
                var row = copy[y];
                for (var x = 0; x < row.Count; x++) width[x] = Math.Max(width[x], (row[x] ?? "").Length);
            }

            var sep = width.Select(x => new string(BoxHorizontal, x)).ToList();
            copy.Insert(headerHeight, sep);

            var ret = new StringBuilder();
            for (var y = 0; y < copy.Count; y++)
            {
                var row = copy[y];
                for (var x = 0; x < cols; x++)
                {
                    if (x > 0) ret.Append(y == headerHeight ? BoxCross : BoxVertical);
                    var v = (x < row.Count ? row[x] : null) ?? "";
                    if (v.Length < width[x])
                    {
                        var pad = new string(' ', -v.Length + width[x]);
                        if (rightAlignment[x] && y >= headerHeight)
                            v = pad + v;
                        else
                            v = v + pad;
                    }

                    if (x == cols - 1) v = (v ?? "").TrimEnd();
                    ret.Append(v);
                }

                ret.AppendLine();
            }

            return ret.ToString();
        }

        private char BoxHorizontal => NeedUnicode ? '─' : '-';
        private char BoxVertical => NeedUnicode ? '│' : '|';
        private char BoxCross => NeedUnicode ? '┼' : '+';

	}
}
