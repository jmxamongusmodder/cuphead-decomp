using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000789 RID: 1929
public class RumRunnersLevelCopBall : AbstractProjectile
{
	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06002A92 RID: 10898 RVA: 0x0018DDC2 File Offset: 0x0018C1C2
	// (set) Token: 0x06002A93 RID: 10899 RVA: 0x0018DDCA File Offset: 0x0018C1CA
	public bool leaveScreen { get; set; }

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06002A94 RID: 10900 RVA: 0x0018DDD3 File Offset: 0x0018C1D3
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x0018DDDA File Offset: 0x0018C1DA
	protected override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x0018DE08 File Offset: 0x0018C208
	public void Init(Vector3 position, Vector3 velocity, float speed, float health, LevelProperties.RumRunners.CopBall properties, Transform snoutPos)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.properties = properties;
		this.health = health;
		this.velocity = velocity;
		this.offset = base.GetComponent<Collider2D>().bounds.size.x / 2f;
		this.leaveScreen = false;
		this.circleCollider.enabled = false;
		this.launched = false;
		this.snoutPos = snoutPos;
		if (properties.constSpeed)
		{
			this.speed = speed;
		}
		else
		{
			base.StartCoroutine(this.gradualSpeed_cr());
		}
		RumRunnersLevelCopBall.LastSortingIndex--;
		if (RumRunnersLevelCopBall.LastSortingIndex < 10)
		{
			RumRunnersLevelCopBall.LastSortingIndex = 15;
		}
		base.GetComponent<SpriteRenderer>().sortingOrder = RumRunnersLevelCopBall.LastSortingIndex;
		this.audioLoopNumber = RumRunnersLevelCopBall.CurrentAudioLoopIndex + 1;
		RumRunnersLevelCopBall.CurrentAudioLoopIndex = MathUtilities.NextIndex(RumRunnersLevelCopBall.CurrentAudioLoopIndex, RumRunnersLevelCopBall.AudioLoopCount);
		this.SFX_RUMRUN_P3_BallCop_VocalShouts_Loop(this.audioLoopNumber);
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x0018DF0E File Offset: 0x0018C30E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x0018DF2C File Offset: 0x0018C32C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Death(true);
		}
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x0018DF64 File Offset: 0x0018C364
	public void Launch()
	{
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.checkToDie_cr());
		this.circleCollider.enabled = true;
		base.GetComponent<SpriteRenderer>().sortingLayerName = "Projectiles";
		this.launched = true;
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x0018DFBB File Offset: 0x0018C3BB
	private void LateUpdate()
	{
		if (!this.launched)
		{
			base.transform.position = this.snoutPos.position;
		}
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x0018DFE0 File Offset: 0x0018C3E0
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.position += this.velocity * this.speed * CupheadTime.FixedDelta;
			this.CheckBounds();
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x0018DFFC File Offset: 0x0018C3FC
	private void CheckBounds()
	{
		bool flag = this.properties.sideWallBounce && !this.leaveScreen;
		Quaternion? quaternion = null;
		Vector3 zero = Vector3.zero;
		if (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - this.offset && this.velocity.y > 0f)
		{
			this.velocity.y = -Mathf.Abs(this.velocity.y);
			quaternion = new Quaternion?(Quaternion.identity);
			zero = new Vector3(0f, this.offset);
			this.SFX_RUMRUN_P3_BallCop_Bounce();
		}
		if (base.transform.position.y < (float)Level.Current.Ground + this.offset && this.velocity.y < 0f)
		{
			this.velocity.y = Mathf.Abs(this.velocity.y);
			quaternion = new Quaternion?(Quaternion.Euler(0f, 0f, 180f));
			zero = new Vector3(0f, -this.offset);
			this.SFX_RUMRUN_P3_BallCop_Bounce();
		}
		if (flag && base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax - this.offset && this.velocity.x > 0f)
		{
			this.velocity.x = -Mathf.Abs(this.velocity.x);
			quaternion = new Quaternion?(Quaternion.Euler(0f, 0f, 270f));
			zero = new Vector3(this.offset, 0f);
			this.SFX_RUMRUN_P3_BallCop_Bounce();
		}
		if (flag && base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin + this.offset && this.velocity.x < 0f)
		{
			this.velocity.x = Mathf.Abs(this.velocity.x);
			quaternion = new Quaternion?(Quaternion.Euler(0f, 0f, 90f));
			zero = new Vector3(-this.offset, 0f);
			this.SFX_RUMRUN_P3_BallCop_Bounce();
		}
		if (quaternion != null)
		{
			Effect effect = this.dustEffect.Create(base.transform.position + zero);
			effect.transform.rotation = quaternion.Value;
			if (quaternion.Value == Quaternion.identity)
			{
				effect.transform.Find("Dirt").gameObject.SetActive(true);
				effect.animator.SetInteger("DirtEffect", UnityEngine.Random.Range(0, 3));
			}
		}
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x0018E30C File Offset: 0x0018C70C
	private IEnumerator shoot_cr()
	{
		int copBallBulletAngleStringMainIndex = UnityEngine.Random.Range(0, this.properties.copBallBulletAngleString.Length);
		string[] copBallBulletAngleString = this.properties.copBallBulletAngleString[copBallBulletAngleStringMainIndex].Split(new char[]
		{
			','
		});
		int copBallBulletAngleStringIndex = UnityEngine.Random.Range(0, copBallBulletAngleString.Length);
		int copBallBulletTypeStringMainIndex = UnityEngine.Random.Range(0, this.properties.copBallBulletTypeString.Length);
		string[] copBallBulletTypeString = this.properties.copBallBulletTypeString[copBallBulletTypeStringMainIndex].Split(new char[]
		{
			','
		});
		int copBallBulletTypeStringIndex = UnityEngine.Random.Range(0, copBallBulletTypeString.Length);
		yield return CupheadTime.WaitForSeconds(this, this.properties.copBallShootHesitate);
		for (;;)
		{
			BasicProjectile bullet;
			if (copBallBulletTypeString[copBallBulletTypeStringIndex][0] == 'P')
			{
				bullet = this.copBulletPink;
			}
			else
			{
				bullet = this.copBullet;
			}
			float angle = 0f;
			Parser.FloatTryParse(copBallBulletAngleString[copBallBulletAngleStringIndex], out angle);
			bullet.Create(base.transform.position, angle, this.properties.copBallBulletSpeed);
			yield return CupheadTime.WaitForSeconds(this, this.properties.copBallShootDelay);
			if (copBallBulletAngleStringIndex < copBallBulletAngleString.Length - 1)
			{
				copBallBulletAngleStringIndex++;
			}
			else
			{
				copBallBulletAngleStringMainIndex = (copBallBulletAngleStringMainIndex + 1) % this.properties.copBallBulletAngleString.Length;
				copBallBulletAngleStringIndex = 0;
			}
			if (copBallBulletTypeStringIndex < copBallBulletTypeString.Length - 1)
			{
				copBallBulletTypeStringIndex++;
			}
			else
			{
				copBallBulletTypeStringMainIndex = (copBallBulletTypeStringMainIndex + 1) % this.properties.copBallBulletTypeString.Length;
				copBallBulletTypeStringIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x0018E327 File Offset: 0x0018C727
	public void Death(bool playAudio)
	{
		this.SFX_RUMRUN_P3_BallCop_Die(this.audioLoopNumber, playAudio);
		this.deathEffect.Create(base.transform.position);
		this.StopAllCoroutines();
		this.Recycle<RumRunnersLevelCopBall>();
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x0018E35C File Offset: 0x0018C75C
	private IEnumerator checkToDie_cr()
	{
		while (base.transform.position.x >= -1140f && base.transform.position.x <= 1140f)
		{
			yield return null;
		}
		this.Death(false);
		yield break;
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x0018E378 File Offset: 0x0018C778
	private IEnumerator gradualSpeed_cr()
	{
		float t = 0f;
		float time = this.properties.gradualSpeedTime;
		float val = 1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.speed = this.properties.gradualSpeed.GetFloatAt(val - t / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x0018E393 File Offset: 0x0018C793
	private void SFX_RUMRUN_P3_BallCop_Bounce()
	{
		AudioManager.Play("sfx_dlc_rumrun_copball_bounce");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_copball_bounce");
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x0018E3B0 File Offset: 0x0018C7B0
	private void SFX_RUMRUN_P3_BallCop_VocalShouts_Loop(int loopNumber)
	{
		string key = "sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop_" + loopNumber;
		AudioManager.PlayLoop(key);
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop");
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x0018E3E4 File Offset: 0x0018C7E4
	private void SFX_RUMRUN_P3_BallCop_Die(int loopNumber, bool playAudio)
	{
		string key = "sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop_" + loopNumber;
		AudioManager.Stop(key);
		if (playAudio)
		{
			AudioManager.Play("sfx_dlc_rumrun_copball_bounce");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_copball_bounce");
		}
	}

	// Token: 0x04003358 RID: 13144
	private const float DIE_OFFSET_X = 500f;

	// Token: 0x04003359 RID: 13145
	private static readonly int AudioLoopCount = 6;

	// Token: 0x0400335A RID: 13146
	private static int CurrentAudioLoopIndex;

	// Token: 0x0400335B RID: 13147
	private static int LastSortingIndex;

	// Token: 0x0400335C RID: 13148
	[SerializeField]
	private BasicProjectile copBullet;

	// Token: 0x0400335D RID: 13149
	[SerializeField]
	private BasicProjectile copBulletPink;

	// Token: 0x0400335E RID: 13150
	[SerializeField]
	private Effect dustEffect;

	// Token: 0x0400335F RID: 13151
	[SerializeField]
	private Effect deathEffect;

	// Token: 0x04003360 RID: 13152
	private LevelProperties.RumRunners.CopBall properties;

	// Token: 0x04003361 RID: 13153
	private Vector3 velocity;

	// Token: 0x04003362 RID: 13154
	private DamageReceiver damageReceiver;

	// Token: 0x04003363 RID: 13155
	[SerializeField]
	private CircleCollider2D circleCollider;

	// Token: 0x04003364 RID: 13156
	private float health;

	// Token: 0x04003365 RID: 13157
	private float speed;

	// Token: 0x04003366 RID: 13158
	private float offset;

	// Token: 0x04003367 RID: 13159
	private int audioLoopNumber;

	// Token: 0x04003368 RID: 13160
	private bool launched;

	// Token: 0x04003369 RID: 13161
	private Transform snoutPos;
}
