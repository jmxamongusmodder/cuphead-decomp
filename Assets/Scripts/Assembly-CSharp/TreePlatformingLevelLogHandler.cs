using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000892 RID: 2194
public class TreePlatformingLevelLogHandler : AbstractPausableComponent
{
	// Token: 0x06003307 RID: 13063 RVA: 0x001DA38C File Offset: 0x001D878C
	private void Start()
	{
		this.dropPosition = base.transform.position;
		this.SetupLogs();
		this.HP = this.maxHP;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06003308 RID: 13064 RVA: 0x001DA3E0 File Offset: 0x001D87E0
	private void SetupLogs()
	{
		int num = 0;
		this.logs = new List<TreePlatformingLevelLog>();
		this.checkedLogs = new List<bool>(this.logOrder.Length);
		base.GetComponent<HitFlash>().otherRenderers = new SpriteRenderer[this.logOrder.Length];
		for (int i = 0; i < this.logOrder.Length; i++)
		{
			TreePlatformingLevelLog treePlatformingLevelLog = UnityEngine.Object.Instantiate<TreePlatformingLevelLog>(this.logPrefabs[(int)this.logOrder[i]]);
			treePlatformingLevelLog.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ceiling + 300f);
			treePlatformingLevelLog.transform.parent = base.transform;
			treePlatformingLevelLog.animator.SetBool("hasLegs", i == 0);
			treePlatformingLevelLog.GetComponent<SpriteRenderer>().sortingOrder = num++;
			treePlatformingLevelLog.GetComponent<DamageReceiver>().enabled = false;
			treePlatformingLevelLog.SetDirection(this.facingRight);
			this.logs.Add(treePlatformingLevelLog);
			this.checkedLogs.Add(false);
			base.GetComponent<HitFlash>().otherRenderers[i] = treePlatformingLevelLog.GetComponent<SpriteRenderer>();
			if (treePlatformingLevelLog.CanShoot)
			{
				this.shootableLogs++;
			}
		}
		this.amountToKillLog = this.maxHP / (float)this.logs.Count;
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x001DA54C File Offset: 0x001D894C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP < this.amountToKillLog * (float)this.logs.Count)
		{
			if (this.HP > 0f)
			{
				this.KillLog();
			}
			else
			{
				if (this.logs.Count > 0 && this.logs[0] != null)
				{
					this.logs[0].KillLog();
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x001DA5EC File Offset: 0x001D89EC
	private void KillLog()
	{
		if (this.logs.Count - 1 > 0)
		{
			this.logs[this.logs.Count - 1].KillLog();
			this.logs.RemoveAt(this.logs.Count - 1);
			if (this.checkedLogs.Count - 1 > 0)
			{
				this.checkedLogs.RemoveAt(this.logs.Count - 1);
			}
		}
		if (this.logs.Count > 0)
		{
			base.GetComponent<BoxCollider2D>().offset = new Vector2(0f, (this.logs[0].transform.localPosition.y + this.logs[this.logs.Count - 1].transform.localPosition.y) / 2f);
			base.GetComponent<BoxCollider2D>().size = new Vector2(this.logs[0].GetComponent<BoxCollider2D>().bounds.size.x, this.logs[0].GetComponent<BoxCollider2D>().bounds.size.y * (float)this.logs.Count);
		}
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x001DA74C File Offset: 0x001D8B4C
	private IEnumerator check_to_start_cr()
	{
		base.StartCoroutine(this.drop_logs_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x001DA768 File Offset: 0x001D8B68
	private IEnumerator drop_logs_cr()
	{
		float t = 0f;
		float time = 0.4f;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (int i = 0; i < this.logs.Count; i++)
		{
			float offset = (i != 0) ? (this.logs[i].GetComponent<BoxCollider2D>().bounds.size.y / 2f + this.logs[i - 1].GetComponent<BoxCollider2D>().bounds.size.y / 2f) : 0f;
			float start = CupheadLevelCamera.Current.Bounds.yMax + 300f;
			float end = this.dropPosition.y + offset;
			while (t < time)
			{
				if (this.logs[i] != null)
				{
					t += CupheadTime.FixedDelta;
					float t2 = EaseUtils.Ease(EaseUtils.EaseType.punch, 0f, 1f, t / time);
					this.logs[i].transform.SetPosition(null, new float?(Mathf.Lerp(start, end, t2)), null);
				}
				yield return wait;
			}
			this.effect.Create(new Vector3(this.logs[i].transform.position.x, this.logs[i].transform.position.y - this.logs[i].GetComponent<BoxCollider2D>().bounds.size.y / 2f));
			t = 0f;
			this.dropPosition = this.logs[i].transform.position;
			this.logs[i].start = this.logs[i].transform.position.y;
			yield return wait;
		}
		foreach (TreePlatformingLevelLog treePlatformingLevelLog in this.logs)
		{
			treePlatformingLevelLog.GetComponent<DamageReceiver>().enabled = true;
		}
		base.GetComponent<BoxCollider2D>().offset = new Vector2(0f, (this.logs[0].transform.localPosition.y + this.logs[this.logs.Count - 1].transform.localPosition.y) / 2f);
		base.GetComponent<BoxCollider2D>().size = new Vector2(this.logs[0].GetComponent<BoxCollider2D>().bounds.size.x, this.logs[0].GetComponent<BoxCollider2D>().bounds.size.y * (float)this.logs.Count);
		base.StartCoroutine(this.shoot_cr());
		yield break;
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x001DA784 File Offset: 0x001D8B84
	private IEnumerator check_to_slide_cr()
	{
		float amount = 0f;
		int indexToSlide = 1000;
		for (;;)
		{
			bool hasRemoved = false;
			int i = 0;
			while (i < this.logs.Count)
			{
				if (this.logs[i].isDying && !hasRemoved)
				{
					amount = this.logs[i].GetComponent<BoxCollider2D>().bounds.size.y;
					if (this.logs[i].CanShoot)
					{
						this.shootableLogs--;
					}
					this.logs.RemoveAt(i);
					this.checkedLogs.RemoveAt(i);
					indexToSlide = i;
					hasRemoved = true;
				}
				else
				{
					if (i >= indexToSlide)
					{
						while (this.logs[i].isSliding)
						{
							yield return null;
						}
						if (this.logs[i] != null)
						{
							this.logs[i].SlideDown(amount);
						}
					}
					if (i == this.logs.Count - 1)
					{
						indexToSlide = 1000;
					}
					i++;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x001DA7A0 File Offset: 0x001D8BA0
	private IEnumerator shoot_cr()
	{
		int rand = 0;
		while (this.shootableLogs > 0 && this.logs.Count > 0)
		{
			yield return CupheadTime.WaitForSeconds(this, this.logs[rand].ShootDelay);
			for (int i = 0; i < this.checkedLogs.Count; i++)
			{
				this.checkedLogs[i] = false;
			}
			while (this.checkedLogs.Contains(false))
			{
				rand = UnityEngine.Random.Range(0, this.checkedLogs.Count);
				if (this.logs[rand].CanShoot && !this.checkedLogs[rand])
				{
					AudioManager.Play("level_platform_logface_attack");
					this.emitAudioFromObject.Add("level_platform_logface_attack");
					this.logs[rand].OnShoot();
					break;
				}
				this.checkedLogs[rand] = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x001DA7BC File Offset: 0x001D8BBC
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		float num = 1000f;
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(new Vector3(base.transform.position.x, base.transform.position.y + num / 2f), new Vector3(this.logPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x, num, 0f));
	}

	// Token: 0x04003B2D RID: 15149
	[SerializeField]
	private float maxHP;

	// Token: 0x04003B2E RID: 15150
	private float HP;

	// Token: 0x04003B2F RID: 15151
	private float amountToKillLog;

	// Token: 0x04003B30 RID: 15152
	[SerializeField]
	private bool facingRight;

	// Token: 0x04003B31 RID: 15153
	[Header("SET UP LOGS HERE:")]
	[SerializeField]
	private TreePlatformingLevelLogHandler.LogTypes[] logOrder;

	// Token: 0x04003B32 RID: 15154
	[Header("DON'T TOUCH THIS:")]
	[SerializeField]
	private TreePlatformingLevelLog[] logPrefabs;

	// Token: 0x04003B33 RID: 15155
	[SerializeField]
	private Effect effect;

	// Token: 0x04003B34 RID: 15156
	private List<TreePlatformingLevelLog> logs;

	// Token: 0x04003B35 RID: 15157
	private Vector3 dropPosition;

	// Token: 0x04003B36 RID: 15158
	private int shootableLogs;

	// Token: 0x04003B37 RID: 15159
	private int logsKilled;

	// Token: 0x04003B38 RID: 15160
	private List<bool> checkedLogs;

	// Token: 0x04003B39 RID: 15161
	private DamageDealer damageDealer;

	// Token: 0x04003B3A RID: 15162
	private DamageReceiver damageReceiver;

	// Token: 0x02000893 RID: 2195
	private enum LogTypes
	{
		// Token: 0x04003B3C RID: 15164
		A,
		// Token: 0x04003B3D RID: 15165
		B,
		// Token: 0x04003B3E RID: 15166
		C,
		// Token: 0x04003B3F RID: 15167
		D,
		// Token: 0x04003B40 RID: 15168
		E,
		// Token: 0x04003B41 RID: 15169
		F
	}
}
