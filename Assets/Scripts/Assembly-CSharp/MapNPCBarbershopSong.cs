using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000949 RID: 2377
public class MapNPCBarbershopSong : MonoBehaviour
{
	// Token: 0x06003784 RID: 14212 RVA: 0x001FE4C2 File Offset: 0x001FC8C2
	private void Start()
	{
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.AddDialoguerEvents();
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x001FE4D8 File Offset: 0x001FC8D8
	private void Update()
	{
		if (this.songCoroutine != null && this.delay && (this.input.GetAnyButtonDown() || this.songEndedOrPlayerStop))
		{
			base.StopCoroutine(this.songCoroutine);
			this.songCoroutine = null;
			this.delay = false;
			this.songEndedOrPlayerStop = true;
			AudioManager.Stop("mus_barbershop");
			AudioManager.FadeBGMVolume(1f, 0.5f, false);
			AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 1f, 0.5f);
			for (int i = 0; i < this.barbershopAnimators.Length; i++)
			{
				this.barbershopAnimators[i].SetTrigger("endsong");
			}
			this.songEndedOrPlayerStop = false;
			if (Map.Current != null)
			{
				Map.Current.CurrentState = Map.State.Ready;
			}
			for (int j = 0; j < Map.Current.players.Length; j++)
			{
				if (!(Map.Current.players[j] == null))
				{
					Map.Current.players[j].Enable();
				}
			}
		}
	}

	// Token: 0x06003786 RID: 14214 RVA: 0x001FE5FC File Offset: 0x001FC9FC
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x06003787 RID: 14215 RVA: 0x001FE604 File Offset: 0x001FCA04
	private IEnumerator sing_cr()
	{
		AudioManager.FadeBGMVolume(0f, 0.5f, true);
		AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 0.01f, 0.5f);
		AudioManager.Play("mus_barbershop");
		yield return null;
		for (int i = 0; i < Map.Current.players.Length; i++)
		{
			if (!(Map.Current.players[i] == null))
			{
				Map.Current.players[i].Disable();
			}
		}
		if (Map.Current != null)
		{
			Map.Current.CurrentState = Map.State.Event;
		}
		for (int j = 0; j < this.barbershopAnimators.Length; j++)
		{
			this.barbershopAnimators[j].SetTrigger("sing");
		}
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.delay = true;
		yield return this.barbershopAnimators[3].WaitForAnimationToStart(this, "anim_map_barbershop_sing_hold", false);
		this.barbershopAnimators[0].SetTrigger("trans");
		yield return new WaitForSeconds(0.083333336f);
		this.barbershopAnimators[1].SetTrigger("trans");
		yield return new WaitForSeconds(0.083333336f);
		this.barbershopAnimators[2].SetTrigger("trans");
		yield return new WaitForSeconds(0.083333336f);
		this.barbershopAnimators[3].SetTrigger("trans");
		yield return this.barbershopAnimators[3].WaitForAnimationToStart(this, "anim_map_barbershop_sing_idle_boil", true);
		this.barbershopAnimators[0].SetTrigger("blink");
		yield return new WaitForSeconds(0.083333336f);
		this.barbershopAnimators[1].SetTrigger("blink");
		yield return new WaitForSeconds(0.083333336f);
		this.barbershopAnimators[2].SetTrigger("blink");
		yield return new WaitForSeconds(0.083333336f);
		this.barbershopAnimators[3].SetTrigger("blink");
		yield return this.barbershopAnimators[3].WaitForAnimationToStart(this, "anim_map_barbershop_sing_main_loop", true);
		while (!this.songEndedOrPlayerStop)
		{
			yield return null;
			if (!AudioManager.CheckIfPlaying("mus_barbershop"))
			{
				this.songEndedOrPlayerStop = true;
			}
		}
		for (int k = 0; k < this.barbershopAnimators.Length; k++)
		{
			this.barbershopAnimators[k].SetTrigger("endsong");
		}
		yield return null;
		this.songCoroutine = null;
		this.songEndedOrPlayerStop = false;
		if (Map.Current != null)
		{
			Map.Current.CurrentState = Map.State.Ready;
		}
		for (int l = 0; l < Map.Current.players.Length; l++)
		{
			if (!(Map.Current.players[l] == null))
			{
				Map.Current.players[l].Enable();
			}
		}
		AudioManager.FadeBGMVolume(1f, 0.5f, false);
		AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 1f, 0.5f);
		yield break;
	}

	// Token: 0x06003788 RID: 14216 RVA: 0x001FE61F File Offset: 0x001FCA1F
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003789 RID: 14217 RVA: 0x001FE637 File Offset: 0x001FCA37
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x001FE650 File Offset: 0x001FCA50
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "QuartetSing")
		{
			if (this.songCoroutine != null)
			{
				base.StopCoroutine(this.songCoroutine);
			}
			this.songCoroutine = base.StartCoroutine(this.sing_cr());
		}
	}

	// Token: 0x04003F96 RID: 16278
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04003F97 RID: 16279
	[SerializeField]
	private Animator[] barbershopAnimators = new Animator[4];

	// Token: 0x04003F98 RID: 16280
	private Coroutine songCoroutine;

	// Token: 0x04003F99 RID: 16281
	public bool songEndedOrPlayerStop;

	// Token: 0x04003F9A RID: 16282
	private bool delay;

	// Token: 0x04003F9B RID: 16283
	[HideInInspector]
	public bool SkipDialogueEvent;
}
