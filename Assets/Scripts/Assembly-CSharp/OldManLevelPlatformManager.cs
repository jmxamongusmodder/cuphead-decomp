using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000709 RID: 1801
public class OldManLevelPlatformManager : LevelProperties.OldMan.Entity
{
	// Token: 0x060026D5 RID: 9941 RVA: 0x0016B588 File Offset: 0x00169988
	public Vector3[] GetPlatformPositions()
	{
		Vector3[] array = new Vector3[this.allPlatforms.Length];
		for (int i = 0; i < this.allPlatforms.Length; i++)
		{
			array[i] = this.allPlatforms[i].platform.position;
		}
		return array;
	}

	// Token: 0x060026D6 RID: 9942 RVA: 0x0016B5DB File Offset: 0x001699DB
	public Transform GetPlatform(int i)
	{
		if (this.allPlatforms[i] == null)
		{
			return null;
		}
		return this.allPlatforms[i].platform.transform;
	}

	// Token: 0x060026D7 RID: 9943 RVA: 0x0016B5FE File Offset: 0x001699FE
	public bool PlatformRemoved(int which)
	{
		return this.allPlatforms[which].removed;
	}

	// Token: 0x060026D8 RID: 9944 RVA: 0x0016B610 File Offset: 0x00169A10
	public override void LevelInit(LevelProperties.OldMan properties)
	{
		base.LevelInit(properties);
		for (int i = 0; i < this.allPlatforms.Length; i++)
		{
			this.allPlatforms[i].platform.SetPosition(null, new float?(properties.CurrentState.platforms.minHeight), null);
		}
		base.StartCoroutine(this.handle_platforms_cr());
		base.StartCoroutine(this.handle_remove_platforms_cr());
	}

	// Token: 0x060026D9 RID: 9945 RVA: 0x0016B690 File Offset: 0x00169A90
	public void EndPhase()
	{
		this.inPhaseOne = false;
		this.mainBeardTufts.enabled = false;
		this.beardSettles[0].transform.parent.gameObject.SetActive(true);
	}

	// Token: 0x060026DA RID: 9946 RVA: 0x0016B6C4 File Offset: 0x00169AC4
	private IEnumerator handle_remove_platforms_cr()
	{
		float bossHealthMax = base.properties.CurrentHealth;
		float bossHealthMin = bossHealthMax * base.properties.GetNextStateHealthTrigger();
		int currentCount = 0;
		string[] removeOrder = base.properties.CurrentState.platforms.removeOrder[UnityEngine.Random.Range(0, base.properties.CurrentState.platforms.removeOrder.Length)].Split(new char[]
		{
			','
		});
		string[] removeThreshold = base.properties.CurrentState.platforms.removeThreshold.Split(new char[]
		{
			','
		});
		if (removeOrder.Length != removeThreshold.Length)
		{
			global::Debug.Break();
		}
		if (removeOrder.Length == 0)
		{
			yield break;
		}
		for (int i = 0; i < removeOrder.Length; i++)
		{
			int item = 0;
			Parser.IntTryParse(removeOrder[i], out item);
			float item2 = 0f;
			Parser.FloatTryParse(removeThreshold[i], out item2);
			this.removeOrderList.Add(item);
			this.removeThresholdList.Add(item2);
		}
		while (currentCount < this.removeThresholdList.Count)
		{
			float t = Mathf.InverseLerp(bossHealthMax, bossHealthMin, base.properties.CurrentHealth);
			if (t > this.removeThresholdList[currentCount])
			{
				this.RemovePlatform(this.removeOrderList[currentCount]);
				currentCount++;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060026DB RID: 9947 RVA: 0x0016B6E0 File Offset: 0x00169AE0
	private IEnumerator handle_platforms_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		LevelProperties.OldMan.Platforms p = base.properties.CurrentState.platforms;
		int orderMainIndex = UnityEngine.Random.Range(0, p.moveOrder.Length);
		string[] orderString = p.moveOrder[orderMainIndex].Split(new char[]
		{
			','
		});
		int orderIndex = UnityEngine.Random.Range(0, orderString.Length);
		bool skipPlatform = false;
		bool stoppedMoving = false;
		while (!stoppedMoving)
		{
			if (!this.inPhaseOne)
			{
				stoppedMoving = true;
				yield return null;
			}
			skipPlatform = false;
			orderString = p.moveOrder[orderMainIndex].Split(new char[]
			{
				','
			});
			string[] spawnOrder = orderString[orderIndex].Split(new char[]
			{
				'-'
			});
			foreach (string s in spawnOrder)
			{
				int num = 0;
				Parser.IntTryParse(s, out num);
				if (this.allPlatforms[num].isMoving)
				{
					skipPlatform = true;
				}
				else
				{
					base.StartCoroutine(this.move_platform_cr(this.allPlatforms[num]));
				}
			}
			if (!skipPlatform)
			{
				yield return CupheadTime.WaitForSeconds(this, p.delayRange.RandomFloat());
			}
			if (orderIndex < orderString.Length - 1)
			{
				orderIndex++;
			}
			else
			{
				orderMainIndex = (orderMainIndex + 1) % p.moveOrder.Length;
				orderIndex = 0;
			}
			yield return null;
		}
		base.StartCoroutine(this.end_phase_cr());
		yield break;
	}

	// Token: 0x060026DC RID: 9948 RVA: 0x0016B6FC File Offset: 0x00169AFC
	private IEnumerator end_phase_cr()
	{
		List<int> order = new List<int>(5);
		for (int j = 0; j < 5; j++)
		{
			if (!this.allPlatforms[j].removed)
			{
				order.Add(j);
			}
		}
		for (int k = 0; k < order.Count; k++)
		{
			int value = order[k];
			int index = UnityEngine.Random.Range(0, order.Count);
			order[k] = order[index];
			order[index] = value;
		}
		for (int i = 0; i < order.Count; i++)
		{
			base.StartCoroutine(this.slide_out_cr(this.allPlatforms[order[i]]));
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x060026DD RID: 9949 RVA: 0x0016B718 File Offset: 0x00169B18
	public void RemovePlatform(int which)
	{
		this.allPlatforms[which].removed = true;
		this.mainBeardTufts.enabled = false;
		this.beardSettles[0].transform.parent.gameObject.SetActive(true);
		base.StartCoroutine(this.slide_out_cr(this.allPlatforms[which]));
	}

	// Token: 0x060026DE RID: 9950 RVA: 0x0016B771 File Offset: 0x00169B71
	public void AttachGnome(int which, OldManLevelGnomeClimber c)
	{
		this.allPlatforms[which].activeClimber = c;
	}

	// Token: 0x060026DF RID: 9951 RVA: 0x0016B784 File Offset: 0x00169B84
	private IEnumerator move_platform_cr(OldManLevelPlatform movingPlatform)
	{
		LevelProperties.OldMan.Platforms p = base.properties.CurrentState.platforms;
		float t = 0f;
		float time = p.moveTime / 2f;
		movingPlatform.isMoving = true;
		while (t < time && this.inPhaseOne && !movingPlatform.removed)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			float lastPos = movingPlatform.platform.transform.position.y;
			movingPlatform.platform.SetPosition(null, new float?(Mathf.Lerp(p.minHeight, p.maxHeight, val)), null);
			movingPlatform.effectiveVel = movingPlatform.platform.transform.position.y - lastPos;
			yield return null;
		}
		if (this.inPhaseOne && !movingPlatform.removed)
		{
			t = 0f;
			movingPlatform.platform.SetPosition(null, new float?(p.maxHeight), null);
		}
		while (t < time && this.inPhaseOne && !movingPlatform.removed)
		{
			t += CupheadTime.Delta;
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			float lastPos2 = movingPlatform.platform.transform.position.y;
			movingPlatform.platform.SetPosition(null, new float?(Mathf.Lerp(p.maxHeight, p.minHeight, val2)), null);
			movingPlatform.effectiveVel = movingPlatform.platform.transform.position.y - lastPos2;
			yield return null;
		}
		if (this.inPhaseOne && !movingPlatform.removed)
		{
			movingPlatform.platform.SetPosition(null, new float?(p.minHeight), null);
			movingPlatform.isMoving = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060026E0 RID: 9952 RVA: 0x0016B7A8 File Offset: 0x00169BA8
	private IEnumerator slide_out_cr(OldManLevelPlatform movingPlatform)
	{
		LevelProperties.OldMan.Platforms p = base.properties.CurrentState.platforms;
		int id = 4 - Array.IndexOf<OldManLevelPlatform>(this.allPlatforms, movingPlatform);
		YieldInstruction wait = new WaitForFixedUpdate();
		float moveHeight = p.minHeight * 2f - 50f;
		if (movingPlatform.effectiveVel > 0f)
		{
			movingPlatform.effectiveVel *= 0.5f;
		}
		float t = this.wobbleBeforeRemoveTime;
		while (t > 0f || movingPlatform.activeClimber)
		{
			t -= CupheadTime.Delta;
			movingPlatform.platform.transform.GetChild(0).transform.GetChild(0).localPosition = new Vector3(Mathf.Sin(t * 100f) * 2.5f, 0f);
			yield return null;
		}
		movingPlatform.platform.transform.GetChild(0).transform.GetChild(0).localPosition = Vector3.zero;
		while (movingPlatform.platform.transform.position.y > moveHeight)
		{
			if (!CupheadTime.IsPaused())
			{
				movingPlatform.platform.SetPosition(null, new float?(Mathf.Clamp(movingPlatform.platform.transform.position.y + movingPlatform.effectiveVel, -1000f, 117f)), null);
			}
			movingPlatform.effectiveVel -= 10f * CupheadTime.FixedDelta;
			if (movingPlatform.platform.transform.position.y < -384f)
			{
				this.beardSettles[id].Play("Settle");
			}
			yield return wait;
		}
		if (movingPlatform.platform.GetComponentInChildren<LevelPlayerController>())
		{
			LevelPlayerController[] componentsInChildren = movingPlatform.platform.GetComponentsInChildren<LevelPlayerController>();
			foreach (LevelPlayerController levelPlayerController in componentsInChildren)
			{
				levelPlayerController.transform.parent = null;
			}
		}
		UnityEngine.Object.Destroy(movingPlatform.platform.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x04002F81 RID: 12161
	private const float PLATFORM_EXIT_SPEED = 10f;

	// Token: 0x04002F82 RID: 12162
	[SerializeField]
	private OldManLevelPlatform[] allPlatforms;

	// Token: 0x04002F83 RID: 12163
	[SerializeField]
	private Animator[] beardSettles;

	// Token: 0x04002F84 RID: 12164
	[SerializeField]
	private SpriteRenderer mainBeardTufts;

	// Token: 0x04002F85 RID: 12165
	[SerializeField]
	private float wobbleBeforeRemoveTime = 1f;

	// Token: 0x04002F86 RID: 12166
	private bool inPhaseOne = true;

	// Token: 0x04002F87 RID: 12167
	private float lastPos;

	// Token: 0x04002F88 RID: 12168
	private List<int> removeOrderList = new List<int>();

	// Token: 0x04002F89 RID: 12169
	private List<float> removeThresholdList = new List<float>();
}
