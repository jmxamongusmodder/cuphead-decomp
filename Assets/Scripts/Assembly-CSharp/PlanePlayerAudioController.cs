using System;
using System.Collections;

// Token: 0x02000A94 RID: 2708
public class PlanePlayerAudioController : AbstractPlanePlayerComponent
{
	// Token: 0x060040E6 RID: 16614 RVA: 0x00234FB5 File Offset: 0x002333B5
	protected override void OnAwake()
	{
		base.OnAwake();
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x00234FBD File Offset: 0x002333BD
	private void Start()
	{
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		AudioManager.PlayLoop("player_plane_engine");
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x00234FE5 File Offset: 0x002333E5
	public void LevelInit()
	{
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x00234FE7 File Offset: 0x002333E7
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (info.damage > 0f)
		{
			AudioManager.Play("player_plane_hit");
			if (base.player.stats.Health > 0)
			{
				base.StartCoroutine(this.change_pitch_cr());
			}
		}
	}

	// Token: 0x060040EA RID: 16618 RVA: 0x00235028 File Offset: 0x00233428
	private IEnumerator change_pitch_cr()
	{
		if (base.player.stats.Health == 1)
		{
			AudioManager.Play("player_damage_crack_level4");
			this.emitAudioFromObject.Add("player_damage_crack_level4");
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.3f, 0.4f);
		}
		else if (base.player.stats.Health == 2)
		{
			AudioManager.Play("player_damage_crack_level3");
			this.emitAudioFromObject.Add("player_damage_crack_level3");
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.35f, 0.4f);
		}
		else if (base.player.stats.Health == 3)
		{
			AudioManager.Play("player_damage_crack_level2");
			this.emitAudioFromObject.Add("player_damage_crack_level2");
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.4f, 0.4f);
		}
		else
		{
			AudioManager.Play("player_damage_crack_level1");
			this.emitAudioFromObject.Add("player_damage_crack_level1");
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.5f, 0.4f);
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.ChangeSFXPitch("player_plane_engine", 1f, 1f);
		yield return null;
		yield break;
	}
}
