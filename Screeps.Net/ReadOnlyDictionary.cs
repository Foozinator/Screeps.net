using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net
{
	//public class ReadOnlyDictionary<TKey, TValue> where TKey : object
	//{
	//	private Dictionary<TKey, TValue> _store;

	//	public ReadOnlyDictionary( Dictionary<TKey, TValue> src = null )
	//	{
	//		_store = src ?? new Dictionary<TKey, TValue>();
	//	}

	//	public IReadOnlyList<TKey> Keys
	//	{
	//		get { return _store.Keys.ToList(); }
	//	}

	//	public IReadOnlyList<TValue> Values
	//	{
	//		get { return _store.Values.ToList(); }
	//	}

	//	public TValue this[ TKey key ]
	//	{
	//		get
	//		{
	//			var storeKey = _store.Keys.FirstOrDefault( k => k == key );
	//			return _store[ storeKey ];
	//		}
	//	}

	//	public int Count
	//	{
	//		get { return _store.Count; }
	//	}
	//}
}
