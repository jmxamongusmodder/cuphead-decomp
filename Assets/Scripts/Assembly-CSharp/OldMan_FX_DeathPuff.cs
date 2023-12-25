using System;

// Token: 0x020006FB RID: 1787
public class OldMan_FX_DeathPuff : AbstractPausableComponent
{
	// Token: 0x06002647 RID: 9799 RVA: 0x00165A9C File Offset: 0x00163E9C
	private void AnimationEvent_SFX_OMM_GnomeDeathPuff()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_popper_deathpoof");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_popper_deathpoof");
	}
}
