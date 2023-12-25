using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200042E RID: 1070
public class StringVariantGenerator
{
	// Token: 0x06000F9E RID: 3998 RVA: 0x0009C004 File Offset: 0x0009A404
	private StringVariantGenerator()
	{
		this.characterGenerators = new Dictionary<char, StringVariantGenerator.CharacterGenerator>
		{
			{
				'a',
				new StringVariantGenerator.CharacterGenerator("aA*", null, null)
			},
			{
				'b',
				new StringVariantGenerator.CharacterGenerator("bB(", null, null)
			},
			{
				'c',
				new StringVariantGenerator.CharacterGenerator("cC)", null, null)
			},
			{
				'd',
				new StringVariantGenerator.CharacterGenerator("dD", null, null)
			},
			{
				'e',
				new StringVariantGenerator.CharacterGenerator("eE&", null, null)
			},
			{
				'f',
				new StringVariantGenerator.CharacterGenerator("fF", null, null)
			},
			{
				'g',
				new StringVariantGenerator.CharacterGenerator("gG", null, null)
			},
			{
				'h',
				new StringVariantGenerator.CharacterGenerator("hH-", null, null)
			},
			{
				'i',
				new StringVariantGenerator.CharacterGenerator("iI", null, null)
			},
			{
				'j',
				new StringVariantGenerator.CharacterGenerator("jJ", null, null)
			},
			{
				'k',
				new StringVariantGenerator.CharacterGenerator("kK", null, null)
			},
			{
				'l',
				new StringVariantGenerator.CharacterGenerator("lL%", null, null)
			},
			{
				'm',
				new StringVariantGenerator.CharacterGenerator("mM", null, null)
			},
			{
				'n',
				new StringVariantGenerator.CharacterGenerator("nN^", null, null)
			},
			{
				'o',
				new StringVariantGenerator.CharacterGenerator("oO+", null, null)
			},
			{
				'p',
				new StringVariantGenerator.CharacterGenerator("pP", null, null)
			},
			{
				'q',
				new StringVariantGenerator.CharacterGenerator("qQ", null, null)
			},
			{
				'r',
				new StringVariantGenerator.CharacterGenerator("rR@", null, null)
			},
			{
				's',
				new StringVariantGenerator.CharacterGenerator("sS#", null, null)
			},
			{
				't',
				new StringVariantGenerator.CharacterGenerator("tT$", null, null)
			},
			{
				'u',
				new StringVariantGenerator.CharacterGenerator("uU", null, null)
			},
			{
				'v',
				new StringVariantGenerator.CharacterGenerator("vV", null, null)
			},
			{
				'w',
				new StringVariantGenerator.CharacterGenerator("wW", null, null)
			},
			{
				'x',
				new StringVariantGenerator.CharacterGenerator("xX", null, null)
			},
			{
				'y',
				new StringVariantGenerator.CharacterGenerator("yY", null, null)
			},
			{
				'z',
				new StringVariantGenerator.CharacterGenerator("zZ", null, null)
			},
			{
				'-',
				new StringVariantGenerator.CharacterGenerator(":;", new List<string>
				{
					"["
				}, new List<string>
				{
					"["
				})
			},
			{
				'!',
				new StringVariantGenerator.CharacterGenerator("!1", new List<string>
				{
					"[",
					"{",
					"[[",
					"{{"
				}, null)
			},
			{
				'~',
				new StringVariantGenerator.CharacterGenerator("~`", new List<string>
				{
					"[",
					"{",
					"[[",
					"{{"
				}, null)
			},
			{
				'\'',
				new StringVariantGenerator.CharacterGenerator("'\"", new List<string>
				{
					"[",
					"{",
					string.Empty
				}, new List<string>
				{
					"[",
					"{",
					string.Empty
				})
			},
			{
				'.',
				new StringVariantGenerator.CharacterGenerator(".>", new List<string>
				{
					"[",
					"{",
					"[[",
					"{{"
				}, null)
			},
			{
				',',
				new StringVariantGenerator.CharacterGenerator(",<", new List<string>
				{
					"[",
					"{",
					string.Empty
				}, new List<string>
				{
					"[",
					"{",
					string.Empty
				})
			},
			{
				'?',
				new StringVariantGenerator.CharacterGenerator("?/", new List<string>
				{
					"[",
					"{",
					"[[",
					"{{"
				}, null)
			},
			{
				' ',
				new StringVariantGenerator.CharacterGenerator("   ]  }", null, null)
			}
		};
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0009C452 File Offset: 0x0009A852
	public static StringVariantGenerator Instance
	{
		get
		{
			if (StringVariantGenerator._instance == null)
			{
				StringVariantGenerator._instance = new StringVariantGenerator();
			}
			return StringVariantGenerator._instance;
		}
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x0009C470 File Offset: 0x0009A870
	public string Generate(string input)
	{
		foreach (StringVariantGenerator.CharacterGenerator characterGenerator in this.characterGenerators.Values)
		{
			characterGenerator.Init();
		}
		string text = string.Empty;
		bool flag = false;
		input = input.Replace(" -", "-");
		input = input.Replace("- ", "-");
		foreach (char c in input)
		{
			if (c == '<')
			{
				flag = true;
			}
			if (c == '>')
			{
				flag = false;
			}
			if (!flag && this.characterGenerators.ContainsKey(c))
			{
				text += this.characterGenerators[c].generate();
			}
			else
			{
				text += c;
			}
		}
		return text;
	}

	// Token: 0x040018D2 RID: 6354
	private static StringVariantGenerator _instance;

	// Token: 0x040018D3 RID: 6355
	private Dictionary<char, StringVariantGenerator.CharacterGenerator> characterGenerators;

	// Token: 0x0200042F RID: 1071
	private class CharacterGenerator
	{
		// Token: 0x06000FA1 RID: 4001 RVA: 0x0009C584 File Offset: 0x0009A984
		public CharacterGenerator(string variants, List<string> randomPrefixes = null, List<string> randomSuffixes = null)
		{
			this.variants = variants;
			this.randomPrefixes = randomPrefixes;
			this.randomSuffixes = randomSuffixes;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0009C5A1 File Offset: 0x0009A9A1
		public void Init()
		{
			this.currentIndex = UnityEngine.Random.Range(0, this.variants.Length);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0009C5BC File Offset: 0x0009A9BC
		public string generate()
		{
			this.currentIndex = (this.currentIndex + 1) % this.variants.Length;
			string text = this.variants[this.currentIndex].ToString();
			if (this.randomPrefixes != null)
			{
				text = this.randomPrefixes.RandomChoice<string>() + text;
			}
			if (this.randomSuffixes != null)
			{
				text += this.randomSuffixes.RandomChoice<string>();
			}
			return text;
		}

		// Token: 0x040018D4 RID: 6356
		private int currentIndex;

		// Token: 0x040018D5 RID: 6357
		private string variants;

		// Token: 0x040018D6 RID: 6358
		private List<string> randomPrefixes;

		// Token: 0x040018D7 RID: 6359
		private List<string> randomSuffixes;
	}
}
