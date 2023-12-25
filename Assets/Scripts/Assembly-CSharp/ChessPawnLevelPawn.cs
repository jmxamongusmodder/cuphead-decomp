using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000545 RID: 1349
public class ChessPawnLevelPawn : AbstractProjectile
{
	// Token: 0x17000338 RID: 824
	// (get) Token: 0x060018C6 RID: 6342 RVA: 0x000E078A File Offset: 0x000DEB8A
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x060018C7 RID: 6343 RVA: 0x000E0791 File Offset: 0x000DEB91
	// (set) Token: 0x060018C8 RID: 6344 RVA: 0x000E0799 File Offset: 0x000DEB99
	public float speed { get; private set; }

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x060018C9 RID: 6345 RVA: 0x000E07A2 File Offset: 0x000DEBA2
	// (set) Token: 0x060018CA RID: 6346 RVA: 0x000E07AA File Offset: 0x000DEBAA
	public bool inUse { get; private set; }

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x060018CB RID: 6347 RVA: 0x000E07B3 File Offset: 0x000DEBB3
	// (set) Token: 0x060018CC RID: 6348 RVA: 0x000E07BB File Offset: 0x000DEBBB
	public int currentIndex { get; private set; }

	// Token: 0x060018CD RID: 6349 RVA: 0x000E07C4 File Offset: 0x000DEBC4
	public ChessPawnLevelPawn Init(ChessPawnLevel level)
	{
		ChessPawnLevelPawn chessPawnLevelPawn = UnityEngine.Object.Instantiate<ChessPawnLevelPawn>(this, Camera.main.transform.position + Vector3.up * 2000f, Quaternion.identity);
		chessPawnLevelPawn.level = level;
		return chessPawnLevelPawn;
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x000E0808 File Offset: 0x000DEC08
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.level = null;
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x000E0817 File Offset: 0x000DEC17
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x000E0835 File Offset: 0x000DEC35
	protected override void Die()
	{
		base.Die();
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x000E083D File Offset: 0x000DEC3D
	public void SetIndex(int i)
	{
		this.currentIndex = i;
		if (i >= 0)
		{
			this.lastIndex = i;
		}
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x000E0854 File Offset: 0x000DEC54
	protected override void OnDieDistance()
	{
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x000E0856 File Offset: 0x000DEC56
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x000E0858 File Offset: 0x000DEC58
	public override void OnLevelEnd()
	{
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000E085C File Offset: 0x000DEC5C
	public override void OnParry(AbstractPlayerController player)
	{
		this.parryCount++;
		if (PlayerManager.BothPlayersActive() && this.parryCount < 2)
		{
			return;
		}
		this.SetParryable(false);
		base.StartCoroutine(this.disable_collision_cr());
		if (this.state == ChessPawnLevelPawn.State.Run)
		{
			base.animator.SetTrigger("Parry");
		}
		this.parriedHead.CreatePart(base.transform.position + Vector3.up * 100f);
		this.headRenderer.enabled = false;
		base.StartCoroutine(this.SFX_KOG_PAWN_PawnParry_cr());
		this.level.TakeDamage();
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000E090D File Offset: 0x000DED0D
	public void StartIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000E091C File Offset: 0x000DED1C
	private IEnumerator intro_cr()
	{
		this.inUse = true;
		yield return base.StartCoroutine(this.drop_cr(true));
		this.inUse = false;
		yield break;
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x000E0938 File Offset: 0x000DED38
	private IEnumerator drop_cr(bool isIntro)
	{
		AnimationHelper animationHelper = base.GetComponent<AnimationHelper>();
		Vector3 targetPosition = this.level.GetPosition(this.currentIndex);
		base.animator.Play("IntroStart");
		base.animator.SetInteger("Intro", (!isIntro) ? 0 : (this.currentIndex % 2 + 1));
		targetPosition.z = (float)(this.currentIndex % 2) * 0.0001f;
		base.animator.Update(0f);
		animationHelper.Speed = 0f;
		this.bodyRenderer.sortingOrder = 0;
		this.headRenderer.sortingOrder = 1;
		float t = 0f;
		Vector3 dropPosition = targetPosition + ChessPawnLevelPawn.DropPositionOffset;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < 1f)
		{
			base.transform.position = Vector3.Lerp(dropPosition, targetPosition, t);
			t += CupheadTime.FixedDelta * 8f;
			yield return wait;
		}
		animationHelper.Speed = 1f;
		base.transform.position = targetPosition;
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		yield break;
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x000E095A File Offset: 0x000DED5A
	public void Attack(float warningTime, float horiztonalMovement, float dropSpeed, float runDelay, float runSpeed, float returnSpeed)
	{
		base.StartCoroutine(this.attack_cr(warningTime, horiztonalMovement, dropSpeed, runDelay, runSpeed, returnSpeed));
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x000E0974 File Offset: 0x000DED74
	private IEnumerator attack_cr(float warningTime, float horizontalMovement, float dropSpeed, float runDelay, float runSpeed, float returnDelay)
	{
		this.inUse = true;
		this.initialPosition = base.transform.position;
		YieldInstruction wait = new WaitForFixedUpdate();
		base.animator.SetTrigger("JumpWarning");
		yield return CupheadTime.WaitForSeconds(this, warningTime);
		this.currentIndex = -1;
		this.state = ChessPawnLevelPawn.State.Jump;
		this.collider.enabled = true;
		base.animator.SetTrigger("Jump");
		yield return CupheadTime.WaitForSeconds(this, 0.125f);
		if (horizontalMovement != 0f)
		{
			base.transform.SetScale(new float?(-Mathf.Sign(horizontalMovement)), null, null);
		}
		Coroutine horizontalMovementCoroutine = base.StartCoroutine(this.horizontalMovement_cr(horizontalMovement, dropSpeed));
		while (!this.beginFall)
		{
			yield return null;
		}
		this.beginFall = false;
		float t = 0f;
		while (t < 1f)
		{
			Vector3 position = base.transform.position;
			position.y = this.initialPosition.y - 650f + 650f * Mathf.Sin(1.5707964f + t * 3.1415927f / 2f);
			if (position.y < base.transform.position.y)
			{
				this.bodyRenderer.sortingOrder = 20;
				this.headRenderer.sortingOrder = 21;
			}
			base.transform.position = position;
			t += CupheadTime.FixedDelta * dropSpeed;
			yield return wait;
		}
		base.StopCoroutine(horizontalMovementCoroutine);
		base.transform.position = new Vector3(base.transform.position.x, this.initialPosition.y - 650f);
		float testDir = Mathf.Sign(PlayerManager.GetNext().transform.position.x - base.transform.position.x);
		bool quickLand = this.level.ClearToRun(testDir, base.transform.position);
		if (runDelay == 0f && quickLand)
		{
			base.animator.SetInteger("Land", 1);
			base.transform.SetScale(new float?(testDir), null, null);
		}
		else
		{
			base.animator.SetInteger("Land", 2);
			float delay = runDelay - 0.625f;
			yield return CupheadTime.WaitForSeconds(this, runDelay);
			while (!this.level.ClearToRun(testDir, base.transform.position))
			{
				yield return wait;
				testDir = Mathf.Sign(PlayerManager.GetNext().transform.position.x - base.transform.position.x);
			}
			base.animator.SetInteger("Land", 3);
			base.transform.SetScale(new float?(testDir), null, null);
			yield return base.animator.WaitForAnimationToStart(this, "LandLongToRun", false);
		}
		this.state = ChessPawnLevelPawn.State.Run;
		this.speed = runSpeed * testDir;
		while (Mathf.Abs(base.transform.position.x - Camera.main.transform.position.x) < 850f)
		{
			base.transform.position += this.speed * CupheadTime.FixedDelta * Vector3.right;
			yield return wait;
		}
		this.speed = 0f;
		this.state = ChessPawnLevelPawn.State.Idle;
		base.animator.SetInteger("Land", 0);
		this.collider.enabled = false;
		this.currentIndex = this.level.GetReturnIndex();
		yield return base.StartCoroutine(this.drop_cr(false));
		this.inUse = false;
		yield break;
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x000E09B4 File Offset: 0x000DEDB4
	private IEnumerator horizontalMovement_cr(float horizontalMovement, float dropSpeed)
	{
		float duration = 1f / dropSpeed;
		duration += 0.45833334f;
		float horizontalSpeed = horizontalMovement / duration;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			Vector3 position = base.transform.position;
			position.x += CupheadTime.FixedDelta * horizontalSpeed;
			base.transform.position = position;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x000E09E0 File Offset: 0x000DEDE0
	public void Death()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
		if (this.headRenderer.enabled)
		{
			this.parriedHead.CreatePart(base.transform.position + Vector3.up * 100f);
			this.collider.enabled = false;
		}
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x000E0A48 File Offset: 0x000DEE48
	private IEnumerator death_cr()
	{
		this.collider.enabled = false;
		base.transform.SetScale(new float?(1f), null, null);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, this.initialPosition.x);
		base.GetComponent<AnimationHelper>().Speed = 1f;
		base.animator.Play("DeathTwitch", 0, (float)this.lastIndex * 0.125f);
		Effect smoke = this.deathSmokeEffect.Create(base.transform.position);
		base.StartCoroutine(this.move_smoke_cr(smoke));
		float delay = (float)(this.lastIndex % 4 * 2 + this.lastIndex / 2) * this.deathTwitchDelayFixed + UnityEngine.Random.Range(this.deathTwitchDelayRange.minimum, this.deathTwitchDelayRange.maximum);
		yield return CupheadTime.WaitForSeconds(this, delay);
		base.animator.Play("DeathAngel", 0, UnityEngine.Random.Range(0f, 1f));
		smoke.animator.Play("Explode");
		this.deathBody.CreatePart(base.transform.position);
		for (;;)
		{
			Vector3 position = base.transform.position;
			position.y += this.deathFloatUpSpeed * CupheadTime.Delta;
			base.transform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x000E0A64 File Offset: 0x000DEE64
	private IEnumerator move_smoke_cr(Effect smoke)
	{
		SpriteRenderer smokeRenderer = smoke.GetComponent<SpriteRenderer>();
		while (smoke != null)
		{
			smoke.transform.position = base.transform.position + MathUtils.AngleToDirection((float)UnityEngine.Random.Range(0, 360)) * 50f;
			while (smokeRenderer != null && smokeRenderer.sprite != null)
			{
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x000E0A86 File Offset: 0x000DEE86
	private void animationEvent_BeginFall()
	{
		this.beginFall = true;
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x000E0A90 File Offset: 0x000DEE90
	private IEnumerator disable_collision_cr()
	{
		this.collider.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.noHeadCollider.enabled = true;
		yield break;
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x000E0AAB File Offset: 0x000DEEAB
	private void AnimationEvent_SFX_KOG_PAWN_PawnLand()
	{
		AudioManager.Play("sfx_dlc_kog_pawn_land");
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_land");
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x000E0AC7 File Offset: 0x000DEEC7
	private void AnimationEvent_SFX_KOG_PAWN_PawnJumpDown()
	{
		AudioManager.Play("sfx_dlc_kog_pawn_jumpdown");
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_jumpdown");
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x000E0AE3 File Offset: 0x000DEEE3
	private void AnimationEvent_SFX_KOG_PAWN_PawnParryHit()
	{
		AudioManager.Play(string.Empty);
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parryhit");
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x000E0B00 File Offset: 0x000DEF00
	private IEnumerator SFX_KOG_PAWN_PawnParry_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		AudioManager.Play("sfx_dlc_kog_pawn_parryhit");
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parryhit");
		yield return CupheadTime.WaitForSeconds(this, 0.15f);
		AudioManager.Play("sfx_dlc_kog_pawn_parrywoodbreak");
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parrywoodbreak");
		yield break;
	}

	// Token: 0x040021D1 RID: 8657
	private const float FALL_DISTANCE = 650f;

	// Token: 0x040021D2 RID: 8658
	private static readonly Vector3 DropPositionOffset = new Vector3(0f, 100f);

	// Token: 0x040021D3 RID: 8659
	[SerializeField]
	private Collider2D collider;

	// Token: 0x040021D4 RID: 8660
	[SerializeField]
	private SpriteRenderer bodyRenderer;

	// Token: 0x040021D5 RID: 8661
	[SerializeField]
	private SpriteRenderer headRenderer;

	// Token: 0x040021D6 RID: 8662
	[SerializeField]
	private SpriteDeathParts parriedHead;

	// Token: 0x040021D7 RID: 8663
	[SerializeField]
	private float deathTwitchDelayFixed;

	// Token: 0x040021D8 RID: 8664
	[SerializeField]
	private Rangef deathTwitchDelayRange;

	// Token: 0x040021D9 RID: 8665
	[SerializeField]
	private float deathFloatUpSpeed;

	// Token: 0x040021DA RID: 8666
	[SerializeField]
	private Effect deathSmokeEffect;

	// Token: 0x040021DB RID: 8667
	[SerializeField]
	private SpriteDeathParts deathBody;

	// Token: 0x040021DC RID: 8668
	[SerializeField]
	private BoxCollider2D noHeadCollider;

	// Token: 0x040021DD RID: 8669
	private ChessPawnLevel level;

	// Token: 0x040021DE RID: 8670
	private ChessPawnLevelPawn.State state;

	// Token: 0x040021DF RID: 8671
	private Vector3 initialPosition;

	// Token: 0x040021E0 RID: 8672
	private bool beginFall;

	// Token: 0x040021E1 RID: 8673
	private int parryCount;

	// Token: 0x040021E2 RID: 8674
	private int lastIndex;

	// Token: 0x02000546 RID: 1350
	private enum State
	{
		// Token: 0x040021E7 RID: 8679
		Idle,
		// Token: 0x040021E8 RID: 8680
		Jump,
		// Token: 0x040021E9 RID: 8681
		Run
	}
}
