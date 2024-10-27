using System.Collections.Generic;
using System;

namespace Universe.GenericTreeTable
{
	// TODO: Store ToText() outside of TTreeKeyPart, similar to 
	public interface ITextable
	{
		string ToText();
	}

	internal static class DictionaryExtensions
	{
		public static V GetOrAdd<K, V>(this IDictionary<K, V> dictionary, K key, Func<K, V> getNewValue)
		{
			if (dictionary.TryGetValue(key, out var ret))
				return ret;

			ret = getNewValue(key);
			dictionary.Add(key, ret);
			return ret;
		}

	}

}
