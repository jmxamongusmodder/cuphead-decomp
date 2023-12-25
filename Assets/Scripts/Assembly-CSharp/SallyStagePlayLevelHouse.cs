using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007AF RID: 1967
public class SallyStagePlayLevelHouse : AbstractPausableComponent
{
	// Token: 0x06002C34 RID: 11316 RVA: 0x0019FD18 File Offset: 0x0019E118
	public void StartPhase2(SallyStagePlayLevel parent, LevelProperties.SallyStagePlay properties)
	{
		this.SetUp(parent, properties);
	}

	// Token: 0x06002C35 RID: 11317 RVA: 0x0019FD22 File Offset: 0x0019E122
	public void StartAttacks()
	{
		if (!SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			base.StartCoroutine(this.family_cr());
		}
		else
		{
			base.StartCoroutine(this.nuns_cr());
		}
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x0019FD4D File Offset: 0x0019E14D
	private void SetUp(SallyStagePlayLevel parent, LevelProperties.SallyStagePlay properties)
	{
		this.parent = parent;
		this.properties = properties;
		parent.OnPhase3 += this.OnPhase3;
		base.StartCoroutine(this.setup_windows_cr());
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x0019FD7C File Offset: 0x0019E17C
	private IEnumerator setup_windows_cr()
	{
		Vector3 pos = Vector3.zero;
		int num = 1;
		this.windows = new SallyStagePlayLevelWindow[9];
		for (int i = 0; i < 9; i++)
		{
			this.windows[i] = UnityEngine.Object.Instantiate<SallyStagePlayLevelWindow>(this.windowPrefab);
			this.windows[i].transform.position = this.windowRoots[i].position;
			this.windows[i].Init(this.windowRoots[i].position, this.parent);
			this.windows[i].transform.parent = base.transform;
			this.windows[i].windowNum = num + i;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x0019FD98 File Offset: 0x0019E198
	private IEnumerator nuns_cr()
	{
		LevelProperties.SallyStagePlay.Nun p = this.properties.CurrentState.nun;
		string[] windowPattern = p.appearPosition.GetRandom<string>().Split(new char[]
		{
			','
		});
		int windowPos = 0;
		int pinkStringMainIndex = UnityEngine.Random.Range(0, p.pinkString.Length);
		string[] pinkString = p.pinkString[pinkStringMainIndex].Split(new char[]
		{
			','
		});
		int pinkStringIndex = UnityEngine.Random.Range(0, pinkString.Length);
		for (;;)
		{
			for (int i = 0; i < windowPattern.Length; i++)
			{
				Parser.IntTryParse(windowPattern[i], out windowPos);
				foreach (SallyStagePlayLevelWindow window in this.windows)
				{
					if (window.windowNum == windowPos)
					{
						window.WindowOpenNun(this.properties, pinkString[pinkStringIndex][0] == 'P');
						if (pinkStringIndex < pinkString.Length - 1)
						{
							pinkStringIndex++;
						}
						else
						{
							pinkStringMainIndex = (pinkStringMainIndex + 1) % p.pinkString.Length;
							pinkStringIndex = 0;
						}
						yield return CupheadTime.WaitForSeconds(this, p.reappearDelayRange.RandomFloat());
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x0019FDB4 File Offset: 0x0019E1B4
	private IEnumerator family_cr()
	{
		LevelProperties.SallyStagePlay.Baby p = this.properties.CurrentState.baby;
		string[] windowPattern = p.appearPosition.GetRandom<string>().Split(new char[]
		{
			','
		});
		int windowPos = 0;
		for (;;)
		{
			for (int i = 0; i < windowPattern.Length; i++)
			{
				Parser.IntTryParse(windowPattern[i], out windowPos);
				foreach (SallyStagePlayLevelWindow window in this.windows)
				{
					if (window.windowNum == windowPos)
					{
						window.WindowOpenBaby(this.properties);
						yield return CupheadTime.WaitForSeconds(this, p.hesitate);
						yield return CupheadTime.WaitForSeconds(this, p.reappearDelayRange.RandomFloat());
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x0019FDCF File Offset: 0x0019E1CF
	private void OnPhase3()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject, 1f);
		this.parent.OnPhase3 -= this.OnPhase3;
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x0019FDFE File Offset: 0x0019E1FE
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.windowPrefab = null;
	}

	// Token: 0x040034DF RID: 13535
	[SerializeField]
	private Transform[] windowRoots;

	// Token: 0x040034E0 RID: 13536
	[SerializeField]
	private SallyStagePlayLevelWindow windowPrefab;

	// Token: 0x040034E1 RID: 13537
	private SallyStagePlayLevelWindow[] windows;

	// Token: 0x040034E2 RID: 13538
	private LevelProperties.SallyStagePlay properties;

	// Token: 0x040034E3 RID: 13539
	private SallyStagePlayLevel parent;

	// Token: 0x040034E4 RID: 13540
	private const int WINDOW_NUM = 9;
}
