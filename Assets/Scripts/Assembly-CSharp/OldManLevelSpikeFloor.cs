using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000711 RID: 1809
public class OldManLevelSpikeFloor : AbstractCollidableObject
{
	// Token: 0x06002740 RID: 10048 RVA: 0x0017073A File Offset: 0x0016EB3A
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponentInChildren<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002741 RID: 10049 RVA: 0x00170765 File Offset: 0x0016EB65
	protected override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06002742 RID: 10050 RVA: 0x0017078C File Offset: 0x0016EB8C
	public void SetID(int i)
	{
		this.id = i;
		base.animator.SetInteger("Variant", i % 4);
		this.gnomeRenderer.flipX = (i % 8 > 3);
		this.tuftRenderer.flipX = this.gnomeRenderer.flipX;
	}

	// Token: 0x06002743 RID: 10051 RVA: 0x001707DC File Offset: 0x0016EBDC
	private string AnimSuffix()
	{
		switch (this.id % 4)
		{
		case 0:
			return "A";
		case 1:
			return "B";
		case 2:
			return "C";
		default:
			return "D";
		}
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x00170820 File Offset: 0x0016EC20
	private string PopStartSuffix()
	{
		switch (this.id % 4)
		{
		case 0:
			return "A_C";
		case 1:
			return "B";
		case 2:
			return "A_C";
		default:
			return "D";
		}
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x00170864 File Offset: 0x0016EC64
	private string PopWarningSuffix()
	{
		switch (this.id % 4)
		{
		case 0:
			return "A_C";
		case 1:
			return "B_D";
		case 2:
			return "A_C";
		default:
			return "B_D";
		}
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x001708A8 File Offset: 0x0016ECA8
	private void Update()
	{
		if (this.spikeState == OldManLevelSpikeFloor.SpikeState.Gnomed)
		{
			return;
		}
		if (this.spikeState != OldManLevelSpikeFloor.SpikeState.Spiked && !this.deathTimeOut && this.MinDistanceToPlayer(base.transform.position) < 50f)
		{
			this.ChangeState(OldManLevelSpikeFloor.SpikeState.Spiked);
		}
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x001708FC File Offset: 0x0016ECFC
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.spikeState != OldManLevelSpikeFloor.SpikeState.Gnomed)
		{
			return;
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle);
			Level.Current.RegisterMinionKilled();
			this.Dead();
		}
	}

	// Token: 0x06002748 RID: 10056 RVA: 0x00170950 File Offset: 0x0016ED50
	public void SetProperties(LevelProperties.OldMan properties)
	{
		this.spikeProperties = properties.CurrentState.spikes;
		this.gnomeProperties = properties.CurrentState.turret;
		this.gnomeShootPatternString = new PatternString(this.gnomeProperties.attackString, true, true);
		this.gnomePinkPatternString = new PatternString(this.gnomeProperties.pinkShotString, true, true);
		this.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle);
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x001709B6 File Offset: 0x0016EDB6
	public void SpawnGnome()
	{
		this.ChangeState(OldManLevelSpikeFloor.SpikeState.Gnomed);
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x001709C0 File Offset: 0x0016EDC0
	private void ChangeState(OldManLevelSpikeFloor.SpikeState state)
	{
		if (this.exit)
		{
			return;
		}
		if (this.spikeState == OldManLevelSpikeFloor.SpikeState.Idle || state == OldManLevelSpikeFloor.SpikeState.Idle)
		{
			if (this.gnomeCR != null)
			{
				base.StopCoroutine(this.gnomeCR);
			}
			if (this.spikeCR != null)
			{
				base.StopCoroutine(this.spikeCR);
			}
			base.animator.ResetTrigger("OnPimple");
			base.animator.ResetTrigger("OnPop");
			base.animator.ResetTrigger("OnWarning");
			base.animator.SetBool("IsAttacking", false);
			this.spikeState = state;
			if (state != OldManLevelSpikeFloor.SpikeState.Gnomed)
			{
				if (state != OldManLevelSpikeFloor.SpikeState.Idle)
				{
					if (state == OldManLevelSpikeFloor.SpikeState.Spiked)
					{
						this.spikeCR = base.StartCoroutine(this.spike_up_cr());
					}
				}
				else if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_" + this.AnimSuffix()))
				{
					base.StartCoroutine(this.restart_idle_cr());
				}
			}
			else
			{
				this.gnomeCR = base.StartCoroutine(this.gnome_up_cr());
			}
		}
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x00170AE8 File Offset: 0x0016EEE8
	private IEnumerator restart_idle_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.5f, 1f));
		base.animator.Play("Restart_Idle_" + this.PopStartSuffix());
		this.deathTimeOut = false;
		yield break;
	}

	// Token: 0x0600274C RID: 10060 RVA: 0x00170B04 File Offset: 0x0016EF04
	private float MinDistanceToPlayer(Vector3 pos)
	{
		float num = float.MaxValue;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			float num2 = Vector3.SqrMagnitude(pos - player.transform.position);
			if (num2 < num)
			{
				num = num2;
			}
		}
		if (player2 != null)
		{
			float num3 = Vector3.SqrMagnitude(pos - player2.transform.position);
			if (num3 < num)
			{
				num = num3;
			}
		}
		return Mathf.Sqrt(num);
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x00170B88 File Offset: 0x0016EF88
	private IEnumerator gnome_up_cr()
	{
		this.hp = this.gnomeProperties.hp;
		base.animator.SetTrigger("OnPimple");
		yield return base.animator.WaitForAnimationToEnd(this, "Pop_Start_" + this.PopStartSuffix(), false, true);
		float t = 0f;
		while (t < this.gnomeProperties.appearWarning && !this.exit)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		t = 0f;
		while (t < this.gnomeProperties.spawnSecondaryBuffer && this.MinDistanceToPlayer(base.transform.position) < this.gnomeProperties.spawnDistanceCheck && !this.exit)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("OnPop");
		yield return null;
		while (this.spikeState == OldManLevelSpikeFloor.SpikeState.Gnomed)
		{
			t = 0f;
			while (t < this.gnomeProperties.shotDelay && !this.exit)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			base.animator.SetBool("IsAttacking", true);
			this.shootAngle = this.gnomeShootPatternString.PopFloat();
			if (this.shootAngle != 0f && ((this.shootAngle <= 180f && this.dontShootLeft) || (this.shootAngle > 180f && this.dontShootRight)))
			{
				this.shootAngle = 360f - this.shootAngle;
			}
			base.animator.SetBool("Diagonal", this.shootAngle != 0f);
			t = 0f;
			while (t < this.gnomeProperties.warningDuration && !this.exit)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			yield return null;
			if (!this.exit)
			{
				base.transform.localScale = new Vector3((float)((!(this.shootAngle > 180f ^ this.gnomeRenderer.flipX)) ? 1 : -1), 1f);
			}
			base.animator.SetBool("IsAttacking", false);
			yield return new WaitForEndOfFrame();
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x00170BA4 File Offset: 0x0016EFA4
	private float MinPlayerDistance()
	{
		float num = float.MaxValue;
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null) && levelPlayerController.transform.position.y <= base.transform.position.y + 200f)
			{
				float num2 = Mathf.Abs(base.transform.position.x - levelPlayerController.transform.position.x);
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x00170C80 File Offset: 0x0016F080
	private IEnumerator spike_up_cr()
	{
		if (!((OldManLevel)Level.Current).playedFirstSpikeSound)
		{
			this.SFX_OMM_Gnome_SpikeRaiseFirst();
			((OldManLevel)Level.Current).playedFirstSpikeSound = true;
		}
		base.transform.GetChild(0).gameObject.tag = "EnemyProjectile";
		base.animator.SetTrigger("OnWarning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning_Start_" + this.AnimSuffix(), false, true);
		float t = 0f;
		while (t < this.spikeProperties.warningDuration && !this.exit)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetBool("IsAttacking", true);
		t = 0f;
		while (t < this.spikeProperties.attackDuration && !this.exit)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		while (this.MinPlayerDistance() < 75f && !this.exit)
		{
			yield return null;
		}
		yield return null;
		base.animator.SetBool("IsAttacking", false);
		yield return base.animator.WaitForAnimationToStart(this, "Idle_" + this.AnimSuffix(), false);
		this.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle);
		base.transform.GetChild(0).gameObject.tag = "Enemy";
		yield return null;
		yield break;
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x00170C9C File Offset: 0x0016F09C
	public void Dead()
	{
		Vector3 position = new Vector3(this.shootRoot.position.x, this.shootRoot.position.y - 100f);
		this.deathPuff.Create(position);
		base.animator.Play("None");
		for (int i = 0; i < this.deathParts.Length; i++)
		{
			if (i != 0 || UnityEngine.Random.Range(0, 10) == 0)
			{
				this.deathParts[i].CreatePart(position);
			}
		}
		this.deathTimeOut = true;
		AudioManager.Play("sfx_dlc_omm_gnome_death");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death");
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x00170D55 File Offset: 0x0016F155
	public void Exit()
	{
		base.StartCoroutine(this.exit_cr());
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x00170D64 File Offset: 0x0016F164
	private IEnumerator exit_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 1f));
		this.exit = true;
		base.animator.SetBool("Dead", true);
		yield return base.animator.WaitForAnimationToStart(this, "None", false);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x00170D80 File Offset: 0x0016F180
	private void AniEvent_ShootProjectile()
	{
		BasicProjectile basicProjectile = (this.gnomePinkPatternString.PopLetter() != 'P') ? this.gnomeProjectile : this.gnomePinkProjectile;
		if (this.shootAngle == 0f)
		{
			basicProjectile.Create(this.shootRoot.position, this.shootAngle, this.gnomeProperties.shotSpeed);
			this.shootFXRenderer.transform.eulerAngles = Vector3.zero;
			this.shootFXRenderer.transform.localPosition = Vector3.up * 18f;
		}
		else
		{
			basicProjectile.Create(this.shootRoot.position + Vector3.right * 40f * Mathf.Sign(this.shootAngle - 180f), this.shootAngle, this.gnomeProperties.shotSpeed);
			this.shootFXRenderer.transform.eulerAngles = new Vector3(0f, 0f, 40f * Mathf.Sign(this.shootAngle) * (float)((this.shootAngle <= 180f) ? 1 : -1));
			this.shootFXRenderer.transform.localPosition = new Vector3(30.5f * (float)((!this.gnomeRenderer.flipX) ? 1 : -1), 33f);
		}
		AudioManager.Play("sfx_dlc_omm_gnome_shoot_projectile");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_shoot_projectile");
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x00170F0F File Offset: 0x0016F30F
	private void SFX_OMM_Gnome_SpikeRaiseFirst()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_spike_raisefirst");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_raisefirst");
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x00170F2B File Offset: 0x0016F32B
	private void AnimationEvent_SFX_OMM_Gnome_SpikeRaise()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_spike_raise");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_raise");
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x00170F47 File Offset: 0x0016F347
	private void AnimationEvent_SFX_OMM_Gnome_SpikeRetract()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_spike_retract");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_retract");
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x00170F63 File Offset: 0x0016F363
	private void AnimationEvent_SFX_OMM_Gnome_BeardAnticipation()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_beard_anticipation");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_beard_anticipation");
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x00170F7F File Offset: 0x0016F37F
	private void AnimationEvent_SFX_OMM_Gnome_BeardPopup()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_beard_popup");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_beard_popup");
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x00170F9C File Offset: 0x0016F39C
	private void WORKAROUND_NullifyFields()
	{
		this.deathPuff = null;
		this.deathParts = null;
		this.shootRoot = null;
		this.gnomeProjectile = null;
		this.gnomePinkProjectile = null;
		this.gnomeShootPatternString = null;
		this.gnomePinkPatternString = null;
		this.spikeCR = null;
		this.gnomeCR = null;
		this.gnomeRenderer = null;
		this.tuftRenderer = null;
		this.shootFXRenderer = null;
	}

	// Token: 0x04002FFC RID: 12284
	private const float SPIKE_TRIGGER_RANGE = 50f;

	// Token: 0x04002FFD RID: 12285
	private const float MIN_DISTANCE_TO_STAY_SPIKED = 75f;

	// Token: 0x04002FFE RID: 12286
	[Header("Death FX")]
	[SerializeField]
	private Effect deathPuff;

	// Token: 0x04002FFF RID: 12287
	[SerializeField]
	private SpriteDeathParts[] deathParts;

	// Token: 0x04003000 RID: 12288
	[Header("Prefabs")]
	[SerializeField]
	private Transform shootRoot;

	// Token: 0x04003001 RID: 12289
	[SerializeField]
	private BasicProjectile gnomeProjectile;

	// Token: 0x04003002 RID: 12290
	[SerializeField]
	private BasicProjectile gnomePinkProjectile;

	// Token: 0x04003003 RID: 12291
	public OldManLevelSpikeFloor.SpikeState spikeState;

	// Token: 0x04003004 RID: 12292
	private LevelProperties.OldMan.Spikes spikeProperties;

	// Token: 0x04003005 RID: 12293
	private LevelProperties.OldMan.Turret gnomeProperties;

	// Token: 0x04003006 RID: 12294
	private PatternString gnomeShootPatternString;

	// Token: 0x04003007 RID: 12295
	private PatternString gnomePinkPatternString;

	// Token: 0x04003008 RID: 12296
	private float hp;

	// Token: 0x04003009 RID: 12297
	private DamageReceiver damageReceiver;

	// Token: 0x0400300A RID: 12298
	private float shootAngle;

	// Token: 0x0400300B RID: 12299
	private Coroutine spikeCR;

	// Token: 0x0400300C RID: 12300
	private Coroutine gnomeCR;

	// Token: 0x0400300D RID: 12301
	[SerializeField]
	private bool dontShootLeft;

	// Token: 0x0400300E RID: 12302
	[SerializeField]
	private bool dontShootRight;

	// Token: 0x0400300F RID: 12303
	[SerializeField]
	private SpriteRenderer gnomeRenderer;

	// Token: 0x04003010 RID: 12304
	[SerializeField]
	private SpriteRenderer tuftRenderer;

	// Token: 0x04003011 RID: 12305
	[SerializeField]
	private SpriteRenderer shootFXRenderer;

	// Token: 0x04003012 RID: 12306
	private int id;

	// Token: 0x04003013 RID: 12307
	private bool exit;

	// Token: 0x04003014 RID: 12308
	private bool deathTimeOut;

	// Token: 0x02000712 RID: 1810
	public enum SpikeState
	{
		// Token: 0x04003016 RID: 12310
		Idle,
		// Token: 0x04003017 RID: 12311
		Spiked,
		// Token: 0x04003018 RID: 12312
		Gnomed
	}
}
