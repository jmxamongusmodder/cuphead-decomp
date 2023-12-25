using System;
using System.Collections.Generic;

// Token: 0x020009CC RID: 2508
public static class NintendoSwitchRemappingHelper
{
	// Token: 0x06003AEE RID: 15086 RVA: 0x00212768 File Offset: 0x00210B68
	public static void DuplicateXML(Dictionary<string, string> updatedMappings, Dictionary<string, string> existingMappings)
	{
		if (updatedMappings.Count > 1)
		{
			throw new Exception("More than one mapping found!");
		}
		foreach (KeyValuePair<string, string> keyValuePair in updatedMappings)
		{
			string text = null;
			if (keyValuePair.Key.Contains(NintendoSwitchRemappingHelper.P1Identifier))
			{
				text = NintendoSwitchRemappingHelper.P1Identifier;
			}
			else if (keyValuePair.Key.Contains(NintendoSwitchRemappingHelper.P2Identifier))
			{
				text = NintendoSwitchRemappingHelper.P2Identifier;
			}
			if (text == null)
			{
				throw new Exception("Unable to determine controller mapping origin");
			}
			Dictionary<string, string> dictionary = null;
			string oldValue = null;
			string oldValue2 = null;
			foreach (KeyValuePair<string, string> keyValuePair2 in NintendoSwitchRemappingHelper.DualControllers)
			{
				if (keyValuePair.Key.Contains(keyValuePair2.Key))
				{
					oldValue = keyValuePair2.Key;
					oldValue2 = keyValuePair2.Value;
					dictionary = NintendoSwitchRemappingHelper.DualControllers;
					break;
				}
			}
			foreach (KeyValuePair<string, string> keyValuePair3 in NintendoSwitchRemappingHelper.SingleControllers)
			{
				if (keyValuePair.Key.Contains(keyValuePair3.Key))
				{
					oldValue = keyValuePair3.Key;
					oldValue2 = keyValuePair3.Value;
					dictionary = NintendoSwitchRemappingHelper.SingleControllers;
					break;
				}
			}
			if (dictionary == null)
			{
				throw new Exception("Unable to determine controller search values");
			}
			string value = keyValuePair.Value;
			foreach (KeyValuePair<string, string> keyValuePair4 in dictionary)
			{
				string key = keyValuePair4.Key;
				string value2 = keyValuePair4.Value;
				string key2 = keyValuePair.Key.Replace(oldValue, key);
				string text2 = value.Replace(oldValue, key);
				text2 = text2.Replace(oldValue2, value2);
				existingMappings[key2] = text2;
			}
		}
	}

	// Token: 0x040042AA RID: 17066
	private static readonly string P1Identifier = "r2|0";

	// Token: 0x040042AB RID: 17067
	private static readonly string P2Identifier = "r2|1";

	// Token: 0x040042AC RID: 17068
	private static readonly Dictionary<string, string> DualControllers = new Dictionary<string, string>
	{
		{
			"521b808c-0248-4526-bc10-f1d16ee76bf1",
			"Joy-Con (Dual)"
		},
		{
			"7bf3154b-9db8-4d52-950f-cd0eed8a5819",
			"Pro Controller"
		},
		{
			"1fbdd13b-0795-4173-8a95-a2a75de9d204",
			"Handheld"
		}
	};

	// Token: 0x040042AD RID: 17069
	private static readonly Dictionary<string, string> SingleControllers = new Dictionary<string, string>
	{
		{
			"3eb01142-da0e-4a86-8ae8-a15c2b1f2a04",
			"Joy-Con (L)"
		},
		{
			"605dc720-1b38-473d-a459-67d5857aa6ea",
			"Joy-Con (R)"
		}
	};
}
