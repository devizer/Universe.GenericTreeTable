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

        public bool NeedUnicode { get; set; } = false;
        public bool HideHeader { get; set; } = false;
        public bool HideColumnBorders { get; set; } = false;

        protected ConsoleTable()
        {
	        _CellsAccessor = new CellsAccessor(this);
        }

        public ConsoleTable(List<List<string>> multilineColumns) : this((IEnumerable<IEnumerable<string>>)multilineColumns)
        {
        }

        public ConsoleTable(List<string[]> multilineColumns) : this((IEnumerable<IEnumerable<string>>)multilineColumns)
        {
        }

		public ConsoleTable(IEnumerable<IEnumerable<string>> multilineColumns) : this()
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

		public ConsoleTable(params string[] singlelineColumns) : this()
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
				row.Add(PreProcessInputValue(v));

            content.Add(row);
        }

        private string PreProcessInputValue(object v)
        {
	        if (v is double?)
	        {
		        var d = (double?)v;
		        return !d.HasValue ? "-" : d.Value.ToString("n2");
	        }
	        else if (v is int?)
	        {
		        var d = (int?)v;
		        return !d.HasValue ? "-" : d.Value.ToString("n0");
	        }
	        else
	        {
		        return (Convert.ToString(v));
	        }
		}

		public int LineCount => this.content.Count;
        private CellsAccessor _CellsAccessor;
        public CellsAccessor Cells => _CellsAccessor;

		public class CellsAccessor
        {
	        private readonly ConsoleTable _owner;
	        public CellsAccessor(ConsoleTable owner)
	        {
		        _owner = owner;
	        }

	        public object this[int row, int column]
	        {
		        // getter always return string
		        get
		        {
			        if (row >= 0 && row < _owner.content.Count)
			        {
				        var line = _owner.content[row];
						if (column >= 0 && column < line.Count)
							return line[column];
			        }

			        return null;
		        }
		        set
		        {
			        if (row >= 0 && row < _owner.content.Count)
			        {
				        var line = _owner.content[row];
						while(line.Count <= column) line.Add(null);
						line[column] = _owner.PreProcessInputValue(value);
			        }
			        else
				        throw new ArgumentException($"row argument is {row} should be in range 0 ... {_owner.content.Count - 1}");
		        }
	        }
        }

        public override string ToString()
        {
            var copy = new List<List<string>>();
            // If single line header (removed)
            // copy.Add(header.Select(x => Convert.ToString(x)).ToList());
			// If Multiline header
			var headerHeight = header.Count == 0 ? 0 : header.Select(x => x.Length).Max(); // rows in header
			if (!HideHeader)
			{
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
            if (!HideHeader)
            {
	            copy.Insert(headerHeight, sep);
            }

            var ret = new StringBuilder();
            for (var y = 0; y < copy.Count; y++)
            {
                var row = copy[y];
                for (var x = 0; x < cols; x++)
                {
	                if (!HideColumnBorders)
	                {
		                if (x > 0) ret.Append(y == headerHeight && !HideHeader ? BoxCross : BoxVertical);
	                }

	                var v = (x < row.Count ? row[x] : null) ?? "";
                    if (v.Length < width[x])
                    {
                        var pad = new string(' ', -v.Length + width[x]);
                        if (rightAlignment[x] && (y >= headerHeight || HideHeader))
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
