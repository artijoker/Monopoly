using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
	class Optional<T> : IReadOnlyList<T> where T : struct {

		private readonly IReadOnlyList<T> values;

		public Optional(T value) => values = new[] { value };
		public Optional() => values = new T[0];

		public T this[int index] => values[index];
		public int Count => values.Count;
		public bool HasValue => values.Count == 1;
		public T Value => values[0];
		public IEnumerator<T> GetEnumerator() => values.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();

	}
}
