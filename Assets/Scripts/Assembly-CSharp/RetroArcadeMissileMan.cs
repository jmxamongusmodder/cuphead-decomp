using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000746 RID: 1862
public class RetroArcadeMissileMan : RetroArcadeEnemy
{
	// Token: 0x06002891 RID: 10385 RVA: 0x0017A672 File Offset: 0x00178A72
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
		this.hp = properties.CurrentState.missile.hp;
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x0017A694 File Offset: 0x00178A94
	public void StartMissile()
	{
		base.gameObject.SetActive(true);
		this.missile = UnityEngine.Object.Instantiate<RetroArcadeMissile>(this.missilePrefab);
		this.missile.Init(this.shootRootLeft.position, -90f, this.properties.CurrentState.missile, this.pivotPoint.position);
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.fire_missiles_cr());
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x0017A714 File Offset: 0x00178B14
	private IEnumerator move_cr()
	{
		float time = this.properties.CurrentState.missile.manMoveTime;
		bool movingRight = Rand.Bool();
		for (;;)
		{
			float t = 0f;
			float start = base.transform.position.x;
			float end;
			if (movingRight)
			{
				end = 80f;
			}
			else
			{
				end = -80f;
			}
			while (t < time)
			{
				float val = t / time;
				base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(new float?(end), null, null);
			movingRight = !movingRight;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x0017A730 File Offset: 0x00178B30
	private IEnumerator fire_missiles_cr()
	{
		string[] dirString = this.properties.CurrentState.missile.directionString.Split(new char[]
		{
			','
		});
		int dirIndex = UnityEngine.Random.Range(0, dirString.Length);
		bool onRight = false;
		for (;;)
		{
			onRight = (dirString[dirIndex] == "R");
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.missile.timerRelease.RandomFloat());
			yield return null;
			this.missile.StartCircle(onRight, this.pivotPoint.position);
			dirIndex = (dirIndex + 1) % dirString.Length;
		}
		yield break;
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x0017A74B File Offset: 0x00178B4B
	public override void Dead()
	{
		base.Dead();
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(this.missile.gameObject);
		this.properties.DealDamageToNextNamedState();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003165 RID: 12645
	[SerializeField]
	private RetroArcadeMissile missilePrefab;

	// Token: 0x04003166 RID: 12646
	[SerializeField]
	private Transform shootRootLeft;

	// Token: 0x04003167 RID: 12647
	[SerializeField]
	private Transform shootRootRight;

	// Token: 0x04003168 RID: 12648
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x04003169 RID: 12649
	private const float MAX_X_POS = 80f;

	// Token: 0x0400316A RID: 12650
	private LevelProperties.RetroArcade properties;

	// Token: 0x0400316B RID: 12651
	private RetroArcadeMissile missile;
}
