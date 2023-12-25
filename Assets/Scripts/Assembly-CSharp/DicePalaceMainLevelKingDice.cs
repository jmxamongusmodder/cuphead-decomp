using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D7 RID: 1495
public class DicePalaceMainLevelKingDice : LevelProperties.DicePalaceMain.Entity
{
	// Token: 0x06001D70 RID: 7536 RVA: 0x0010E3F8 File Offset: 0x0010C7F8
	private void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver.enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		Level.Current.OnWinEvent += this.OnDeath;
		AudioManager.Play("king_dice_intro");
		this.emitAudioFromObject.Add("king_dice_intro");
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x0010E470 File Offset: 0x0010C870
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x0010E484 File Offset: 0x0010C884
	public void StartKingDiceBattle()
	{
		AudioManager.FadeBGMVolume(0f, 0.5f, true);
		AudioManager.Play("king_dice_trans");
		AudioManager.PlayBGMPlaylistManually(false);
		base.animator.SetBool("IsAttacking", true);
		base.animator.SetBool("IsBattling", true);
		LevelIntroAnimation levelIntroAnimation = LevelIntroAnimation.Create(null);
		levelIntroAnimation.Play();
		base.StartCoroutine(this.cards_cr());
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x0010E4ED File Offset: 0x0010C8ED
	private void RevealSFX()
	{
		AudioManager.Play("king_dice_reveal");
		this.emitAudioFromObject.Add("king_dice_reveal");
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x0010E50C File Offset: 0x0010C90C
	public void RevealDice()
	{
		DicePalaceMainLevel dicePalaceMainLevel = Level.Current as DicePalaceMainLevel;
		dicePalaceMainLevel.GameManager.RevealDice();
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x0010E530 File Offset: 0x0010C930
	private IEnumerator cards_cr()
	{
		LevelProperties.DicePalaceMain.Cards p = base.properties.CurrentState.cards;
		int cardIndex = UnityEngine.Random.Range(0, p.cardString.Length);
		string[] sideString = p.cardSideOrder.GetRandom<string>().Split(new char[]
		{
			','
		});
		int suitIndex = UnityEngine.Random.Range(0, 3);
		int sideIndex = UnityEngine.Random.Range(0, sideString.Length);
		bool onLeft = false;
		Vector3 rootPos = Vector3.zero;
		this.damageReceiver.enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		for (;;)
		{
			string[] cardString = p.cardString[cardIndex].Split(new char[]
			{
				','
			});
			if (sideString[sideIndex][0] == 'L')
			{
				onLeft = true;
				rootPos = this.leftRoot.transform.position;
			}
			else if (sideString[sideIndex][0] == 'R')
			{
				onLeft = false;
				rootPos = this.rightRoot.transform.position;
			}
			else
			{
				global::Debug.LogError("Invalid pattern string", null);
			}
			base.animator.SetBool("OnLeftAttack", onLeft);
			yield return base.animator.WaitForAnimationToEnd(this, (!onLeft) ? "Attack_Right" : "Attack_Left", false, true);
			AudioManager.PlayLoop("king_dice_march_loop");
			this.emitAudioFromObject.Add("king_dice_march_loop");
			base.StartCoroutine(this.kd_laugh_cr());
			for (int i = 0; i < cardString.Length; i++)
			{
				if (cardString[i][0] == 'R')
				{
					DicePalaceMainLevelCard dicePalaceMainLevelCard = this.cardRegular.Create(rootPos, p, onLeft);
					dicePalaceMainLevelCard.transform.SetScale(new float?((float)((!onLeft) ? -1 : 1)), null, null);
					dicePalaceMainLevelCard.GetComponent<SpriteRenderer>().sortingOrder = i;
					suitIndex = (suitIndex + 1) % 3;
				}
				else if (cardString[i][0] == 'P')
				{
					DicePalaceMainLevelCard dicePalaceMainLevelCard2 = this.cardPink.Create(rootPos, p, onLeft);
					dicePalaceMainLevelCard2.transform.SetScale(new float?((float)((!onLeft) ? -1 : 1)), null, null);
					dicePalaceMainLevelCard2.GetComponent<SpriteRenderer>().sortingOrder = i;
				}
				else
				{
					global::Debug.LogError("Invalid pattern string", null);
				}
				yield return CupheadTime.WaitForSeconds(this, p.cardDelay);
			}
			AudioManager.Stop("king_dice_march_loop");
			base.animator.SetBool("IsAttacking", false);
			yield return CupheadTime.WaitForSeconds(this, p.hesitate);
			base.animator.SetBool("IsAttacking", true);
			sideIndex = (sideIndex + 1) % sideString.Length;
			cardIndex = (cardIndex + 1) % p.cardString.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x0010E54B File Offset: 0x0010C94B
	private void AttackSFX()
	{
		AudioManager.PlayLoop("king_dice_attack");
		this.emitAudioFromObject.Add("king_dice_attack");
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x0010E567 File Offset: 0x0010C967
	private void IntroSFX()
	{
		AudioManager.Play("king_dice_intro");
		this.emitAudioFromObject.Add("king_dice_intro");
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x0010E583 File Offset: 0x0010C983
	private void VoxCurious()
	{
		AudioManager.Play("vox_curious");
		this.emitAudioFromObject.Add("vox_curious");
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x0010E59F File Offset: 0x0010C99F
	private void AttackSFXStop()
	{
		AudioManager.Stop("king_dice_attack");
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x0010E5AC File Offset: 0x0010C9AC
	private void OnDeath()
	{
		AudioManager.PlayLoop("king_dice_death");
		AudioManager.Play("vox_death");
		this.emitAudioFromObject.Add("vox_death");
		base.animator.SetTrigger("OnDeath");
		this.StopAllCoroutines();
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Background";
		component.sortingOrder = 100;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x0010E61C File Offset: 0x0010CA1C
	private IEnumerator kd_laugh_cr()
	{
		MinMax delay = new MinMax(1f, 3.5f);
		while (base.animator.GetBool("IsAttacking"))
		{
			AudioManager.Play("king_dice_attack_vox");
			this.emitAudioFromObject.Add("king_dice_attack_vox");
			while (AudioManager.CheckIfPlaying("king_dice_attack_vox"))
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, delay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x0010E637 File Offset: 0x0010CA37
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.cardPink = null;
		this.cardRegular = null;
	}

	// Token: 0x04002663 RID: 9827
	[SerializeField]
	private Transform rightRoot;

	// Token: 0x04002664 RID: 9828
	[SerializeField]
	private Transform leftRoot;

	// Token: 0x04002665 RID: 9829
	[SerializeField]
	private DicePalaceMainLevelCard cardRegular;

	// Token: 0x04002666 RID: 9830
	[SerializeField]
	private DicePalaceMainLevelCard cardPink;

	// Token: 0x04002667 RID: 9831
	private DamageReceiver damageReceiver;
}
