using System.Collections.Generic;

namespace Universe.GenericTreeTable
{
	public interface ITreeTableConfiguration<TTreeKeyPart, TNodeData>
	{
		// TODO: KeyPartEqualityComparer?
		IEqualityComparer<TTreeKeyPart> EqualityComparer { get; }
		string KeyPartToText(TTreeKeyPart keyPart);
		ConsoleTable CreateColumns();
		void WriteColumns(ConsoleTable table, string renderedKey, TNodeData nodeData);
		string Separator { get; }
	}
}
