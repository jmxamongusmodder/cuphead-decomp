using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200055A RID: 1370
public class ClownLevelClownHelium : LevelProperties.Clown.Entity
{
	// Token: 0x17000346 RID: 838
	// (get) Token: 0x0600199F RID: 6559 RVA: 0x000E8DBD File Offset: 0x000E71BD
	// (set) Token: 0x060019A0 RID: 6560 RVA: 0x000E8DC5 File Offset: 0x000E71C5
	public ClownLevelClownHelium.State state { get; private set; }

	// Token: 0x060019A1 RID: 6561 RVA: 0x000E8DD0 File Offset: 0x000E71D0
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = this.head.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.head.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x000E8E1C File Offset: 0x000E721C
	public override void LevelInit(LevelProperties.Clown properties)
	{
		base.LevelInit(properties);
		this.pivotPoint.transform.position = this.heliumStopPos.transform.position;
		this.headMoving = true;
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x000E8E4C File Offset: 0x000E724C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x000E8E5F File Offset: 0x000E725F
	protected virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000E8E80 File Offset: 0x000E7280
	public void StartHeliumTank()
	{
		this.StopAllCoroutines();
		this.state = ClownLevelClownHelium.State.Helium;
		base.StartCoroutine(this.helium_tank_intro_cr());
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000E8E9C File Offset: 0x000E729C
	private IEnumerator helium_tank_intro_cr()
	{
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
		float t = 0f;
		float time = 5f;
		Vector2 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.heliumStopPos.position, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.heliumStopPos.position;
		base.StartCoroutine(this.helium_tank_cr());
		base.StartCoroutine(this.tank_effects_cr());
		base.StartCoroutine(this.pipe_puffs_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x000E8EB8 File Offset: 0x000E72B8
	private void SpawnBalloonDogs(ClownLevelDogBalloon dogPrefab, Vector3 startPos, bool isFlipped)
	{
		LevelProperties.Clown.HeliumClown heliumClown = base.properties.CurrentState.heliumClown;
		AbstractPlayerController next = PlayerManager.GetNext();
		if (dogPrefab != null)
		{
			ClownLevelDogBalloon clownLevelDogBalloon = UnityEngine.Object.Instantiate<ClownLevelDogBalloon>(dogPrefab);
			clownLevelDogBalloon.Init(heliumClown.dogHP, startPos, heliumClown.dogSpeed, next, heliumClown, isFlipped);
		}
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x000E8F0A File Offset: 0x000E730A
	private void HeliumTankSFX()
	{
		AudioManager.Play("clown_helium_tanks");
		this.emitAudioFromObject.Add("clown_helium_tanks");
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x000E8F28 File Offset: 0x000E7328
	private IEnumerator helium_tank_cr()
	{
		this.emitAudioFromObject.Add("clown_helium_tanks");
		AudioManager.Play("clown_helium_intro_continue");
		this.emitAudioFromObject.Add("clown_helium_intro_continue");
		AudioManager.Play("clown_helium_extend_pipes");
		this.emitAudioFromObject.Add("clown_helium_extend_pipes");
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Helium_Intro_End", 3, false, true);
		LevelProperties.Clown.HeliumClown p = base.properties.CurrentState.heliumClown;
		string[] spawnPattern = p.dogSpawnOrder.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] delayPattern = p.dogDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] typePattern = p.dogTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		Vector3 pickedPipePos = Vector3.zero;
		float waitTime = 0f;
		bool isFlipped = false;
		int spawnIndex = UnityEngine.Random.Range(0, spawnPattern.Length);
		int delayIndex = UnityEngine.Random.Range(0, delayPattern.Length);
		int typeIndex = UnityEngine.Random.Range(0, typePattern.Length);
		for (;;)
		{
			ClownLevelDogBalloon toSpawn = null;
			string[] nextPos = spawnPattern[spawnIndex].Split(new char[]
			{
				'-'
			});
			foreach (string s in nextPos)
			{
				int pipeSelection;
				Parser.IntTryParse(s, out pipeSelection);
				foreach (ClownLevelClownHelium.PipePositions pipePositions in this.pipePositions)
				{
					if (pipePositions.orderNum == pipeSelection)
					{
						pickedPipePos = pipePositions.pipeEntrance.position;
						isFlipped = (pipeSelection > 3);
					}
				}
				if (typePattern[typeIndex][0] == 'R')
				{
					toSpawn = this.regularDog;
				}
				else if (typePattern[typeIndex][0] == 'P')
				{
					toSpawn = this.pinkDog;
				}
				this.SpawnBalloonDogs(toSpawn, pickedPipePos, isFlipped);
				typeIndex = (typeIndex + 1) % typePattern.Length;
			}
			Parser.FloatTryParse(delayPattern[delayIndex], out waitTime);
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			spawnIndex = (spawnIndex + 1) % spawnPattern.Length;
			delayIndex = (delayIndex + 1) % delayPattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x000E8F44 File Offset: 0x000E7344
	private IEnumerator pipe_puffs_cr()
	{
		string order = "0,5,1,4,2,3,5,1,2,3";
		int orderIndex = UnityEngine.Random.Range(0, order.Split(new char[]
		{
			','
		}).Length);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.16f, 0.65f));
			this.pipePositions[Parser.IntParse(order.Split(new char[]
			{
				','
			})[orderIndex])].pipeEntrance.GetComponent<Animator>().SetInteger("Type", UnityEngine.Random.Range(0, 3));
			this.pipePositions[Parser.IntParse(order.Split(new char[]
			{
				','
			})[orderIndex])].pipeEntrance.GetComponent<Animator>().SetTrigger("OnPuff");
			orderIndex = (orderIndex + 1) % order.Split(new char[]
			{
				','
			}).Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x000E8F60 File Offset: 0x000E7360
	private IEnumerator tank_effects_cr()
	{
		bool isRight = Rand.Bool();
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.16f, 0.85f));
			this.tankEffects.SetBool("isLeft", isRight);
			this.tankEffects.SetTrigger("OnPuff");
			isRight = !isRight;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x000E8F7B File Offset: 0x000E737B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.regularDog = null;
		this.pinkDog = null;
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x000E8F94 File Offset: 0x000E7394
	private void SetHead()
	{
		this.pivotPoint.transform.position = this.head.transform.position;
		this.head.GetComponent<Collider2D>().enabled = true;
		base.animator.SetTrigger("Head");
		base.StartCoroutine(this.head_moving_cr());
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x000E8FEF File Offset: 0x000E73EF
	private void SetBody()
	{
		base.animator.Play("Helium_Idle");
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000E9004 File Offset: 0x000E7404
	private IEnumerator head_moving_cr()
	{
		for (;;)
		{
			if (this.headMoving)
			{
				this.PathMovement();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x000E9020 File Offset: 0x000E7420
	private void PathMovement()
	{
		this.angle += 1.8f * CupheadTime.Delta * this.hitPauseCoefficient();
		Vector3 a = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		Vector3 b = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		this.head.transform.position = this.pivotPoint.position;
		this.head.transform.position += a + b;
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x000E90D5 File Offset: 0x000E74D5
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x000E90EC File Offset: 0x000E74EC
	private IEnumerator death_cr()
	{
		this.head.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.head_moving_cr());
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
		this.head.transform.parent = null;
		base.StartCoroutine(this.head_death_cr());
		float moveSpeed = base.properties.CurrentState.heliumClown.heliumMoveSpeed;
		float acceleration = base.properties.CurrentState.heliumClown.heliumAcceleration;
		float endPos = -860f;
		while (base.transform.position.y > endPos)
		{
			moveSpeed += acceleration;
			base.transform.AddPosition(0f, -moveSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x000E9108 File Offset: 0x000E7508
	private IEnumerator head_death_cr()
	{
		this.StartExplosions();
		float moveSpeed = base.properties.CurrentState.heliumClown.heliumMoveSpeed;
		float acceleration = base.properties.CurrentState.heliumClown.heliumAcceleration;
		float endPos = 1060f;
		float t = 0f;
		float time = 1f;
		Vector2 start = this.head.transform.position;
		Vector2 end = new Vector3(this.head.transform.position.x, this.heliumStopPos.transform.position.y - 50f, 0f);
		this.headMoving = false;
		base.animator.SetTrigger("Dead");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			this.head.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		while (this.head.transform.position.y < endPos)
		{
			if (CupheadTime.Delta != 0f)
			{
				moveSpeed += acceleration;
				this.head.transform.AddPosition(0f, moveSpeed * CupheadTime.Delta, 0f);
			}
			yield return null;
		}
		this.EndExplosions();
		this.clownHorse.StartCarouselHorse();
		UnityEngine.Object.Destroy(this.head.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x000E9123 File Offset: 0x000E7523
	private void StartExplosions()
	{
		this.head.GetComponent<LevelBossDeathExploder>().StartExplosion();
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x000E9135 File Offset: 0x000E7535
	private void EndExplosions()
	{
		this.head.GetComponent<LevelBossDeathExploder>().StopExplosions();
	}

	// Token: 0x040022BC RID: 8892
	[SerializeField]
	private Animator tankEffects;

	// Token: 0x040022BD RID: 8893
	[SerializeField]
	private ClownLevelClownHorse clownHorse;

	// Token: 0x040022BE RID: 8894
	[SerializeField]
	private GameObject head;

	// Token: 0x040022BF RID: 8895
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x040022C0 RID: 8896
	[SerializeField]
	private Transform heliumStopPos;

	// Token: 0x040022C1 RID: 8897
	[SerializeField]
	private ClownLevelClownHelium.PipePositions[] pipePositions;

	// Token: 0x040022C2 RID: 8898
	[SerializeField]
	private ClownLevelDogBalloon regularDog;

	// Token: 0x040022C3 RID: 8899
	[SerializeField]
	private ClownLevelDogBalloon pinkDog;

	// Token: 0x040022C4 RID: 8900
	private DamageReceiver damageReceiver;

	// Token: 0x040022C5 RID: 8901
	private bool headMoving;

	// Token: 0x040022C6 RID: 8902
	private float angle;

	// Token: 0x040022C7 RID: 8903
	private float loopSize = 10f;

	// Token: 0x0200055B RID: 1371
	public enum State
	{
		// Token: 0x040022C9 RID: 8905
		BumperCar,
		// Token: 0x040022CA RID: 8906
		Helium,
		// Token: 0x040022CB RID: 8907
		Death
	}

	// Token: 0x0200055C RID: 1372
	[Serializable]
	public class PipePositions
	{
		// Token: 0x040022CC RID: 8908
		public Transform pipeEntrance;

		// Token: 0x040022CD RID: 8909
		public int orderNum;
	}
}
