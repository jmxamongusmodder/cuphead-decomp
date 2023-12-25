using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000422 RID: 1058
public class DebugConsoleData : ScriptableObject
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000F55 RID: 3925 RVA: 0x00097288 File Offset: 0x00095688
	public DebugConsoleData.Command Current
	{
		get
		{
			this.CleanIndex();
			return this.commands[this.index];
		}
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x000972A1 File Offset: 0x000956A1
	public void PrepareForSave()
	{
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x000972A3 File Offset: 0x000956A3
	private void CleanIndex()
	{
		if (this.index >= this.commands.Count)
		{
			this.index = this.commands.Count - 1;
		}
	}

	// Token: 0x0400185F RID: 6239
	public static string PATH = "TC_DebugConsole/tc_debug_console_data";

	// Token: 0x04001860 RID: 6240
	public int index;

	// Token: 0x04001861 RID: 6241
	public List<DebugConsoleData.Command> commands = new List<DebugConsoleData.Command>();

	// Token: 0x02000423 RID: 1059
	[Serializable]
	public class Command
	{
		// Token: 0x04001862 RID: 6242
		public string command = "new.command";

		// Token: 0x04001863 RID: 6243
		public KeyCode key;

		// Token: 0x04001864 RID: 6244
		public string rewiredAction = string.Empty;

		// Token: 0x04001865 RID: 6245
		public List<DebugConsoleData.Command.Argument> arguments = new List<DebugConsoleData.Command.Argument>();

		// Token: 0x04001866 RID: 6246
		public string help = string.Empty;

		// Token: 0x04001867 RID: 6247
		public string code = string.Empty;

		// Token: 0x04001868 RID: 6248
		public bool closeConsole;

		// Token: 0x02000424 RID: 1060
		[Serializable]
		public class Argument
		{
			// Token: 0x04001869 RID: 6249
			public DebugConsoleData.Command.Argument.Type type;

			// Token: 0x0400186A RID: 6250
			public string name = "argName";

			// Token: 0x02000425 RID: 1061
			public enum Type
			{
				// Token: 0x0400186C RID: 6252
				Int,
				// Token: 0x0400186D RID: 6253
				Float,
				// Token: 0x0400186E RID: 6254
				Bool,
				// Token: 0x0400186F RID: 6255
				String
			}
		}
	}
}
