using System;
using System.Collections;
using UnityEngine.SceneManagement;

// Token: 0x02000442 RID: 1090
public class LoadFirstScene : AbstractMonoBehaviour
{
	// Token: 0x06001003 RID: 4099 RVA: 0x0009E633 File Offset: 0x0009CA33
	private void Start()
	{
		SceneManager.LoadScene(0);
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x0009E63C File Offset: 0x0009CA3C
	private IEnumerator load_cr()
	{
		yield return null;
		yield break;
	}
}
