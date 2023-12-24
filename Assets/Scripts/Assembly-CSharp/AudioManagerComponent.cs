using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManagerComponent : AbstractMonoBehaviour
{
	[Serializable]
	public class SoundGroup
	{
		[Serializable]
		public class Source
		{
			[SerializeField]
			internal AudioSource audio;
			public float originalVolume;
			public bool wasJustPlayed;
			public bool isFadedOut;
			public bool noLoop;
		}

		[SerializeField]
		private List<AudioManagerComponent.SoundGroup.Source> sources;
		public Sfx trigger;
		public string key;
		public Transform emissionTransform;
		public bool activatedManually;
		public bool isFadedOut;
	}

	[SerializeField]
	private AudioManager.Channel channel;
	[SerializeField]
	private List<AudioManagerComponent.SoundGroup.Source> bgmSources;
	[SerializeField]
	private List<AudioManagerComponent.SoundGroup.Source> bgmAlternates;
	[SerializeField]
	private List<AudioManagerComponent.SoundGroup> sounds;
	[SerializeField]
	private List<AudioManagerComponent.SoundGroup> bgmPlaylist;
	[SerializeField]
	private bool autoplayBGM;
	[SerializeField]
	private bool autoplayBGMPlaylist;
	[SerializeField]
	private float[] minValue;
}
