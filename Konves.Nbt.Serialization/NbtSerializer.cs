using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Konves.Nbt.Serialization
{
	public class NbtSerializer
	{
		public NbtSerializer(Type type)
		{
			m_type = type;
		}

		public object Deserialize(Stream stream)
		{
			return Deserialize(new NbtReader(stream));
		}

		public object Deserialize(BinaryReader binaryReader)
		{
			return Deserialize(new NbtReader(binaryReader));
		}

		public object Deserialize(NbtReader nbtReader)
		{
			NbtTagInfo tagInfo = nbtReader.ReadTagInfo();
			if (tagInfo.Type != NbtTagType.Compound)
				throw new FormatException("the deserialized stream must contain root tag which is compound.");

			NbtCompound root = nbtReader.ReadCompound(tagInfo);

			return DeserializeTag(root, m_type);
		}

		public void Serialize(Stream stream, object o)
		{
			Serialize(new NbtWriter(stream), o);
		}

		public void Serialize(BinaryWriter binaryWriter, object o)
		{
			Serialize(new NbtWriter(binaryWriter), o);
		}

		public void Serialize(NbtWriter nbtWriter, object o)
		{
			throw new NotImplementedException();
		}

		private static object DeserializeTag(NbtTag tag, Type type)
		{
			switch (tag.Type)
			{
				case NbtTagType.Byte:
				case NbtTagType.Double:
				case NbtTagType.Float:
				case NbtTagType.Int:
				case NbtTagType.Long:
				case NbtTagType.Short:
				case NbtTagType.String:
					return Convert.ChangeType(tag.GetValue(), type);
				case NbtTagType.ByteArray:
					break;
				case NbtTagType.Compound:
					return DeserializeCompound(tag as NbtCompound, type);
				case NbtTagType.IntArray:
					break;
				case NbtTagType.List:
					return DeserializeList(tag as NbtList, type);
				default:
					throw new ArgumentOutOfRangeException();
			}

			throw new NotImplementedException();
		}

		private static object DeserializeCompound(NbtCompound tag, Type type)
		{
			object obj = type.Assembly.CreateInstance(type.FullName);

			var data = GetTypeData(type);

			foreach (NbtTag t in tag.Value)
			{
				PropertyInfo pi;

				if (data.TryGetValue(t.Name, out pi))
					pi.SetValue(obj, DeserializeTag(t, pi.PropertyType), null);
			}

			return obj;
		}

		private static object DeserializeList(NbtList tag, Type type)
		{
			Type elementType;

			if (type.HasElementType)
			{
				elementType = type.GetElementType(); 
			}

			throw new NotImplementedException();
		}

		private static Dictionary<string, PropertyInfo> GetTypeData(Type type)
		{
			return
				type.GetProperties()
				.Select(pi => new { PropertyInfo = pi, Attributes = pi.GetCustomAttributes(true) })
				.Where(g => !g.Attributes.Select(a => a.GetType().AssemblyQualifiedName).Contains(typeof(NbtIgnoreAttribute).AssemblyQualifiedName))
				.Select(x => new { x.PropertyInfo, Attribute = x.Attributes.FirstOrDefault(a => typeof(NbtTagAttribute).IsAssignableFrom(a.GetType())) })
				.ToDictionary(x => x.Attribute == null || !(x.Attribute as NbtTagAttribute).HasName ? x.PropertyInfo.Name : (x.Attribute as NbtTagAttribute).Name, x => x.PropertyInfo);
		}

		readonly Type m_type;
	}
}
