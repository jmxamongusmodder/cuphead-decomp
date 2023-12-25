using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005AE RID: 1454
public class DicePalaceCigarLevelCigar : LevelProperties.DicePalaceCigar.Entity
{
	// Token: 0x06001C0B RID: 7179 RVA: 0x001016C4 File Offset: 0x000FFAC4
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		base.Awake();
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x0010171D File Offset: 0x000FFB1D
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x00101735 File Offset: 0x000FFB35
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x00101753 File Offset: 0x000FFB53
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x00101768 File Offset: 0x000FFB68
	public override void LevelInit(LevelProperties.DicePalaceCigar properties)
	{
		this.onRightSpawn = true;
		this.isFiring = false;
		this.rightAsh.SetActive(false);
		this.spitAttackCountIndex = UnityEngine.Random.Range(0, properties.CurrentState.spiralSmoke.attackCount.Split(new char[]
		{
			','
		}).Length);
		this.spitAttackDirectionIndex = UnityEngine.Random.Range(0, properties.CurrentState.spiralSmoke.rotationDirectionString.Split(new char[]
		{
			','
		}).Length);
		this.ghostAttackDelayIndex = UnityEngine.Random.Range(0, properties.CurrentState.cigaretteGhost.attackDelayString.Split(new char[]
		{
			','
		}).Length);
		this.ghostSpawnPositionIndex = UnityEngine.Random.Range(0, properties.CurrentState.cigaretteGhost.spawnPositionString.Split(new char[]
		{
			','
		}).Length);
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.OnDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x00101888 File Offset: 0x000FFC88
	private IEnumerator intro_cr()
	{
		AudioManager.PlayLoop("dice_palace_cigar_intro_start_loop");
		this.emitAudioFromObject.Add("dice_palace_cigar_intro_start_loop");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		yield return null;
		yield break;
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x001018A3 File Offset: 0x000FFCA3
	private void StopIntroLoop()
	{
		AudioManager.Stop("dice_palace_cigar_intro_start_loop");
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x001018AF File Offset: 0x000FFCAF
	private void OnIntroEnd()
	{
		base.StartCoroutine(this.attack_cr());
		base.StartCoroutine(this.ghostAttack_cr());
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x001018CC File Offset: 0x000FFCCC
	private IEnumerator attack_cr()
	{
		for (;;)
		{
			base.GetComponent<BoxCollider2D>().enabled = true;
			this.maxCounter = Parser.IntParse(base.properties.CurrentState.spiralSmoke.attackCount.Split(new char[]
			{
				','
			})[this.spitAttackCountIndex]);
			this.isFiring = true;
			while (this.isFiring)
			{
				if (this.counter > this.maxCounter)
				{
					this.isFiring = false;
					this.counter = 0;
					break;
				}
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.spiralSmoke.hesitateBeforeAttackDelay);
				this.counter++;
				base.animator.SetTrigger("IsAttacking");
				yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
				yield return null;
			}
			this.spitAttackCountIndex++;
			if (this.spitAttackCountIndex >= base.properties.CurrentState.spiralSmoke.attackCount.Split(new char[]
			{
				','
			}).Length)
			{
				this.spitAttackCountIndex = 0;
			}
			this.spitAttackDirectionIndex++;
			if (this.spitAttackDirectionIndex >= base.properties.CurrentState.spiralSmoke.rotationDirectionString.Split(new char[]
			{
				','
			}).Length)
			{
				this.spitAttackDirectionIndex = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.cigar.warningDelay);
			base.animator.SetTrigger("OnStateChange");
			yield return base.animator.WaitForAnimationToEnd(this, "Teleport_End", false, true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x001018E7 File Offset: 0x000FFCE7
	private void TeleportSFX()
	{
		AudioManager.Play("dice_palace_cigar_teleport");
		this.emitAudioFromObject.Add("dice_palace_cigar_teleport");
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x00101903 File Offset: 0x000FFD03
	private void AttackSFX()
	{
		AudioManager.Play("dice_palace_cigar_attack");
		this.emitAudioFromObject.Add("dice_palace_cigar_attack");
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x00101920 File Offset: 0x000FFD20
	private void SwitchSides()
	{
		this.leftAsh.SetActive(!this.onRightSpawn);
		this.rightAsh.SetActive(this.onRightSpawn);
		this.onRightSpawn = !this.onRightSpawn;
		if (!this.facingBack)
		{
			base.transform.Rotate(Vector3.up, 180f);
		}
		if (this.onRightSpawn)
		{
			base.transform.position = this.rightSpawnPointFacingRight.position;
		}
		else
		{
			base.transform.position = this.leftSpawnPointFacingRight.position;
		}
		base.StartCoroutine(this.finish_teleport_cr());
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x001019CC File Offset: 0x000FFDCC
	private void Rotate()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		if (next.transform.position.x < base.transform.position.x && !this.onRightSpawn)
		{
			base.transform.position = this.leftSpawnPointFacingLeft.position;
			base.transform.Rotate(Vector3.up, 180f);
			this.facingBack = true;
		}
		else if (next.transform.position.x > base.transform.position.x && this.onRightSpawn)
		{
			base.transform.position = this.rightSpawnPointFacingLeft.position;
			base.transform.Rotate(Vector3.up, 180f);
			this.facingBack = true;
		}
		else
		{
			base.transform.Rotate(Vector3.up, 0f);
			this.facingBack = false;
		}
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x00101AD8 File Offset: 0x000FFED8
	private void CheckIfBackward()
	{
		if (this.facingBack)
		{
			base.transform.Rotate(Vector3.up, 180f);
			if (this.onRightSpawn)
			{
				base.transform.position = this.rightSpawnPointFacingRight.position;
			}
			else
			{
				base.transform.position = this.leftSpawnPointFacingRight.position;
			}
			this.facingBack = false;
		}
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x00101B48 File Offset: 0x000FFF48
	private IEnumerator finish_teleport_cr()
	{
		AudioManager.PlayLoop("dice_palace_cigar_teleport_warning_loop");
		this.emitAudioFromObject.Add("dice_palace_cigar_teleport_warning_loop");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.cigar.warningDelay);
		this.VOXTeleportWarning();
		AudioManager.Stop("dice_palace_cigar_teleport_warning_loop");
		AudioManager.Play("dice_palace_cigar_teleport_end");
		this.emitAudioFromObject.Add("dice_palace_cigar_teleport_end");
		base.animator.SetTrigger("Continue");
		yield return null;
		yield break;
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x00101B64 File Offset: 0x000FFF64
	private void SpitAttack()
	{
		bool onRight;
		if (this.facingBack)
		{
			onRight = !this.onRightSpawn;
		}
		else
		{
			onRight = this.onRightSpawn;
		}
		AbstractProjectile abstractProjectile = this.spitPrefab.Create(this.spitSpawnPoint.position, (float)((int)base.transform.eulerAngles.y));
		if (base.properties.CurrentState.spiralSmoke.rotationDirectionString.Split(new char[]
		{
			','
		})[this.spitAttackDirectionIndex][0] == '1')
		{
			abstractProjectile.GetComponent<DicePalaceCigarLevelCigarSpit>().InitProjectile(base.properties, true, onRight);
		}
		else
		{
			abstractProjectile.GetComponent<DicePalaceCigarLevelCigarSpit>().InitProjectile(base.properties, false, onRight);
		}
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x00101C2C File Offset: 0x0010002C
	private IEnumerator ghostAttack_cr()
	{
		for (;;)
		{
			float spawnPosx = UnityEngine.Random.Range(this.ghostSpawnPoint.transform.position.x - this.ghostOffset, this.ghostSpawnPoint.transform.position.x + this.ghostOffset);
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.cigaretteGhost.attackDelayString.Split(new char[]
			{
				','
			})[this.ghostAttackDelayIndex]));
			AbstractProjectile proj = this.ghostPrefab.Create(new Vector2(spawnPosx, this.ghostSpawnPoint.transform.position.y));
			proj.GetComponent<DicePalaceCigarLevelCigaretteGhost>().InitGhost(base.properties);
			this.ghostAttackDelayIndex++;
			if (this.ghostAttackDelayIndex >= base.properties.CurrentState.cigaretteGhost.attackDelayString.Split(new char[]
			{
				','
			}).Length)
			{
				this.ghostAttackDelayIndex = 0;
			}
			this.ghostSpawnPositionIndex++;
			if (this.ghostSpawnPositionIndex >= base.properties.CurrentState.cigaretteGhost.spawnPositionString.Split(new char[]
			{
				','
			}).Length)
			{
				this.ghostSpawnPositionIndex = 0;
			}
		}
		yield break;
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x00101C47 File Offset: 0x00100047
	private void SmokeAB()
	{
		this.smokeA.Create(this.smokeSpawnPoint.transform.position);
		this.smokeB.Create(this.smokeSpawnPoint.transform.position);
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00101C81 File Offset: 0x00100081
	private void SmokeB()
	{
		this.smokeB.Create(this.smokeSpawnPoint.transform.position);
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x00101CA0 File Offset: 0x001000A0
	private void SwitchLayer()
	{
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Map.ToString();
		base.GetComponent<SpriteRenderer>().sortingOrder = 200;
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x00101CD7 File Offset: 0x001000D7
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.smokeA = null;
		this.smokeB = null;
		this.spitPrefab = null;
		this.ghostPrefab = null;
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x00101D04 File Offset: 0x00100104
	private void OnDeath()
	{
		AudioManager.Play("dice_palace_cigar_death");
		this.emitAudioFromObject.Add("dice_palace_cigar_death");
		this.VOXDeath();
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x00101D53 File Offset: 0x00100153
	private void DeathSFX()
	{
		AudioManager.Play("dice_palace_cigar_death_end");
		this.emitAudioFromObject.Add("dice_palace_cigar_death_end");
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x00101D6F File Offset: 0x0010016F
	private void VOXIntro()
	{
		AudioManager.Play("cigar_vox_intro");
		this.emitAudioFromObject.Add("cigar_vox_intro");
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x00101D8B File Offset: 0x0010018B
	private void VOXDeath()
	{
		AudioManager.Play("cigar_vox_death");
		this.emitAudioFromObject.Add("cigar_vox_death");
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x00101DA7 File Offset: 0x001001A7
	private void VOXTeleport()
	{
		AudioManager.Play("cigar_vox_pre_teleport");
		this.emitAudioFromObject.Add("cigar_vox_pre_teleport");
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x00101DC3 File Offset: 0x001001C3
	private void VOXTeleportWarning()
	{
		AudioManager.Play("cigar_vox_warning");
		this.emitAudioFromObject.Add("cigar_vox_warning");
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x00101DE0 File Offset: 0x001001E0
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector2(this.ghostSpawnPoint.transform.position.x - this.ghostOffset, this.ghostSpawnPoint.transform.position.y), new Vector2(this.ghostSpawnPoint.transform.position.x + this.ghostOffset, this.ghostSpawnPoint.transform.position.y));
	}

	// Token: 0x0400251A RID: 9498
	[Space(5f)]
	[SerializeField]
	private GameObject leftAshTray;

	// Token: 0x0400251B RID: 9499
	[SerializeField]
	private GameObject rightAshTray;

	// Token: 0x0400251C RID: 9500
	[Space(5f)]
	[SerializeField]
	private GameObject leftAsh;

	// Token: 0x0400251D RID: 9501
	[SerializeField]
	private GameObject rightAsh;

	// Token: 0x0400251E RID: 9502
	[Space(5f)]
	[SerializeField]
	private Transform leftSpawnPointFacingRight;

	// Token: 0x0400251F RID: 9503
	[SerializeField]
	private Transform leftSpawnPointFacingLeft;

	// Token: 0x04002520 RID: 9504
	[SerializeField]
	private Transform rightSpawnPointFacingLeft;

	// Token: 0x04002521 RID: 9505
	[SerializeField]
	private Transform rightSpawnPointFacingRight;

	// Token: 0x04002522 RID: 9506
	[Space(5f)]
	[SerializeField]
	private Transform smokeSpawnPoint;

	// Token: 0x04002523 RID: 9507
	[SerializeField]
	private Effect smokeA;

	// Token: 0x04002524 RID: 9508
	[SerializeField]
	private Effect smokeB;

	// Token: 0x04002525 RID: 9509
	[SerializeField]
	private CollisionChild collisionChild;

	// Token: 0x04002526 RID: 9510
	[Space(10f)]
	[SerializeField]
	private DicePalaceCigarLevelCigarSpit spitPrefab;

	// Token: 0x04002527 RID: 9511
	[SerializeField]
	private Transform spitSpawnPoint;

	// Token: 0x04002528 RID: 9512
	[SerializeField]
	private DicePalaceCigarLevelCigaretteGhost ghostPrefab;

	// Token: 0x04002529 RID: 9513
	[SerializeField]
	private Transform ghostSpawnPoint;

	// Token: 0x0400252A RID: 9514
	[SerializeField]
	private float ghostOffset;

	// Token: 0x0400252B RID: 9515
	private bool isVisible;

	// Token: 0x0400252C RID: 9516
	private bool onRightSpawn;

	// Token: 0x0400252D RID: 9517
	private bool isFiring;

	// Token: 0x0400252E RID: 9518
	private bool facingBack;

	// Token: 0x0400252F RID: 9519
	private int spitAttackCountIndex;

	// Token: 0x04002530 RID: 9520
	private int spitAttackDirectionIndex;

	// Token: 0x04002531 RID: 9521
	private int ghostAttackDelayIndex;

	// Token: 0x04002532 RID: 9522
	private int ghostSpawnPositionIndex;

	// Token: 0x04002533 RID: 9523
	private int counter;

	// Token: 0x04002534 RID: 9524
	private int maxCounter;

	// Token: 0x04002535 RID: 9525
	private DamageReceiver damageReceiver;

	// Token: 0x04002536 RID: 9526
	private DamageDealer damageDealer;
}
