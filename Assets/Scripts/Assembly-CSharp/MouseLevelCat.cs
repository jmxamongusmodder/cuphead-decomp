using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006E6 RID: 1766
public class MouseLevelCat : LevelProperties.Mouse.Entity
{
	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x060025C2 RID: 9666 RVA: 0x001617FD File Offset: 0x0015FBFD
	// (set) Token: 0x060025C3 RID: 9667 RVA: 0x00161805 File Offset: 0x0015FC05
	public MouseLevelCat.State state { get; private set; }

	// Token: 0x060025C4 RID: 9668 RVA: 0x00161810 File Offset: 0x0015FC10
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.headStartPos = this.head.localPosition;
		this.head.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060025C5 RID: 9669 RVA: 0x0016186D File Offset: 0x0015FC6D
	public override void LevelInit(LevelProperties.Mouse properties)
	{
		base.LevelInit(properties);
		this.fallingObjectsIndex = UnityEngine.Random.Range(0, properties.CurrentState.claw.fallingObjectStrings.Length);
	}

	// Token: 0x060025C6 RID: 9670 RVA: 0x00161894 File Offset: 0x0015FC94
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060025C7 RID: 9671 RVA: 0x001618A7 File Offset: 0x0015FCA7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.fallingObjectPrefabs = null;
	}

	// Token: 0x060025C8 RID: 9672 RVA: 0x001618B6 File Offset: 0x0015FCB6
	public void StartIntro()
	{
		base.properties.OnBossDeath += this.OnBossDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060025C9 RID: 9673 RVA: 0x001618DC File Offset: 0x0015FCDC
	private IEnumerator intro_cr()
	{
		this.head.GetComponent<Collider2D>().enabled = true;
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		base.transform.position = this.startPosition;
		base.animator.SetTrigger("StartIntro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 2, false, true);
		this.state = MouseLevelCat.State.Idle;
		yield break;
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x001618F7 File Offset: 0x0015FCF7
	public void EatMouse()
	{
		this.mouse.BeEaten();
		base.StartCoroutine(this.tail_cr());
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x00161914 File Offset: 0x0015FD14
	public void StartWallBreak()
	{
		this.wallAnimator.SetTrigger("OnContinue");
		foreach (GameObject obj in this.toDestroyOnWallBreakStart)
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x00161958 File Offset: 0x0015FD58
	public void EndWallBreak()
	{
		this.wallAnimator.SetTrigger("OnContinue");
		foreach (GameObject obj in this.toDestroyOnWallBreakEnd)
		{
			UnityEngine.Object.Destroy(obj);
		}
		UnityEngine.Object.Destroy(this.foreground);
		this.alternateForeground.SetActive(true);
	}

	// Token: 0x060025CD RID: 9677 RVA: 0x001619B4 File Offset: 0x0015FDB4
	private IEnumerator tail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 0.75f));
			base.animator.SetTrigger("SwitchTailDirection");
			yield return base.animator.WaitForAnimationToStart(this, "Idle_Left", 1, false);
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 0.75f));
			base.animator.SetTrigger("SwitchTailDirection");
			yield return base.animator.WaitForAnimationToStart(this, "Idle_Right", 1, false);
		}
		yield break;
	}

	// Token: 0x060025CE RID: 9678 RVA: 0x001619D0 File Offset: 0x0015FDD0
	private void BlinkMaybe()
	{
		this.blinks++;
		if (this.blinks >= this.maxBlinks)
		{
			this.blinks = 0;
			this.maxBlinks = UnityEngine.Random.Range(8, 16);
			this.blinkOverlaySprite.enabled = true;
			base.animator.SetBool("Blinking", true);
		}
		else
		{
			this.blinkOverlaySprite.enabled = false;
			base.animator.SetBool("Blinking", false);
		}
	}

	// Token: 0x060025CF RID: 9679 RVA: 0x00161A50 File Offset: 0x0015FE50
	public void StartClaw(bool left)
	{
		this.state = MouseLevelCat.State.Claw;
		base.StartCoroutine(this.claw_cr(left));
	}

	// Token: 0x060025D0 RID: 9680 RVA: 0x00161A68 File Offset: 0x0015FE68
	private IEnumerator claw_cr(bool left)
	{
		LevelProperties.Mouse.Claw p = base.properties.CurrentState.claw;
		MouseLevelCatPaw paw = (!left) ? this.rightPaw : this.leftPaw;
		float totalPawAttackTime = 0.584f + 2f * p.holdGroundTime;
		float totalPawLeaveTime = 0.584f * p.moveSpeed / p.leaveSpeed;
		float headMoveBackTime = p.holdGroundTime + totalPawLeaveTime + 0.417f;
		base.animator.SetTrigger((!left) ? "StartClawRight" : "StartClawLeft");
		yield return base.animator.WaitForAnimationToStart(this, (!left) ? "Claw_Right_Start" : "Claw_Left_Start", 2, false);
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
		paw.Attack(p);
		base.StartCoroutine(this.spawnFallingObjects_cr(left));
		yield return base.StartCoroutine(this.tween_cr(this.head.transform, this.headStartPos, this.headMoveTransform.localPosition, Quaternion.identity, this.headMoveTransform.rotation, EaseUtils.EaseType.easeOutQuad, totalPawAttackTime));
		base.StartCoroutine(this.tween_cr(this.head.transform, this.headMoveTransform.localPosition, this.headStartPos, this.headMoveTransform.rotation, Quaternion.identity, EaseUtils.EaseType.easeInOutSine, headMoveBackTime));
		yield return CupheadTime.WaitForSeconds(this, headMoveBackTime - 0.417f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, (!left) ? "Claw_Right_End" : "Claw_Left_End", 2, false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack);
		this.state = MouseLevelCat.State.Idle;
		yield break;
	}

	// Token: 0x060025D1 RID: 9681 RVA: 0x00161A8C File Offset: 0x0015FE8C
	private IEnumerator spawnFallingObjects_cr(bool left)
	{
		LevelProperties.Mouse.Claw p = base.properties.CurrentState.claw;
		MouseLevelCatPaw paw = (!left) ? this.rightPaw : this.leftPaw;
		yield return paw.animator.WaitForAnimationToStart(this, "Attack_Hit", false);
		this.fallingObjectsIndex = (this.fallingObjectsIndex + 1) % p.fallingObjectStrings.Length;
		string[] pattern = p.fallingObjectStrings[this.fallingObjectsIndex].Split(new char[]
		{
			','
		});
		float waitTime = 0f;
		foreach (string instruction in pattern)
		{
			if (instruction[0] == 'D')
			{
				Parser.FloatTryParse(instruction.Substring(1), out waitTime);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, waitTime);
				string[] positions = instruction.Split(new char[]
				{
					'-'
				});
				foreach (string s in positions)
				{
					float xPos = 0f;
					Parser.FloatTryParse(s, out xPos);
					this.fallingObjectPrefabs.RandomChoice<MouseLevelFallingObject>().Create(xPos, p);
				}
				waitTime = p.objectSpawnDelay;
			}
		}
		yield break;
	}

	// Token: 0x060025D2 RID: 9682 RVA: 0x00161AAE File Offset: 0x0015FEAE
	public void StartGhostMouse()
	{
		this.state = MouseLevelCat.State.GhostMouse;
		base.StartCoroutine(this.jailHead_cr());
	}

	// Token: 0x060025D3 RID: 9683 RVA: 0x00161AC4 File Offset: 0x0015FEC4
	private IEnumerator jailHead_cr()
	{
		MouseLevelGhostMouse[] ghostMice = (!base.properties.CurrentState.ghostMouse.fourMice) ? this.twoGhostMice : this.fourGhostMice;
		bool unspawnedGhosts = false;
		foreach (MouseLevelGhostMouse mouseLevelGhostMouse in ghostMice)
		{
			if (mouseLevelGhostMouse.state == MouseLevelGhostMouse.State.Unspawned)
			{
				unspawnedGhosts = true;
				break;
			}
		}
		if (unspawnedGhosts)
		{
			base.animator.SetTrigger("StartGhostMouse");
			yield return base.animator.WaitForAnimationToStart(this, "Jail_Loop", 2, false);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.ghostMouse.jailDuration);
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToEnd(this, "Jail_End", 2, false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.ghostMouse.hesitateAfterAttack);
		}
		this.state = MouseLevelCat.State.Idle;
		yield break;
	}

	// Token: 0x060025D4 RID: 9684 RVA: 0x00161ADF File Offset: 0x0015FEDF
	public void SpawnGhostMice()
	{
		base.StartCoroutine(this.spawnGhostMice_cr());
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x00161AF0 File Offset: 0x0015FEF0
	private IEnumerator spawnGhostMice_cr()
	{
		MouseLevelGhostMouse[] ghostMice = (!base.properties.CurrentState.ghostMouse.fourMice) ? this.twoGhostMice : this.fourGhostMice;
		ghostMice.Shuffle<MouseLevelGhostMouse>();
		foreach (MouseLevelGhostMouse mouse in ghostMice)
		{
			if (mouse.state == MouseLevelGhostMouse.State.Unspawned)
			{
				mouse.Spawn(base.properties);
				yield return CupheadTime.WaitForSeconds(this, 0.1f);
			}
		}
		while (ghostMice[ghostMice.Length - 1].state == MouseLevelGhostMouse.State.Intro)
		{
			yield return null;
		}
		if (!this.alreadyManagingGhostMice)
		{
			base.StartCoroutine(this.manageGhostMice_cr());
		}
		yield break;
	}

	// Token: 0x060025D6 RID: 9686 RVA: 0x00161B0C File Offset: 0x0015FF0C
	private IEnumerator manageGhostMice_cr()
	{
		this.alreadyManagingGhostMice = true;
		MouseLevelGhostMouse[] ghostMice = (!base.properties.CurrentState.ghostMouse.fourMice) ? this.twoGhostMice : this.fourGhostMice;
		int shotsTillPinkAttack = base.properties.CurrentState.ghostMouse.pinkBallRange.RandomInt();
		bool anyMiceSpawned = true;
		while (anyMiceSpawned)
		{
			ghostMice.Shuffle<MouseLevelGhostMouse>();
			anyMiceSpawned = false;
			foreach (MouseLevelGhostMouse mouse in ghostMice)
			{
				if (mouse.state != MouseLevelGhostMouse.State.Unspawned && mouse.state != MouseLevelGhostMouse.State.Dying)
				{
					anyMiceSpawned = true;
				}
				if (mouse.state == MouseLevelGhostMouse.State.Idle)
				{
					shotsTillPinkAttack--;
					bool pinkAttack = false;
					if (shotsTillPinkAttack == 0)
					{
						pinkAttack = true;
						shotsTillPinkAttack = base.properties.CurrentState.ghostMouse.pinkBallRange.RandomInt();
					}
					mouse.Attack(pinkAttack);
					while (mouse.state == MouseLevelGhostMouse.State.Attack)
					{
						yield return null;
					}
					yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.ghostMouse.attackDelayRange.RandomFloat());
				}
			}
			yield return null;
		}
		this.alreadyManagingGhostMice = false;
		yield break;
	}

	// Token: 0x060025D7 RID: 9687 RVA: 0x00161B28 File Offset: 0x0015FF28
	public void OnBossDeath()
	{
		this.state = MouseLevelCat.State.Dying;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(this.leftPaw.gameObject);
		UnityEngine.Object.Destroy(this.rightPaw.gameObject);
		this.head.transform.localPosition = this.headStartPos;
		this.head.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		base.animator.SetTrigger("Die");
		this.headFrontRenderer.sortingLayerName = "Enemies";
		MouseLevelGhostMouse[] array = (!base.properties.CurrentState.ghostMouse.fourMice) ? this.twoGhostMice : this.fourGhostMice;
		foreach (MouseLevelGhostMouse mouseLevelGhostMouse in array)
		{
			mouseLevelGhostMouse.Die();
		}
	}

	// Token: 0x060025D8 RID: 9688 RVA: 0x00161C18 File Offset: 0x00160018
	private IEnumerator tween_cr(Transform trans, Vector2 startPos, Vector2 endPos, Quaternion startRotation, Quaternion endRotation, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.localPosition = startPos;
		trans.localRotation = startRotation;
		float accumulator = 0f;
		while (t < time)
		{
			accumulator += CupheadTime.Delta;
			while (accumulator > 0.041666668f)
			{
				accumulator -= 0.041666668f;
				float t2 = EaseUtils.Ease(ease, 0f, 1f, t / time);
				trans.localPosition = Vector2.Lerp(startPos, endPos, t2);
				trans.localRotation = Quaternion.Slerp(startRotation, endRotation, t2);
				t += 0.041666668f;
			}
			yield return null;
		}
		trans.localPosition = endPos;
		trans.localRotation = endRotation;
		yield return null;
		yield break;
	}

	// Token: 0x060025D9 RID: 9689 RVA: 0x00161C61 File Offset: 0x00160061
	private void SoundCatIntro()
	{
		AudioManager.Play("level_mouse_cat_intro");
	}

	// Token: 0x060025DA RID: 9690 RVA: 0x00161C6D File Offset: 0x0016006D
	private void SoundCatJailEnd()
	{
		AudioManager.Play("level_mouse_cat_jail_end");
		this.emitAudioFromObject.Add("level_mouse_cat_jail_end");
	}

	// Token: 0x04002E46 RID: 11846
	private const string EnemiesLayerName = "Enemies";

	// Token: 0x04002E47 RID: 11847
	private const int BODY_LAYER = 0;

	// Token: 0x04002E48 RID: 11848
	private const int TAIL_LAYER = 1;

	// Token: 0x04002E49 RID: 11849
	private const int HEAD_LAYER = 2;

	// Token: 0x04002E4B RID: 11851
	[SerializeField]
	private Vector2 startPosition;

	// Token: 0x04002E4C RID: 11852
	[SerializeField]
	private MouseLevelBrokenCanMouse mouse;

	// Token: 0x04002E4D RID: 11853
	[SerializeField]
	private Animator wallAnimator;

	// Token: 0x04002E4E RID: 11854
	[SerializeField]
	private LevelPlatform wallPlatform;

	// Token: 0x04002E4F RID: 11855
	[SerializeField]
	private GameObject foreground;

	// Token: 0x04002E50 RID: 11856
	[SerializeField]
	private GameObject alternateForeground;

	// Token: 0x04002E51 RID: 11857
	[SerializeField]
	private GameObject[] toDestroyOnWallBreakStart;

	// Token: 0x04002E52 RID: 11858
	[SerializeField]
	private GameObject[] toDestroyOnWallBreakEnd;

	// Token: 0x04002E53 RID: 11859
	[SerializeField]
	private SpriteRenderer blinkOverlaySprite;

	// Token: 0x04002E54 RID: 11860
	[SerializeField]
	private Transform head;

	// Token: 0x04002E55 RID: 11861
	[SerializeField]
	private MouseLevelCatPaw leftPaw;

	// Token: 0x04002E56 RID: 11862
	[SerializeField]
	private MouseLevelCatPaw rightPaw;

	// Token: 0x04002E57 RID: 11863
	[SerializeField]
	private Transform headMoveTransform;

	// Token: 0x04002E58 RID: 11864
	[SerializeField]
	private MouseLevelFallingObject[] fallingObjectPrefabs;

	// Token: 0x04002E59 RID: 11865
	[SerializeField]
	private MouseLevelGhostMouse[] twoGhostMice;

	// Token: 0x04002E5A RID: 11866
	[SerializeField]
	private MouseLevelGhostMouse[] fourGhostMice;

	// Token: 0x04002E5B RID: 11867
	[SerializeField]
	private SpriteRenderer headFrontRenderer;

	// Token: 0x04002E5C RID: 11868
	private DamageReceiver damageReceiver;

	// Token: 0x04002E5D RID: 11869
	private Vector2 headStartPos;

	// Token: 0x04002E5E RID: 11870
	private int fallingObjectsIndex;

	// Token: 0x04002E5F RID: 11871
	private int blinks;

	// Token: 0x04002E60 RID: 11872
	private int maxBlinks = 8;

	// Token: 0x04002E61 RID: 11873
	private bool alreadyManagingGhostMice;

	// Token: 0x020006E7 RID: 1767
	public enum State
	{
		// Token: 0x04002E63 RID: 11875
		Init,
		// Token: 0x04002E64 RID: 11876
		Idle,
		// Token: 0x04002E65 RID: 11877
		Claw,
		// Token: 0x04002E66 RID: 11878
		GhostMouse,
		// Token: 0x04002E67 RID: 11879
		Dying
	}
}
