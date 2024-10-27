using System.Collections.Generic;
using System.Text;

namespace Universe.GenericTreeTable
{
	public class Node<T>
	{
		public T State { get; set; }
		public string Name { get; set; }
		public List<Node<T>> Children { get; } = new List<Node<T>>();

		internal StringBuilder AscIIBuilder { get; } = new StringBuilder();
		public string AscII => AscIIBuilder.ToString();
	}
}
