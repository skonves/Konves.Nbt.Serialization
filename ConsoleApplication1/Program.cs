using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konves.Nbt.Serialization;
using Konves.Nbt;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{

			NbtSerializer s = new NbtSerializer(typeof(Player));

			using (System.IO.FileStream file = new System.IO.FileStream(@"e:\Users\Stephen\Desktop\mcedit\world\players\skonves.dat",System.IO.FileMode.Open))
			{
				using (System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(file,System.IO.Compression.CompressionMode.Decompress))
				{
					using (NbtReader reader = new NbtReader(zip))
					{

						object obj = s.Deserialize(reader);

					}
				}
			}

			//System.IO.Compression.GZipStream

		}
	}

	public class Player
	{
		[NbtByte("foodLevel")]
		public int FoodLevel { get; set; }

		[NbtByte("Sleeping")]
		public bool IsSleeping { get; set; }
		
		[NbtCompound("abilities")]
		public PlayerAbilities Abilities { get; set; }

		[NbtList("Inventory", NbtTagType.Compound)]
		public List<Item> Inventory { get; set; } 
	}

	public class PlayerAbilities
	{
		[NbtByte("mayBuild")]
		public bool MayBuild { get; set; }
	}

	public class Item
	{
		[NbtByte("Count")]
		public int Count { get; set; }

		[NbtByte("Slot")]
		public int Slot { get; set; }

		[NbtShort("Damage")]
		public int Damage { get; set; }
	}
}
