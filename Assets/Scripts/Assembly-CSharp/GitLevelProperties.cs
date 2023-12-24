using System;
using System.Collections.Generic;

[Serializable]
public class GitLevelProperties
{
	[Serializable]
	public class GitLevel
	{
		public string name;
		public string levelClassPath;
		public string levelObjectPath;
	}

	public List<GitLevelProperties.GitLevel> levels;
}
