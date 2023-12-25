using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020003CC RID: 972
public class AudioManagerMixer : MonoBehaviour
{
	// Token: 0x06000C96 RID: 3222 RVA: 0x00088FB6 File Offset: 0x000873B6
	private static void Init()
	{
		if (AudioManagerMixer.Manager == null)
		{
			AudioManagerMixer.Manager = Resources.Load<AudioManagerMixer>("Audio/AudioMixer");
		}
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x00088FD7 File Offset: 0x000873D7
	public static AudioMixer GetMixer()
	{
		AudioManagerMixer.Init();
		return AudioManagerMixer.Manager.mixer;
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x00088FE8 File Offset: 0x000873E8
	public static AudioManagerMixer.Groups GetGroups()
	{
		AudioManagerMixer.Init();
		return AudioManagerMixer.Manager.audioGroups;
	}

	// Token: 0x04001626 RID: 5670
	private const string PATH = "Audio/AudioMixer";

	// Token: 0x04001627 RID: 5671
	private static AudioManagerMixer Manager;

	// Token: 0x04001628 RID: 5672
	[SerializeField]
	private AudioMixer mixer;

	// Token: 0x04001629 RID: 5673
	[SerializeField]
	private AudioManagerMixer.Groups audioGroups;

	// Token: 0x020003CD RID: 973
	[Serializable]
	public class Groups
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00089001 File Offset: 0x00087401
		public AudioMixerGroup master
		{
			get
			{
				return this._master;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00089009 File Offset: 0x00087409
		public AudioMixerGroup master_Options
		{
			get
			{
				return this._master_Options;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00089011 File Offset: 0x00087411
		public AudioMixerGroup bgm_Options
		{
			get
			{
				return this._bgm_Options;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x00089019 File Offset: 0x00087419
		public AudioMixerGroup sfx_Options
		{
			get
			{
				return this._sfx_Options;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x00089021 File Offset: 0x00087421
		public AudioMixerGroup bgm
		{
			get
			{
				return this._bgm;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00089029 File Offset: 0x00087429
		public AudioMixerGroup levelBgm
		{
			get
			{
				return this._levelBgm;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00089031 File Offset: 0x00087431
		public AudioMixerGroup musicSting
		{
			get
			{
				return this._musicSting;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00089039 File Offset: 0x00087439
		public AudioMixerGroup sfx
		{
			get
			{
				return this._sfx;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00089041 File Offset: 0x00087441
		public AudioMixerGroup levelSfx
		{
			get
			{
				return this._levelSfx;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x00089049 File Offset: 0x00087449
		public AudioMixerGroup ambience
		{
			get
			{
				return this._ambience;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00089051 File Offset: 0x00087451
		public AudioMixerGroup creatures
		{
			get
			{
				return this._creatures;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00089059 File Offset: 0x00087459
		public AudioMixerGroup announcer
		{
			get
			{
				return this._announcer;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00089061 File Offset: 0x00087461
		public AudioMixerGroup super
		{
			get
			{
				return this._super;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00089069 File Offset: 0x00087469
		public AudioMixerGroup noise
		{
			get
			{
				return this._noise;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x00089071 File Offset: 0x00087471
		public AudioMixerGroup noiseConstant
		{
			get
			{
				return this._noiseConstant;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00089079 File Offset: 0x00087479
		public AudioMixerGroup noiseShortterm
		{
			get
			{
				return this._noiseShortterm;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00089081 File Offset: 0x00087481
		public AudioMixerGroup noise1920s
		{
			get
			{
				return this._noise1920s;
			}
		}

		// Token: 0x0400162A RID: 5674
		[SerializeField]
		private AudioMixerGroup _master;

		// Token: 0x0400162B RID: 5675
		[SerializeField]
		private AudioMixerGroup _master_Options;

		// Token: 0x0400162C RID: 5676
		[SerializeField]
		private AudioMixerGroup _bgm_Options;

		// Token: 0x0400162D RID: 5677
		[SerializeField]
		private AudioMixerGroup _sfx_Options;

		// Token: 0x0400162E RID: 5678
		[Space(10f)]
		[Header("BGM")]
		[SerializeField]
		private AudioMixerGroup _bgm;

		// Token: 0x0400162F RID: 5679
		[SerializeField]
		private AudioMixerGroup _levelBgm;

		// Token: 0x04001630 RID: 5680
		[SerializeField]
		private AudioMixerGroup _musicSting;

		// Token: 0x04001631 RID: 5681
		[Space(10f)]
		[Header("SFX")]
		[SerializeField]
		private AudioMixerGroup _sfx;

		// Token: 0x04001632 RID: 5682
		[SerializeField]
		private AudioMixerGroup _levelSfx;

		// Token: 0x04001633 RID: 5683
		[SerializeField]
		private AudioMixerGroup _ambience;

		// Token: 0x04001634 RID: 5684
		[SerializeField]
		private AudioMixerGroup _creatures;

		// Token: 0x04001635 RID: 5685
		[SerializeField]
		private AudioMixerGroup _announcer;

		// Token: 0x04001636 RID: 5686
		[SerializeField]
		private AudioMixerGroup _super;

		// Token: 0x04001637 RID: 5687
		[Space(10f)]
		[Header("Noise")]
		[SerializeField]
		private AudioMixerGroup _noise;

		// Token: 0x04001638 RID: 5688
		[SerializeField]
		private AudioMixerGroup _noiseConstant;

		// Token: 0x04001639 RID: 5689
		[SerializeField]
		private AudioMixerGroup _noiseShortterm;

		// Token: 0x0400163A RID: 5690
		[SerializeField]
		private AudioMixerGroup _noise1920s;
	}
}
