using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000613 RID: 1555
public class FlowerLevelVenusSpawn : AbstractCollidableObject
{
	// Token: 0x06001F67 RID: 8039 RVA: 0x001206BC File Offset: 0x0011EABC
	public void OnVenusSpawn(FlowerLevelFlower parent, int hp, float rotSpeed, int moveSpeed, float rotDelay)
	{
		AudioManager.Play("flower_venus_a_chomp");
		this.rotationDelay = rotDelay;
		this.rotationSpeed = rotSpeed;
		this.movementSpeed = moveSpeed;
		this.lockRotation = false;
		this.currentHP = (float)hp;
		this.parent = parent;
		this.parent.OnDeathEvent += this.Die;
		base.animator.SetInteger("Variant", UnityEngine.Random.Range(0, 2));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001F68 RID: 8040 RVA: 0x0012073B File Offset: 0x0011EB3B
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.currentHP -= info.damage;
		if (this.currentHP <= 0f)
		{
			this.Die();
			this.damageReceiver.enabled = false;
		}
	}

	// Token: 0x06001F69 RID: 8041 RVA: 0x00120774 File Offset: 0x0011EB74
	private IEnumerator spawnPetals_cr()
	{
		yield return new WaitForEndOfFrame();
		yield return CupheadTime.WaitForSeconds(this, base.animator.GetCurrentAnimatorStateInfo(0).length / 4f);
		this.SpawnPetals();
		yield break;
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x00120790 File Offset: 0x0011EB90
	public void SpawnPetals()
	{
		Vector3 vector = base.transform.position + Vector3.up * (float)(UnityEngine.Random.Range(-10, 10) + 70);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.petalA, vector, Quaternion.identity);
		gameObject.GetComponent<Animator>().Play("Plant_LeafA", UnityEngine.Random.Range(0, 1));
		base.StartCoroutine(this.fade_cr(gameObject, 0.8f, 125f, false));
		gameObject = UnityEngine.Object.Instantiate<GameObject>(this.petalB, vector + Vector3.down * 50f, Quaternion.identity);
		gameObject.GetComponent<Animator>().Play("Plant_LeafB");
		base.StartCoroutine(this.fade_cr(gameObject, 1f, 100f, true));
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x00120858 File Offset: 0x0011EC58
	private IEnumerator die_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Death", 0, false, true);
		base.GetComponent<SpriteRenderer>().enabled = false;
		yield break;
	}

	// Token: 0x06001F6C RID: 8044 RVA: 0x00120874 File Offset: 0x0011EC74
	private IEnumerator fade_cr(GameObject petal, float duration, float speed, bool lastPetal = false)
	{
		SpriteRenderer petalSprite = petal.GetComponent<SpriteRenderer>();
		float currentTime = duration;
		float pct = currentTime / duration;
		while (pct >= 0f)
		{
			Color c = petalSprite.material.color;
			c.a = pct;
			petalSprite.material.color = c;
			petalSprite.transform.position += Vector3.down * speed * CupheadTime.Delta;
			currentTime -= CupheadTime.Delta;
			pct = currentTime / duration;
			yield return null;
		}
		UnityEngine.Object.Destroy(petal);
		if (lastPetal)
		{
			this.StopAllCoroutines();
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x001208AC File Offset: 0x0011ECAC
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return base.animator.WaitForAnimationToEnd(this, true);
		for (;;)
		{
			if (!this.lockRotation && this.rotationDelay <= 0f)
			{
				Vector3 vector = PlayerManager.GetNext().center - base.transform.position;
				base.transform.right = Vector3.RotateTowards(base.transform.right, -vector.normalized * base.transform.localScale.x, this.rotationSpeed * CupheadTime.FixedDelta, 0f);
				if (base.transform.localScale.x == 1f)
				{
					if (Vector3.Angle(base.transform.right, -vector.normalized) < 5f)
					{
						this.lockRotation = true;
					}
				}
				else if (Vector3.Angle(base.transform.right, -vector.normalized) > 175f)
				{
					this.lockRotation = true;
				}
			}
			base.transform.position -= base.transform.right * (float)this.movementSpeed * CupheadTime.FixedDelta * base.transform.localScale.x;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x001208C8 File Offset: 0x0011ECC8
	private void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("flower_minion_simple_deathpop");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop");
		base.StartCoroutine(this.die_cr());
		base.StartCoroutine(this.spawnPetals_cr());
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x0012092B File Offset: 0x0011ED2B
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x00120961 File Offset: 0x0011ED61
	private void Update()
	{
		if (this.rotationDelay > 0f)
		{
			this.rotationDelay -= CupheadTime.Delta;
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x001209A0 File Offset: 0x0011EDA0
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x001209BE File Offset: 0x0011EDBE
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		this.Die();
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x06001F73 RID: 8051 RVA: 0x001209CE File Offset: 0x0011EDCE
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.StartCoroutine(this.offScreenDeath_cr());
		base.OnCollisionCeiling(hit, phase);
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x001209E5 File Offset: 0x0011EDE5
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.StartCoroutine(this.offScreenDeath_cr());
		base.OnCollisionWalls(hit, phase);
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x001209FC File Offset: 0x0011EDFC
	protected override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Die;
		this.StopAllCoroutines();
		base.OnDestroy();
		this.petalA = null;
		this.petalB = null;
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x00120A30 File Offset: 0x0011EE30
	private IEnumerator offScreenDeath_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001F77 RID: 8055 RVA: 0x00120A4B File Offset: 0x0011EE4B
	private void RandomiseVariant()
	{
		base.animator.SetInteger("Variant", UnityEngine.Random.Range(0, 3));
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x00120A64 File Offset: 0x0011EE64
	private void VenusGrowEndAudio()
	{
	}

	// Token: 0x04002801 RID: 10241
	[SerializeField]
	private GameObject petalA;

	// Token: 0x04002802 RID: 10242
	[SerializeField]
	private GameObject petalB;

	// Token: 0x04002803 RID: 10243
	private bool lockRotation;

	// Token: 0x04002804 RID: 10244
	private float rotationSpeed;

	// Token: 0x04002805 RID: 10245
	private int movementSpeed;

	// Token: 0x04002806 RID: 10246
	private float rotationDelay;

	// Token: 0x04002807 RID: 10247
	private float currentHP;

	// Token: 0x04002808 RID: 10248
	private DamageDealer damageDealer;

	// Token: 0x04002809 RID: 10249
	private DamageReceiver damageReceiver;

	// Token: 0x0400280A RID: 10250
	private FlowerLevelFlower parent;
}
