using System;
using System.Collections.Generic;
using System.Diagnostics;
using GCFreeUtils;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020003B7 RID: 951
public static class AudioManager
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06000BC1 RID: 3009 RVA: 0x00084C08 File Offset: 0x00083008
	// (remove) Token: 0x06000BC2 RID: 3010 RVA: 0x00084C3C File Offset: 0x0008303C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnAttenuationHandler OnAttenuation;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000BC3 RID: 3011 RVA: 0x00084C70 File Offset: 0x00083070
	// (remove) Token: 0x06000BC4 RID: 3012 RVA: 0x00084CA4 File Offset: 0x000830A4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnTransformHandler OnFollowObject;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000BC5 RID: 3013 RVA: 0x00084CD8 File Offset: 0x000830D8
	// (remove) Token: 0x06000BC6 RID: 3014 RVA: 0x00084D0C File Offset: 0x0008310C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnPanEventHandler OnPanEvent;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000BC7 RID: 3015 RVA: 0x00084D40 File Offset: 0x00083140
	// (remove) Token: 0x06000BC8 RID: 3016 RVA: 0x00084D74 File Offset: 0x00083174
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnSnapshotHandler OnSnapshotEvent;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000BC9 RID: 3017 RVA: 0x00084DA8 File Offset: 0x000831A8
	// (remove) Token: 0x06000BCA RID: 3018 RVA: 0x00084DDC File Offset: 0x000831DC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnChangeBGMHandler OnBGMSlowdown;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000BCB RID: 3019 RVA: 0x00084E10 File Offset: 0x00083210
	// (remove) Token: 0x06000BCC RID: 3020 RVA: 0x00084E44 File Offset: 0x00083244
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnChangeBGMVolumeHandler OnBGMFadeVolume;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000BCD RID: 3021 RVA: 0x00084E78 File Offset: 0x00083278
	// (remove) Token: 0x06000BCE RID: 3022 RVA: 0x00084EAC File Offset: 0x000832AC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnStartBGMAlternateHandler OnStartBGMAlternate;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000BCF RID: 3023 RVA: 0x00084EE0 File Offset: 0x000832E0
	// (remove) Token: 0x06000BD0 RID: 3024 RVA: 0x00084F14 File Offset: 0x00083314
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnChangeSFXHandler OnSFXSlowDown;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06000BD1 RID: 3025 RVA: 0x00084F48 File Offset: 0x00083348
	// (remove) Token: 0x06000BD2 RID: 3026 RVA: 0x00084F7C File Offset: 0x0008337C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnChangeSFXStartEndHandler OnSFXFadeVolume;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000BD3 RID: 3027 RVA: 0x00084FB0 File Offset: 0x000833B0
	// (remove) Token: 0x06000BD4 RID: 3028 RVA: 0x00084FE4 File Offset: 0x000833E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnWarbleBGMPitchHandler OnBGMPitchWarble;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06000BD5 RID: 3029 RVA: 0x00085018 File Offset: 0x00083418
	// (remove) Token: 0x06000BD6 RID: 3030 RVA: 0x0008504C File Offset: 0x0008344C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnBGMPlayListManualHandler OnPlayManualBGM;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06000BD7 RID: 3031 RVA: 0x00085080 File Offset: 0x00083480
	// (remove) Token: 0x06000BD8 RID: 3032 RVA: 0x000850B4 File Offset: 0x000834B4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnSfxHandler OnPlayEvent;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06000BD9 RID: 3033 RVA: 0x000850E8 File Offset: 0x000834E8
	// (remove) Token: 0x06000BDA RID: 3034 RVA: 0x0008511C File Offset: 0x0008351C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnSfxHandler OnPlayLoopEvent;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06000BDB RID: 3035 RVA: 0x00085150 File Offset: 0x00083550
	// (remove) Token: 0x06000BDC RID: 3036 RVA: 0x00085184 File Offset: 0x00083584
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnSfxHandler OnStopEvent;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06000BDD RID: 3037 RVA: 0x000851B8 File Offset: 0x000835B8
	// (remove) Token: 0x06000BDE RID: 3038 RVA: 0x000851EC File Offset: 0x000835EC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnSfxHandler OnPauseEvent;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06000BDF RID: 3039 RVA: 0x00085220 File Offset: 0x00083620
	// (remove) Token: 0x06000BE0 RID: 3040 RVA: 0x00085254 File Offset: 0x00083654
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnSfxHandler OnUnpauseEvent;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06000BE1 RID: 3041 RVA: 0x00085288 File Offset: 0x00083688
	// (remove) Token: 0x06000BE2 RID: 3042 RVA: 0x000852BC File Offset: 0x000836BC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnStopAllEvent;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06000BE3 RID: 3043 RVA: 0x000852F0 File Offset: 0x000836F0
	// (remove) Token: 0x06000BE4 RID: 3044 RVA: 0x00085324 File Offset: 0x00083724
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnStopBGMEvent;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06000BE5 RID: 3045 RVA: 0x00085358 File Offset: 0x00083758
	// (remove) Token: 0x06000BE6 RID: 3046 RVA: 0x0008538C File Offset: 0x0008378C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnPlayBGMEvent;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06000BE7 RID: 3047 RVA: 0x000853C0 File Offset: 0x000837C0
	// (remove) Token: 0x06000BE8 RID: 3048 RVA: 0x000853F4 File Offset: 0x000837F4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnPlayBGMPlaylistEvent;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000BE9 RID: 3049 RVA: 0x00085428 File Offset: 0x00083828
	// (remove) Token: 0x06000BEA RID: 3050 RVA: 0x0008545C File Offset: 0x0008385C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnPauseAllSFXEvent;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000BEB RID: 3051 RVA: 0x00085490 File Offset: 0x00083890
	// (remove) Token: 0x06000BEC RID: 3052 RVA: 0x000854C4 File Offset: 0x000838C4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnUnpauseAllSFXEvent;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000BED RID: 3053 RVA: 0x000854F8 File Offset: 0x000838F8
	// (remove) Token: 0x06000BEE RID: 3054 RVA: 0x0008552C File Offset: 0x0008392C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnStopManualBGMTrackEvent;

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00085560 File Offset: 0x00083960
	private static AudioMixer mixer
	{
		get
		{
			if (AudioManager._mixer == null)
			{
				AudioManager._mixer = AudioManagerMixer.GetMixer();
			}
			return AudioManager._mixer;
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00085584 File Offset: 0x00083984
	// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x000855B0 File Offset: 0x000839B0
	public static float sfxOptionsVolume
	{
		get
		{
			float result;
			AudioManager.mixer.GetFloat(AudioManager.Property.Options_SFXVolume.ToString(), out result);
			return result;
		}
		set
		{
			AudioManager.mixer.SetFloat(AudioManager.Property.Options_SFXVolume.ToString(), value);
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x000855D8 File Offset: 0x000839D8
	// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x00085604 File Offset: 0x00083A04
	public static float bgmOptionsVolume
	{
		get
		{
			float result;
			AudioManager.mixer.GetFloat(AudioManager.Property.Options_BGMVolume.ToString(), out result);
			return result;
		}
		set
		{
			AudioManager.mixer.SetFloat(AudioManager.Property.Options_BGMVolume.ToString(), value);
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0008562C File Offset: 0x00083A2C
	// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00085658 File Offset: 0x00083A58
	public static float masterVolume
	{
		get
		{
			float result;
			AudioManager.mixer.GetFloat(AudioManager.Property.MasterVolume.ToString(), out result);
			return result;
		}
		set
		{
			AudioManager.mixer.SetFloat(AudioManager.Property.MasterVolume.ToString(), value);
		}
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00085680 File Offset: 0x00083A80
	public static bool CheckIfPlaying(string key)
	{
		AudioManager.checkIfPlaying = false;
		if (AudioManager.OnCheckEvent != null)
		{
			key = key.ToLowerIfNecessary();
			AudioManager.checkIfPlaying = AudioManager.OnCheckEvent.CallAnyTrue(key);
			return AudioManager.checkIfPlaying;
		}
		return false;
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x000856B1 File Offset: 0x00083AB1
	public static void PlayBGMPlaylistManually(bool goThroughPlaylistAfter)
	{
		if (AudioManager.OnPlayManualBGM != null)
		{
			AudioManager.OnPlayManualBGM(goThroughPlaylistAfter);
		}
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x000856C8 File Offset: 0x00083AC8
	public static void StopBGMPlaylistManually()
	{
		if (AudioManager.OnStopManualBGMTrackEvent != null)
		{
			AudioManager.OnStopManualBGMTrackEvent();
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x000856DE File Offset: 0x00083ADE
	public static void ChangeSFXPitch(string key, float endPitch, float time)
	{
		if (AudioManager.OnSFXSlowDown != null)
		{
			AudioManager.OnSFXSlowDown(key, endPitch, time);
		}
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x000856F7 File Offset: 0x00083AF7
	public static void ChangeBGMPitch(float endPitch, float time)
	{
		if (AudioManager.OnBGMSlowdown != null)
		{
			AudioManager.OnBGMSlowdown(endPitch, time);
		}
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0008570F File Offset: 0x00083B0F
	public static void FadeBGMVolume(float endVolume, float time, bool fadeOut)
	{
		if (AudioManager.OnBGMFadeVolume != null)
		{
			AudioManager.OnBGMFadeVolume(endVolume, time, fadeOut);
		}
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x00085728 File Offset: 0x00083B28
	public static void WarbleBGMPitch(int warbles, float[] minValue, float[] maxValue, float[] incrementTime, float[] playTime)
	{
		if (AudioManager.OnBGMPitchWarble != null)
		{
			AudioManager.OnBGMPitchWarble(warbles, minValue, maxValue, incrementTime, playTime);
		}
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x00085744 File Offset: 0x00083B44
	public static void StartBGMAlternate(int index)
	{
		if (AudioManager.OnStartBGMAlternate != null)
		{
			AudioManager.OnStartBGMAlternate(index);
		}
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0008575B File Offset: 0x00083B5B
	public static void Attenuation(string key, bool attenuation, float endVolume)
	{
		if (AudioManager.OnAttenuation != null)
		{
			AudioManager.OnAttenuation(key, attenuation, endVolume);
		}
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x00085774 File Offset: 0x00083B74
	public static void Play(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnPlayEvent != null)
		{
			AudioManager.OnPlayEvent(key);
		}
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x00085793 File Offset: 0x00083B93
	public static void Stop(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnStopEvent != null)
		{
			AudioManager.OnStopEvent(key);
		}
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x000857B2 File Offset: 0x00083BB2
	public static void PlayLoop(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnPlayLoopEvent != null)
		{
			AudioManager.OnPlayLoopEvent(key);
		}
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x000857D1 File Offset: 0x00083BD1
	public static void Pause(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnPauseEvent != null)
		{
			AudioManager.OnPauseEvent(key);
		}
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x000857F0 File Offset: 0x00083BF0
	public static void Unpaused(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnUnpauseEvent != null)
		{
			AudioManager.OnUnpauseEvent(key);
		}
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x0008580F File Offset: 0x00083C0F
	public static void Pan(string key, float value)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnPanEvent != null)
		{
			AudioManager.OnPanEvent(key, value);
		}
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x0008582F File Offset: 0x00083C2F
	public static void FadeSFXVolume(string key, float endVolume, float time)
	{
		AudioManager.FadeSFXVolume(key, -1f, endVolume, time);
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x0008583E File Offset: 0x00083C3E
	public static void FadeSFXVolume(string key, float startVolume, float endVolume, float time)
	{
		if (AudioManager.OnSFXFadeVolume != null)
		{
			AudioManager.OnSFXFadeVolume(key, startVolume, endVolume, time);
		}
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x00085858 File Offset: 0x00083C58
	public static void FadeSFXVolumeLinear(string key, float endVolume, float time)
	{
		AudioManager.FadeSFXVolumeLinear(key, -1f, endVolume, time);
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x00085867 File Offset: 0x00083C67
	public static void FadeSFXVolumeLinear(string key, float startVolume, float endVolume, float time)
	{
		if (AudioManager.OnSFXFadeVolumeLinear != null)
		{
			AudioManager.OnSFXFadeVolumeLinear(key, startVolume, endVolume, time);
		}
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00085884 File Offset: 0x00083C84
	public static void FollowObject(IEnumerable<string> keys, Transform transform)
	{
		foreach (string text in keys)
		{
			AudioManager.FollowObject(keys, transform);
		}
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x000858D8 File Offset: 0x00083CD8
	public static void FollowObject(string key, Transform transform)
	{
		key.ToLowerIfNecessary();
		if (AudioManager.OnFollowObject != null)
		{
			AudioManager.OnFollowObject(key, transform);
		}
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x000858F7 File Offset: 0x00083CF7
	[Obsolete("Use Play(string key) instead")]
	public static void Play(Sfx sfx)
	{
		AudioManager.Play(sfx.ToString());
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x0008590B File Offset: 0x00083D0B
	[Obsolete("Use Stop(string key) instead")]
	public static void Stop(Sfx sfx)
	{
		AudioManager.Stop(sfx.ToString());
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x0008591F File Offset: 0x00083D1F
	public static void StopAll()
	{
		if (AudioManager.OnStopAllEvent != null)
		{
			AudioManager.OnStopAllEvent();
		}
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00085935 File Offset: 0x00083D35
	public static void StopBGM()
	{
		if (AudioManager.OnStopBGMEvent != null)
		{
			AudioManager.OnStopBGMEvent();
		}
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x0008594B File Offset: 0x00083D4B
	public static void PlayBGM()
	{
		if (AudioManager.OnPlayBGMEvent != null)
		{
			AudioManager.OnPlayBGMEvent();
		}
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00085961 File Offset: 0x00083D61
	public static void PlaylistBGM()
	{
		if (AudioManager.OnPlayBGMPlaylistEvent != null)
		{
			AudioManager.OnPlayBGMPlaylistEvent();
		}
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00085977 File Offset: 0x00083D77
	public static void PauseAllSFX()
	{
		if (AudioManager.OnPauseAllSFXEvent != null)
		{
			AudioManager.OnPauseAllSFXEvent();
		}
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x0008598D File Offset: 0x00083D8D
	public static void UnpauseAllSFX()
	{
		if (AudioManager.OnUnpauseAllSFXEvent != null)
		{
			AudioManager.OnUnpauseAllSFXEvent();
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x000859A3 File Offset: 0x00083DA3
	public static void SnapshotTransition(string[] snapshotNames, float[] weights, float time)
	{
		if (AudioManager.OnSnapshotEvent != null)
		{
			AudioManager.OnSnapshotEvent(snapshotNames, weights, time);
		}
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000859BC File Offset: 0x00083DBC
	public static void HandleSnapshot(string snapshot, float time)
	{
		string[] array = new string[]
		{
			AudioManager.Snapshots.Cutscene.ToString(),
			AudioManager.Snapshots.FrontEnd.ToString(),
			AudioManager.Snapshots.Unpaused.ToString(),
			AudioManager.Snapshots.Unpaused_Clean.ToString(),
			AudioManager.Snapshots.Unpaused_1920s.ToString(),
			AudioManager.Snapshots.Loadscreen.ToString(),
			AudioManager.Snapshots.Paused.ToString(),
			AudioManager.Snapshots.Super.ToString(),
			AudioManager.Snapshots.SuperStart.ToString(),
			AudioManager.Snapshots.Death.ToString(),
			AudioManager.Snapshots.EquipMenu.ToString(),
			AudioManager.Snapshots.RumRunners_RedBeam.ToString(),
			AudioManager.Snapshots.RumRunners_GreenBeam.ToString(),
			AudioManager.Snapshots.RumRunners_YellowBeam.ToString()
		};
		float[] array2 = new float[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = ((!(array[i] == snapshot)) ? 0f : 1f);
		}
		AudioManager.SnapshotTransition(array, array2, time);
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00085B30 File Offset: 0x00083F30
	public static void SnapshotReset(string sceneName, float time)
	{
		string[] array = new string[]
		{
			AudioManager.Snapshots.Cutscene.ToString(),
			AudioManager.Snapshots.FrontEnd.ToString(),
			AudioManager.Snapshots.Unpaused.ToString(),
			AudioManager.Snapshots.Unpaused_Clean.ToString(),
			AudioManager.Snapshots.Unpaused_1920s.ToString(),
			AudioManager.Snapshots.Loadscreen.ToString(),
			AudioManager.Snapshots.Paused.ToString(),
			AudioManager.Snapshots.Super.ToString(),
			AudioManager.Snapshots.SuperStart.ToString(),
			AudioManager.Snapshots.Death.ToString(),
			AudioManager.Snapshots.EquipMenu.ToString(),
			AudioManager.Snapshots.RumRunners_RedBeam.ToString(),
			AudioManager.Snapshots.RumRunners_GreenBeam.ToString(),
			AudioManager.Snapshots.RumRunners_YellowBeam.ToString()
		};
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (SettingsData.Data.vintageAudioEnabled)
			{
				if (array[i] == AudioManager.Snapshots.Unpaused_1920s.ToString())
				{
					num = i;
				}
			}
			else if (sceneName == Scenes.scene_level_retro_arcade.ToString())
			{
				if (array[i] == AudioManager.Snapshots.Unpaused_Clean.ToString())
				{
					num = i;
				}
			}
			else if (array[i] == AudioManager.Snapshots.Unpaused.ToString())
			{
				num = i;
			}
		}
		float[] array2 = new float[array.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array2[j] = ((j != num) ? 0f : 1f);
		}
		AudioManager.SnapshotTransition(array, array2, time);
	}

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06000C16 RID: 3094 RVA: 0x00085D54 File Offset: 0x00084154
	// (remove) Token: 0x06000C17 RID: 3095 RVA: 0x00085D88 File Offset: 0x00084188
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event AudioManager.OnChangeSFXStartEndHandler OnSFXFadeVolumeLinear;

	// Token: 0x04001572 RID: 5490
	private const float VOLUME_MAX = 0f;

	// Token: 0x04001573 RID: 5491
	private const float VOLUME_MIN = -80f;

	// Token: 0x04001574 RID: 5492
	public static GCFreePredicateList<string> OnCheckEvent = new GCFreePredicateList<string>(10, true);

	// Token: 0x0400158D RID: 5517
	private static AudioMixer _mixer;

	// Token: 0x0400158E RID: 5518
	private static bool checkIfPlaying;

	// Token: 0x020003B8 RID: 952
	public enum Channel
	{
		// Token: 0x04001590 RID: 5520
		Default,
		// Token: 0x04001591 RID: 5521
		Level
	}

	// Token: 0x020003B9 RID: 953
	public enum Property
	{
		// Token: 0x04001593 RID: 5523
		MasterVolume,
		// Token: 0x04001594 RID: 5524
		Options_BGMVolume,
		// Token: 0x04001595 RID: 5525
		Options_SFXVolume
	}

	// Token: 0x020003BA RID: 954
	public enum Snapshots
	{
		// Token: 0x04001597 RID: 5527
		Cutscene,
		// Token: 0x04001598 RID: 5528
		FrontEnd,
		// Token: 0x04001599 RID: 5529
		Unpaused,
		// Token: 0x0400159A RID: 5530
		Unpaused_Clean,
		// Token: 0x0400159B RID: 5531
		Unpaused_1920s,
		// Token: 0x0400159C RID: 5532
		Loadscreen,
		// Token: 0x0400159D RID: 5533
		Paused,
		// Token: 0x0400159E RID: 5534
		Super,
		// Token: 0x0400159F RID: 5535
		SuperStart,
		// Token: 0x040015A0 RID: 5536
		Death,
		// Token: 0x040015A1 RID: 5537
		EquipMenu,
		// Token: 0x040015A2 RID: 5538
		RumRunners_RedBeam,
		// Token: 0x040015A3 RID: 5539
		RumRunners_GreenBeam,
		// Token: 0x040015A4 RID: 5540
		RumRunners_YellowBeam
	}

	// Token: 0x020003BB RID: 955
	// (Invoke) Token: 0x06000C1A RID: 3098
	public delegate bool OnCheckIfPlaying(string key);

	// Token: 0x020003BC RID: 956
	// (Invoke) Token: 0x06000C1E RID: 3102
	public delegate void OnSfxHandler(string key);

	// Token: 0x020003BD RID: 957
	// (Invoke) Token: 0x06000C22 RID: 3106
	public delegate void OnTransformHandler(string key, Transform transform);

	// Token: 0x020003BE RID: 958
	// (Invoke) Token: 0x06000C26 RID: 3110
	public delegate void OnAttenuationHandler(string key, bool attenuation, float endVolume);

	// Token: 0x020003BF RID: 959
	// (Invoke) Token: 0x06000C2A RID: 3114
	public delegate void OnChangeBGMHandler(float end, float time);

	// Token: 0x020003C0 RID: 960
	// (Invoke) Token: 0x06000C2E RID: 3118
	public delegate void OnChangeBGMVolumeHandler(float end, float time, bool fadeOut);

	// Token: 0x020003C1 RID: 961
	// (Invoke) Token: 0x06000C32 RID: 3122
	public delegate void OnStartBGMAlternateHandler(int index);

	// Token: 0x020003C2 RID: 962
	// (Invoke) Token: 0x06000C36 RID: 3126
	public delegate void OnChangeSFXHandler(string key, float end, float time);

	// Token: 0x020003C3 RID: 963
	// (Invoke) Token: 0x06000C3A RID: 3130
	public delegate void OnChangeSFXStartEndHandler(string key, float start, float end, float time);

	// Token: 0x020003C4 RID: 964
	// (Invoke) Token: 0x06000C3E RID: 3134
	public delegate void OnWarbleBGMPitchHandler(int warbles, float[] minValue, float[] maxValue, float[] warbleTime, float[] playTime);

	// Token: 0x020003C5 RID: 965
	// (Invoke) Token: 0x06000C42 RID: 3138
	public delegate void OnSnapshotHandler(string[] names, float[] weight, float time);

	// Token: 0x020003C6 RID: 966
	// (Invoke) Token: 0x06000C46 RID: 3142
	public delegate void OnPanEventHandler(string key, float value);

	// Token: 0x020003C7 RID: 967
	// (Invoke) Token: 0x06000C4A RID: 3146
	public delegate void OnBGMPlayListManualHandler(bool loopPlayListAfter);
}
