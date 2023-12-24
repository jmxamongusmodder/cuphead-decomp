using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManagerMixer : MonoBehaviour
{
	[Serializable]
	public class Groups
	{
		[SerializeField]
		private AudioMixerGroup _master;
		[SerializeField]
		private AudioMixerGroup _master_Options;
		[SerializeField]
		private AudioMixerGroup _bgm_Options;
		[SerializeField]
		private AudioMixerGroup _sfx_Options;
		[SerializeField]
		private AudioMixerGroup _bgm;
		[SerializeField]
		private AudioMixerGroup _levelBgm;
		[SerializeField]
		private AudioMixerGroup _musicSting;
		[SerializeField]
		private AudioMixerGroup _sfx;
		[SerializeField]
		private AudioMixerGroup _levelSfx;
		[SerializeField]
		private AudioMixerGroup _ambience;
		[SerializeField]
		private AudioMixerGroup _creatures;
		[SerializeField]
		private AudioMixerGroup _announcer;
		[SerializeField]
		private AudioMixerGroup _super;
		[SerializeField]
		private AudioMixerGroup _noise;
		[SerializeField]
		private AudioMixerGroup _noiseConstant;
		[SerializeField]
		private AudioMixerGroup _noiseShortterm;
		[SerializeField]
		private AudioMixerGroup _noise1920s;
	}

	[SerializeField]
	private AudioMixer mixer;
	[SerializeField]
	private Groups audioGroups;
}
