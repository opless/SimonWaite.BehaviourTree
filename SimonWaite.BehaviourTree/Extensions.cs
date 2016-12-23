using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

		#region Json.net JSON
		public static T FromJsonByNewtonsoft<T> (this string json)
		{
			var settings = new JsonSerializerSettings () {
				Formatting = Newtonsoft.Json.Formatting.Indented,
				TypeNameHandling = TypeNameHandling.All

			};
			settings.Converters.Add (new StringEnumConverter (false));
			var ser = JsonSerializer.Create (settings);


			using (var sr = new StringReader (json)) {
				var ret = (T)ser.Deserialize (sr, typeof (T));
				return ret;
			}
		}
		public static string ToJsonByNewtonsoft<T> (this T obj)
		{
			var settings = new JsonSerializerSettings () {
				Formatting = Newtonsoft.Json.Formatting.Indented,
				TypeNameHandling = TypeNameHandling.Auto
			};

			settings.Converters.Add (new StringEnumConverter (false));

			var ser = JsonSerializer.Create (settings);

			using (var sw = new StringWriter ()) {
				ser.Serialize (sw, obj);

				return sw.ToString ();
			}

		}
		#endregion
	}
}
