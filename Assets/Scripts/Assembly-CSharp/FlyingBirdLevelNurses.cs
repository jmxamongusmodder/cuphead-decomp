using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000625 RID: 1573
public class FlyingBirdLevelNurses : AbstractCollidableObject
{
	// Token: 0x06001FFA RID: 8186 RVA: 0x00125B08 File Offset: 0x00123F08
	public void InitNurse(LevelProperties.FlyingBird.Nurses properties)
	{
		this.nurses = base.transform.GetChildTransforms();
		foreach (Transform transform in this.nurses)
		{
			transform.gameObject.SetActive(true);
		}
		this.leftSideShooting = (UnityEngine.Random.Range(-1, 1) >= 0);
		this.properties = properties;
		this.attackIndex = UnityEngine.Random.Range(0, properties.attackCount.Split(new char[]
		{
			','
		}).Length);
		this.pinkPattern = properties.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = 0;
		foreach (Transform transform2 in this.nurses)
		{
			if (transform2.GetComponent<Collider2D>() != null)
			{
				transform2.GetComponent<Collider2D>().enabled = true;
			}
		}
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001FFB RID: 8187 RVA: 0x00125C0C File Offset: 0x0012400C
	private IEnumerator attack_cr()
	{
		bool shootLeft = Rand.Bool();
		bool multiplayer = PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null;
		for (;;)
		{
			int max = Parser.IntParse(this.properties.attackCount.Split(new char[]
			{
				','
			})[this.attackIndex]);
			for (int i = 0; i < max; i++)
			{
				if (i != 0)
				{
					yield return CupheadTime.WaitForSeconds(this, this.properties.attackRepeatDelay);
				}
				if (shootLeft)
				{
					base.animator.SetBool("ANurseATK", true);
				}
				else
				{
					base.animator.SetBool("BNurseATK", true);
				}
				shootLeft = !shootLeft;
				if (multiplayer)
				{
					this.target++;
					if (this.target > PlayerId.PlayerTwo)
					{
						this.target = PlayerId.PlayerOne;
					}
				}
			}
			this.leftSideShooting = !this.leftSideShooting;
			yield return CupheadTime.WaitForSeconds(this, this.properties.attackMainDelay);
			this.attackIndex++;
			if (this.attackIndex >= this.properties.attackCount.Split(new char[]
			{
				','
			}).Length)
			{
				this.attackIndex = 0;
			}
		}
		yield break;
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x00125C28 File Offset: 0x00124028
	private void ShootLeft()
	{
		this.spitFXLeft.SetActive(false);
		AbstractProjectile abstractProjectile = this.pillPrefab.Create(this.shootLeftPosRoot.position + base.transform.up.normalized * 0.1f);
		abstractProjectile.GetComponent<FlyingBirdLevelNursePill>().InitPill(this.properties, this.target, this.pinkPattern[this.pinkIndex] == "P");
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		base.animator.SetBool("ANurseATK", false);
		this.spitFXLeft.SetActive(true);
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x00125CE4 File Offset: 0x001240E4
	private void ShootRight()
	{
		this.spitFXRight.SetActive(false);
		AbstractProjectile abstractProjectile = this.pillPrefab.Create(this.shootRightPosRoot.position + base.transform.up.normalized * 0.1f);
		abstractProjectile.GetComponent<FlyingBirdLevelNursePill>().InitPill(this.properties, this.target, this.pinkPattern[this.pinkIndex] == "P");
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		base.animator.SetBool("BNurseATK", false);
		this.spitFXRight.SetActive(true);
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x00125D9D File Offset: 0x0012419D
	private void ShootSFX()
	{
		AudioManager.Play("nurse_attack");
		this.emitAudioFromObject.Add("nurse_attack");
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x00125DB9 File Offset: 0x001241B9
	public void Die()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x0400287C RID: 10364
	private const string Regular = "R";

	// Token: 0x0400287D RID: 10365
	private const string Parry = "P";

	// Token: 0x0400287E RID: 10366
	[SerializeField]
	private AbstractProjectile pillPrefab;

	// Token: 0x0400287F RID: 10367
	[SerializeField]
	private Transform shootRightPosRoot;

	// Token: 0x04002880 RID: 10368
	[SerializeField]
	private Transform shootLeftPosRoot;

	// Token: 0x04002881 RID: 10369
	[SerializeField]
	private GameObject spitFXLeft;

	// Token: 0x04002882 RID: 10370
	[SerializeField]
	private GameObject spitFXRight;

	// Token: 0x04002883 RID: 10371
	private bool leftSideShooting;

	// Token: 0x04002884 RID: 10372
	private int attackIndex;

	// Token: 0x04002885 RID: 10373
	private PlayerId target;

	// Token: 0x04002886 RID: 10374
	private string[] pinkPattern;

	// Token: 0x04002887 RID: 10375
	private int pinkIndex;

	// Token: 0x04002888 RID: 10376
	public Transform[] nurses;

	// Token: 0x04002889 RID: 10377
	private LevelProperties.FlyingBird.Nurses properties;
}
