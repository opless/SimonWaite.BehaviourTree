using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree
{
	public static class Extensions
	{
		static public List<T> Randomize<T> (this List<T> input, bool random, IContext ctx)
		{
			if (input == null)
				throw new ArgumentNullException ("input");

			if (!random) {
				return input;
			}

			var list = new List<T> (input);

			list.Sort ((T x, T y) => 2 - (ctx.RandomInt () % 3));

			return list;
		}
	}
}
