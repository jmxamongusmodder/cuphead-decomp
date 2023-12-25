using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F0 RID: 1264
public class BaronessLevelJawbreaker : BaronessLevelMiniBossBase
{
	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06001617 RID: 5655 RVA: 0x000C6295 File Offset: 0x000C4695
	// (set) Token: 0x06001618 RID: 5656 RVA: 0x000C629D File Offset: 0x000C469D
	public BaronessLevelJawbreaker.State state { get; private set; }

	// Token: 0x06001619 RID: 5657 RVA: 0x000C62A8 File Offset: 0x000C46A8
	protected override void Awake()
	{
		base.Awake();
		this.isDying = false;
		this.state = BaronessLevelJawbreaker.State.Spawned;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x000C62F8 File Offset: 0x000C46F8
	protected override void Start()
	{
		base.Start();
		this.sprite.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 150;
		base.StartCoroutine(this.check_rotation_cr());
		base.StartCoroutine(this.switch_cr());
		base.StartCoroutine(this.reset_sprite_cr());
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x000C6368 File Offset: 0x000C4768
	private IEnumerator switch_cr()
	{
		base.StartCoroutine(this.fade_color_cr());
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.sprite.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 251;
		yield break;
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x000C6383 File Offset: 0x000C4783
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x000C63A4 File Offset: 0x000C47A4
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		if (this.aim == null || this.player == null || this.state == BaronessLevelJawbreaker.State.Unspawned)
		{
			return;
		}
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x000C6424 File Offset: 0x000C4824
	private void FixedUpdate()
	{
		if (this.state == BaronessLevelJawbreaker.State.Spawned)
		{
			base.transform.position -= base.transform.right * this.properties.jawbreakerHomingSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			this.aim.LookAt2D(2f * base.transform.position - this.player.center);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.aim.rotation, this.rotationSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
		}
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x000C64EC File Offset: 0x000C48EC
	private IEnumerator check_rotation_cr()
	{
		for (;;)
		{
			if (((this.player.transform.position.x < base.transform.position.x && !this.lookingLeft) || (this.player.transform.position.x > base.transform.position.x && this.lookingLeft)) && !this.isTurning)
			{
				this.isTurning = true;
				base.animator.SetTrigger("Turn");
				yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
				this.lookingLeft = !this.lookingLeft;
				this.isTurning = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x000C6508 File Offset: 0x000C4908
	private void Turn()
	{
		this.sprite.transform.SetScale(new float?(-this.sprite.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x000C6558 File Offset: 0x000C4958
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health < 0f && this.state == BaronessLevelJawbreaker.State.Spawned)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			base.StartCoroutine(this.stopminis_cr());
			this.StartDeath();
		}
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x000C65E4 File Offset: 0x000C49E4
	public void Init(LevelProperties.Baroness.Jawbreaker properties, AbstractPlayerController player, Vector2 pos, float rotationSpeed, float health)
	{
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
		this.properties = properties;
		this.player = player;
		this.rotationSpeed = rotationSpeed;
		this.health = health;
		base.transform.position = pos;
		this.spawnPos = base.transform.position;
		base.StartCoroutine(this.pickplayer_cr());
		this.minisRoutine = base.StartCoroutine(this.minis_cr());
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x000C6684 File Offset: 0x000C4A84
	private IEnumerator pickplayer_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.jawbreakerHomeDuration);
			this.player = PlayerManager.GetNext();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x000C669F File Offset: 0x000C4A9F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.miniBluePrefab = null;
		this.miniRedPrefab = null;
		this.ghostPrefab = null;
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x000C66BC File Offset: 0x000C4ABC
	private IEnumerator minis_cr()
	{
		this.targetPos = this.followPoint;
		Transform targetPos2 = this.targetPos;
		this.prefabsList = new List<BaronessLevelJawbreakerMini>();
		float spawnTime = this.properties.jawbreakerMiniSpace / this.properties.jawbreakerHomingSpeed;
		for (int i = 0; i < this.properties.jawbreakerMinis; i++)
		{
			if (i % 2 == 0)
			{
				yield return CupheadTime.WaitForSeconds(this, spawnTime);
				BaronessLevelJawbreakerMini blueminijawbreakers = UnityEngine.Object.Instantiate<BaronessLevelJawbreakerMini>(this.miniBluePrefab);
				blueminijawbreakers.Init(this.properties, this.spawnPos, this.targetPos, this.rotationSpeed);
				targetPos2 = blueminijawbreakers.transform;
				this.prefabsList.Add(blueminijawbreakers);
			}
			else if (i % 2 == 1)
			{
				yield return CupheadTime.WaitForSeconds(this, spawnTime);
				BaronessLevelJawbreakerMini redminijawbreakers = UnityEngine.Object.Instantiate<BaronessLevelJawbreakerMini>(this.miniRedPrefab);
				redminijawbreakers.Init(this.properties, this.spawnPos, targetPos2, this.rotationSpeed);
				this.targetPos = redminijawbreakers.transform;
				this.prefabsList.Add(redminijawbreakers);
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x000C66D8 File Offset: 0x000C4AD8
	private IEnumerator stopminis_cr()
	{
		base.StopCoroutine(this.minisRoutine);
		for (int i = 0; i < this.prefabsList.Count; i++)
		{
			this.prefabsList[i].Stop();
			yield return null;
		}
		base.StartCoroutine(this.killminis_cr());
		yield break;
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x000C66F4 File Offset: 0x000C4AF4
	private IEnumerator killminis_cr()
	{
		this.prefabsList.Reverse();
		for (int i = 0; i < this.prefabsList.Count; i++)
		{
			this.prefabsList[i].StartDying();
			yield return CupheadTime.WaitForSeconds(this, 0.8f);
		}
		yield break;
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x000C670F File Offset: 0x000C4B0F
	public void StartDeath()
	{
		this.state = BaronessLevelJawbreaker.State.Explode;
		base.StartCoroutine(this.dying_cr());
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x000C6728 File Offset: 0x000C4B28
	public IEnumerator dying_cr()
	{
		this.StartExplosions();
		this.isDying = true;
		base.transform.rotation = Quaternion.identity;
		base.animator.SetTrigger("Dead");
		base.GetComponent<Collider2D>().enabled = false;
		yield return base.animator.WaitForAnimationToEnd(this, "Death", false, true);
		BaronessLevelJawbreakerGhost ghost = UnityEngine.Object.Instantiate<BaronessLevelJawbreakerGhost>(this.ghostPrefab);
		ghost.transform.position = base.transform.position;
		this.Die();
		yield break;
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x000C6744 File Offset: 0x000C4B44
	private IEnumerator reset_sprite_cr()
	{
		for (;;)
		{
			this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x000C675F File Offset: 0x000C4B5F
	private void SoundJawbreakerMouth()
	{
		AudioManager.Play("level_baroness_large_jawbreaker_mouth");
		this.emitAudioFromObject.Add("level_baroness_large_jawbreaker_mouth");
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x000C677B File Offset: 0x000C4B7B
	private void SoundJawbreakerDeath()
	{
		AudioManager.Stop("level_baroness_large_jawbreaker_mouth");
		AudioManager.Play("level_baroness_large_jawbreaker_death");
	}

	// Token: 0x04001F60 RID: 8032
	private const float ROTATE_FRAME_TIME = 0.083333336f;

	// Token: 0x04001F62 RID: 8034
	[SerializeField]
	private Transform sprite;

	// Token: 0x04001F63 RID: 8035
	[SerializeField]
	private BaronessLevelJawbreakerMini miniBluePrefab;

	// Token: 0x04001F64 RID: 8036
	[SerializeField]
	private BaronessLevelJawbreakerMini miniRedPrefab;

	// Token: 0x04001F65 RID: 8037
	[SerializeField]
	private Transform followPoint;

	// Token: 0x04001F66 RID: 8038
	[SerializeField]
	private BaronessLevelJawbreakerGhost ghostPrefab;

	// Token: 0x04001F67 RID: 8039
	private List<BaronessLevelJawbreakerMini> prefabsList;

	// Token: 0x04001F68 RID: 8040
	private LevelProperties.Baroness.Jawbreaker properties;

	// Token: 0x04001F69 RID: 8041
	private AbstractPlayerController player;

	// Token: 0x04001F6A RID: 8042
	private DamageDealer damageDealer;

	// Token: 0x04001F6B RID: 8043
	private DamageReceiver damageReceiver;

	// Token: 0x04001F6C RID: 8044
	private float health;

	// Token: 0x04001F6D RID: 8045
	private float rotationSpeed;

	// Token: 0x04001F6E RID: 8046
	private bool lookingLeft = true;

	// Token: 0x04001F6F RID: 8047
	private bool isTurning;

	// Token: 0x04001F70 RID: 8048
	private Transform aim;

	// Token: 0x04001F71 RID: 8049
	private Transform targetPos;

	// Token: 0x04001F72 RID: 8050
	private Vector3 spawnPos;

	// Token: 0x04001F73 RID: 8051
	private Vector3 deathPosition;

	// Token: 0x04001F74 RID: 8052
	private Coroutine minisRoutine;

	// Token: 0x020004F1 RID: 1265
	public enum State
	{
		// Token: 0x04001F76 RID: 8054
		Unspawned,
		// Token: 0x04001F77 RID: 8055
		Spawned,
		// Token: 0x04001F78 RID: 8056
		Explode,
		// Token: 0x04001F79 RID: 8057
		Ghost
	}
}
