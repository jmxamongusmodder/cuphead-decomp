using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005ED RID: 1517
public class DragonLevelDragon : LevelProperties.Dragon.Entity
{
	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06001E1A RID: 7706 RVA: 0x00114F46 File Offset: 0x00113346
	// (set) Token: 0x06001E1B RID: 7707 RVA: 0x00114F4E File Offset: 0x0011334E
	public DragonLevelDragon.State state { get; private set; }

	// Token: 0x06001E1C RID: 7708 RVA: 0x00114F57 File Offset: 0x00113357
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x00114F8D File Offset: 0x0011338D
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x00114FAC File Offset: 0x001133AC
	private void Start()
	{
		Level.Current.OnIntroEvent += this.OnIntro;
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x00114FC4 File Offset: 0x001133C4
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x00114FDC File Offset: 0x001133DC
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x00115005 File Offset: 0x00113405
	private void OnBossDeath()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		AudioManager.Stop("level_dragon_sucking_air");
		this.StopAllCoroutines();
		base.animator.Play("Death");
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x0011503A File Offset: 0x0011343A
	private void StartWingSFX()
	{
		AudioManager.PlayLoop("level_dragon_left_dragon_peashot_idle_loop");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_idle_loop");
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x00115056 File Offset: 0x00113456
	private void StopWingSFX()
	{
		AudioManager.Stop("level_dragon_left_dragon_peashot_idle_loop");
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x00115062 File Offset: 0x00113462
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.meteorPrefab = null;
		this.smokePrefab = null;
		this.peashotPrefab = null;
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x0011507F File Offset: 0x0011347F
	private void OnIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x00115090 File Offset: 0x00113490
	private IEnumerator intro_cr()
	{
		this.state = DragonLevelDragon.State.Init;
		base.animator.Play("Intro");
		AudioManager.Play("level_dragon_left_dragon_intro");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, 0.6f);
		this.state = DragonLevelDragon.State.Idle;
		yield break;
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x001150AB File Offset: 0x001134AB
	public void StartPeashot()
	{
		this.state = DragonLevelDragon.State.Peashot;
		base.StartCoroutine(this.peashot_cr());
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x001150C1 File Offset: 0x001134C1
	private void PeashotInSFX()
	{
		AudioManager.Play("level_dragon_left_dragon_peashot_in");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_in");
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x001150E0 File Offset: 0x001134E0
	private IEnumerator peashot_cr()
	{
		LevelProperties.Dragon.Peashot p = base.properties.CurrentState.peashot;
		string[] pattern = p.patternString.GetRandom<string>().Split(new char[]
		{
			','
		});
		base.animator.SetBool("Peashot", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Peashot_In", false, true);
		base.animator.Play("Peashot_Zinger");
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].ToLower() == "p")
			{
				this.peashotRoot.LookAt2D(PlayerManager.GetNext().center);
				for (int c = 0; c < p.colorString.Length; c++)
				{
					int color = 0;
					char c2 = p.colorString[c];
					if (c2 != 'O')
					{
						if (c2 != 'P')
						{
							if (c2 == 'B')
							{
								color = 1;
							}
						}
						else
						{
							color = 2;
						}
					}
					else
					{
						color = 0;
					}
					AudioManager.Play("level_dragon_left_dragon_peashot_fire");
					this.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_fire");
					(this.peashotPrefab.Create(this.peashotRoot.position, this.peashotRoot.eulerAngles.z, p.speed) as DragonLevelPeashot).color = color;
					yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
				}
			}
			else
			{
				float delay = 0f;
				Parser.FloatTryParse(pattern[i], out delay);
				yield return CupheadTime.WaitForSeconds(this, delay);
			}
		}
		base.animator.SetBool("Peashot", false);
		yield return base.animator.WaitForAnimationToStart(this, "Peashot_Out", false);
		AudioManager.Play("level_dragon_left_dragon_peashot_out");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_out");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DragonLevelDragon.State.Idle;
		yield break;
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x001150FC File Offset: 0x001134FC
	private void SpawnSmokeFX()
	{
		Effect effect = UnityEngine.Object.Instantiate<Effect>(this.smokePrefab);
		effect.transform.position = this.smokeRoot.transform.position;
		effect.GetComponent<Animator>().Play((!Rand.Bool()) ? "Smoke_FX_B" : "Smoke_FX_A");
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x00115154 File Offset: 0x00113554
	public void StartMeteor()
	{
		this.state = DragonLevelDragon.State.Meteor;
		base.StartCoroutine(this.meteor_cr());
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x0011516C File Offset: 0x0011356C
	private void FireMeteor()
	{
		AudioManager.Play("level_dragon_left_dragon_meteor_spit");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_spit");
		if (this.meteorState == DragonLevelMeteor.State.Both)
		{
			this.meteorPrefab.Create(this.mouthRoot.position, new DragonLevelMeteor.Properties(this.currentMeteorProperties.timeY, this.currentMeteorProperties.speedX, DragonLevelMeteor.State.Up));
			this.meteorPrefab.Create(this.mouthRoot.position, new DragonLevelMeteor.Properties(this.currentMeteorProperties.timeY, this.currentMeteorProperties.speedX, DragonLevelMeteor.State.Down));
		}
		else
		{
			this.meteorPrefab.Create(this.mouthRoot.position, new DragonLevelMeteor.Properties(this.currentMeteorProperties.timeY, this.currentMeteorProperties.speedX, this.meteorState));
		}
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x00115250 File Offset: 0x00113650
	private IEnumerator meteor_cr()
	{
		this.currentMeteorProperties = base.properties.CurrentState.meteor;
		char[] meteorPattern = this.currentMeteorProperties.pattern.GetRandom<string>().ToCharArray();
		base.animator.SetTrigger("OnMeteor");
		base.animator.SetBool("Repeat", true);
		yield return base.animator.WaitForAnimationToStart(this, "MeteorStart", false);
		AudioManager.Play("level_dragon_left_dragon_meteor_start");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_start");
		for (int i = 0; i < meteorPattern.Length; i++)
		{
			char c = meteorPattern[i];
			switch (c)
			{
			case 'B':
				this.meteorState = DragonLevelMeteor.State.Both;
				break;
			default:
				if (c != 'U')
				{
				}
				this.meteorState = DragonLevelMeteor.State.Up;
				break;
			case 'D':
				this.meteorState = DragonLevelMeteor.State.Down;
				break;
			case 'F':
				this.meteorState = DragonLevelMeteor.State.Forward;
				break;
			}
			if (i >= meteorPattern.Length - 1)
			{
				base.animator.SetBool("Repeat", false);
			}
			yield return base.animator.WaitForAnimationToStart(this, "Meteor_Anticipation_Loop", false);
			AudioManager.Play("level_dragon_left_dragon_meteor_anticipation_loop");
			this.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_anticipation_loop");
			yield return CupheadTime.WaitForSeconds(this, this.currentMeteorProperties.shotDelay);
			base.animator.SetTrigger("OnMeteor");
			AudioManager.Stop("level_dragon_left_dragon_meteor_anticipation_loop");
			yield return base.animator.WaitForAnimationToStart(this, "Meteor_Attack", false);
			AudioManager.Play("level_dragon_left_dragon_meteor_attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Meteor_Attack", false, true);
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Meteor_Attack_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.currentMeteorProperties.hesitate);
		this.state = DragonLevelDragon.State.Idle;
		yield break;
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x0011526B File Offset: 0x0011366B
	public void Leave()
	{
		base.StartCoroutine(this.leave_cr());
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x0011527C File Offset: 0x0011367C
	private IEnumerator leave_cr()
	{
		while (this.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		Vector2 endPos = base.transform.position;
		endPos.x += 500f;
		yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, endPos, EaseUtils.EaseType.easeInSine, 1.5f));
		this.damages.SetActive(false);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		Vector2 dashEndPos = this.dash.position;
		dashEndPos.x = -1300f;
		this.StopWingSFX();
		AudioManager.Play("level_dragon_dash");
		yield return base.StartCoroutine(this.tween_cr(this.dash, this.dash.position, dashEndPos, EaseUtils.EaseType.linear, 1.3f));
		yield return CupheadTime.WaitForSeconds(this, 0.75f);
		this.leftSideDragon.StartIntro();
		UnityEngine.Object.Destroy(this.dash.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x00115298 File Offset: 0x00113698
	private IEnumerator tween_cr(Transform trans, Vector2 start, Vector2 end, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.position = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			trans.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		trans.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x040026E3 RID: 9955
	[Space(10f)]
	[SerializeField]
	private Transform mouthRoot;

	// Token: 0x040026E4 RID: 9956
	[SerializeField]
	private DragonLevelMeteor meteorPrefab;

	// Token: 0x040026E5 RID: 9957
	[SerializeField]
	private Effect smokePrefab;

	// Token: 0x040026E6 RID: 9958
	[SerializeField]
	private Transform smokeRoot;

	// Token: 0x040026E7 RID: 9959
	[Space(10f)]
	[SerializeField]
	private DragonLevelPeashot peashotPrefab;

	// Token: 0x040026E8 RID: 9960
	[SerializeField]
	private Transform peashotRoot;

	// Token: 0x040026E9 RID: 9961
	[Space(10f)]
	[SerializeField]
	private Transform chargeRoot;

	// Token: 0x040026EA RID: 9962
	[SerializeField]
	private Transform dash;

	// Token: 0x040026EB RID: 9963
	[SerializeField]
	private DragonLevelLeftSideDragon leftSideDragon;

	// Token: 0x040026EC RID: 9964
	[SerializeField]
	private GameObject damages;

	// Token: 0x040026ED RID: 9965
	private LevelProperties.Dragon.Meteor currentMeteorProperties;

	// Token: 0x040026EE RID: 9966
	private DamageDealer damageDealer;

	// Token: 0x040026EF RID: 9967
	private DamageReceiver damageReceiver;

	// Token: 0x040026F0 RID: 9968
	private bool dead;

	// Token: 0x040026F1 RID: 9969
	private DragonLevelMeteor.State meteorState;

	// Token: 0x020005EE RID: 1518
	public enum State
	{
		// Token: 0x040026F3 RID: 9971
		Init,
		// Token: 0x040026F4 RID: 9972
		Idle,
		// Token: 0x040026F5 RID: 9973
		Meteor,
		// Token: 0x040026F6 RID: 9974
		Peashot
	}
}
