using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020003C9 RID: 969
public class AudioManagerComponent : AbstractMonoBehaviour
{
	// Token: 0x06000C4E RID: 3150 RVA: 0x00085DF8 File Offset: 0x000841F8
	protected override void Awake()
	{
		base.Awake();
		this.SetChannels();
		this.dict = new Dictionary<string, AudioManagerComponent.SoundGroup>();
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.Init(true);
			this.dict[soundGroup.key.ToLowerIfNecessary()] = soundGroup;
		}
		foreach (AudioManagerComponent.SoundGroup soundGroup2 in this.bgmPlaylist)
		{
			soundGroup2.Init(true);
			this.dict[soundGroup2.key.ToLowerIfNecessary()] = soundGroup2;
		}
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmAlternates)
		{
			source.Init(true);
		}
		foreach (AudioManagerComponent.SoundGroup.Source source2 in this.bgmSources)
		{
			source2.Init(true);
		}
		this.AddEvents();
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00085F88 File Offset: 0x00084388
	private void OnDestroy()
	{
		this.RemoveEvents();
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00085F90 File Offset: 0x00084390
	private void OnValidate()
	{
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			if (string.IsNullOrEmpty(soundGroup.key))
			{
				soundGroup.key = soundGroup.trigger.ToString();
			}
			soundGroup.key = soundGroup.key.ToLower();
		}
		foreach (AudioManagerComponent.SoundGroup soundGroup2 in this.bgmPlaylist)
		{
			if (string.IsNullOrEmpty(soundGroup2.key))
			{
				soundGroup2.key = soundGroup2.trigger.ToString();
			}
			soundGroup2.key = soundGroup2.key.ToLower();
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x0008609C File Offset: 0x0008449C
	private void AddEvents()
	{
		AudioManager.OnPlayBGMEvent += this.StartBGM;
		AudioManager.OnPlayBGMPlaylistEvent += this.StartBGMPlaylist;
		AudioManager.OnSnapshotEvent += this.SnapshotTransition;
		AudioManager.OnCheckEvent.Add(new Predicate<string>(this.OnIsPlaying));
		AudioManager.OnPlayEvent += this.OnPlay;
		AudioManager.OnPlayLoopEvent += this.OnPlayLoop;
		AudioManager.OnStopEvent += this.OnStop;
		AudioManager.OnPauseEvent += this.OnPause;
		AudioManager.OnUnpauseEvent += this.OnUnpause;
		AudioManager.OnFollowObject += this.OnFollowOject;
		AudioManager.OnPanEvent += this.OnPan;
		AudioManager.OnStopAllEvent += this.OnStopAll;
		AudioManager.OnStopBGMEvent += this.OnStopBGM;
		AudioManager.OnPauseAllSFXEvent += this.OnPauseAllSFX;
		AudioManager.OnUnpauseAllSFXEvent += this.OnUnpauseAllSFX;
		AudioManager.OnBGMSlowdown += this.OnBGMSlowdown;
		AudioManager.OnSFXSlowDown += this.OnSFXSlowDown;
		AudioManager.OnSFXFadeVolume += this.OnSFXVolume;
		AudioManager.OnSFXFadeVolumeLinear += this.OnSFXVolumeLinear;
		AudioManager.OnBGMPitchWarble += this.OnBGMWarblePitch;
		AudioManager.OnAttenuation += this.OnAttenuation;
		AudioManager.OnPlayManualBGM += this.PlayManualBGMTrack;
		AudioManager.OnStopManualBGMTrackEvent += this.StopManualBGMTrack;
		AudioManager.OnBGMFadeVolume += this.OnBGMVolumeFade;
		AudioManager.OnStartBGMAlternate += this.StartBGMAlternate;
		if (this.autoplayBGM)
		{
			SceneLoader.OnLoaderCompleteEvent += this.StartBGM;
		}
		if (this.autoplayBGMPlaylist)
		{
			SceneLoader.OnLoaderCompleteEvent += this.StartBGMPlaylist;
		}
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00086290 File Offset: 0x00084690
	private void RemoveEvents()
	{
		AudioManager.OnPlayBGMEvent -= this.StartBGM;
		AudioManager.OnPlayBGMPlaylistEvent -= this.StartBGMPlaylist;
		AudioManager.OnSnapshotEvent -= this.SnapshotTransition;
		AudioManager.OnCheckEvent.Remove(new Predicate<string>(this.OnIsPlaying));
		AudioManager.OnPlayEvent -= this.OnPlay;
		AudioManager.OnPlayLoopEvent -= this.OnPlayLoop;
		AudioManager.OnStopEvent -= this.OnStop;
		AudioManager.OnPauseEvent -= this.OnPause;
		AudioManager.OnUnpauseEvent -= this.OnUnpause;
		AudioManager.OnFollowObject -= this.OnFollowOject;
		AudioManager.OnPanEvent -= this.OnPan;
		AudioManager.OnStopAllEvent -= this.OnStopAll;
		AudioManager.OnStopBGMEvent -= this.OnStopBGM;
		AudioManager.OnPauseAllSFXEvent -= this.OnPauseAllSFX;
		AudioManager.OnUnpauseAllSFXEvent -= this.OnUnpauseAllSFX;
		AudioManager.OnBGMSlowdown -= this.OnBGMSlowdown;
		AudioManager.OnSFXSlowDown -= this.OnSFXSlowDown;
		AudioManager.OnSFXFadeVolume -= this.OnSFXVolume;
		AudioManager.OnSFXFadeVolumeLinear -= this.OnSFXVolumeLinear;
		AudioManager.OnBGMPitchWarble -= this.OnBGMWarblePitch;
		AudioManager.OnAttenuation -= this.OnAttenuation;
		AudioManager.OnPlayManualBGM -= this.PlayManualBGMTrack;
		AudioManager.OnStopManualBGMTrackEvent -= this.StopManualBGMTrack;
		AudioManager.OnBGMFadeVolume -= this.OnBGMVolumeFade;
		AudioManager.OnStartBGMAlternate -= this.StartBGMAlternate;
		if (this.autoplayBGM)
		{
			SceneLoader.OnLoaderCompleteEvent -= this.StartBGM;
		}
		if (this.autoplayBGMPlaylist)
		{
			SceneLoader.OnLoaderCompleteEvent -= this.StartBGMPlaylist;
		}
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00086484 File Offset: 0x00084884
	private void Update()
	{
		for (int i = 0; i < this.sounds.Count; i++)
		{
			AudioManagerComponent.SoundGroup soundGroup = this.sounds[i];
			if (soundGroup.emissionTransform != null)
			{
				soundGroup.FollowObject(soundGroup.emissionTransform.position);
			}
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x000864DC File Offset: 0x000848DC
	private void StartBGM()
	{
		this.StopBGM();
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			if (source.noLoop)
			{
				source.Play();
			}
			else
			{
				source.PlayLooped();
			}
		}
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00086554 File Offset: 0x00084954
	private void StartBGMAlternate(int index)
	{
		this.StopBGM();
		if (this.bgmAlternates.Count > index && this.bgmAlternates[index] != null)
		{
			if (this.bgmAlternates[index].noLoop)
			{
				this.bgmAlternates[index].Play();
			}
			else
			{
				this.bgmAlternates[index].PlayLooped();
			}
		}
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x000865C8 File Offset: 0x000849C8
	private void StopBGM()
	{
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			source.Stop();
		}
		foreach (AudioManagerComponent.SoundGroup.Source source2 in this.bgmAlternates)
		{
			source2.Stop();
		}
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00086670 File Offset: 0x00084A70
	private void OnLevelStart()
	{
		this.StartBGM();
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00086678 File Offset: 0x00084A78
	private void OnStopBGM()
	{
		this.StopBGM();
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00086680 File Offset: 0x00084A80
	private void OnBGMSlowdown(float end, float time)
	{
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			base.StartCoroutine(source.change_pitch_cr(end, time));
		}
		foreach (AudioManagerComponent.SoundGroup.Source source2 in this.bgmAlternates)
		{
			base.StartCoroutine(source2.change_pitch_cr(end, time));
		}
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (this.bgmPlaylist[i].CheckIfPlaying())
			{
				base.StartCoroutine(this.bgmPlaylist[i].change_pitch_sfx(end, time));
			}
		}
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x0008678C File Offset: 0x00084B8C
	private void OnBGMVolumeFade(float end, float time, bool onFadeout)
	{
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			if ((source.isPlaying() && onFadeout) || (!onFadeout && source.isFadedOut))
			{
				base.StartCoroutine(source.change_volume_cr(end, time, onFadeout));
			}
		}
		foreach (AudioManagerComponent.SoundGroup.Source source2 in this.bgmAlternates)
		{
			if ((source2.isPlaying() && onFadeout) || (!onFadeout && source2.isFadedOut))
			{
				base.StartCoroutine(source2.change_volume_cr(end, time, onFadeout));
			}
		}
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if ((this.bgmPlaylist[i].CheckIfPlaying() && onFadeout) || (!onFadeout && this.bgmPlaylist[i].isFadedOut))
			{
				base.StartCoroutine(this.bgmPlaylist[i].change_volume_cr(end, time, onFadeout));
			}
		}
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00086900 File Offset: 0x00084D00
	private void OnBGMWarblePitch(int warbles, float[] minValue, float[] maxValue, float[] warbleTime, float[] playTime)
	{
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			base.StartCoroutine(source.warble_pitch_cr(warbles, minValue, maxValue, warbleTime, playTime));
		}
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (this.bgmPlaylist[i].CheckIfPlaying())
			{
				base.StartCoroutine(this.bgmPlaylist[i].warble_pitch_cr(warbles, minValue, maxValue, warbleTime, playTime));
			}
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x000869BC File Offset: 0x00084DBC
	public void PlayManualBGMTrack(bool loopPlayListAfter)
	{
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (this.bgmPlaylist[i].activatedManually)
			{
				if (loopPlayListAfter)
				{
					this.bgmPlaylist[i].Play();
					base.StartCoroutine(this.handle_cr(this.bgmPlaylist[i].ClipLength()));
				}
				else
				{
					this.bgmPlaylist[i].PlayLoop();
				}
			}
		}
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x00086A48 File Offset: 0x00084E48
	private IEnumerator handle_cr(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);
		this.StartBGMPlaylist();
		yield return null;
		yield break;
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00086A6C File Offset: 0x00084E6C
	public void StopManualBGMTrack()
	{
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (this.bgmPlaylist[i].activatedManually)
			{
				this.bgmPlaylist[i].Stop();
			}
		}
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00086ABC File Offset: 0x00084EBC
	private void StartBGMPlaylist()
	{
		bool flag = true;
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (!this.bgmPlaylist[i].activatedManually)
			{
				flag = false;
			}
		}
		if (flag)
		{
			return;
		}
		this.StopBGM();
		if (this.bgmPlaylist.Count > 0)
		{
			PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(SceneLoader.CurrentLevel);
			levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) % this.bgmPlaylist.Count;
			base.StartCoroutine(this.play_track_cr());
		}
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x00086B54 File Offset: 0x00084F54
	private IEnumerator play_track_cr()
	{
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(SceneLoader.CurrentLevel);
		for (;;)
		{
			while (this.bgmPlaylist[levelData.bgmPlayListCurrent].activatedManually)
			{
				levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) % this.bgmPlaylist.Count;
				yield return new WaitForFixedUpdate();
			}
			this.bgmPlaylist[levelData.bgmPlayListCurrent].Play();
			yield return new WaitForSeconds(this.bgmPlaylist[levelData.bgmPlayListCurrent].ClipLength());
			levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) % this.bgmPlaylist.Count;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x00086B6F File Offset: 0x00084F6F
	private void OnPlay(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			if (AudioManagerComponent.ShowAudioVariations || AudioManagerComponent.ShowAudioPlaying)
			{
			}
			this.dict[key].Play();
		}
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x00086BA7 File Offset: 0x00084FA7
	private void OnPlayLoop(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			if (AudioManagerComponent.ShowAudioVariations || AudioManagerComponent.ShowAudioPlaying)
			{
			}
			this.dict[key].PlayLoop();
		}
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x00086BDF File Offset: 0x00084FDF
	private void OnStop(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].Stop();
		}
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x00086C03 File Offset: 0x00085003
	private void OnPause(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].Pause();
		}
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x00086C27 File Offset: 0x00085027
	private void OnUnpause(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].Unpause();
		}
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x00086C4C File Offset: 0x0008504C
	private void OnPauseAllSFX()
	{
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.Pause();
		}
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00086CA8 File Offset: 0x000850A8
	private void OnUnpauseAllSFX()
	{
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.Unpause();
		}
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00086D04 File Offset: 0x00085104
	private void OnFollowOject(string key, Transform transform)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].emissionTransform = transform;
			this.dict[key].FollowObject(this.dict[key].emissionTransform.position);
		}
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00086D5B File Offset: 0x0008515B
	private bool OnIsPlaying(string key)
	{
		return this.dict.ContainsKey(key) && this.dict[key].CheckIfPlaying();
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x00086D81 File Offset: 0x00085181
	private void OnAttenuation(string key, bool attenuating, float endVolume)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].OnAttenuate(attenuating, endVolume);
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00086DA7 File Offset: 0x000851A7
	private void OnPan(string key, float value)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].Pan(value);
		}
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00086DCC File Offset: 0x000851CC
	private void OnStopAll()
	{
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.Stop();
		}
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00086E28 File Offset: 0x00085228
	private void OnSFXSlowDown(string key, float end, float time)
	{
		if (this.dict.ContainsKey(key))
		{
			base.StartCoroutine(this.dict[key].change_pitch_sfx(end, time));
		}
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00086E55 File Offset: 0x00085255
	private void OnSFXVolume(string key, float start, float end, float time)
	{
		if (this.dict.ContainsKey(key))
		{
			base.StartCoroutine(this.dict[key].change_volume_sfx(start, end, time, false));
		}
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00086E85 File Offset: 0x00085285
	private void OnSFXVolumeLinear(string key, float start, float end, float time)
	{
		if (this.dict.ContainsKey(key))
		{
			base.StartCoroutine(this.dict[key].change_volume_sfx(start, end, time, true));
		}
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00086EB8 File Offset: 0x000852B8
	private void SnapshotTransition(string[] snapshotNames, float[] weights, float time)
	{
		AudioManagerMixer.Groups groups = AudioManagerMixer.GetGroups();
		List<AudioMixerGroup> list = new List<AudioMixerGroup>();
		List<AudioMixerSnapshot> list2 = new List<AudioMixerSnapshot>();
		list.Add(groups.master);
		list.Add(groups.bgm_Options);
		list.Add(groups.sfx_Options);
		list.Add(groups.master_Options);
		list.Add(groups.sfx);
		list.Add(groups.levelSfx);
		list.Add(groups.ambience);
		list.Add(groups.creatures);
		list.Add(groups.announcer);
		list.Add(groups.super);
		list.Add(groups.bgm);
		list.Add(groups.levelBgm);
		list.Add(groups.musicSting);
		list.Add(groups.noise);
		list.Add(groups.noiseConstant);
		list.Add(groups.noiseShortterm);
		list.Add(groups.noise1920s);
		for (int i = 0; i < weights.Length; i++)
		{
			if (list[i].audioMixer.FindSnapshot(snapshotNames[i]) != null)
			{
				list2.Add(list[0].audioMixer.FindSnapshot(snapshotNames[i]));
			}
			else
			{
				global::Debug.LogError("Snapshot string is invalid", null);
			}
		}
		foreach (AudioMixerGroup audioMixerGroup in list)
		{
			audioMixerGroup.audioMixer.TransitionToSnapshots(list2.ToArray(), weights, time);
		}
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0008705C File Offset: 0x0008545C
	private void SetChannels()
	{
		AudioManagerMixer.Groups groups = AudioManagerMixer.GetGroups();
		AudioManager.Channel channel = this.channel;
		AudioMixerGroup audioMixerGroup;
		AudioMixerGroup mixerGroup;
		if (channel == AudioManager.Channel.Default || channel != AudioManager.Channel.Level)
		{
			audioMixerGroup = groups.bgm;
			mixerGroup = groups.sfx;
			AudioMixerGroup noiseConstant = groups.noiseConstant;
			AudioMixerGroup noiseShortterm = groups.noiseShortterm;
		}
		else
		{
			audioMixerGroup = groups.levelBgm;
			mixerGroup = groups.levelSfx;
		}
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			if (source.audio.outputAudioMixerGroup == null)
			{
				source.audio.outputAudioMixerGroup = audioMixerGroup;
			}
		}
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.SetMixerGroup(mixerGroup);
		}
		foreach (AudioManagerComponent.SoundGroup soundGroup2 in this.bgmPlaylist)
		{
			soundGroup2.SetMixerGroup(audioMixerGroup);
		}
	}

	// Token: 0x0400160E RID: 5646
	[SerializeField]
	private AudioManager.Channel channel;

	// Token: 0x0400160F RID: 5647
	[SerializeField]
	private List<AudioManagerComponent.SoundGroup.Source> bgmSources;

	// Token: 0x04001610 RID: 5648
	[SerializeField]
	private List<AudioManagerComponent.SoundGroup.Source> bgmAlternates;

	// Token: 0x04001611 RID: 5649
	[SerializeField]
	private List<AudioManagerComponent.SoundGroup> sounds = new List<AudioManagerComponent.SoundGroup>();

	// Token: 0x04001612 RID: 5650
	[SerializeField]
	private List<AudioManagerComponent.SoundGroup> bgmPlaylist = new List<AudioManagerComponent.SoundGroup>();

	// Token: 0x04001613 RID: 5651
	[SerializeField]
	private bool autoplayBGM = true;

	// Token: 0x04001614 RID: 5652
	[SerializeField]
	private bool autoplayBGMPlaylist = true;

	// Token: 0x04001615 RID: 5653
	[SerializeField]
	private float[] minValue;

	// Token: 0x04001616 RID: 5654
	private Dictionary<string, AudioManagerComponent.SoundGroup> dict;

	// Token: 0x04001617 RID: 5655
	public static bool ShowAudioPlaying;

	// Token: 0x04001618 RID: 5656
	public static bool ShowAudioVariations;

	// Token: 0x020003CA RID: 970
	[Serializable]
	public class SoundGroup
	{
		// Token: 0x06000C73 RID: 3187 RVA: 0x000871F8 File Offset: 0x000855F8
		internal void Init(bool initializeDeferrals = false)
		{
			this.key = this.key.ToLowerIfNecessary();
			for (int i = 0; i < this.sources.Count; i++)
			{
				if (this.sources[i].audio == null)
				{
					this.sources.RemoveAt(i);
					i--;
				}
			}
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.Init(initializeDeferrals);
			}
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000872B0 File Offset: 0x000856B0
		internal void SetMixerGroup(AudioMixerGroup group)
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				if (source.audio != null && source.audio.outputAudioMixerGroup == null)
				{
					source.audio.outputAudioMixerGroup = group;
				}
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00087338 File Offset: 0x00085738
		public void SetVolume(float v)
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.SetVolume(v);
			}
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00087394 File Offset: 0x00085794
		public void Play()
		{
			AudioManagerComponent.SoundGroup.Source source = this.GetSource();
			if (this.sources.Count > 1)
			{
				foreach (AudioManagerComponent.SoundGroup.Source source2 in this.sources)
				{
					if (!source.wasJustPlayed)
					{
						break;
					}
					source = this.GetSource();
				}
			}
			source.wasJustPlayed = true;
			source.Play();
			foreach (AudioManagerComponent.SoundGroup.Source source3 in this.sources)
			{
				if (source3 != source)
				{
					source3.wasJustPlayed = false;
				}
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00087480 File Offset: 0x00085880
		public void PlayLoop()
		{
			this.GetSource().PlayLooped();
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00087490 File Offset: 0x00085890
		public void Pan(float pan)
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.Pan(pan);
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x000874EC File Offset: 0x000858EC
		public void FollowObject(Vector3 position)
		{
			for (int i = 0; i < this.sources.Count; i++)
			{
				AudioManagerComponent.SoundGroup.Source source = this.sources[i];
				source.FollowObject(position);
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0008752C File Offset: 0x0008592C
		public bool CheckIfPlaying()
		{
			this.isPlaying = false;
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.isPlaying();
				if (source.isPlaying())
				{
					this.isPlaying = true;
				}
			}
			return this.isPlaying;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x000875A8 File Offset: 0x000859A8
		public float ClipLength()
		{
			float result = 0f;
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				result = source.ClipLength();
			}
			return result;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0008760C File Offset: 0x00085A0C
		public void OnAttenuate(bool attentuating, float endVolume)
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.OnAttenuate(attentuating, endVolume);
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0008766C File Offset: 0x00085A6C
		public IEnumerator warble_pitch_cr(int warbles, float[] minValue, float[] maxValue, float[] incrementAmount, float[] playTime)
		{
			bool isDecreasing = Rand.Bool();
			float t = 0f;
			float startPitch = 1f;
			foreach (AudioManagerComponent.SoundGroup.Source s in this.sources)
			{
				if (s != null && s.audio.clip != null)
				{
					for (int i = 0; i < warbles; i++)
					{
						while (t < playTime[i])
						{
							t += CupheadTime.Delta;
							if (isDecreasing)
							{
								if (s.audio.pitch > minValue[i])
								{
									s.audio.pitch -= incrementAmount[i];
								}
								else
								{
									isDecreasing = false;
								}
							}
							else if (s.audio.pitch < maxValue[i])
							{
								s.audio.pitch += incrementAmount[i];
							}
							else
							{
								isDecreasing = true;
							}
							yield return null;
						}
						t = 0f;
						yield return null;
					}
					s.audio.pitch = startPitch;
				}
			}
			yield break;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x000876AC File Offset: 0x00085AAC
		public IEnumerator change_pitch_sfx(float end, float time)
		{
			foreach (AudioManagerComponent.SoundGroup.Source s in this.sources)
			{
				float t = 0f;
				if (s != null && s.audio.clip != null)
				{
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						s.audio.pitch = Mathf.Lerp(s.audio.pitch, end, val);
						t += Time.deltaTime;
						yield return null;
					}
					s.audio.pitch = end;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x000876D8 File Offset: 0x00085AD8
		public IEnumerator change_volume_sfx(float start, float end, float time, bool linear)
		{
			foreach (AudioManagerComponent.SoundGroup.Source s in this.sources)
			{
				float t = 0f;
				if (s != null && s.audio.clip != null)
				{
					float initialVolume = (start < 0f) ? s.audio.volume : start;
					if (!linear && start >= 0f)
					{
						s.audio.volume = initialVolume;
					}
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						if (linear)
						{
							s.audio.volume = Mathf.Lerp(initialVolume, end, val);
						}
						else
						{
							s.audio.volume = Mathf.Lerp(s.audio.volume, end, val);
						}
						t += Time.deltaTime;
						yield return null;
					}
					s.audio.volume = end;
					if (end == 0f)
					{
						s.audio.Stop();
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00087710 File Offset: 0x00085B10
		public IEnumerator change_volume_cr(float endVolume, float time, bool onFadeOut)
		{
			foreach (AudioManagerComponent.SoundGroup.Source s in this.sources)
			{
				float t = 0f;
				float startVol = (!onFadeOut) ? 0f : s.audio.volume;
				float endVol = (!onFadeOut) ? s.audio.volume : endVolume;
				if (!onFadeOut)
				{
					s.audio.Play();
					this.isFadedOut = false;
				}
				else
				{
					this.isFadedOut = true;
				}
				if (s.audio != null && s.audio.clip != null)
				{
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						s.audio.volume = Mathf.Lerp(startVol, endVol, val);
						t += Time.deltaTime;
						yield return null;
					}
					if (onFadeOut)
					{
						s.audio.Stop();
						s.audio.volume = s.originalVolume;
						this.isFadedOut = true;
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00087740 File Offset: 0x00085B40
		public void Stop()
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.Stop();
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0008779C File Offset: 0x00085B9C
		public void Pause()
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.Pause();
			}
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000877F8 File Offset: 0x00085BF8
		public void Unpause()
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.UnPause();
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00087854 File Offset: 0x00085C54
		private AudioManagerComponent.SoundGroup.Source GetSource()
		{
			return this.sources[UnityEngine.Random.Range(0, this.sources.Count)];
		}

		// Token: 0x04001619 RID: 5657
		[SerializeField]
		private List<AudioManagerComponent.SoundGroup.Source> sources = new List<AudioManagerComponent.SoundGroup.Source>
		{
			new AudioManagerComponent.SoundGroup.Source()
		};

		// Token: 0x0400161A RID: 5658
		public Sfx trigger;

		// Token: 0x0400161B RID: 5659
		public string key;

		// Token: 0x0400161C RID: 5660
		private bool isPlaying;

		// Token: 0x0400161D RID: 5661
		public Transform emissionTransform;

		// Token: 0x0400161E RID: 5662
		public bool activatedManually;

		// Token: 0x0400161F RID: 5663
		public bool isFadedOut;

		// Token: 0x04001620 RID: 5664
		private float volume;

		// Token: 0x020003CB RID: 971
		[Serializable]
		public class Source
		{
			// Token: 0x06000C86 RID: 3206 RVA: 0x0008787C File Offset: 0x00085C7C
			internal void Init(bool initializeDeferrals)
			{
				if (initializeDeferrals)
				{
					DeferredAudioSource component = this.audio.GetComponent<DeferredAudioSource>();
					if (component != null)
					{
						component.Initialize();
					}
				}
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.ignoreListenerPause = true;
					this.originalVolume = this.audio.volume;
				}
			}

			// Token: 0x06000C87 RID: 3207 RVA: 0x000878F1 File Offset: 0x00085CF1
			public void SetVolume(float v)
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.volume = v * this.originalVolume;
				}
			}

			// Token: 0x06000C88 RID: 3208 RVA: 0x00087930 File Offset: 0x00085D30
			public void Play()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.PlayOneShot(this.audio.clip);
					if (!AudioManagerComponent.ShowAudioPlaying || AudioManagerComponent.ShowAudioVariations)
					{
					}
				}
			}

			// Token: 0x06000C89 RID: 3209 RVA: 0x00087990 File Offset: 0x00085D90
			public void PlayLooped()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.loop = true;
					this.audio.Play();
					if (!AudioManagerComponent.ShowAudioPlaying || AudioManagerComponent.ShowAudioVariations)
					{
					}
				}
			}

			// Token: 0x06000C8A RID: 3210 RVA: 0x000879F0 File Offset: 0x00085DF0
			public IEnumerator change_pitch_cr(float end, float time)
			{
				float t = 0f;
				if (this.audio != null && this.audio.clip != null)
				{
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						this.audio.pitch = Mathf.Lerp(this.audio.pitch, end, val);
						t += Time.deltaTime;
						yield return null;
					}
					this.audio.pitch = end;
				}
				yield break;
			}

			// Token: 0x06000C8B RID: 3211 RVA: 0x00087A1C File Offset: 0x00085E1C
			public IEnumerator change_volume_cr(float endVolume, float time, bool onFadeOut)
			{
				float t = 0f;
				float startVol = (!onFadeOut) ? 0f : this.audio.volume;
				float endVol = (!onFadeOut) ? this.audio.volume : endVolume;
				if (!onFadeOut)
				{
					this.audio.Play();
					this.isFadedOut = false;
				}
				else
				{
					this.isFadedOut = true;
				}
				if (this.audio != null && this.audio.clip != null)
				{
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						this.audio.volume = Mathf.Lerp(startVol, endVol, val);
						t += Time.deltaTime;
						yield return null;
					}
					if (onFadeOut)
					{
						this.audio.Stop();
						this.audio.volume = this.originalVolume;
						this.isFadedOut = true;
					}
				}
				yield return null;
				yield break;
			}

			// Token: 0x06000C8C RID: 3212 RVA: 0x00087A4C File Offset: 0x00085E4C
			public IEnumerator warble_pitch_cr(int warbles, float[] minValue, float[] maxValue, float[] incrementAmount, float[] playTime)
			{
				bool isDecreasing = Rand.Bool();
				float t = 0f;
				float startPitch = 1f;
				if (this.audio != null && this.audio.clip != null)
				{
					for (int i = 0; i < warbles; i++)
					{
						while (t < playTime[i])
						{
							t += CupheadTime.Delta;
							if (isDecreasing)
							{
								if (this.audio.pitch > minValue[i])
								{
									this.audio.pitch -= incrementAmount[i];
								}
								else
								{
									isDecreasing = false;
								}
							}
							else if (this.audio.pitch < maxValue[i])
							{
								this.audio.pitch += incrementAmount[i];
							}
							else
							{
								isDecreasing = true;
							}
							yield return null;
						}
						t = 0f;
						yield return null;
					}
					this.audio.pitch = startPitch;
				}
				yield break;
			}

			// Token: 0x06000C8D RID: 3213 RVA: 0x00087A8C File Offset: 0x00085E8C
			public void Stop()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.loop = false;
					this.audio.Stop();
				}
			}

			// Token: 0x06000C8E RID: 3214 RVA: 0x00087ACC File Offset: 0x00085ECC
			public void Pause()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.Pause();
				}
			}

			// Token: 0x06000C8F RID: 3215 RVA: 0x00087B00 File Offset: 0x00085F00
			public void UnPause()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.UnPause();
				}
			}

			// Token: 0x06000C90 RID: 3216 RVA: 0x00087B34 File Offset: 0x00085F34
			public void Pan(float pan)
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.panStereo = pan;
				}
			}

			// Token: 0x06000C91 RID: 3217 RVA: 0x00087B6C File Offset: 0x00085F6C
			public float ClipLength()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					return this.audio.clip.length;
				}
				global::Debug.LogError("Clip is null", null);
				return 0f;
			}

			// Token: 0x06000C92 RID: 3218 RVA: 0x00087BC1 File Offset: 0x00085FC1
			public void FollowObject(Vector3 position)
			{
				if (this.audio != null)
				{
					this.audio.transform.position = position;
				}
			}

			// Token: 0x06000C93 RID: 3219 RVA: 0x00087BE5 File Offset: 0x00085FE5
			public bool isPlaying()
			{
				return this.audio != null && this.audio.clip != null && this.audio.isPlaying;
			}

			// Token: 0x06000C94 RID: 3220 RVA: 0x00087C1C File Offset: 0x0008601C
			public void OnAttenuate(bool attenuating, float volumeChange)
			{
				if (this.audio != null && this.audio.clip != null)
				{
					if (attenuating)
					{
						this.audio.volume = volumeChange;
					}
					else
					{
						this.audio.volume = this.originalVolume;
					}
				}
			}

			// Token: 0x04001621 RID: 5665
			[SerializeField]
			internal AudioSource audio;

			// Token: 0x04001622 RID: 5666
			public float originalVolume;

			// Token: 0x04001623 RID: 5667
			public bool wasJustPlayed;

			// Token: 0x04001624 RID: 5668
			public bool isFadedOut;

			// Token: 0x04001625 RID: 5669
			public bool noLoop;
		}
	}
}
