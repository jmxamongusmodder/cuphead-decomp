using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056E RID: 1390
public class DevilLevelDemon : AbstractCollidableObject
{
	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06001A48 RID: 6728 RVA: 0x000F0EF3 File Offset: 0x000EF2F3
	// (set) Token: 0x06001A49 RID: 6729 RVA: 0x000F0EFB File Offset: 0x000EF2FB
	public Vector3 JumpRoot { get; set; }

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06001A4A RID: 6730 RVA: 0x000F0F04 File Offset: 0x000EF304
	// (set) Token: 0x06001A4B RID: 6731 RVA: 0x000F0F0C File Offset: 0x000EF30C
	public Vector3 RunRoot { get; set; }

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06001A4C RID: 6732 RVA: 0x000F0F15 File Offset: 0x000EF315
	// (set) Token: 0x06001A4D RID: 6733 RVA: 0x000F0F1D File Offset: 0x000EF31D
	public Vector3 PillarDestination { get; set; }

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06001A4E RID: 6734 RVA: 0x000F0F26 File Offset: 0x000EF326
	// (set) Token: 0x06001A4F RID: 6735 RVA: 0x000F0F2E File Offset: 0x000EF32E
	public Vector3 FrontSpawn { get; set; }

	// Token: 0x06001A50 RID: 6736 RVA: 0x000F0F37 File Offset: 0x000EF337
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x000F0F6D File Offset: 0x000EF36D
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x000F0F85 File Offset: 0x000EF385
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x000F0FB0 File Offset: 0x000EF3B0
	public DevilLevelDemon Create(Vector2 position, float direction, float speed, float hp, DevilLevelSittingDevil parent)
	{
		DevilLevelDemon devilLevelDemon = this.InstantiatePrefab<DevilLevelDemon>();
		devilLevelDemon.transform.position = position;
		devilLevelDemon.frontDirection = direction;
		devilLevelDemon.speed = speed;
		devilLevelDemon.hp = hp;
		devilLevelDemon.parent = parent;
		devilLevelDemon.transform.localScale = new Vector3(direction * 0.9f, 0.9f, 0.9f);
		devilLevelDemon.sprite.color = this.backgroundTint;
		int num = UnityEngine.Random.Range(0, 3);
		if (num == 0)
		{
			devilLevelDemon.animator.SetFloat("PeekVariation", (float)UnityEngine.Random.Range(0, 3) / 2f);
			devilLevelDemon.animator.SetTrigger("JumpOut");
		}
		else if (num == 1)
		{
			devilLevelDemon.animator.SetFloat("PeekVariation", (float)UnityEngine.Random.Range(0, 3) / 2f);
			devilLevelDemon.animator.SetTrigger("RunOut");
		}
		else
		{
			devilLevelDemon.animator.Play("JumpOut");
		}
		return devilLevelDemon;
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x000F10B1 File Offset: 0x000EF4B1
	private void Start()
	{
		DevilLevelSittingDevil devilLevelSittingDevil = this.parent;
		devilLevelSittingDevil.OnPhase1Death = (Action)Delegate.Combine(devilLevelSittingDevil.OnPhase1Death, new Action(this.Die));
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x000F10DA File Offset: 0x000EF4DA
	public void PlaceForJump()
	{
		base.transform.position = this.JumpRoot;
		this.hasJumped = true;
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x000F10F4 File Offset: 0x000EF4F4
	public void StartMoving()
	{
		if (!this.moving)
		{
			base.StartCoroutine(this.demonMovement_cr());
			AudioManager.Play("devil_imp_spawn");
			this.emitAudioFromObject.Add("devil_imp_spawn");
		}
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x000F1128 File Offset: 0x000EF528
	protected IEnumerator demonMovement_cr()
	{
		this.moving = true;
		if (!this.hasJumped)
		{
			base.transform.position = this.RunRoot;
		}
		Vector3 backDirection = (this.PillarDestination - base.transform.position).normalized;
		while (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 150f)))
		{
			base.transform.position += backDirection * this.speed * CupheadTime.Delta;
			float scaleDelta = 0.099999964f * CupheadTime.Delta;
			base.transform.localScale -= new Vector3(this.frontDirection * scaleDelta, scaleDelta, scaleDelta);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.frontWaitTime);
		base.transform.localScale = new Vector3(-this.frontDirection, 1f, 1f);
		base.transform.position = this.FrontSpawn;
		this.collider2d.enabled = true;
		this.sprite.sortingLayerName = "Enemies";
		this.sprite.sortingOrder = 0;
		this.sprite.color = Color.black;
		for (;;)
		{
			base.transform.AddPosition(this.frontDirection * this.speed * CupheadTime.Delta, 0f, 0f);
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 150f)))
			{
				this.enteredScreen = true;
			}
			else if (this.enteredScreen)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x000F1143 File Offset: 0x000EF543
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x000F116E File Offset: 0x000EF56E
	private void RemoveEvent()
	{
		DevilLevelSittingDevil devilLevelSittingDevil = this.parent;
		devilLevelSittingDevil.OnPhase1Death = (Action)Delegate.Remove(devilLevelSittingDevil.OnPhase1Death, new Action(this.Die));
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x000F1197 File Offset: 0x000EF597
	protected override void OnDestroy()
	{
		this.RemoveEvent();
		base.OnDestroy();
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x000F11A8 File Offset: 0x000EF5A8
	private void Die()
	{
		AudioManager.Play("devil_imp_death");
		this.emitAudioFromObject.Add("devil_imp_death");
		this.explosion.Create(this.collider2d.bounds.center);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x000F11F9 File Offset: 0x000EF5F9
	private void ImpStepSFX()
	{
		AudioManager.Play("devil_imp_step");
		this.emitAudioFromObject.Add("devil_imp_step");
	}

	// Token: 0x0400236C RID: 9068
	private const string PeekVariationParameterName = "PeekVariation";

	// Token: 0x0400236D RID: 9069
	private const string JumpOutParameterName = "JumpOut";

	// Token: 0x0400236E RID: 9070
	private const string RunOutParameterName = "RunOut";

	// Token: 0x0400236F RID: 9071
	private const string JumpOutStateName = "JumpOut";

	// Token: 0x04002370 RID: 9072
	private const string EnemyLayerName = "Enemies";

	// Token: 0x04002371 RID: 9073
	private const int PeekVariations = 3;

	// Token: 0x04002372 RID: 9074
	private const float StartScale = 0.9f;

	// Token: 0x04002373 RID: 9075
	private const float PillarScale = 0.8f;

	// Token: 0x04002374 RID: 9076
	[SerializeField]
	private Collider2D collider2d;

	// Token: 0x04002375 RID: 9077
	[SerializeField]
	private float frontWaitTime;

	// Token: 0x04002376 RID: 9078
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04002377 RID: 9079
	[SerializeField]
	private Color backgroundTint;

	// Token: 0x04002378 RID: 9080
	[SerializeField]
	private PlatformingLevelGenericExplosion explosion;

	// Token: 0x04002379 RID: 9081
	private DamageDealer damageDealer;

	// Token: 0x0400237A RID: 9082
	private bool enteredScreen;

	// Token: 0x0400237B RID: 9083
	private float frontDirection;

	// Token: 0x0400237C RID: 9084
	private float speed;

	// Token: 0x0400237D RID: 9085
	private float hp;

	// Token: 0x0400237E RID: 9086
	private bool moving;

	// Token: 0x0400237F RID: 9087
	private bool hasJumped;

	// Token: 0x04002380 RID: 9088
	private DamageReceiver damageReceiver;

	// Token: 0x04002381 RID: 9089
	private DevilLevelSittingDevil parent;
}
