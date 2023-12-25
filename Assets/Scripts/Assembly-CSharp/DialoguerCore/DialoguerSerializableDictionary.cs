using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DialoguerCore
{
	// Token: 0x02000B80 RID: 2944
	[XmlRoot("dictionary")]
	public class DialoguerSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		// Token: 0x060046CF RID: 18127 RVA: 0x0024FAE9 File Offset: 0x0024DEE9
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x0024FAEC File Offset: 0x0024DEEC
		public void ReadXml(XmlReader reader)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			bool isEmptyElement = reader.IsEmptyElement;
			reader.Read();
			if (isEmptyElement)
			{
				return;
			}
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				reader.ReadStartElement("key");
				TKey key = (TKey)((object)xmlSerializer.Deserialize(reader));
				reader.ReadEndElement();
				reader.ReadStartElement("value");
				TValue value = (TValue)((object)xmlSerializer2.Deserialize(reader));
				reader.ReadEndElement();
				base.Add(key, value);
				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x0024FBA4 File Offset: 0x0024DFA4
		public void WriteXml(XmlWriter writer)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			foreach (TKey tkey in base.Keys)
			{
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");
				xmlSerializer.Serialize(writer, tkey);
				writer.WriteEndElement();
				writer.WriteStartElement("value");
				TValue tvalue = base[tkey];
				xmlSerializer2.Serialize(writer, tvalue);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
	}
}
