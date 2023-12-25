using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class BeeLevelQueen : LevelProperties.Bee.Entity
{
	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06001733 RID: 5939 RVA: 0x000D03D3 File Offset: 0x000CE7D3
	// (set) Token: 0x06001734 RID: 5940 RVA: 0x000D03DB File Offset: 0x000CE7DB
	public BeeLevelQueen.State state { get; private set; }

	// Token: 0x06001735 RID: 5941 RVA: 0x000D03E4 File Offset: 0x000CE7E4
	protected override void Awake()
	{
		base.Awake();
		base.RegisterCollisionChild(this.head.gameObject);
		this.EnableBody(false);
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x000D043D File Offset: 0x000CE83D
	private void Start()
	{
		AudioManager.Play("bee_queen_intro_vocal");
		this.emitAudioFromObject.Add("bee_queen_intro_vocal");
		Level.Current.OnIntroEvent += this.OnIntro;
	}

	// Token: 0x06001737 RID: 5943 RVA: 0x000D046F File Offset: 0x000CE86F
	private void OnIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001738 RID: 5944 RVA: 0x000D047E File Offset: 0x000CE87E
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x000D0496 File Offset: 0x000CE896
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x000D04BF File Offset: 0x000CE8BF
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(this.followerRoot.position, this.followerRadius);
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x000D04F4 File Offset: 0x000CE8F4
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != BeeLevelQueen.State.Death)
		{
			this.state = BeeLevelQueen.State.Death;
			this.Death();
		}
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x000D0540 File Offset: 0x000CE940
	private void EnableBody(bool p)
	{
		this.head.SetActive(p);
		this.body.SetActive(p);
		this.chain.SetActive(p);
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x000D0568 File Offset: 0x000CE968
	private void MagicEffect()
	{
		Transform transform = this.dustEffect.Create(base.transform.position).transform;
		Transform transform2 = this.sparkEffect.Create(base.transform.position).transform;
		transform.SetParent(base.transform);
		transform2.SetParent(base.transform);
		transform.ResetLocalTransforms();
		transform2.ResetLocalTransforms();
	}

	// Token: 0x0600173E RID: 5950 RVA: 0x000D05D1 File Offset: 0x000CE9D1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.puff = null;
		this.spitPrefab = null;
		this.blackHolePrefab = null;
		this.trianglePrefab = null;
		this.triangleInvinciblePrefab = null;
		this.followerPrefab = null;
		this.dustEffect = null;
		this.sparkEffect = null;
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x000D0614 File Offset: 0x000CEA14
	private IEnumerator intro_cr()
	{
		this.SetTrigger(BeeLevelQueen.Triggers.Continue);
		this.state = BeeLevelQueen.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x000D062F File Offset: 0x000CEA2F
	private void SfxIntroKnife()
	{
		AudioManager.Play("bee_queen_intro_cutlery");
		this.emitAudioFromObject.Add("bee_queen_intro_cutlery");
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x000D064B File Offset: 0x000CEA4B
	private void SfxIntroSnap()
	{
		AudioManager.Play("bee_queen_intro_finger_click");
		this.emitAudioFromObject.Add("bee_queen_intro_finger_click");
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x000D0667 File Offset: 0x000CEA67
	public void StartChain()
	{
		this.state = BeeLevelQueen.State.Chain;
		base.StartCoroutine(this.chain_cr());
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x000D067D File Offset: 0x000CEA7D
	private void FireChainStartSFX()
	{
		AudioManager.Play("bee_queen_chain_head_spit_start");
		this.emitAudioFromObject.Add("bee_queen_chain_head_spit_start");
	}

	// Token: 0x06001744 RID: 5956 RVA: 0x000D069C File Offset: 0x000CEA9C
	private void FireChainProjectile()
	{
		AudioManager.Play("bee_queen_chain_head_spit_attack");
		this.emitAudioFromObject.Add("bee_queen_chain_head_spit_attack");
		this.spitPrefab.Create(this.spitRoot.position, new Vector2(base.transform.localScale.x, 1f), this.currentChain.speed, new Vector2(this.currentChain.timeX, this.currentChain.timeY));
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x000D0724 File Offset: 0x000CEB24
	private void ChainFlip()
	{
		base.transform.SetScale(new float?(base.transform.localScale.x * -1f), new float?(1f), new float?(1f));
	}

	// Token: 0x06001746 RID: 5958 RVA: 0x000D0770 File Offset: 0x000CEB70
	private IEnumerator chain_cr()
	{
		this.currentChain = base.properties.CurrentState.chain;
		base.transform.ResetLocalTransforms();
		base.transform.SetPosition(new float?(-250f), new float?(0f), new float?(0f));
		base.animator.Play("Warning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		base.transform.SetPosition(new float?(0f), new float?(550f), new float?(0f));
		this.EnableBody(true);
		this.SetBool(BeeLevelQueen.Bools.Repeat, true);
		base.animator.Play("Chain_Idle");
		base.animator.Play("Head_Closed_Idle", base.animator.GetLayerIndex("Head"));
		yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeOutQuart, 0.6f));
		AudioManager.Play("bee_queen_chain_ascend_vocal");
		this.emitAudioFromObject.Add("bee_queen_chain_ascend_vocal");
		AudioManager.Play("bee_queen_chain_head_ascend");
		this.emitAudioFromObject.Add("bee_queen_chain_head_ascend");
		base.StartCoroutine(this.tween_cr(this.chain.transform, this.chain.transform.position, new Vector2(0f, -100f), EaseUtils.EaseType.easeInQuart, 0.6f));
		yield return base.StartCoroutine(this.tween_cr(this.head.transform, this.head.transform.position, new Vector2(0f, -100f), EaseUtils.EaseType.easeInQuart, 0.6f));
		CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		base.animator.Play("Spit_Start", base.animator.GetLayerIndex("Head"));
		yield return CupheadTime.WaitForSeconds(this, 1f);
		if (!base.properties.CurrentState.chain.chainForever)
		{
			for (int i = 0; i < this.currentChain.count; i++)
			{
				AudioManager.Play("bee_chain_head_spit_delay");
				this.emitAudioFromObject.Add("bee_chain_head_spit_delay");
				yield return CupheadTime.WaitForSeconds(this, this.currentChain.delay);
				if (i >= this.currentChain.count - 1)
				{
					this.SetBool(BeeLevelQueen.Bools.Repeat, false);
				}
				this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			}
			yield return base.animator.WaitForAnimationToEnd(this, "Spit_Attack_End", base.animator.GetLayerIndex("Head"), false, true);
			AudioManager.Play("bee_queen_chain_head_decend");
			this.emitAudioFromObject.Add("bee_queen_chain_head_decend");
			base.StartCoroutine(this.tween_cr(this.chain.transform, this.chain.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeInQuart, 0.6f));
			yield return base.StartCoroutine(this.tween_cr(this.head.transform, this.head.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeInQuart, 0.6f));
			CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
			yield return CupheadTime.WaitForSeconds(this, 0.7f);
			yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, new Vector2(0f, 550f), EaseUtils.EaseType.easeInQuart, 0.6f));
			this.EnableBody(false);
			yield return CupheadTime.WaitForSeconds(this, this.currentChain.hesitate);
			this.state = BeeLevelQueen.State.Idle;
			yield break;
		}
		for (;;)
		{
			AudioManager.Play("bee_chain_head_spit_delay");
			this.emitAudioFromObject.Add("bee_chain_head_spit_delay");
			yield return CupheadTime.WaitForSeconds(this, this.currentChain.delay);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			yield return null;
		}
	}

	// Token: 0x06001747 RID: 5959 RVA: 0x000D078B File Offset: 0x000CEB8B
	public void StartBlackHole()
	{
		this.state = BeeLevelQueen.State.BlackHole;
		base.StartCoroutine(this.blackHole_cr());
	}

	// Token: 0x06001748 RID: 5960 RVA: 0x000D07A4 File Offset: 0x000CEBA4
	private IEnumerator blackHole_cr()
	{
		base.transform.ResetLocalTransforms();
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?(1f), new float?(1f));
		base.transform.SetPosition(new float?(290f * base.transform.localScale.x), null, null);
		base.animator.Play("Warning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		this.ClearTrigger(BeeLevelQueen.Triggers.Continue);
		this.SetAttackAnim(BeeLevelQueen.AttackAnimations.BlackHole);
		base.animator.Play("Spell_Start");
		LevelProperties.Bee.BlackHole properties = base.properties.CurrentState.blackHole;
		string[] patternStrings = properties.patterns[UnityEngine.Random.Range(0, properties.patterns.Length)].Split(new char[]
		{
			','
		});
		int[] patternArray = new int[patternStrings.Length];
		for (int j = 0; j < patternStrings.Length; j++)
		{
			Parser.IntTryParse(patternStrings[j], out patternArray[j]);
			patternArray[j] = Mathf.Clamp(patternArray[j], 0, 2);
		}
		int i = 0;
		int count = patternArray.Length;
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_Start", false, true);
		AudioManager.PlayLoop("bee_queen_spell_shake_loop");
		this.emitAudioFromObject.Add("bee_queen_spell_shake_loop");
		while (i < count)
		{
			yield return CupheadTime.WaitForSeconds(this, properties.chargeTime);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Charge_End", false, true);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Attack_Start", false, true);
			BeeLevelQueenBlackHole b = this.blackHolePrefab.Create(this.blackHoleRoots[patternArray[i]].position) as BeeLevelQueenBlackHole;
			b.speed = properties.speed;
			b.health = properties.health;
			b.childDelay = properties.childDelay;
			b.childSpeed = (float)properties.childSpeed;
			if (properties.damageable)
			{
				b.gameObject.AddComponent<Rigidbody2D>();
			}
			yield return CupheadTime.WaitForSeconds(this, properties.attackTime);
			i++;
			this.SetBool(BeeLevelQueen.Bools.Repeat, i != count);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_End", false, true);
		AudioManager.Stop("bee_queen_spell_shake_loop");
		base.transform.SetPosition(new float?(0f), null, null);
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = BeeLevelQueen.State.Idle;
		yield break;
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x000D07BF File Offset: 0x000CEBBF
	public void StartTriangle()
	{
		this.state = BeeLevelQueen.State.Triangle;
		base.StartCoroutine(this.triangle_cr());
	}

	// Token: 0x0600174A RID: 5962 RVA: 0x000D07D8 File Offset: 0x000CEBD8
	private IEnumerator triangle_cr()
	{
		base.transform.ResetLocalTransforms();
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?(1f), new float?(1f));
		base.transform.SetPosition(new float?(290f * base.transform.localScale.x), null, null);
		base.animator.Play("Warning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		this.ClearTrigger(BeeLevelQueen.Triggers.Continue);
		this.SetAttackAnim(BeeLevelQueen.AttackAnimations.Triangle);
		base.animator.Play("Spell_Start");
		this.SetBool(BeeLevelQueen.Bools.Repeat, false);
		LevelProperties.Bee.Triangle properties = base.properties.CurrentState.triangle;
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_Start", false, true);
		AudioManager.PlayLoop("bee_queen_spell_shake_loop");
		this.emitAudioFromObject.Add("bee_queen_spell_shake_loop");
		int i = 0;
		while (i < properties.count)
		{
			yield return CupheadTime.WaitForSeconds(this, properties.chargeTime);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Charge_End", false, true);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Attack_Start", false, true);
			BeeLevelQueenTriangle.Properties p = new BeeLevelQueenTriangle.Properties(PlayerManager.GetNext(), properties.introTime, properties.speed, properties.rotationSpeed, properties.health, properties.childSpeed, properties.childDelay, properties.childHealth, properties.childCount, properties.damageable);
			if (properties.damageable)
			{
				this.trianglePrefab.Create(p);
			}
			else
			{
				this.triangleInvinciblePrefab.Create(p);
			}
			yield return CupheadTime.WaitForSeconds(this, properties.attackTime);
			i++;
			this.SetBool(BeeLevelQueen.Bools.Repeat, i != properties.count);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_End", false, true);
		AudioManager.Stop("bee_queen_spell_shake_loop");
		base.transform.SetPosition(new float?(0f), null, null);
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = BeeLevelQueen.State.Idle;
		yield break;
	}

	// Token: 0x0600174B RID: 5963 RVA: 0x000D07F3 File Offset: 0x000CEBF3
	public void StartMorph()
	{
		base.StartCoroutine(this.morph_cr());
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x000D0804 File Offset: 0x000CEC04
	private IEnumerator morph_cr()
	{
		float t = 0f;
		float time = 2.5f;
		float moveSpeed = 0f;
		base.animator.Play("Warning_Trans");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning_Trans", false, true);
		AudioManager.PlayLoop("bee_queen_spell_antic");
		this.emitAudioFromObject.Add("bee_queen_spell_antic");
		Vector3 endPos = new Vector3(0f, 230f);
		Vector3 startPos = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(startPos, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = endPos;
		AudioManager.Stop("bee_queen_spell_antic");
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Morph_Morph", false, true);
		yield return CupheadTime.WaitForSeconds(this, 0.54f);
		t = 0f;
		while (t < 0.76f)
		{
			moveSpeed = ((t >= 0.3f) ? 300f : 800f);
			base.transform.position += Vector3.up * moveSpeed * CupheadTime.Delta;
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		time = 0.67f;
		startPos = base.transform.position;
		endPos = new Vector3(0f, -960f);
		base.StartCoroutine(this.spawn_puffs_cr());
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(startPos, endPos, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.airplane.StartIntro();
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x000D0820 File Offset: 0x000CEC20
	private void SnapPosition()
	{
		base.transform.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Player.ToString();
		base.transform.GetComponent<SpriteRenderer>().sortingOrder = 100;
		base.transform.position = new Vector3(0f, 960f);
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x000D0878 File Offset: 0x000CEC78
	private void MoveHoney()
	{
		base.StartCoroutine(this.move_honey_cr());
		base.StartCoroutine(this.move_bee_cr());
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x000D0894 File Offset: 0x000CEC94
	private IEnumerator move_honey_cr()
	{
		float t = 0f;
		float time = 2.5f;
		Vector3 startPos = this.bottomHoney.transform.position;
		Vector3 endPos = new Vector3(0f, -560f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.bottomHoney.transform.position = Vector2.Lerp(startPos, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.bottomHoney.transform.position = endPos;
		base.StartCoroutine(CupheadLevelCamera.Current.change_zoom_cr(0.97f, 2f));
		yield return null;
		yield break;
	}

	// Token: 0x06001750 RID: 5968 RVA: 0x000D08B0 File Offset: 0x000CECB0
	private IEnumerator move_bee_cr()
	{
		float t = 0f;
		float time = 3f;
		while (t < time)
		{
			base.transform.position += Vector3.up * 50f * CupheadTime.Delta;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x000D08CC File Offset: 0x000CECCC
	private IEnumerator spawn_puffs_cr()
	{
		foreach (Transform root in this.puffRoots)
		{
			this.puff.Create(root.position);
			yield return CupheadTime.WaitForSeconds(this, 0.134f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x000D08E7 File Offset: 0x000CECE7
	public void StartFollower()
	{
		this.state = BeeLevelQueen.State.Triangle;
		base.StartCoroutine(this.follower_cr());
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x000D0900 File Offset: 0x000CED00
	private IEnumerator follower_cr()
	{
		base.transform.ResetLocalTransforms();
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?(1f), new float?(1f));
		base.transform.SetPosition(new float?(290f * base.transform.localScale.x), null, null);
		base.animator.Play("Warning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		this.ClearTrigger(BeeLevelQueen.Triggers.Continue);
		this.SetAttackAnim(BeeLevelQueen.AttackAnimations.Follower);
		base.animator.Play("Spell_Start");
		this.SetBool(BeeLevelQueen.Bools.Repeat, false);
		LevelProperties.Bee.Follower properties = base.properties.CurrentState.follower;
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_Start", false, true);
		int i = 0;
		while (i < properties.count)
		{
			yield return CupheadTime.WaitForSeconds(this, properties.chargeTime);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Charge_End", false, true);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Attack_Start", false, true);
			Vector2 a = this.followerRoot.position;
			Vector2 vector = new Vector2((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1));
			Vector2 pos = a + vector.normalized * (this.followerRadius * UnityEngine.Random.value);
			BeeLevelQueenFollower.Properties p = new BeeLevelQueenFollower.Properties(PlayerManager.GetNext(), properties.introTime, properties.homingSpeed, properties.homingRotation, properties.homingTime, properties.health, properties.childDelay, properties.childHealth, properties.parryable);
			if (properties.damageable)
			{
				this.followerPrefab.Create(pos, p).gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
			}
			else
			{
				this.followerPrefab.Create(pos, p);
			}
			yield return CupheadTime.WaitForSeconds(this, properties.attackTime);
			i++;
			this.SetBool(BeeLevelQueen.Bools.Repeat, i != properties.count);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_End", false, true);
		base.transform.SetPosition(new float?(0f), null, null);
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = BeeLevelQueen.State.Idle;
		yield break;
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x000D091C File Offset: 0x000CED1C
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

	// Token: 0x06001755 RID: 5973 RVA: 0x000D0955 File Offset: 0x000CED55
	private void SetAttackAnim(BeeLevelQueen.AttackAnimations a)
	{
		this.SetInt(BeeLevelQueen.Integers.Attack, (int)a);
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x000D095F File Offset: 0x000CED5F
	private void SetTrigger(BeeLevelQueen.Triggers t)
	{
		base.animator.SetTrigger(t.ToString());
	}

	// Token: 0x06001757 RID: 5975 RVA: 0x000D0979 File Offset: 0x000CED79
	private void ClearTrigger(BeeLevelQueen.Triggers t)
	{
		base.animator.ResetTrigger(t.ToString());
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x000D0993 File Offset: 0x000CED93
	private void SetInt(BeeLevelQueen.Integers i, int value)
	{
		base.animator.SetInteger(i.ToString(), value);
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x000D09AE File Offset: 0x000CEDAE
	private void SetBool(BeeLevelQueen.Bools b, bool value)
	{
		base.animator.SetBool(b.ToString(), value);
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x000D09C9 File Offset: 0x000CEDC9
	private void Death()
	{
		base.animator.Play("Head_Closed_Idle");
		this.StopAllCoroutines();
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x000D09E1 File Offset: 0x000CEDE1
	private void SpellTossSFX()
	{
		AudioManager.Play("bee_queen_spell_toss");
		this.emitAudioFromObject.Add("bee_queen_spell_toss");
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x000D09FD File Offset: 0x000CEDFD
	private void SpellCastSFX()
	{
		AudioManager.Play("bee_queen_spell_cast");
		this.emitAudioFromObject.Add("bee_queen_spell_cast");
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x000D0A19 File Offset: 0x000CEE19
	private void AttackStartSFX()
	{
		AudioManager.Play("bee_queen_attack_start");
		this.emitAudioFromObject.Add("bee_queen_attack_start");
		AudioManager.PlayLoop("bee_queen_attack_loop");
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x000D0A3F File Offset: 0x000CEE3F
	private void AttackEndSFX()
	{
		AudioManager.Stop("bee_queen_attack_loop");
		AudioManager.Play("bee_queen_attack_end");
		this.emitAudioFromObject.Add("bee_queen_attack_end");
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x000D0A65 File Offset: 0x000CEE65
	private void WarningSFX()
	{
		AudioManager.Play("bee_queen_warning");
		this.emitAudioFromObject.Add("bee_queen_warning");
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x000D0A81 File Offset: 0x000CEE81
	private void FlyDownSFX()
	{
		AudioManager.Play("bee_airplane_fly_down");
		this.emitAudioFromObject.Add("bee_airplane_fly_down");
	}

	// Token: 0x04002070 RID: 8304
	public const float SPELL_X = 290f;

	// Token: 0x04002072 RID: 8306
	[SerializeField]
	private BeeLevelAirplane airplane;

	// Token: 0x04002073 RID: 8307
	[SerializeField]
	private Transform bottomHoney;

	// Token: 0x04002074 RID: 8308
	[SerializeField]
	private Effect puff;

	// Token: 0x04002075 RID: 8309
	[Space(5f)]
	[SerializeField]
	private Transform[] puffRoots;

	// Token: 0x04002076 RID: 8310
	[Space(5f)]
	[SerializeField]
	private GameObject head;

	// Token: 0x04002077 RID: 8311
	[SerializeField]
	private GameObject body;

	// Token: 0x04002078 RID: 8312
	[SerializeField]
	private GameObject chain;

	// Token: 0x04002079 RID: 8313
	[Space(10f)]
	[SerializeField]
	private BeeLevelQueenSpitProjectile spitPrefab;

	// Token: 0x0400207A RID: 8314
	[SerializeField]
	private Transform spitRoot;

	// Token: 0x0400207B RID: 8315
	[Space(10f)]
	[SerializeField]
	private BeeLevelQueenBlackHole blackHolePrefab;

	// Token: 0x0400207C RID: 8316
	[SerializeField]
	private Transform[] blackHoleRoots;

	// Token: 0x0400207D RID: 8317
	[Space(10f)]
	[SerializeField]
	private BeeLevelQueenTriangle trianglePrefab;

	// Token: 0x0400207E RID: 8318
	[SerializeField]
	private BeeLevelQueenTriangle triangleInvinciblePrefab;

	// Token: 0x0400207F RID: 8319
	[Space(10f)]
	[SerializeField]
	private float followerRadius = 200f;

	// Token: 0x04002080 RID: 8320
	[SerializeField]
	private Transform followerRoot;

	// Token: 0x04002081 RID: 8321
	[SerializeField]
	private BeeLevelQueenFollower followerPrefab;

	// Token: 0x04002082 RID: 8322
	[Space(10f)]
	[SerializeField]
	private Effect dustEffect;

	// Token: 0x04002083 RID: 8323
	[SerializeField]
	private Effect sparkEffect;

	// Token: 0x04002084 RID: 8324
	private DamageReceiver damageReceiver;

	// Token: 0x04002085 RID: 8325
	private DamageDealer damageDealer;

	// Token: 0x04002086 RID: 8326
	private LevelProperties.Bee.Chain currentChain;

	// Token: 0x02000517 RID: 1303
	public enum State
	{
		// Token: 0x04002088 RID: 8328
		Intro,
		// Token: 0x04002089 RID: 8329
		Idle,
		// Token: 0x0400208A RID: 8330
		BlackHole,
		// Token: 0x0400208B RID: 8331
		Triangle,
		// Token: 0x0400208C RID: 8332
		Follower,
		// Token: 0x0400208D RID: 8333
		Chain,
		// Token: 0x0400208E RID: 8334
		Death
	}

	// Token: 0x02000518 RID: 1304
	private enum AttackAnimations
	{
		// Token: 0x04002090 RID: 8336
		BlackHole,
		// Token: 0x04002091 RID: 8337
		Triangle,
		// Token: 0x04002092 RID: 8338
		Follower
	}

	// Token: 0x02000519 RID: 1305
	private enum Triggers
	{
		// Token: 0x04002094 RID: 8340
		Continue
	}

	// Token: 0x0200051A RID: 1306
	private enum Integers
	{
		// Token: 0x04002096 RID: 8342
		Attack
	}

	// Token: 0x0200051B RID: 1307
	private enum Bools
	{
		// Token: 0x04002098 RID: 8344
		Repeat
	}
}
