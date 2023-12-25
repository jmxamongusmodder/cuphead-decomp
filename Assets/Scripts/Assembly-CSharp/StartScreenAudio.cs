using System;
using Rewired;
using UnityEngine;

// Token: 0x020009B5 RID: 2485
public class StartScreenAudio : AbstractMonoBehaviour
{
	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06003A45 RID: 14917 RVA: 0x00211C84 File Offset: 0x00210084
	public static StartScreenAudio Instance
	{
		get
		{
			return StartScreenAudio.startScreenAudio;
		}
	}

	// Token: 0x06003A46 RID: 14918 RVA: 0x00211C8B File Offset: 0x0021008B
	private void Start()
	{
		this.blockInput = CreditsScreen.goodEnding;
		this.players = new Player[]
		{
			PlayerManager.GetPlayerInput(PlayerId.PlayerOne),
			PlayerManager.GetPlayerInput(PlayerId.PlayerTwo)
		};
	}

	// Token: 0x06003A47 RID: 14919 RVA: 0x00211CB8 File Offset: 0x002100B8
	private void Update()
	{
		if (this.blockInput)
		{
			return;
		}
		if (this.codeIndex < this.code.Length)
		{
			foreach (Player player in this.players)
			{
				if (player.GetAnyButtonDown())
				{
					if (player.GetButtonDown((int)this.code[this.codeIndex]))
					{
						this.codeIndex++;
					}
					else if (!player.GetButtonDown((int)this.code[this.codeIndex]))
					{
						this.codeIndex = 0;
					}
				}
			}
		}
		else
		{
			if (this.bgmAlt2.clip == null)
			{
				this.bgmAlt2.GetComponent<DeferredAudioSource>().Initialize();
			}
			AudioManager.StopBGM();
			this.bgmAlt2.Play();
			this.blockInput = true;
		}
	}

	// Token: 0x06003A48 RID: 14920 RVA: 0x00211D9A File Offset: 0x0021019A
	protected override void Awake()
	{
		base.Awake();
		StartScreenAudio.startScreenAudio = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0400427B RID: 17019
	[SerializeField]
	private AudioSource bgmAlt2;

	// Token: 0x0400427C RID: 17020
	private static StartScreenAudio startScreenAudio;

	// Token: 0x0400427D RID: 17021
	private CupheadButton[] code = new CupheadButton[]
	{
		CupheadButton.MenuUp,
		CupheadButton.MenuUp,
		CupheadButton.MenuDown,
		CupheadButton.MenuDown,
		CupheadButton.MenuLeft,
		CupheadButton.MenuRight,
		CupheadButton.MenuLeft,
		CupheadButton.MenuRight,
		CupheadButton.Cancel,
		CupheadButton.Accept
	};

	// Token: 0x0400427E RID: 17022
	private int codeIndex;

	// Token: 0x0400427F RID: 17023
	private Player[] players;

	// Token: 0x04004280 RID: 17024
	private bool blockInput;
}
