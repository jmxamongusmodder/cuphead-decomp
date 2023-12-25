using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class DevilLevelHand : AbstractCollidableObject
{
	// Token: 0x06001A9B RID: 6811 RVA: 0x000F3DF0 File Offset: 0x000F21F0
	protected override void Awake()
	{
		base.Awake();
		this.isDead = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.demonSprite.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.demonSprite.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x000F3E4E File Offset: 0x000F224E
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x000F3E66 File Offset: 0x000F2266
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.isInvincible)
		{
			return;
		}
		this.hp -= info.damage;
		if (this.hp < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x000F3E9D File Offset: 0x000F229D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x000F3EC8 File Offset: 0x000F22C8
	public void StartPattern(LevelProperties.Devil.Hands properties)
	{
		this.properties = properties;
		this.pinkStringIndex = UnityEngine.Random.Range(0, properties.pinkString.Length);
		this.maxHp = properties.HP;
		this.hp = this.maxHp;
		this.state = DevilLevelHand.State.Idle;
		this.startPos = new Vector2(base.transform.position.x, properties.yRange.max);
		base.transform.position = this.startPos;
		this.handLocalStartPos = this.handSprite.transform.localPosition;
		this.demonLocalStartPos = this.demonSprite.transform.localPosition;
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x000F3F96 File Offset: 0x000F2396
	public void SpawnIn()
	{
		base.StartCoroutine(this.move_in_cr());
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x000F3FA8 File Offset: 0x000F23A8
	private IEnumerator move_in_cr()
	{
		this.startAtTop = true;
		this.despawned = false;
		base.transform.position = this.startPos;
		this.handSprite.transform.localPosition = this.handLocalStartPos;
		this.demonSprite.transform.localPosition = this.demonLocalStartPos;
		base.animator.Play("Off", 1);
		base.animator.Play("Hand_Loop");
		float xPos = 547f;
		Vector3 start = new Vector3(this.startPos.x, this.properties.yRange.max);
		Vector3 end = new Vector3((!this.onLeft) ? xPos : (-xPos), this.properties.yRange.max);
		float t = 0f;
		float time = 1f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(start, end, t / time);
			yield return wait;
		}
		this.isInvincible = false;
		base.StartCoroutine(this.hand_move_up_cr());
		yield break;
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x000F3FC4 File Offset: 0x000F23C4
	private IEnumerator move_cr()
	{
		float moveTime = (this.properties.yRange.max - this.properties.yRange.min) / this.properties.speed;
		float t = 0f;
		float startY = this.demonSprite.transform.position.y;
		float endY = this.properties.yRange.min;
		for (;;)
		{
			while (!this.isSliding)
			{
				yield return null;
			}
			startY = this.demonSprite.transform.position.y;
			endY = this.properties.yRange.min;
			this.startAtTop = false;
			while (this.isSliding)
			{
				t = 0f;
				while (t < moveTime && this.isSliding)
				{
					this.demonSprite.transform.SetPosition(null, new float?(Mathf.Lerp(startY, endY, t / moveTime)), null);
					t += CupheadTime.FixedDelta;
					yield return new WaitForFixedUpdate();
				}
				startY = ((!this.startAtTop) ? this.properties.yRange.min : this.properties.yRange.max);
				endY = ((!this.startAtTop) ? this.properties.yRange.max : this.properties.yRange.min);
				this.startAtTop = !this.startAtTop;
				yield return new WaitForFixedUpdate();
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x000F3FE0 File Offset: 0x000F23E0
	public void Fire()
	{
		if (this.properties.pinkString[this.pinkStringIndex] == 'P')
		{
			BasicProjectile basicProjectile = this.bulletPinkPrefab.Create(this.bulletRoot.position, this.shootAngle, this.properties.bulletSpeed);
			basicProjectile.transform.SetScale(new float?((float)((!this.onLeft) ? 1 : 1)), new float?((float)((!this.onLeft) ? -1 : 1)), null);
		}
		else
		{
			BasicProjectile basicProjectile2 = this.bulletPrefab.Create(this.bulletRoot.position, this.shootAngle, this.properties.bulletSpeed);
			basicProjectile2.transform.SetScale(new float?((float)((!this.onLeft) ? 1 : 1)), new float?((float)((!this.onLeft) ? -1 : 1)), null);
		}
		this.pinkStringIndex = (this.pinkStringIndex + 1) % this.properties.pinkString.Length;
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x000F4110 File Offset: 0x000F2510
	public void Die()
	{
		this.isSliding = false;
		this.isInvincible = true;
		base.StartCoroutine(this.demon_move_down_cr());
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000F4130 File Offset: 0x000F2530
	private IEnumerator hand_move_up_cr()
	{
		base.animator.SetTrigger("OnRelease");
		yield return base.animator.WaitForAnimationToEnd(this, "Hand_Release_Start", false, true);
		this.isSliding = true;
		float t = 0f;
		float time = 0.5f;
		float start = this.handSprite.transform.position.y;
		float end = 860f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			this.handSprite.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, t / time)), null);
			yield return wait;
		}
		yield return wait;
		yield break;
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x000F414C File Offset: 0x000F254C
	private IEnumerator demon_move_down_cr()
	{
		base.animator.SetTrigger("OnDeath");
		float t = 0f;
		float time = 1f;
		float start = this.demonSprite.transform.position.y;
		float end = -860f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			this.demonSprite.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, t / time)), null);
			yield return wait;
		}
		yield return wait;
		if (this.isDead)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.despawned = true;
		this.hp = this.maxHp;
		yield break;
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x000F4167 File Offset: 0x000F2567
	private void SFXAttack()
	{
		AudioManager.Play("fat_bat_attack");
		this.emitAudioFromObject.Add("fat_bat_attack");
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000F4183 File Offset: 0x000F2583
	private void SFXDeath()
	{
		AudioManager.Play("fat_bat_die");
		this.emitAudioFromObject.Add("fat_bat_die");
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000F419F File Offset: 0x000F259F
	private void SFXHandRelease()
	{
		AudioManager.Play("p3_hand_release_start");
		this.emitAudioFromObject.Add("p3_hand_release_start");
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x000F41BB File Offset: 0x000F25BB
	private void SFXFatSpawn()
	{
		AudioManager.Play("fat_bat_spawn");
		this.emitAudioFromObject.Add("fat_bat_spawn");
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000F41D7 File Offset: 0x000F25D7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bulletPrefab = null;
		this.bulletPinkPrefab = null;
	}

	// Token: 0x040023C7 RID: 9159
	public DevilLevelHand.State state;

	// Token: 0x040023C8 RID: 9160
	public bool despawned;

	// Token: 0x040023C9 RID: 9161
	public bool isDead;

	// Token: 0x040023CA RID: 9162
	[SerializeField]
	private bool onLeft;

	// Token: 0x040023CB RID: 9163
	[SerializeField]
	private float shootAngle;

	// Token: 0x040023CC RID: 9164
	[SerializeField]
	private Transform bulletRoot;

	// Token: 0x040023CD RID: 9165
	[SerializeField]
	private BasicProjectile bulletPrefab;

	// Token: 0x040023CE RID: 9166
	[SerializeField]
	private BasicProjectile bulletPinkPrefab;

	// Token: 0x040023CF RID: 9167
	[Header("Sprites")]
	[SerializeField]
	private SpriteRenderer demonSprite;

	// Token: 0x040023D0 RID: 9168
	private Vector3 demonLocalStartPos;

	// Token: 0x040023D1 RID: 9169
	[SerializeField]
	private SpriteRenderer handSprite;

	// Token: 0x040023D2 RID: 9170
	private Vector3 handLocalStartPos;

	// Token: 0x040023D3 RID: 9171
	private LevelProperties.Devil.Hands properties;

	// Token: 0x040023D4 RID: 9172
	private DamageReceiver damageReceiver;

	// Token: 0x040023D5 RID: 9173
	private DamageDealer damageDealer;

	// Token: 0x040023D6 RID: 9174
	private Vector3 startPos;

	// Token: 0x040023D7 RID: 9175
	private float hp;

	// Token: 0x040023D8 RID: 9176
	private float maxHp;

	// Token: 0x040023D9 RID: 9177
	private bool isInvincible = true;

	// Token: 0x040023DA RID: 9178
	private bool isSliding;

	// Token: 0x040023DB RID: 9179
	private bool startAtTop = true;

	// Token: 0x040023DC RID: 9180
	private int pinkStringIndex;

	// Token: 0x02000578 RID: 1400
	public enum State
	{
		// Token: 0x040023DE RID: 9182
		Uninitialized,
		// Token: 0x040023DF RID: 9183
		Idle
	}
}
