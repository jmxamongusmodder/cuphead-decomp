using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007BC RID: 1980
public class SallyStagePlayLevelWindow : AbstractCollidableObject
{
	// Token: 0x06002CC6 RID: 11462 RVA: 0x001A64C7 File Offset: 0x001A48C7
	private void Start()
	{
		this.startPos = base.transform.position;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x001A64FD File Offset: 0x001A48FD
	public void Init(Vector2 pos, SallyStagePlayLevel parent)
	{
		base.transform.position = pos;
		this.parent = parent;
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x001A6518 File Offset: 0x001A4918
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP <= 0f && !this.isDying)
		{
			if (this.isBaby)
			{
				base.StartCoroutine(this.baby_slide_off());
			}
			else
			{
				this.NunDead();
			}
		}
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x001A6576 File Offset: 0x001A4976
	public void WindowClosed()
	{
		base.animator.Play("Off");
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x001A6588 File Offset: 0x001A4988
	private void LeftWindow()
	{
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x001A6598 File Offset: 0x001A4998
	public void WindowOpenNun(LevelProperties.SallyStagePlay properties, bool isPink)
	{
		this.isBaby = false;
		this.speed = properties.CurrentState.nun.rulerSpeed;
		this.HP = (float)properties.CurrentState.nun.HP;
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		this.isPink = isPink;
		foreach (SpriteRenderer spriteRenderer in this.nunPink)
		{
			spriteRenderer.enabled = isPink;
		}
		base.animator.Play("Window_Nun");
	}

	// Token: 0x06002CCC RID: 11468 RVA: 0x001A6630 File Offset: 0x001A4A30
	public void ShootRuler()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = next.transform.position - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(v);
		SallyStagePlayLevelWindowProjectile sallyStagePlayLevelWindowProjectile = (!this.isPink) ? this.ruler : this.rulerPink;
		sallyStagePlayLevelWindowProjectile.Create(base.transform.position, rotation, this.speed, this.parent);
	}

	// Token: 0x06002CCD RID: 11469 RVA: 0x001A66AC File Offset: 0x001A4AAC
	public void WindowOpenBaby(LevelProperties.SallyStagePlay properties)
	{
		this.isBaby = true;
		this.speed = properties.CurrentState.baby.bottleSpeed;
		this.HP = (float)properties.CurrentState.baby.HP;
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		string str = (!Rand.Bool()) ? "_Boy" : "_Girl";
		base.animator.Play("Window_Baby" + str);
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x001A6738 File Offset: 0x001A4B38
	private void ShootBottle()
	{
		Vector3 v = new Vector3(base.transform.position.x, -360f, 0f) - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(v);
		this.bottle.Create(new Vector2(base.transform.position.x, base.transform.position.y - 30f), rotation, this.speed, this.parent);
	}

	// Token: 0x06002CCF RID: 11471 RVA: 0x001A67D0 File Offset: 0x001A4BD0
	private IEnumerator baby_slide_off()
	{
		this.isDying = true;
		base.animator.SetTrigger("Dead");
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		Vector3 start = base.transform.position;
		Vector3 end = new Vector3(base.transform.position.x, base.transform.position.y - 50f);
		float t = 0f;
		float time = 0.1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.position = Vector3.Lerp(start, end, t / time);
			yield return null;
		}
		yield return null;
		this.isDying = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.animator.Play("Off");
		base.transform.position = this.startPos;
		yield return null;
		yield break;
	}

	// Token: 0x06002CD0 RID: 11472 RVA: 0x001A67EB File Offset: 0x001A4BEB
	private void NunDead()
	{
		base.animator.SetTrigger("Dead");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x001A6809 File Offset: 0x001A4C09
	private void SoundBabyBoy()
	{
		AudioManager.Play("sally_baby_boy");
		this.emitAudioFromObject.Add("sally_baby_boy");
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x001A6825 File Offset: 0x001A4C25
	private void SoundBabyGirl()
	{
		AudioManager.Play("sally_baby_girl");
		this.emitAudioFromObject.Add("sally_baby_girl");
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x001A6841 File Offset: 0x001A4C41
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.rulerPink = null;
		this.ruler = null;
		this.bottle = null;
	}

	// Token: 0x04003540 RID: 13632
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04003541 RID: 13633
	[SerializeField]
	private SallyStagePlayLevelWindowProjectile rulerPink;

	// Token: 0x04003542 RID: 13634
	[SerializeField]
	private SallyStagePlayLevelWindowProjectile ruler;

	// Token: 0x04003543 RID: 13635
	[SerializeField]
	private SallyStagePlayLevelWindowProjectile bottle;

	// Token: 0x04003544 RID: 13636
	[SerializeField]
	private SpriteRenderer[] nunPink;

	// Token: 0x04003545 RID: 13637
	public int windowNum;

	// Token: 0x04003546 RID: 13638
	private LevelProperties.SallyStagePlay properties;

	// Token: 0x04003547 RID: 13639
	private SallyStagePlayLevel parent;

	// Token: 0x04003548 RID: 13640
	private Vector3 startPos;

	// Token: 0x04003549 RID: 13641
	private float speed;

	// Token: 0x0400354A RID: 13642
	private float HP;

	// Token: 0x0400354B RID: 13643
	private bool isDying;

	// Token: 0x0400354C RID: 13644
	private bool isBaby = true;

	// Token: 0x0400354D RID: 13645
	private bool isPink;
}
