using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

// Token: 0x02000384 RID: 900
public class Xml
{
	// Token: 0x06000AA6 RID: 2726 RVA: 0x0007F9A4 File Offset: 0x0007DDA4
	public static string Serialize(object obj)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
		using (TextWriter textWriter = new StringWriter(stringBuilder))
		{
			xmlSerializer.Serialize(textWriter, obj);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0007F9FC File Offset: 0x0007DDFC
	public static T Deserialize<T>(string xml)
	{
		T result = default(T);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		using (TextReader textReader = new StringReader(xml))
		{
			result = (T)((object)xmlSerializer.Deserialize(textReader));
		}
		return result;
	}
}
