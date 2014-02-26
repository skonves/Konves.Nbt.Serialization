using System;

namespace Konves.Nbt.Serialization
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtByteAttribute : NbtTagAttribute
	{
		public NbtByteAttribute()
			: base(NbtTagType.Byte) { }

		public NbtByteAttribute(string name)
			: base(NbtTagType.Byte, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtShortAttribute : NbtTagAttribute
	{
		public NbtShortAttribute()
			: base(NbtTagType.Short) { }

		public NbtShortAttribute(string name)
			: base(NbtTagType.Short, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtListAttribute : NbtTagAttribute
	{
		public NbtListAttribute(NbtTagType elementType)
			: base(NbtTagType.List)
		{
			m_elementType = elementType;
		}

		public NbtListAttribute(string name, NbtTagType elementType)
			: base(NbtTagType.List, name)
		{
			m_elementType = elementType;
		}

		public NbtTagType ElementType { get { return m_elementType; } }

		readonly NbtTagType m_elementType;
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtCompoundAttribute : NbtTagAttribute
	{
		public NbtCompoundAttribute()
			: base(NbtTagType.Compound) { }

		public NbtCompoundAttribute(string name)
			: base(NbtTagType.Compound, name) { }
	}
}
