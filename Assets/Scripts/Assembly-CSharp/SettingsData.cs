using System;
using UnityEngine;

[Serializable]
public class SettingsData
{
	public bool hasBootedUpGame;
	public float overscan;
	public float chromaticAberration;
	public int screenWidth;
	public int screenHeight;
	public int vSyncCount;
	public bool fullScreen;
	public bool forceOriginalTitleScreen;
	public float masterVolume;
	public float sFXVolume;
	public float musicVolume;
	public bool canVibrate;
	public bool rotateControlsWithCamera;
	public int language;
	public bool chromaticAberrationEffect;
	public bool noiseEffect;
	public bool subtleBlurEffect;
	[SerializeField]
	private float brightness;
}
