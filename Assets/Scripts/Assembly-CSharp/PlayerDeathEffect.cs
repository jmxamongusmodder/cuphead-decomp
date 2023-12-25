using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A09 RID: 2569
public class PlayerDeathEffect : AbstractMonoBehaviour
{
	// Token: 0x06003CAB RID: 15531 RVA: 0x001CA6F8 File Offset: 0x001C8AF8
	public PlayerDeathEffect Create(PlayerId playerId, PlayerInput input, Vector2 pos, int deathCount, PlayerMode mode, bool canParry)
	{
		PlayerDeathEffect playerDeathEffect = UnityEngine.Object.Instantiate<PlayerDeathEffect>(this);
		playerDeathEffect.name = playerDeathEffect.name.Replace("(Clone)", string.Empty);
		playerDeathEffect.Init(playerId, input, pos, deathCount, mode, canParry);
		return playerDeathEffect;
	}

	// Token: 0x06003CAC RID: 15532 RVA: 0x001CA738 File Offset: 0x001C8B38
	public void CreateExplosionOnly(PlayerId playerId, Vector2 pos, PlayerMode mode)
	{
		if (mode != PlayerMode.Level)
		{
			if (mode == PlayerMode.Plane)
			{
				PlayerDeathEffect playerDeathEffect = UnityEngine.Object.Instantiate<PlayerDeathEffect>(this);
				LevelPlayerDeathEffect levelPlayerDeathEffect = UnityEngine.Object.Instantiate<LevelPlayerDeathEffect>(playerDeathEffect.explosionPrefab);
				levelPlayerDeathEffect.Init(pos);
				UnityEngine.Object.Destroy(playerDeathEffect.gameObject);
			}
		}
		else
		{
			PlayerDeathEffect playerDeathEffect2 = UnityEngine.Object.Instantiate<PlayerDeathEffect>(this);
			LevelPlayerDeathEffect levelPlayerDeathEffect2 = UnityEngine.Object.Instantiate<LevelPlayerDeathEffect>(playerDeathEffect2.explosionPrefab);
			LevelPlayerController levelPlayerController = PlayerManager.GetPlayer(playerId) as LevelPlayerController;
			levelPlayerDeathEffect2.Init(pos, playerId, levelPlayerController.motor.Grounded);
			UnityEngine.Object.Destroy(playerDeathEffect2.gameObject);
		}
	}

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x06003CAD RID: 15533 RVA: 0x001CA7C8 File Offset: 0x001C8BC8
	// (remove) Token: 0x06003CAE RID: 15534 RVA: 0x001CA800 File Offset: 0x001C8C00
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AbstractPlayerController.OnReviveHandler OnPreReviveEvent;

	// Token: 0x14000088 RID: 136
	// (add) Token: 0x06003CAF RID: 15535 RVA: 0x001CA838 File Offset: 0x001C8C38
	// (remove) Token: 0x06003CB0 RID: 15536 RVA: 0x001CA870 File Offset: 0x001C8C70
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AbstractPlayerController.OnReviveHandler OnReviveEvent;

	// Token: 0x06003CB1 RID: 15537 RVA: 0x001CA8A6 File Offset: 0x001C8CA6
	protected override void Awake()
	{
		base.Awake();
		this.parrySwitch.OnActivate += this.OnParrySwitch;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x001CA8D7 File Offset: 0x001C8CD7
	protected virtual void Start()
	{
		base.StartCoroutine(this.checkOutOfFrame_cr());
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x001CA8E8 File Offset: 0x001C8CE8
	private void Init(PlayerId playerId, PlayerInput input, Vector2 pos, int deathCount, PlayerMode mode, bool canParry)
	{
		this.playerInput = input;
		this.playerId = playerId;
		this.deathCount = deathCount;
		if (deathCount >= 10)
		{
			this.parrySwitch.gameObject.SetActive(false);
		}
		this.playerMode = mode;
		AbstractPlayerController player = PlayerManager.GetPlayer(playerId);
		base.animator.SetInteger("Mode", (int)this.playerMode);
		base.animator.SetBool("CanParry", canParry);
		if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
		{
			this.spriteRenderer = ((!player.stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman) : this.chalice);
		}
		else
		{
			this.spriteRenderer = ((!player.stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead) : this.chalice);
		}
		this.effect.enabled = !PlayerManager.GetPlayer(playerId).stats.isChalice;
		this.chaliceEffect.enabled = PlayerManager.GetPlayer(playerId).stats.isChalice;
		this.cuphead.gameObject.SetActive(false);
		this.mugman.gameObject.SetActive(false);
		this.chalice.gameObject.SetActive(false);
		this.spriteRenderer.gameObject.SetActive(true);
		this.parrySwitch.gameObject.SetActive(canParry);
		base.transform.position = pos;
	}

	// Token: 0x06003CB4 RID: 15540 RVA: 0x001CAA90 File Offset: 0x001C8E90
	private void OnAnimationComplete()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(this.playerId);
		LevelPlayerController levelPlayerController = (LevelPlayerController)player;
		LevelPlayerDeathEffect levelPlayerDeathEffect = UnityEngine.Object.Instantiate<LevelPlayerDeathEffect>(this.explosionPrefab);
		levelPlayerDeathEffect.Init(base.transform.position, this.playerId, levelPlayerController.motor.Grounded);
		base.StartCoroutine(this.float_cr());
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x001CAAF0 File Offset: 0x001C8EF0
	private void OnAnimationCompletePlane()
	{
		LevelPlayerDeathEffect levelPlayerDeathEffect = UnityEngine.Object.Instantiate<LevelPlayerDeathEffect>(this.explosionPrefab);
		levelPlayerDeathEffect.Init(base.transform.position);
		base.StartCoroutine(this.float_cr());
	}

	// Token: 0x06003CB6 RID: 15542 RVA: 0x001CAB2C File Offset: 0x001C8F2C
	public void GameOverUnpause()
	{
		base.animator.enabled = true;
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = true;
		this.ignoreGlobalTime = true;
	}

	// Token: 0x06003CB7 RID: 15543 RVA: 0x001CAB5C File Offset: 0x001C8F5C
	protected virtual void OnParrySwitch()
	{
		if (this.exiting)
		{
			return;
		}
		this.exiting = true;
		this.StopAllCoroutines();
		this.parrySwitch.gameObject.SetActive(false);
		if (this.OnPreReviveEvent != null)
		{
			this.OnPreReviveEvent(base.transform.position);
		}
		AudioManager.Play("player_revive");
		AudioManager.Play((!PlayerManager.GetPlayer(this.playerId).stats.isChalice) ? "player_revive_thank_you" : "player_revive_thank_you_chalice");
		base.animator.SetTrigger("OnParry");
	}

	// Token: 0x06003CB8 RID: 15544 RVA: 0x001CABFC File Offset: 0x001C8FFC
	protected virtual void OnReviveParryAnimComplete()
	{
		if (this.OnReviveEvent != null)
		{
			this.OnReviveEvent(base.transform.position);
		}
		this.OnReviveEvent = null;
		AbstractPlayerController player = PlayerManager.GetPlayer(this.playerId);
		if (player.mode == PlayerMode.Level && player.stats.isChalice)
		{
			LevelPlayerController levelPlayerController = player as LevelPlayerController;
			levelPlayerController.motor.OnChaliceRevive();
		}
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x001CAC7C File Offset: 0x001C907C
	private void ReviveOutOfFrame()
	{
		if (this.exiting || !PlayerManager.Multiplayer)
		{
			return;
		}
		this.exiting = true;
		PlayerId id = (this.playerId != PlayerId.PlayerOne) ? PlayerId.PlayerOne : PlayerId.PlayerTwo;
		AbstractPlayerController player = PlayerManager.GetPlayer(id);
		if (player == null || player.IsDead || !player.stats.PartnerCanSteal || Level.IsTowerOfPowerMain)
		{
			return;
		}
		player.stats.OnPartnerStealHealth();
		this.StopAllCoroutines();
		if (this.OnPreReviveEvent != null)
		{
			this.OnPreReviveEvent(player.center);
		}
		AudioManager.Play("player_revive");
		AudioManager.Play("player_revive_thank_you");
		base.animator.SetTrigger("OnSteal");
		base.transform.position = player.center;
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x001CAD54 File Offset: 0x001C9154
	private void OnReviveStealAnimComplete()
	{
		if (this.OnReviveEvent != null)
		{
			this.OnReviveEvent(base.transform.position);
		}
		this.OnReviveEvent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x001CAD89 File Offset: 0x001C9189
	private void OnPlayerLeave(PlayerId id)
	{
		if (this.playerId == id)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x001CADA2 File Offset: 0x001C91A2
	private void OnDestroy()
	{
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		this.explosionPrefab = null;
		this.cuphead = null;
		this.mugman = null;
		this.parrySwitch = null;
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x001CADD1 File Offset: 0x001C91D1
	public void Clean()
	{
		this.OnDestroy();
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x001CADDC File Offset: 0x001C91DC
	protected IEnumerator float_cr()
	{
		base.animator.SetTrigger("OnIdle");
		float floatSpeed = PlayerDeathEffect.FLOAT_SPEEDS[Mathf.Clamp(this.deathCount, 0, PlayerDeathEffect.FLOAT_SPEEDS.Length - 1)];
		while (true && !this.exiting)
		{
			base.transform.AddPosition(0f, floatSpeed * base.LocalDeltaTime, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x001CADF8 File Offset: 0x001C91F8
	protected virtual IEnumerator checkOutOfFrame_cr()
	{
		for (;;)
		{
			if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(1000f, 10f)) && this.playerInput.actions.GetButtonDown(8) && !this.exiting)
			{
				yield return new WaitForSeconds(0.1f);
				this.ReviveOutOfFrame();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040043F8 RID: 17400
	public const string NAME = "Player_Death";

	// Token: 0x040043F9 RID: 17401
	public const string EFFECT_NAME = "Player_Death_Explosion";

	// Token: 0x040043FA RID: 17402
	private const string PATH = "Player/Player_Death";

	// Token: 0x040043FB RID: 17403
	private const float TIME_TO_SPEED = 1f;

	// Token: 0x040043FC RID: 17404
	private static readonly float[] FLOAT_SPEEDS = new float[]
	{
		125f,
		200f,
		275f
	};

	// Token: 0x040043FD RID: 17405
	private const int REVIVE_Y = 10;

	// Token: 0x040043FE RID: 17406
	public const int DEATH_COUNT_MAX = 10;

	// Token: 0x040043FF RID: 17407
	[SerializeField]
	protected SpriteRenderer cuphead;

	// Token: 0x04004400 RID: 17408
	[SerializeField]
	protected SpriteRenderer mugman;

	// Token: 0x04004401 RID: 17409
	[SerializeField]
	protected SpriteRenderer chalice;

	// Token: 0x04004402 RID: 17410
	[SerializeField]
	protected PlayerDeathParrySwitch parrySwitch;

	// Token: 0x04004403 RID: 17411
	[SerializeField]
	private LevelPlayerDeathEffect explosionPrefab;

	// Token: 0x04004404 RID: 17412
	[SerializeField]
	private SpriteRenderer effect;

	// Token: 0x04004405 RID: 17413
	[SerializeField]
	private SpriteRenderer chaliceEffect;

	// Token: 0x04004406 RID: 17414
	protected PlayerId playerId;

	// Token: 0x04004407 RID: 17415
	protected SpriteRenderer spriteRenderer;

	// Token: 0x04004408 RID: 17416
	protected bool exiting;

	// Token: 0x04004409 RID: 17417
	private PlayerInput playerInput;

	// Token: 0x0400440A RID: 17418
	private int deathCount;

	// Token: 0x0400440B RID: 17419
	private PlayerMode playerMode;
}
