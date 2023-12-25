using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A52 RID: 2642
public class PlayerSuperChaliceIII : AbstractPlayerSuper
{
	// Token: 0x06003EF7 RID: 16119 RVA: 0x00227E54 File Offset: 0x00226254
	protected override void StartSuper()
	{
		base.StartSuper();
		if (!this.player.motor.Grounded)
		{
			base.animator.Play("StartAir");
		}
		base.animator.Update(0f);
		this.minionSpawn = new PatternString(this.minionSpawnString, true);
		this.minionSpawnTiming = new PatternString(this.minionSpawnTimingString, true);
		this.direction = base.transform.localScale.x;
		this.zoomFactor = Camera.main.orthographicSize / 360f;
		AudioManager.Play("player_super_chalice_barrage_start");
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x00227EFC File Offset: 0x002262FC
	private IEnumerator super_cr()
	{
		this.Fire();
		for (;;)
		{
			if (this.player != null)
			{
				if (this.target.position.y < this.player.transform.position.y)
				{
					this.target.position = new Vector3(this.target.position.x, this.player.transform.position.y);
				}
				else
				{
					this.target.position += Vector3.down * this.aimSinkRate * CupheadTime.Delta;
				}
			}
			else
			{
				this.EndSuper(true);
				this.StopAllCoroutines();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x00227F17 File Offset: 0x00226317
	private void StartMinions()
	{
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x00227F34 File Offset: 0x00226334
	private IEnumerator shoot_cr()
	{
		this.mainTimer = WeaponProperties.LevelSuperChaliceIII.superDuration;
		yield return null;
		for (int i = 0; i < this.minionCount; i++)
		{
			int spawnDataOffset = this.minionSpawn.GetSubStringIndex();
			int spawnType = this.minionSpawn.PopInt();
			float angle = (this.direction <= 0f) ? 180f : 0f;
			BasicProjectile bullet = this.minionPrefab.Create(new Vector3(CupheadLevelCamera.Current.transform.position.x - 1000f * Mathf.Sign(this.direction), this.target.position.y + this.minionVerticalRange[spawnDataOffset].RandomFloat() * ((!this.linkRangeToZoom) ? 1f : this.zoomFactor)), angle, this.minionSpeed[spawnDataOffset] * ((!this.linkSpeedToZoom) ? 1f : this.zoomFactor));
			bullet.Damage = this.minionDamage[spawnDataOffset] / ((!this.linkDamageToZoom) ? 1f : this.zoomFactor);
			float s = this.minionScaleRange[spawnDataOffset].RandomFloat() * ((!this.linkScaleToZoom) ? 1f : this.zoomFactor);
			bullet.transform.localScale = new Vector3(s, s);
			((PlayerSuperChaliceIIIMinion)bullet).elementIndex = spawnDataOffset;
			((PlayerSuperChaliceIIIMinion)bullet).wave = this.wave;
			SpriteRenderer r = bullet.GetComponent<SpriteRenderer>();
			r.flipY = (this.direction < 0f);
			r.sortingOrder = ((s >= 1f) ? (100 - spawnType) : (-100 - spawnType));
			bullet.transform.position = new Vector3(bullet.transform.position.x, bullet.transform.position.y, (s - 1f) * 0.0001f);
			bullet.GetComponent<Animator>().Play(spawnType.ToString());
			bullet.DamageRate = 0.2f;
			bullet.PlayerId = this.player.id;
			bullet.GetComponent<Collider2D>().isTrigger = true;
			this.minionTypeCount[spawnType] = (this.minionTypeCount[spawnType] + 1) % 3;
			yield return CupheadTime.WaitForSeconds(this, this.minionSpawnTiming.PopFloat() * ((!this.linkSpawnRateToZoom) ? 1f : (this.zoomFactor * this.spawnRateZoomModifier)));
		}
		while (this.spear && !this.spear.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x00227F50 File Offset: 0x00226350
	private void RespawnChalice()
	{
		this.EndSuper(true);
		this.fxRenderer.sortingLayerName = "Player";
		this.fxRenderer.sortingOrder = -10;
		this.chaliceSprite.sortingLayerName = "Player";
		this.chaliceSprite.sortingOrder = -20;
		this.chaliceRespawned = true;
	}

	// Token: 0x06003EFC RID: 16124 RVA: 0x00227FA8 File Offset: 0x002263A8
	private void ActivateSpear()
	{
		if (this.player != null)
		{
			this.spear.DetachFromSuper(this.player);
		}
		else
		{
			this.spear.transform.parent = null;
		}
		this.spear.gameObject.SetActive(true);
	}

	// Token: 0x040045FB RID: 17915
	[SerializeField]
	private BasicProjectile minionPrefab;

	// Token: 0x040045FC RID: 17916
	private float mainTimer = 100f;

	// Token: 0x040045FD RID: 17917
	private float direction;

	// Token: 0x040045FE RID: 17918
	[SerializeField]
	private int minionCount = 50;

	// Token: 0x040045FF RID: 17919
	[SerializeField]
	private bool wave = true;

	// Token: 0x04004600 RID: 17920
	[SerializeField]
	private float aimSinkRate = 100f;

	// Token: 0x04004601 RID: 17921
	[SerializeField]
	private SpriteRenderer chaliceSprite;

	// Token: 0x04004602 RID: 17922
	[SerializeField]
	private SpriteRenderer fxRenderer;

	// Token: 0x04004603 RID: 17923
	[SerializeField]
	private MinMax[] minionVerticalRange;

	// Token: 0x04004604 RID: 17924
	[SerializeField]
	private MinMax[] minionScaleRange;

	// Token: 0x04004605 RID: 17925
	[SerializeField]
	private float[] minionSpeed;

	// Token: 0x04004606 RID: 17926
	[SerializeField]
	private float[] minionDamage;

	// Token: 0x04004607 RID: 17927
	[SerializeField]
	private string minionSpawnString;

	// Token: 0x04004608 RID: 17928
	private PatternString minionSpawn;

	// Token: 0x04004609 RID: 17929
	[SerializeField]
	private string minionSpawnTimingString;

	// Token: 0x0400460A RID: 17930
	private PatternString minionSpawnTiming;

	// Token: 0x0400460B RID: 17931
	[SerializeField]
	private int[] minionTypeCount = new int[6];

	// Token: 0x0400460C RID: 17932
	[SerializeField]
	private PlayerSuperChaliceIIISpear spear;

	// Token: 0x0400460D RID: 17933
	[SerializeField]
	private Transform target;

	// Token: 0x0400460E RID: 17934
	private float zoomFactor;

	// Token: 0x0400460F RID: 17935
	[SerializeField]
	private bool linkSpeedToZoom = true;

	// Token: 0x04004610 RID: 17936
	[SerializeField]
	private bool linkScaleToZoom;

	// Token: 0x04004611 RID: 17937
	[SerializeField]
	private bool linkRangeToZoom;

	// Token: 0x04004612 RID: 17938
	[SerializeField]
	private bool linkDamageToZoom;

	// Token: 0x04004613 RID: 17939
	[SerializeField]
	private bool linkSpawnRateToZoom;

	// Token: 0x04004614 RID: 17940
	[SerializeField]
	private float spawnRateZoomModifier = 1f;

	// Token: 0x04004615 RID: 17941
	private bool chaliceRespawned;
}
