using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000605 RID: 1541
public class FlowerLevelChomperSeed : AbstractCollidableObject
{
	// Token: 0x06001EA2 RID: 7842 RVA: 0x0011A15C File Offset: 0x0011855C
	public void OnChomperStart(FlowerLevelFlower parent, LevelProperties.Flower.EnemyPlants properties)
	{
		AudioManager.Play("flower_plants_chomper");
		this.currentHP = (float)properties.chomperPlantHP;
		this.parent = parent;
		this.parent.OnDeathEvent += this.StartDeath;
		this.explosion = base.transform.GetChild(0);
		int integer = base.animator.GetInteger("MaxVariants");
		base.animator.SetInteger("Variant", UnityEngine.Random.Range(0, integer));
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x0011A1D8 File Offset: 0x001185D8
	private IEnumerator grow_cr()
	{
		float pct = 0.3f;
		while (pct < 1f)
		{
			this.chomperSprite.transform.localScale = Vector3.one * pct;
			pct += CupheadTime.Delta * 6f;
			if (pct > 1f)
			{
				pct = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x0011A1F4 File Offset: 0x001185F4
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.currentHP -= info.damage;
		if (this.currentHP <= 0f)
		{
			AudioManager.Stop("flower_plants_chomper");
			if (this.isDead)
			{
				return;
			}
			this.isDead = true;
			this.StopAllCoroutines();
			base.GetComponent<BoxCollider2D>().enabled = false;
			base.animator.Play("Death");
			base.StartCoroutine(this.die_cr());
			base.StartCoroutine(this.spawnPetals_cr());
		}
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x0011A280 File Offset: 0x00118680
	private IEnumerator spawnPetals_cr()
	{
		Animator child = this.explosion.GetComponent<Animator>();
		child.SetInteger("Variant", 0);
		yield return CupheadTime.WaitForSeconds(this, base.animator.GetCurrentAnimatorStateInfo(0).length / 4f);
		child.Play("Death");
		yield return new WaitForEndOfFrame();
		float delay = child.GetCurrentAnimatorStateInfo(0).length;
		yield return CupheadTime.WaitForSeconds(this, delay / 4f);
		this.SpawnPetals();
		yield return CupheadTime.WaitForSeconds(this, delay);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x0011A29C File Offset: 0x0011869C
	public void SpawnPetals()
	{
		Vector3 vector = base.transform.position + Vector3.up * (float)(UnityEngine.Random.Range(-10, 10) + 70);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.petalA, vector, Quaternion.identity);
		gameObject.GetComponent<Animator>().Play("Plant_LeafA", UnityEngine.Random.Range(0, 1));
		base.StartCoroutine(this.fade_cr(gameObject, 0.8f, 125f, false));
		gameObject = UnityEngine.Object.Instantiate<GameObject>(this.petalB, vector + Vector3.down * 50f, Quaternion.identity);
		gameObject.GetComponent<Animator>().Play("Plant_LeafB");
		base.StartCoroutine(this.fade_cr(gameObject, 1f, 100f, false));
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x0011A364 File Offset: 0x00118764
	private IEnumerator die_cr()
	{
		yield return new WaitForEndOfFrame();
		yield return base.animator.WaitForAnimationToEnd(this, "Death", 0, false, true);
		this.explosion.GetComponent<Animator>().Play("Death");
		yield break;
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x0011A380 File Offset: 0x00118780
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
			this.Die();
		}
		yield break;
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x0011A3B8 File Offset: 0x001187B8
	private void StartDeath()
	{
		AudioManager.Play("flower_minion_simple_deathpop_low");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop_low");
		this.StopAllCoroutines();
		base.GetComponent<BoxCollider2D>().enabled = false;
		base.animator.Play("Death");
		base.StartCoroutine(this.die_cr());
		base.StartCoroutine(this.spawnPetals_cr());
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x0011A41B File Offset: 0x0011881B
	private void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x0011A43C File Offset: 0x0011883C
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.transform.localScale = new Vector3(base.transform.localScale.x * (float)MathUtils.PlusOrMinus(), base.transform.localScale.y, base.transform.localScale.z);
		base.Awake();
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x0011A4CD File Offset: 0x001188CD
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x0011A4E5 File Offset: 0x001188E5
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x0011A504 File Offset: 0x00118904
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (hit.GetComponent<FlowerLevelFlowerDamageRegion>() != null)
		{
			DamageDealer.DamageInfo info = new DamageDealer.DamageInfo(1f, DamageDealer.Direction.Neutral, hit.transform.position, DamageDealer.DamageSource.Enemy);
			this.OnDamageTaken(info);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x0011A54E File Offset: 0x0011894E
	protected override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.StartDeath;
		base.OnDestroy();
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x0011A56D File Offset: 0x0011896D
	private void OnDeath()
	{
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x0011A56F File Offset: 0x0011896F
	private void SpawnChomper()
	{
		base.animator.Play("Trigger_Plant", 1);
		base.StartCoroutine(this.grow_cr());
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x0011A58F File Offset: 0x0011898F
	private void GroundBurstStartAudio()
	{
		AudioManager.Play("flower_ground_pop");
	}

	// Token: 0x04002774 RID: 10100
	[SerializeField]
	private GameObject petalA;

	// Token: 0x04002775 RID: 10101
	[SerializeField]
	private GameObject petalB;

	// Token: 0x04002776 RID: 10102
	private float currentHP;

	// Token: 0x04002777 RID: 10103
	private Transform explosion;

	// Token: 0x04002778 RID: 10104
	private FlowerLevelFlower parent;

	// Token: 0x04002779 RID: 10105
	[SerializeField]
	private SpriteRenderer chomperSprite;

	// Token: 0x0400277A RID: 10106
	private DamageDealer damageDealer;

	// Token: 0x0400277B RID: 10107
	private DamageReceiver damageReceiver;

	// Token: 0x0400277C RID: 10108
	private bool isDead;
}
