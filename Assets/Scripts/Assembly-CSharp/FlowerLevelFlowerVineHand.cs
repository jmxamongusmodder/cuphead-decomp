using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class FlowerLevelFlowerVineHand : AbstractCollidableObject
{
	// Token: 0x06001F18 RID: 7960 RVA: 0x0011DF8C File Offset: 0x0011C38C
	public void OnVineHandSpawn(float firstHold, float secondHold, int attackPosOne, int attackPosTwo = 0)
	{
		this.holdCount = 0;
		int num = attackPosOne;
		for (int i = 0; i < 2; i++)
		{
			if (i == 1)
			{
				if (attackPosTwo == 0)
				{
					break;
				}
				num = attackPosTwo;
			}
			switch (num)
			{
			case 1:
				this.spawnPosition = new Vector3((float)this.platformOneXPosition, (float)this.vineHandSpawnYPosition, 0f);
				break;
			case 2:
				this.spawnPosition = new Vector3((float)this.platformTwoXPosition, (float)this.vineHandSpawnYPosition, 0f);
				break;
			case 3:
				this.spawnPosition = new Vector3((float)this.platformThreeXPosition, (float)this.vineHandSpawnYPosition, 0f);
				break;
			}
			this.Create(firstHold, secondHold);
		}
	}

	// Token: 0x06001F19 RID: 7961 RVA: 0x0011E059 File Offset: 0x0011C459
	public void InitVineHand(float first, float second)
	{
		this.firstHoldDelay = first;
		this.secondHoldDelay = second;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x0011E074 File Offset: 0x0011C474
	private void Create(float first, float second)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject, this.spawnPosition, Quaternion.identity);
		gameObject.GetComponent<FlowerLevelFlowerVineHand>().InitVineHand(first, second);
	}

	// Token: 0x06001F1B RID: 7963 RVA: 0x0011E0A5 File Offset: 0x0011C4A5
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001F1C RID: 7964 RVA: 0x0011E0BD File Offset: 0x0011C4BD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001F1D RID: 7965 RVA: 0x0011E0E8 File Offset: 0x0011C4E8
	private IEnumerator holdDelay(float delay)
	{
		base.animator.SetBool("OnHold", true);
		if (delay != 0f)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		base.animator.SetBool("OnHold", false);
		yield return null;
		yield break;
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x0011E10C File Offset: 0x0011C50C
	private void OnHold()
	{
		if (this.holdCount == 0)
		{
			base.StartCoroutine(this.holdDelay(this.firstHoldDelay));
			this.holdCount++;
		}
		else
		{
			base.StartCoroutine(this.holdDelay(this.secondHoldDelay));
		}
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x0011E15D File Offset: 0x0011C55D
	private void OnRetracted()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x0011E170 File Offset: 0x0011C570
	private void ContinueBackAnimation()
	{
		base.animator.SetTrigger("ContinueBackAnimation");
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x0011E182 File Offset: 0x0011C582
	private void SoundVineHandGrow()
	{
		AudioManager.Play("flower_vinehand_grow");
		this.emitAudioFromObject.Add("flower_vinehand_grow");
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x0011E19E File Offset: 0x0011C59E
	private void SoundVineHandGrowContinue()
	{
		AudioManager.Play("flower_vinehand_grow_continue");
		this.emitAudioFromObject.Add("flower_vinehand_grow_continue");
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x0011E1BA File Offset: 0x0011C5BA
	private void SoundVineHandGrowRetract()
	{
		AudioManager.Play("flower_vinehand_grow_retract");
		this.emitAudioFromObject.Add("flower_vinehand_grow_retract");
	}

	// Token: 0x040027B6 RID: 10166
	[SerializeField]
	private int platformOneXPosition;

	// Token: 0x040027B7 RID: 10167
	[SerializeField]
	private int platformTwoXPosition;

	// Token: 0x040027B8 RID: 10168
	[SerializeField]
	private int platformThreeXPosition;

	// Token: 0x040027B9 RID: 10169
	[Space(10f)]
	[SerializeField]
	private int vineHandSpawnYPosition;

	// Token: 0x040027BA RID: 10170
	private int holdCount;

	// Token: 0x040027BB RID: 10171
	private float firstHoldDelay;

	// Token: 0x040027BC RID: 10172
	private float secondHoldDelay;

	// Token: 0x040027BD RID: 10173
	private Vector3 spawnPosition;

	// Token: 0x040027BE RID: 10174
	private DamageDealer damageDealer;
}
