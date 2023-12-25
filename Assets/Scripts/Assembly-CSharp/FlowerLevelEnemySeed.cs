using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000607 RID: 1543
public class FlowerLevelEnemySeed : AbstractProjectile
{
	// Token: 0x06001EBB RID: 7867 RVA: 0x0011AD34 File Offset: 0x00119134
	public void OnSeedSpawn(LevelProperties.Flower properties, FlowerLevelFlower parent, char type, bool isActive)
	{
		this.isActive = isActive;
		this.properties = properties;
		switch (type)
		{
		case 'A':
			base.animator.SetInteger("Type", 1);
			break;
		case 'B':
			base.animator.SetInteger("Type", 0);
			break;
		case 'C':
			base.animator.SetInteger("Type", 2);
			this.SetParryable(true);
			break;
		}
		this.fallingSpeed = properties.CurrentState.enemyPlants.fallingSeedSpeed;
		this.type = type;
		this.parent = parent;
		this.parent.OnDeathEvent += this.KillSeed;
	}

	// Token: 0x06001EBC RID: 7868 RVA: 0x0011ADF3 File Offset: 0x001191F3
	private void OnSeedLand()
	{
		base.StartCoroutine(this.onSeedLand_cr());
	}

	// Token: 0x06001EBD RID: 7869 RVA: 0x0011AE04 File Offset: 0x00119204
	private IEnumerator onSeedLand_cr()
	{
		if (this.type == 'B')
		{
			if (!this.plantSpawned)
			{
				base.animator.Play("Chomper_Landing");
				yield return base.animator.WaitForAnimationToEnd(this, "Chomper_Landing", false, true);
				if (this.isActive)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.chomperSpawn);
					gameObject.transform.position = base.transform.position;
					gameObject.GetComponent<FlowerLevelChomperSeed>().OnChomperStart(this.parent, this.properties.CurrentState.enemyPlants);
					this.plantSpawned = true;
					base.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			base.animator.SetTrigger("Landed");
			yield return new WaitForEndOfFrame();
			yield return base.animator.WaitForAnimationToEnd(this, true);
			base.animator.Play("Ground_Burst_Start");
		}
		yield break;
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x0011AE1F File Offset: 0x0011921F
	private void KillSeed()
	{
		this.isActive = false;
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x0011AE28 File Offset: 0x00119228
	protected override void Awake()
	{
		this.isActive = true;
		base.transform.localScale = new Vector3(base.transform.localScale.x * (float)MathUtils.PlusOrMinus(), base.transform.localScale.y, base.transform.localScale.z);
		base.Awake();
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x0011AE92 File Offset: 0x00119292
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x0011AEB0 File Offset: 0x001192B0
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x0011AECE File Offset: 0x001192CE
	protected override void FixedUpdate()
	{
		base.transform.position += -Vector3.up * ((float)this.fallingSpeed * CupheadTime.FixedDelta);
		base.FixedUpdate();
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x0011AF08 File Offset: 0x00119308
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (!this.plantSpawned)
		{
			if (this.isActive)
			{
				this.OnSeedLand();
			}
			else if (this.type == 'C')
			{
				this.type = 'A';
				this.OnSeedLand();
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.fallingSpeed = 0;
			if (base.CanParry)
			{
				this.SetParryable(false);
			}
			this.plantSpawned = true;
		}
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x0011AF8C File Offset: 0x0011938C
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (hit.GetComponent<FlowerLevelFlowerDamageRegion>() != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			switch (this.type)
			{
			case 'A':
				if (hit.GetComponent<FlowerLevelVenusSpawn>() != null)
				{
					this.isActive = false;
				}
				break;
			case 'B':
				if (hit.GetComponent<FlowerLevelChomperSeed>() != null)
				{
					this.isActive = false;
				}
				break;
			case 'C':
				if (hit.GetComponent<FlowerLevelMiniFlowerSpawn>() != null)
				{
					this.isActive = false;
				}
				break;
			}
		}
		base.OnCollisionEnemyProjectile(hit, phase);
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x0011B03F File Offset: 0x0011943F
	protected override void Die()
	{
		base.Die();
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		this.parent.OnMiniFlowerDeath();
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x0011B064 File Offset: 0x00119464
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.KillSeed;
		this.homingVenusFlyTrapSpawn = null;
		this.chomperSpawn = null;
		this.miniFlowerSpawn = null;
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x0011B098 File Offset: 0x00119498
	private void OnSpawnPlant()
	{
		char c = this.type;
		if (c != 'A')
		{
			if (c == 'C')
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.miniFlowerSpawn);
				gameObject.transform.position = this.spawnPoint.transform.position;
				gameObject.GetComponent<FlowerLevelMiniFlowerSpawn>().OnMiniFlowerSpawn(this.parent, this.properties.CurrentState.enemyPlants);
				gameObject.transform.localScale = base.transform.localScale;
			}
		}
		else
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.homingVenusFlyTrapSpawn);
			gameObject.transform.position = this.spawnPoint.transform.position;
			gameObject.GetComponent<FlowerLevelVenusSpawn>().OnVenusSpawn(this.parent, this.properties.CurrentState.enemyPlants.venusPlantHP, (float)this.properties.CurrentState.enemyPlants.venusTurningSpeed, this.properties.CurrentState.enemyPlants.venusMovmentSpeed, this.properties.CurrentState.enemyPlants.venusTurningDelay);
			gameObject.transform.localScale = base.transform.localScale;
		}
		this.plantSpawned = true;
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x0011B1D8 File Offset: 0x001195D8
	private void TriggerVine()
	{
		base.StartCoroutine(this.triggerVine_cr());
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x0011B1E8 File Offset: 0x001195E8
	private IEnumerator triggerVine_cr()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		base.animator.Play("Trigger_Vine", 1);
		yield break;
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x0011B203 File Offset: 0x00119603
	private void OnDeath()
	{
		if (this.plantSpawned)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x0011B21B File Offset: 0x0011961B
	private void GroundPopAudio()
	{
		AudioManager.Play("flower_vine_groundburst_start");
		this.emitAudioFromObject.Add("flower_vine_groundburst_start");
	}

	// Token: 0x06001ECC RID: 7884 RVA: 0x0011B237 File Offset: 0x00119637
	private void VineGrowLargeAudio()
	{
		AudioManager.Play("flower_venus_vine_grow_large");
		this.emitAudioFromObject.Add("flower_venus_vine_grow_large");
	}

	// Token: 0x06001ECD RID: 7885 RVA: 0x0011B253 File Offset: 0x00119653
	private void VineGrowMediumAudio()
	{
		AudioManager.Play("flower_venus_vine_grow_medium");
		this.emitAudioFromObject.Add("flower_venus_vine_grow_medium");
	}

	// Token: 0x06001ECE RID: 7886 RVA: 0x0011B26F File Offset: 0x0011966F
	private void VineGrowSmallAudio()
	{
		AudioManager.Play("flower_venus_vine_grow_small");
		this.emitAudioFromObject.Add("flower_venus_vine_grow_small");
	}

	// Token: 0x04002782 RID: 10114
	private int fallingSpeed;

	// Token: 0x04002783 RID: 10115
	private char type;

	// Token: 0x04002784 RID: 10116
	private bool isActive;

	// Token: 0x04002785 RID: 10117
	private bool plantSpawned;

	// Token: 0x04002786 RID: 10118
	private LevelProperties.Flower properties;

	// Token: 0x04002787 RID: 10119
	private FlowerLevelFlower parent;

	// Token: 0x04002788 RID: 10120
	[SerializeField]
	private GameObject spawnPoint;

	// Token: 0x04002789 RID: 10121
	[Space(10f)]
	[Header("Venus Fly Trap")]
	[SerializeField]
	private Sprite venusSeedTex;

	// Token: 0x0400278A RID: 10122
	[SerializeField]
	private GameObject homingVenusFlyTrapSpawn;

	// Token: 0x0400278B RID: 10123
	[Space(10f)]
	[Header("Chomper")]
	[SerializeField]
	private Sprite chomperSeedTex;

	// Token: 0x0400278C RID: 10124
	[SerializeField]
	private GameObject chomperSpawn;

	// Token: 0x0400278D RID: 10125
	[Space(10f)]
	[Header("Mini Flower")]
	[SerializeField]
	private Sprite miniFlowerSeedTex;

	// Token: 0x0400278E RID: 10126
	[SerializeField]
	private GameObject miniFlowerSpawn;
}
