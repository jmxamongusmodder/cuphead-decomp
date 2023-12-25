using System;
using System.Collections.Generic;

// Token: 0x02000447 RID: 1095
[Serializable]
public class GitLevelProperties
{
	// Token: 0x06001057 RID: 4183 RVA: 0x0009F2BA File Offset: 0x0009D6BA
	public GitLevelProperties()
	{
		this.levels = new List<GitLevelProperties.GitLevel>();
	}

	// Token: 0x040019AB RID: 6571
	public const string UNITY_PATH = "/_CUPHEAD/_Generated/git_data.xml";

	// Token: 0x040019AC RID: 6572
	public const string GIT_TOOLS_PATH = "Assets/_CUPHEAD/_Generated/git_data.xml";

	// Token: 0x040019AD RID: 6573
	public List<GitLevelProperties.GitLevel> levels;

	// Token: 0x02000448 RID: 1096
	[Serializable]
	public class GitLevel
	{
		// Token: 0x040019AE RID: 6574
		public string name;

		// Token: 0x040019AF RID: 6575
		public string levelClassPath;

		// Token: 0x040019B0 RID: 6576
		public string levelObjectPath;
	}
}
