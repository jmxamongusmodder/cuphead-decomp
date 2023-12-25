using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200061F RID: 1567
public class FlyingBirdLevelHeart : AbstractCollidableObject
{
	// Token: 0x06001FE2 RID: 8162 RVA: 0x001249B8 File Offset: 0x00122DB8
	public void InitHeart(LevelProperties.FlyingBird properties)
	{
		this.properties = properties;
		this.mainShootIndex = UnityEngine.Random.Range(0, properties.CurrentState.heart.shootString.Length);
		this.projectileMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.heart.numOfProjectiles.Length);
		this.thisAnimator = base.GetComponent<Animator>();
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x00124A14 File Offset: 0x00122E14
	public void StartHeartAttack()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		this.faceRight = (next.transform.position.x > base.transform.position.x);
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].flipX = this.faceRight;
		}
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.accend_cr());
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x00124A9C File Offset: 0x00122E9C
	private IEnumerator accend_cr()
	{
		float start = base.transform.position.y;
		this.FireSpreadshot();
		while (base.transform.position.y < start + this.properties.CurrentState.heart.heartHeight)
		{
			base.transform.position += Vector3.up * this.properties.CurrentState.heart.movementSpeed * CupheadTime.Delta;
			if (base.transform.localScale.x < 1f)
			{
				base.transform.localScale += Vector3.one * 1.75f * CupheadTime.Delta;
			}
			else
			{
				base.transform.localScale = Vector3.one * 1f;
			}
			yield return null;
			if (this.properties.CurrentHealth <= 0f)
			{
				break;
			}
		}
		while (base.transform.position.y > start)
		{
			base.transform.position += Vector3.down * this.properties.CurrentState.heart.movementSpeed * CupheadTime.Delta;
			if (base.transform.position.y < start + 100f)
			{
				if (base.transform.localScale.x > 0.5f)
				{
					base.transform.localScale -= Vector3.one * 1.75f * CupheadTime.Delta;
				}
				else
				{
					base.transform.localScale = Vector3.one * 0.5f;
				}
			}
			yield return null;
			if (this.properties.CurrentHealth <= 0f)
			{
				break;
			}
		}
		base.transform.SetPosition(null, new float?(start), null);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x00124AB7 File Offset: 0x00122EB7
	private void FireSpreadshot()
	{
		base.StartCoroutine(this.spreadShot_cr());
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x00124AC6 File Offset: 0x00122EC6
	private void SpawnFX()
	{
		this.puffFX.Create(base.transform.position);
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x00124AE0 File Offset: 0x00122EE0
	private IEnumerator spreadShot_cr()
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		string[] shootString = this.properties.CurrentState.heart.shootString[this.mainShootIndex].Split(new char[]
		{
			','
		});
		int shootIndex = UnityEngine.Random.Range(0, shootString.Length);
		for (int i = 0; i < this.properties.CurrentState.heart.shotCount; i++)
		{
			string[] projectileString = this.properties.CurrentState.heart.numOfProjectiles[this.projectileMainIndex].Split(new char[]
			{
				','
			});
			int projectiles = 0;
			float projectileDelay = 0f;
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			Parser.IntTryParse(projectileString[this.projectileSubIndex], out projectiles);
			Parser.FloatTryParse(shootString[shootIndex], out projectileDelay);
			yield return CupheadTime.WaitForSeconds(this, projectileDelay);
			this.thisAnimator.SetTrigger("Attack");
			yield return CupheadTime.WaitForSeconds(this, 0.125f);
			float directionX = player.transform.position.x - base.transform.position.x;
			float directionY = player.transform.position.y - base.transform.position.y;
			AudioManager.Play("level_flyingbird_stretcher_regurgitate_projectile");
			this.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_projectile");
			for (int j = 0; j < projectiles; j++)
			{
				float num = this.properties.CurrentState.heart.spreadAngle.GetFloatAt((float)j / ((float)projectiles - 1f));
				float num2 = this.properties.CurrentState.heart.spreadAngle.max / 2f;
				num -= num2;
				float num3 = Mathf.Atan2(directionY, directionX) * 57.29578f;
				if (this.faceRight && (num3 < -90f || num3 > 90f))
				{
					num3 = 180f - num3;
				}
				else if (!this.faceRight && num3 > -90f && num3 < 90f)
				{
					num3 = -180f - num3;
				}
				Vector3 vector = new Vector3(72f, 0f, 0f);
				vector *= (float)((!this.faceRight) ? 1 : -1);
				this.projectilePrefab.Create(base.transform.position - vector, num3 + num, this.properties.CurrentState.heart.projectileSpeed);
				shootIndex = (shootIndex + 1) % shootString.Length;
			}
			if (shootIndex < shootString.Length - 1)
			{
				shootIndex++;
			}
			else
			{
				this.mainShootIndex = (this.mainShootIndex + 1) % this.properties.CurrentState.heart.shootString.Length;
				shootIndex = 0;
			}
			if (this.projectileSubIndex < projectileString.Length - 1)
			{
				this.projectileSubIndex++;
			}
			else
			{
				this.projectileMainIndex = (this.projectileMainIndex + 1) % this.properties.CurrentState.heart.numOfProjectiles.Length;
				this.projectileSubIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400285A RID: 10330
	private const float ProjectileOffsetX = 72f;

	// Token: 0x0400285B RID: 10331
	private const float ScaleRate = 1.75f;

	// Token: 0x0400285C RID: 10332
	private const float ScaleStartPosition = 100f;

	// Token: 0x0400285D RID: 10333
	private const float InitialScale = 0.5f;

	// Token: 0x0400285E RID: 10334
	private const float TargetScale = 1f;

	// Token: 0x0400285F RID: 10335
	[SerializeField]
	private Effect puffFX;

	// Token: 0x04002860 RID: 10336
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x04002861 RID: 10337
	[SerializeField]
	private SpriteRenderer[] renderers;

	// Token: 0x04002862 RID: 10338
	private int mainShootIndex;

	// Token: 0x04002863 RID: 10339
	private int projectileMainIndex;

	// Token: 0x04002864 RID: 10340
	private int projectileSubIndex;

	// Token: 0x04002865 RID: 10341
	private Animator thisAnimator;

	// Token: 0x04002866 RID: 10342
	private LevelProperties.FlyingBird properties;

	// Token: 0x04002867 RID: 10343
	private bool faceRight;
}
