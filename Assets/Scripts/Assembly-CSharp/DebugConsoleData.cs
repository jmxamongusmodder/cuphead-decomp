using UnityEngine;
using System;
using System.Collections.Generic;

public class DebugConsoleData : ScriptableObject
{
	[Serializable]
	public class Command
	{
		[Serializable]
		public class Argument
		{
			public enum Type
			{
				Int = 0,
				Float = 1,
				Bool = 2,
				String = 3,
			}

			public Type type;
			public string name;
		}

		public string command;
		public KeyCode key;
		public string rewiredAction;
		public List<DebugConsoleData.Command.Argument> arguments;
		public string help;
		public string code;
		public bool closeConsole;
	}

	public int index;
	public List<DebugConsoleData.Command> commands;
}
