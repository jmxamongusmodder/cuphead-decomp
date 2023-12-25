using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B7 RID: 2231
public class FunhousePlatformingLevelJackinBox : PlatformingLevelShootingEnemy
{
	// Token: 0x06003409 RID: 13321 RVA: 0x001E3248 File Offset: 0x001E1648
	protected override void Start()
	{
		base.Start();
		this.directionIndex = UnityEngine.Random.Range(0, base.Properties.jackinDirectionString.Split(new char[]
		{
			','
		}).Length);
		this.jack.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		AudioManager.PlayLoop("funhouse_jackbox_eye_spin_loop");
		this.emitAudioFromObject.Add("funhouse_jackbox_eye_spin_loop");
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x001E32C8 File Offset: 0x001E16C8
	protected override void OnStart()
	{
		base.OnStart();
		base.StartCoroutine(this.pop_up_cr());
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x001E32E0 File Offset: 0x001E16E0
	private IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x001E32FB File Offset: 0x001E16FB
	protected override void Shoot()
	{
		if (this.shootTime)
		{
			base.Shoot();
		}
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x001E3310 File Offset: 0x001E1710
	private IEnumerator pop_up_cr()
	{
		string dir = string.Empty;
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			this.justDied = false;
			if (base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			})[this.directionIndex][0] == 'U')
			{
				base.animator.SetInteger("Direction", 1);
				this.jack.transform.position = this.topRoot.transform.position;
				this.jack.transform.SetEulerAngles(null, null, new float?(270f));
				this.jack.GetComponent<SpriteRenderer>().sortingOrder = -5;
				dir = "Up";
			}
			else if (base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			})[this.directionIndex][0] == 'L')
			{
				base.animator.SetInteger("Direction", 2);
				this.jack.transform.position = this.leftRoot.transform.position;
				this.jack.transform.SetEulerAngles(null, null, new float?(0f));
				this.jack.GetComponent<SpriteRenderer>().sortingOrder = 5;
				dir = "Left";
			}
			else if (base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			})[this.directionIndex][0] == 'D')
			{
				base.animator.SetInteger("Direction", 3);
				this.jack.transform.position = this.bottomRoot.transform.position;
				this.jack.transform.SetEulerAngles(null, null, new float?(90f));
				this.jack.GetComponent<SpriteRenderer>().sortingOrder = -5;
				dir = "Down";
			}
			else if (base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			})[this.directionIndex][0] == 'R')
			{
				base.animator.SetInteger("Direction", 4);
				this.jack.transform.position = this.rightRoot.transform.position;
				this.jack.transform.SetEulerAngles(null, null, new float?(180f));
				this.jack.GetComponent<SpriteRenderer>().sortingOrder = -5;
				dir = "Right";
			}
			base.animator.SetTrigger("OnDirection");
			yield return base.animator.WaitForAnimationToStart(this, "Eye_" + dir, 1, false);
			AudioManager.Stop("funhouse_jackbox_eye_spin_loop");
			yield return CupheadTime.WaitForSeconds(this, 0.3f);
			AudioManager.PlayLoop("funhouse_jackbox_eye_spin_loop");
			this.emitAudioFromObject.Add("funhouse_jackbox_eye_spin_loop");
			base.animator.SetTrigger("OnHead");
			yield return base.animator.WaitForAnimationToEnd(this, "Jack_Head", 3, false, true);
			if (!this.justDied)
			{
				this.shootTime = true;
				this.shootTime = false;
			}
			yield return CupheadTime.WaitForSeconds(this, this.DieTime());
			this.directionIndex = (this.directionIndex + 1) % base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			}).Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x001E332C File Offset: 0x001E172C
	private void HideSprite()
	{
		switch (base.animator.GetInteger("Direction"))
		{
		case 1:
			this.top.GetComponent<SpriteRenderer>().enabled = false;
			break;
		case 2:
			this.left.GetComponent<SpriteRenderer>().enabled = false;
			break;
		case 3:
			this.bottom.GetComponent<SpriteRenderer>().enabled = false;
			break;
		case 4:
			this.right.GetComponent<SpriteRenderer>().enabled = false;
			break;
		}
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x001E33C0 File Offset: 0x001E17C0
	private void SlideSprite()
	{
		switch (base.animator.GetInteger("Direction"))
		{
		case 1:
			base.StartCoroutine(this.slide_in(this.top, Vector3.up));
			break;
		case 2:
			base.StartCoroutine(this.slide_in(this.left, Vector3.left));
			break;
		case 3:
			base.StartCoroutine(this.slide_in(this.bottom, Vector3.down));
			break;
		case 4:
			base.StartCoroutine(this.slide_in(this.right, Vector3.right));
			break;
		}
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x001E3470 File Offset: 0x001E1870
	private void ShootProjectile()
	{
		if (!this.justDied)
		{
			this.player = PlayerManager.GetNext();
			this.projectile.Create(this.jackRoot.transform.position, base.Properties.ProjectileSpeed, base.Properties.jackinShootDelay, this.player, base.animator.GetInteger("Direction"));
			AudioManager.Play("funhouse_jackbox_shoot");
			this.emitAudioFromObject.Add("funhouse_jackbox_shoot");
		}
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x001E34F8 File Offset: 0x001E18F8
	private IEnumerator slide_in(Transform sprite, Vector3 direction)
	{
		Vector3 startPos = sprite.transform.position + -direction * 100f;
		Vector3 endPos = sprite.transform.position;
		sprite.transform.position = startPos;
		float t = 0f;
		float time = 1f;
		sprite.GetComponent<SpriteRenderer>().enabled = true;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0f, 1f, t / time);
			sprite.transform.position = Vector3.Lerp(startPos, endPos, val);
			yield return null;
		}
		AudioManager.Play("funhouse_jackbox_shoot_launch");
		this.emitAudioFromObject.Add("funhouse_jackbox_shoot_launch");
		yield return null;
		yield break;
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x001E3524 File Offset: 0x001E1924
	private float DieTime()
	{
		return (!this.justDied) ? base.Properties.jackinAppearDelay : base.Properties.jackinDeathAppearDelay;
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x001E355E File Offset: 0x001E195E
	protected override void Die()
	{
		this.justDied = true;
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x001E3567 File Offset: 0x001E1967
	private void SoundJackInBoxHeadPop()
	{
		AudioManager.Play("funhouse_jackbox_jack_head");
		this.emitAudioFromObject.Add("funhouse_jackbox_jack_head");
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x001E3583 File Offset: 0x001E1983
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
	}

	// Token: 0x04003C4D RID: 15437
	[SerializeField]
	private FunhousePlatformingLevelJackinBoxProjectile projectile;

	// Token: 0x04003C4E RID: 15438
	[SerializeField]
	private GameObject jack;

	// Token: 0x04003C4F RID: 15439
	[SerializeField]
	private Transform jackRoot;

	// Token: 0x04003C50 RID: 15440
	[SerializeField]
	private Transform topRoot;

	// Token: 0x04003C51 RID: 15441
	[SerializeField]
	private Transform bottomRoot;

	// Token: 0x04003C52 RID: 15442
	[SerializeField]
	private Transform rightRoot;

	// Token: 0x04003C53 RID: 15443
	[SerializeField]
	private Transform leftRoot;

	// Token: 0x04003C54 RID: 15444
	[SerializeField]
	private Transform top;

	// Token: 0x04003C55 RID: 15445
	[SerializeField]
	private Transform bottom;

	// Token: 0x04003C56 RID: 15446
	[SerializeField]
	private Transform right;

	// Token: 0x04003C57 RID: 15447
	[SerializeField]
	private Transform left;

	// Token: 0x04003C58 RID: 15448
	private int directionIndex;

	// Token: 0x04003C59 RID: 15449
	private bool justDied;

	// Token: 0x04003C5A RID: 15450
	private bool shootTime;

	// Token: 0x04003C5B RID: 15451
	private float offset = 50f;

	// Token: 0x04003C5C RID: 15452
	private AbstractPlayerController player;
}
