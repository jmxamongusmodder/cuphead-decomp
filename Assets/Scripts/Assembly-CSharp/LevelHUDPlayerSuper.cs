using System;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class LevelHUDPlayerSuper : AbstractLevelHUDComponent
{
	// Token: 0x06001253 RID: 4691 RVA: 0x000A9958 File Offset: 0x000A7D58
	public override void Init(LevelHUDPlayer hud)
	{
		base.Init(hud);
		this.cardTemplate.Init(base._player.id, base._player.stats.ExCost);
		this.cards = new LevelHUDPlayerSuperCard[5];
		this.cards[0] = this.cardTemplate;
		for (int i = 1; i < this.cards.Length; i++)
		{
			Vector3 localPosition = this.cardTemplate.transform.localPosition + new Vector3(18f * (float)i, 0f, 0f);
			LevelHUDPlayerSuperCard levelHUDPlayerSuperCard = UnityEngine.Object.Instantiate<LevelHUDPlayerSuperCard>(this.cardTemplate);
			levelHUDPlayerSuperCard.rectTransform.SetParent(this.cardTemplate.rectTransform.parent, false);
			levelHUDPlayerSuperCard.rectTransform.localPosition = localPosition;
			levelHUDPlayerSuperCard.Init(base._player.id, base._player.stats.ExCost);
			this.cards[i] = levelHUDPlayerSuperCard;
		}
		this.OnSuperChanged(base._player.stats.SuperMeter);
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x000A9A68 File Offset: 0x000A7E68
	public void OnSuperChanged(float super)
	{
		for (int i = 0; i < this.cards.Length; i++)
		{
			float num = base._player.stats.SuperMeterMax / (float)this.cards.Length;
			float num2 = num * (float)i;
			this.cards[i].SetAmount(super - num2);
		}
		if (super >= base._player.stats.SuperMeterMax)
		{
			foreach (LevelHUDPlayerSuperCard levelHUDPlayerSuperCard in this.cards)
			{
				if (!levelHUDPlayerSuperCard.animator.GetBool("Super"))
				{
					this.superToReady = true;
				}
				else
				{
					this.superToReady = false;
				}
			}
			if (this.superToReady)
			{
				if ((base._player.id == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (base._player.id == PlayerId.PlayerTwo && PlayerManager.player1IsMugman))
				{
					if (!AudioManager.CheckIfPlaying("player_parry_power_increment_cuphead"))
					{
						AudioManager.Play("player_parry_power_increment_cuphead");
					}
				}
				else if (!AudioManager.CheckIfPlaying("player_parry_power_increment_mugman"))
				{
					AudioManager.Play("player_parry_power_increment_mugman");
				}
			}
			foreach (LevelHUDPlayerSuperCard levelHUDPlayerSuperCard2 in this.cards)
			{
				levelHUDPlayerSuperCard2.SetSuper(true);
				if (this.lastSuper != super)
				{
					PlayerId id = base._player.id;
					if (id != PlayerId.PlayerOne)
					{
						if (id == PlayerId.PlayerTwo)
						{
							levelHUDPlayerSuperCard2.animator.Play((!PlayerManager.player1IsMugman) ? "Mugman_Idle_B" : "Cuphead_Idle_B", 0, 0f);
						}
					}
					else
					{
						levelHUDPlayerSuperCard2.animator.Play((!PlayerManager.player1IsMugman) ? "Cuphead_Idle_B" : "Mugman_Idle_B", 0, 0f);
					}
				}
			}
		}
		else
		{
			for (int l = 0; l < this.cards.Length; l++)
			{
				this.cards[l].SetSuper(false);
				float num3 = base._player.stats.SuperMeterMax / (float)this.cards.Length;
				if (super >= num3 + num3 * (float)l)
				{
					this.cards[l].SetEx(true);
					if (this.cards[l].animator.GetCurrentAnimatorStateInfo(0).IsName("Cuphead_Spin_A"))
					{
						if (!AudioManager.CheckIfPlaying("player_parry_power_increment_cuphead"))
						{
							AudioManager.Play("player_parry_power_increment_cuphead");
						}
					}
					else if (this.cards[l].animator.GetCurrentAnimatorStateInfo(0).IsName("Mugman_Spin_A") && !AudioManager.CheckIfPlaying("player_parry_power_increment_mugman"))
					{
						AudioManager.Play("player_parry_power_increment_mugman");
					}
				}
				else
				{
					this.cards[l].SetEx(false);
				}
			}
		}
		this.superToReady = false;
		this.lastSuper = super;
	}

	// Token: 0x04001BBE RID: 7102
	private const float SPACE = 18f;

	// Token: 0x04001BBF RID: 7103
	[SerializeField]
	private LevelHUDPlayerSuperCard cardTemplate;

	// Token: 0x04001BC0 RID: 7104
	private LevelHUDPlayerSuperCard[] cards;

	// Token: 0x04001BC1 RID: 7105
	private float lastSuper;

	// Token: 0x04001BC2 RID: 7106
	private bool superToReady;
}
