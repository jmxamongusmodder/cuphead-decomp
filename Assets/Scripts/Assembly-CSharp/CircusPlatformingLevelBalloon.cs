using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200089A RID: 2202
public class CircusPlatformingLevelBalloon : AbstractPlatformingLevelEnemy
{
	// Token: 0x0600333C RID: 13116 RVA: 0x001DD41A File Offset: 0x001DB81A
	protected override void OnStart()
	{
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x001DD41C File Offset: 0x001DB81C
	public void Init(Vector2 pos, float rotation, string spreadCount, string c)
	{
		base.transform.position = pos;
		this.rotation = rotation;
		this.spreadCount = spreadCount;
		this.SetColor(c);
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x001DD448 File Offset: 0x001DB848
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.movement_cr());
		base.StartCoroutine(this.idle_audio_delayer_cr(this.idleSoundSelected, 2f, 4f));
		this.emitAudioFromObject.Add(this.idleSoundSelected);
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x001DD498 File Offset: 0x001DB898
	private void SetColor(string c)
	{
		if (c != null)
		{
			if (!(c == "B"))
			{
				if (!(c == "G"))
				{
					if (c == "P")
					{
						base.animator.Play("PinkIdle");
						this.idleSoundSelected = "circus_balloon_girl_idle";
						this._canParry = true;
					}
				}
				else
				{
					base.animator.Play("GirlIdle");
					this.idleSoundSelected = "circus_balloon_girl_idle";
				}
			}
			else
			{
				base.animator.Play("BoyIdle");
				this.idleSoundSelected = "circus_balloon_boy_idle";
			}
		}
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x001DD548 File Offset: 0x001DB948
	private IEnumerator movement_cr()
	{
		float angle = 0f;
		Vector3 xVelocity = Vector3.zero;
		for (;;)
		{
			angle += base.Properties.flyingFishSinVelocity * CupheadTime.Delta;
			xVelocity = ((this.rotation != 180f) ? base.transform.right : (-base.transform.right));
			Vector3 moveY = new Vector3(0f, Mathf.Sin(angle) * CupheadTime.Delta * 60f * base.Properties.flyingFishSinSize);
			Vector3 moveX = xVelocity * base.Properties.flyingFishVelocity * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				Vector3 position = base.transform.position + moveX + moveY;
				position.z = -position.x;
				base.transform.position = position;
			}
			if (base.transform.position.x < (float)(Level.Current.Left - 150))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x001DD563 File Offset: 0x001DB963
	protected override void Die()
	{
		if (base.Health <= 0f)
		{
			AudioManager.Play("circus_balloon_hit");
			this.emitAudioFromObject.Add("circus_balloon_hit");
			base.animator.SetTrigger("Death");
		}
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x001DD5A0 File Offset: 0x001DB9A0
	public void ExplodeBalloon()
	{
		string[] array = this.spreadCount.Split(new char[]
		{
			','
		});
		float angle = 0f;
		for (int i = 0; i < array.Length; i++)
		{
			Parser.FloatTryParse(array[i], out angle);
			this.SpawnBullet(angle);
		}
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x001DD5F0 File Offset: 0x001DB9F0
	public void OnEndDeathAnim()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x001DD5FD File Offset: 0x001DB9FD
	private void SpawnBullet(float angle)
	{
		this.projectile.Create(base.transform.position, angle, this.bulletSpeed);
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x001DD622 File Offset: 0x001DBA22
	private void SoundBalloonDeathAnim()
	{
		AudioManager.Play("circus_balloon_death");
		this.emitAudioFromObject.Add("circus_balloon_death");
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x001DD63E File Offset: 0x001DBA3E
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this._canParry = false;
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x001DD65C File Offset: 0x001DBA5C
	private IEnumerator parryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this._canParry = true;
		yield return null;
		yield break;
	}

	// Token: 0x04003B7C RID: 15228
	private const string Blue = "B";

	// Token: 0x04003B7D RID: 15229
	private const string Green = "G";

	// Token: 0x04003B7E RID: 15230
	private const string Pink = "P";

	// Token: 0x04003B7F RID: 15231
	private const string DeathParameterName = "Death";

	// Token: 0x04003B80 RID: 15232
	private const string BoyIdle = "BoyIdle";

	// Token: 0x04003B81 RID: 15233
	private const string GirlIdle = "GirlIdle";

	// Token: 0x04003B82 RID: 15234
	private const string PinkIdle = "PinkIdle";

	// Token: 0x04003B83 RID: 15235
	private const string BoyIdleSound = "circus_balloon_boy_idle";

	// Token: 0x04003B84 RID: 15236
	private const string GirlIdleSound = "circus_balloon_girl_idle";

	// Token: 0x04003B85 RID: 15237
	[SerializeField]
	private float bulletSpeed;

	// Token: 0x04003B86 RID: 15238
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04003B87 RID: 15239
	[SerializeField]
	private float coolDown = 0.4f;

	// Token: 0x04003B88 RID: 15240
	private float rotation;

	// Token: 0x04003B89 RID: 15241
	private string spreadCount;

	// Token: 0x04003B8A RID: 15242
	private string idleSoundSelected;
}
