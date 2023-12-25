using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200055D RID: 1373
public class ClownLevelClownHorse : LevelProperties.Clown.Entity
{
	// Token: 0x17000347 RID: 839
	// (get) Token: 0x060019B8 RID: 6584 RVA: 0x000E9FAC File Offset: 0x000E83AC
	// (set) Token: 0x060019B9 RID: 6585 RVA: 0x000E9FB4 File Offset: 0x000E83B4
	public ClownLevelClownHorse.HorseType horseType { get; private set; }

	// Token: 0x060019BA RID: 6586 RVA: 0x000E9FC0 File Offset: 0x000E83C0
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.clownHorseHead.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.clownHorseHead.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x000EA02F File Offset: 0x000E842F
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (Level.Current.mode == Level.Mode.Easy)
		{
			Level.Current.OnLevelEndEvent += this.Dead;
		}
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x000EA067 File Offset: 0x000E8467
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x000EA085 File Offset: 0x000E8485
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x000EA0A0 File Offset: 0x000E84A0
	public void StartCarouselHorse()
	{
		base.gameObject.SetActive(true);
		base.animator.SetTrigger("Start");
		LevelProperties.Clown.Horse horse = base.properties.CurrentState.horse;
		this.dropMainIndex = UnityEngine.Random.Range(0, horse.DropBulletPositionString.Length);
		this.pinkMainIndex = UnityEngine.Random.Range(0, horse.WavePinkString.Length);
		this.horseTypePattern = horse.HorseString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.horseTypeIndex = UnityEngine.Random.Range(0, this.horseTypePattern.Length);
		this.wavePositionPattern = horse.WavePosString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.wavePinkPattern = horse.WavePinkString[this.pinkMainIndex].Split(new char[]
		{
			','
		});
		this.dropPositionPattern = horse.DropHorsePositionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.dropBulletPositionPattern = horse.DropBulletPositionString[this.dropMainIndex].Split(new char[]
		{
			','
		});
		this.dropBulletIndex = UnityEngine.Random.Range(0, this.dropBulletPositionPattern.Length);
		this.StopAllCoroutines();
		base.StartCoroutine(this.select_horse_cr());
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x000EA1E6 File Offset: 0x000E85E6
	private void BounceSFX()
	{
		AudioManager.Play("clown_horse_clown");
		this.emitAudioFromObject.Add("clown_horse_clown");
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000EA202 File Offset: 0x000E8602
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.regularHorseshoe = null;
		this.pinkHorseshoe = null;
		this.spitFxPrefabA = null;
		this.spitFxPrefabB = null;
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x000EA228 File Offset: 0x000E8628
	private IEnumerator select_horse_cr()
	{
		LevelProperties.Clown.Horse p = base.properties.CurrentState.horse;
		if (this.horseTypePattern[this.horseTypeIndex][0] == 'W')
		{
			base.animator.SetBool("IsWave", true);
			base.StartCoroutine(this.horse_cr(ClownLevelClownHorse.HorseType.Wave, this.wavePositionPattern, p.WaveATKRepeat));
		}
		else if (this.horseTypePattern[this.horseTypeIndex][0] == 'D')
		{
			base.animator.SetBool("IsWave", false);
			base.StartCoroutine(this.horse_cr(ClownLevelClownHorse.HorseType.Drop, this.dropPositionPattern, p.DropATKRepeat));
		}
		else
		{
			global::Debug.LogError("Horse Type Pattern is messed up!", null);
		}
		if (this.horseTypeIndex < this.horseTypePattern.Length - 1)
		{
			this.horseTypeIndex++;
		}
		else
		{
			this.horseTypeIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x000EA244 File Offset: 0x000E8644
	private IEnumerator horse_cr(ClownLevelClownHorse.HorseType horseType, string[] positionPattern, float ATKAmount)
	{
		bool isPink = false;
		float hesitate = 0f;
		float posOffset = 0f;
		float ATKcounter = 0f;
		float YSpeed = 0f;
		LevelProperties.Clown.Horse p = base.properties.CurrentState.horse;
		this.SelectStartPos();
		while (ATKcounter < ATKAmount)
		{
			Parser.FloatTryParse(positionPattern[this.positionIndex], out posOffset);
			float getPos = 360f - posOffset;
			while (base.transform.position.y != getPos)
			{
				this.pos = base.transform.position;
				this.pos.y = Mathf.MoveTowards(base.transform.position.y, getPos, p.HorseSpeed * CupheadTime.Delta);
				base.transform.position = this.pos;
				yield return null;
			}
			base.StartCoroutine(this.spit_fx_cr());
			if (horseType != ClownLevelClownHorse.HorseType.Wave)
			{
				if (horseType == ClownLevelClownHorse.HorseType.Drop)
				{
					hesitate = p.DropHesitate;
					float spawnY = 0f;
					float nextSpawnY = 0f;
					int i = 0;
					this.dropBulletPositionPattern = p.DropBulletPositionString[this.dropMainIndex].Split(new char[]
					{
						','
					});
					string[] droppattern = this.dropBulletPositionPattern[this.dropBulletIndex].Split(new char[]
					{
						'-'
					});
					this.SpitSFX();
					base.animator.SetBool("Spit", true);
					float[] durationBeforeDrops = new float[droppattern.Length];
					List<int> indexPatterns = new List<int>(droppattern.Length);
					for (int l = 0; l < droppattern.Length; l++)
					{
						indexPatterns.Add(l);
					}
					float currentDuration = base.properties.CurrentState.horse.DropBulletDelay;
					bool dropTwo = true;
					while (indexPatterns.Count > 0)
					{
						if (indexPatterns.Count > 1 && dropTwo)
						{
							currentDuration += base.properties.CurrentState.horse.DropBulletTwoDelay.RandomFloat();
							int index = UnityEngine.Random.Range(0, indexPatterns.Count);
							durationBeforeDrops[indexPatterns[index]] = currentDuration;
							indexPatterns.RemoveAt(index);
							index = UnityEngine.Random.Range(0, indexPatterns.Count);
							durationBeforeDrops[indexPatterns[index]] = currentDuration;
							indexPatterns.RemoveAt(index);
							dropTwo = false;
						}
						else
						{
							currentDuration += base.properties.CurrentState.horse.DropBulletOneDelay.RandomFloat();
							int index2 = UnityEngine.Random.Range(0, indexPatterns.Count);
							durationBeforeDrops[indexPatterns[index2]] = currentDuration;
							indexPatterns.RemoveAt(index2);
							dropTwo = true;
						}
					}
					for (int j = 0; j < droppattern.Length; j++)
					{
						if (j < droppattern.Length - 1)
						{
							i = j + 1;
						}
						else
						{
							i = 0;
						}
						Parser.FloatTryParse(droppattern[j], out spawnY);
						Parser.FloatTryParse(droppattern[i], out nextSpawnY);
						float dist = nextSpawnY - spawnY;
						this.FireDropBullets(spawnY, durationBeforeDrops[j]);
						float halfSpeed = p.DropBulletInitalSpeed / 2f;
						yield return CupheadTime.WaitForSeconds(this, dist / halfSpeed / 2f);
					}
					base.animator.SetBool("Spit", false);
					if (this.dropBulletIndex < this.dropBulletPositionPattern.Length - 1)
					{
						this.dropBulletIndex++;
					}
					else
					{
						this.dropMainIndex = (this.dropMainIndex + 1) % p.DropBulletPositionString.Length;
						this.dropBulletIndex = 0;
					}
					yield return CupheadTime.WaitForSeconds(this, p.DropATKDelay);
				}
			}
			else
			{
				hesitate = p.WaveHesitate;
				Vector3 pos = this.projectileRoot.transform.position;
				if (Rand.Bool())
				{
					YSpeed = -p.WaveBulletWaveSpeed;
				}
				else
				{
					YSpeed = p.WaveBulletWaveSpeed;
				}
				this.SpitSFX();
				base.animator.SetBool("Spit", true);
				for (int k = 0; k < p.WaveBulletCount; k++)
				{
					this.wavePinkPattern = p.WavePinkString[this.pinkMainIndex].Split(new char[]
					{
						','
					});
					if (this.wavePinkPattern[this.pinkIndex][0] == 'R')
					{
						isPink = false;
					}
					else if (this.wavePinkPattern[this.pinkIndex][0] == 'P')
					{
						isPink = true;
					}
					this.FireWaveBullets(k, isPink, YSpeed, pos);
					if (this.pinkIndex < this.wavePinkPattern.Length - 1)
					{
						this.pinkIndex++;
					}
					else
					{
						this.pinkMainIndex = (this.pinkMainIndex + 1) % p.WavePinkString.Length;
						this.pinkIndex = 0;
					}
					yield return CupheadTime.WaitForSeconds(this, p.WaveBulletDelay);
				}
				base.animator.SetBool("Spit", false);
				yield return CupheadTime.WaitForSeconds(this, p.WaveATKDelay);
			}
			this.positionIndex %= positionPattern.Length;
			ATKcounter += 1f;
		}
		yield return CupheadTime.WaitForSeconds(this, hesitate);
		while (base.transform.position.y != this.startPos.y)
		{
			this.pos = base.transform.position;
			this.pos.y = Mathf.MoveTowards(base.transform.position.y, this.startPos.y, p.HorseSpeed * CupheadTime.Delta);
			base.transform.position = this.pos;
			yield return null;
		}
		base.StartCoroutine(this.select_horse_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x000EA274 File Offset: 0x000E8674
	private IEnumerator spit_fx_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.167f);
		do
		{
			this.spitFxPrefabA.Create(this.spitFxRoot.position, base.transform.localScale);
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.125f, 0.21f));
			if (!base.animator.GetBool("Spit"))
			{
				break;
			}
			this.spitFxPrefabB.Create(this.spitFxRoot.position, base.transform.localScale);
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.125f, 0.21f));
		}
		while (base.animator.GetBool("Spit"));
		yield break;
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x000EA290 File Offset: 0x000E8690
	private void SelectStartPos()
	{
		this.startPos.y = 860f;
		if (Rand.Bool())
		{
			this.startPos.x = -640f + base.properties.CurrentState.horse.HorseXPosOffset;
			base.transform.position = this.startPos;
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
		}
		else
		{
			this.startPos.x = 640f - base.properties.CurrentState.horse.HorseXPosOffset;
			base.transform.position = this.startPos;
			base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		}
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x000EA37C File Offset: 0x000E877C
	private void SpitSFX()
	{
		AudioManager.Play("clown_horse_head_spit");
		this.emitAudioFromObject.Add("clown_horse_head_spit");
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000EA398 File Offset: 0x000E8798
	private void FireWaveBullets(int index, bool isPink, float YSpeed, Vector3 pos)
	{
		LevelProperties.Clown.Horse horse = base.properties.CurrentState.horse;
		bool onRight = base.transform.position.x > 0f;
		if (isPink)
		{
			ClownLevelHorseshoe clownLevelHorseshoe = UnityEngine.Object.Instantiate<ClownLevelHorseshoe>(this.pinkHorseshoe);
			clownLevelHorseshoe.Init(pos, horse.WaveBulletSpeed, YSpeed, onRight, 0f, horse, ClownLevelClownHorse.HorseType.Wave);
		}
		else
		{
			ClownLevelHorseshoe clownLevelHorseshoe2 = UnityEngine.Object.Instantiate<ClownLevelHorseshoe>(this.regularHorseshoe);
			clownLevelHorseshoe2.Init(pos, horse.WaveBulletSpeed, YSpeed, onRight, 0f, horse, ClownLevelClownHorse.HorseType.Wave);
		}
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x000EA438 File Offset: 0x000E8838
	private void FireDropBullets(float spawnY, float durationBeforeDrop)
	{
		LevelProperties.Clown.Horse horse = base.properties.CurrentState.horse;
		bool onRight = this.projectileRoot.transform.position.x > 0f;
		ClownLevelHorseshoe clownLevelHorseshoe = UnityEngine.Object.Instantiate<ClownLevelHorseshoe>(this.regularHorseshoe);
		clownLevelHorseshoe.Init(this.projectileRoot.transform.position, horse.DropBulletInitalSpeed, spawnY, onRight, durationBeforeDrop, horse, ClownLevelClownHorse.HorseType.Drop);
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x000EA4B2 File Offset: 0x000E88B2
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.horse_death_cr());
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x000EA4C8 File Offset: 0x000E88C8
	private IEnumerator horse_death_cr()
	{
		float t = 0f;
		float time = 3f;
		Vector2 start = base.transform.position;
		this.startPos.x = base.transform.position.x;
		base.GetComponent<SpriteRenderer>().color = ColorUtils.HexToColor("FFFFFFFF");
		this.clownHorseHead.GetComponent<Collider2D>().enabled = false;
		this.clownHorseBody.GetComponent<Collider2D>().enabled = false;
		this.StartExplosions();
		base.animator.Play("Off");
		base.animator.SetTrigger("Dead");
		this.FallHorseScreamSFX();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		this.pos.x = 0f;
		this.pos.y = base.transform.position.y;
		base.transform.position = this.pos;
		yield return CupheadTime.WaitForSeconds(this, 0.75f);
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(this.pos, new Vector3(0f, 250f, 0f), val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.startPos.x = base.transform.position.x;
		this.EndExplosions();
		while (this.clownHorseHead.GetComponent<HitFlash>().flashing)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(this.clownHorseHead.GetComponent<HitFlash>());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		base.GetComponent<SpriteRenderer>().sortingOrder = 200;
		base.animator.SetTrigger("Fall");
		this.moveObject = base.transform;
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		while (!this.droppedClown)
		{
			yield return null;
		}
		t = 0f;
		time = 3f;
		while (t < time)
		{
			if (CupheadTime.Delta != 0f)
			{
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
				this.moveObject.transform.position = Vector2.Lerp(this.moveObject.transform.position, this.startPos, t2);
				t += CupheadTime.Delta;
			}
			yield return null;
		}
		this.moveObject.transform.position = this.startPos;
		UnityEngine.Object.Destroy(this.moveObject.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x000EA4E3 File Offset: 0x000E88E3
	private void Separate()
	{
		this.clownHorseBody.transform.parent = null;
		this.moveObject = this.clownHorseBody.transform;
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x000EA508 File Offset: 0x000E8908
	private IEnumerator clown_fall_cr()
	{
		float fallGravity = -100f;
		float fallAccumulatedGravity = 0f;
		Vector2 fallVelocity = Vector3.zero;
		this.FallHorseSFXOff();
		this.droppedClown = true;
		while (base.transform.position.y > -660f)
		{
			if (CupheadTime.Delta != 0f)
			{
				base.transform.position += (fallVelocity + new Vector2(-300f, fallAccumulatedGravity)) * CupheadTime.FixedDelta;
				fallAccumulatedGravity += fallGravity;
			}
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		while (this.moveObject != null)
		{
			yield return null;
		}
		this.clownSwing.StartSwing();
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x000EA523 File Offset: 0x000E8923
	private void StartExplosions()
	{
		this.clownHorseHead.GetComponent<LevelBossDeathExploder>().StartExplosion();
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x000EA535 File Offset: 0x000E8935
	private void EndExplosions()
	{
		this.clownHorseHead.GetComponent<LevelBossDeathExploder>().StopExplosions();
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x000EA548 File Offset: 0x000E8948
	private void Dead()
	{
		this.StopAllCoroutines();
		base.animator.Play("Off");
		base.animator.SetTrigger("Dead");
		Level.Current.OnLevelEndEvent -= this.Dead;
		base.StartCoroutine(this.move_to_death_spot_cr());
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x000EA5A0 File Offset: 0x000E89A0
	private IEnumerator move_to_death_spot_cr()
	{
		float t = 0f;
		float time = 1f;
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(base.transform.position, new Vector3(this.pos.x, 250f, 0f), val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000EA5BB File Offset: 0x000E89BB
	private void FallHorseSFX()
	{
		AudioManager.Play("clown_horse_death_slide");
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000EA5C7 File Offset: 0x000E89C7
	private void FallHorseScreamSFX()
	{
		if (!this.ScreamSFXPlaying)
		{
			this.ScreamSFXPlaying = true;
			AudioManager.PlayLoop("clown_horse_death");
		}
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000EA5E5 File Offset: 0x000E89E5
	private void FallHorseSFXOff()
	{
		AudioManager.FadeSFXVolume("clown_horse_death", 0f, 1f);
		AudioManager.Play("clown_horse_death_end");
	}

	// Token: 0x040022CF RID: 8911
	[SerializeField]
	private GameObject clownHorseBody;

	// Token: 0x040022D0 RID: 8912
	[SerializeField]
	private GameObject clownHorseHead;

	// Token: 0x040022D1 RID: 8913
	[SerializeField]
	private ClownLevelClownSwing clownSwing;

	// Token: 0x040022D2 RID: 8914
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x040022D3 RID: 8915
	[SerializeField]
	private ClownLevelHorseshoe regularHorseshoe;

	// Token: 0x040022D4 RID: 8916
	[SerializeField]
	private ClownLevelHorseshoe pinkHorseshoe;

	// Token: 0x040022D5 RID: 8917
	[SerializeField]
	private Effect spitFxPrefabA;

	// Token: 0x040022D6 RID: 8918
	[SerializeField]
	private Effect spitFxPrefabB;

	// Token: 0x040022D7 RID: 8919
	[SerializeField]
	private Transform spitFxRoot;

	// Token: 0x040022D8 RID: 8920
	private Vector3 pos;

	// Token: 0x040022D9 RID: 8921
	private Vector3 startPos;

	// Token: 0x040022DA RID: 8922
	private Transform moveObject;

	// Token: 0x040022DB RID: 8923
	private DamageDealer damageDealer;

	// Token: 0x040022DC RID: 8924
	private DamageReceiver damageReceiver;

	// Token: 0x040022DD RID: 8925
	private string[] horseTypePattern;

	// Token: 0x040022DE RID: 8926
	private string[] wavePositionPattern;

	// Token: 0x040022DF RID: 8927
	private string[] wavePinkPattern;

	// Token: 0x040022E0 RID: 8928
	private string[] dropPositionPattern;

	// Token: 0x040022E1 RID: 8929
	private string[] dropBulletPositionPattern;

	// Token: 0x040022E2 RID: 8930
	private int positionIndex;

	// Token: 0x040022E3 RID: 8931
	private int pinkIndex;

	// Token: 0x040022E4 RID: 8932
	private int dropBulletIndex;

	// Token: 0x040022E5 RID: 8933
	private int horseTypeIndex;

	// Token: 0x040022E6 RID: 8934
	private int dropMainIndex;

	// Token: 0x040022E7 RID: 8935
	private int pinkMainIndex;

	// Token: 0x040022E8 RID: 8936
	private bool droppedClown;

	// Token: 0x040022E9 RID: 8937
	private bool ScreamSFXPlaying;

	// Token: 0x0200055E RID: 1374
	public enum HorseType
	{
		// Token: 0x040022EB RID: 8939
		Wave,
		// Token: 0x040022EC RID: 8940
		Drop,
		// Token: 0x040022ED RID: 8941
		Simple
	}
}
