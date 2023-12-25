using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D3 RID: 2003
public class SaltbakerLevelPillarHandler : LevelProperties.Saltbaker.Entity
{
	// Token: 0x06002D78 RID: 11640 RVA: 0x001ACC9E File Offset: 0x001AB09E
	public void TakeDamage(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x001ACCB4 File Offset: 0x001AB0B4
	public void StartPlatforms()
	{
		this.leftPillar.transform.position = new Vector3(base.transform.position.x - base.properties.CurrentState.doomPillar.pillarPosition.min, base.transform.position.y);
		this.rightPillar.transform.position = new Vector3(base.transform.position.x + base.properties.CurrentState.doomPillar.pillarPosition.min, base.transform.position.y);
		base.StartCoroutine(this.create_platforms_cr());
		base.StartCoroutine(this.create_glass_cr());
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x001ACD88 File Offset: 0x001AB188
	public void StartPillarOfDoom()
	{
		LevelProperties.Saltbaker.DoomPillar doomPillar = base.properties.CurrentState.doomPillar;
		base.properties.OnBossDeath += this.Die;
		this.leftAnimator.Play("IntroA", 0, 0f);
		this.rightAnimator.Play("IntroB", 0, 0f);
		this.SFX_SALTB_P4_TornadoPillars_Loop();
		base.StartCoroutine(this.move_horizontal_cr());
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x001ACDFC File Offset: 0x001AB1FC
	public void StartHeart()
	{
		this.darkHeart.gameObject.SetActive(true);
		this.darkHeart.Init(Vector3.up * 500f, this.leftPillar, this.rightPillar, base.properties.CurrentState.darkHeart, this);
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x001ACE54 File Offset: 0x001AB254
	public float GetPlatformFallSpeed()
	{
		return Mathf.Lerp(base.properties.CurrentState.doomPillar.platformFallSpeed.min, base.properties.CurrentState.doomPillar.platformFallSpeed.max, Mathf.InverseLerp(0f, base.properties.CurrentState.doomPillar.phaseTime, this.phaseTimer));
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x001ACEC0 File Offset: 0x001AB2C0
	private IEnumerator create_glass_cr()
	{
		this.chunkOrderString = new PatternString(this.chunkOrder, true);
		this.chunkPositionString = new PatternString(this.chunkPosition, true);
		this.chunkSpawnTimeString = new PatternString(this.chunkSpawnTime, true);
		for (;;)
		{
			float t = this.chunkSpawnTimeString.PopFloat();
			while (t > 0f)
			{
				t -= CupheadTime.Delta * Mathf.Lerp(0.5f, 1f, Mathf.InverseLerp(0f, base.properties.CurrentState.doomPillar.phaseTime, this.phaseTimer));
				yield return null;
			}
			int usableChunk = -1;
			for (int i = 0; i < this.chunks.Count; i++)
			{
				if (this.chunks[i].transform.position.y < -520f)
				{
					usableChunk = i;
					break;
				}
			}
			if (usableChunk == -1)
			{
				this.chunks.Add(UnityEngine.Object.Instantiate<SaltbakerLevelGlassChunk>(this.chunkPrefab));
				usableChunk = this.chunks.Count - 1;
			}
			float xPos = Mathf.Lerp((float)Level.Current.Left, (float)Level.Current.Right, this.chunkPositionString.PopFloat());
			int id = this.chunkOrderString.PopInt();
			bool inBack = Rand.Bool();
			float fallSpeed = this.GetPlatformFallSpeed() + (float)((!inBack) ? UnityEngine.Random.Range(100, 200) : UnityEngine.Random.Range(-100, -75));
			this.chunks[usableChunk].Reset(new Vector3(xPos, 520f), fallSpeed, id < 4, this.chunkFlip[id], Rand.Bool(), inBack, id % 4);
			this.chunkFlip[id] = !this.chunkFlip[id];
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x001ACEDC File Offset: 0x001AB2DC
	private IEnumerator create_platforms_cr()
	{
		int bigCounter = 0;
		int mediumCounter = 0;
		int smallCounter = 0;
		float pillarSpawnOffset = 100f;
		LevelProperties.Saltbaker.DoomPillar p = base.properties.CurrentState.doomPillar;
		float pillarXBuffer = 146f + this.leftPillar.GetComponent<BoxCollider2D>().size.x / 2f;
		YieldInstruction wait = new WaitForFixedUpdate();
		PatternString platformSize = new PatternString(p.platformSizeString, true, true);
		PatternString platformPosX = new PatternString(p.platformXSpawnString, true, true);
		PatternString platformPosY = new PatternString(p.platformYSpawnString, true, true);
		if (p.platformXYUnified)
		{
			platformPosY.SetMainStringIndex(platformPosX.GetMainStringIndex());
			platformPosY.SetSubStringIndex(platformPosX.GetSubStringIndex());
			if (platformPosX.SubStringLength() != platformPosY.SubStringLength())
			{
				global::Debug.Break();
			}
			platformPosY.PopFloat();
		}
		float spawnDistance = 0f;
		for (;;)
		{
			Vector3 spawnPos = new Vector3(Mathf.Lerp(this.leftPillar.transform.position.x + pillarXBuffer, this.rightPillar.transform.position.x - pillarXBuffer, (platformPosX.PopFloat() + 1f) / 2f), (float)Level.Current.Ceiling + pillarSpawnOffset + spawnDistance);
			if (this.suppressCenterPlatforms)
			{
				spawnPos = new Vector3((!Rand.Bool()) ? (this.rightPillar.transform.position.x - pillarXBuffer) : (this.leftPillar.transform.position.x + pillarXBuffer), spawnPos.y);
			}
			spawnDistance = platformPosY.PopFloat();
			GameObject platform = null;
			char c = platformSize.PopLetter();
			if (c != 'L')
			{
				if (c != 'M')
				{
					if (c != 'S')
					{
						global::Debug.LogError("Pattern string is incorrect.", null);
						global::Debug.Break();
					}
					else
					{
						platform = UnityEngine.Object.Instantiate<GameObject>(this.smallPlatform[smallCounter % 2]);
						platform.transform.GetChild(0).localScale = new Vector3((float)((smallCounter % 4 >= 2) ? -1 : 1), 1f);
						smallCounter++;
					}
				}
				else
				{
					platform = UnityEngine.Object.Instantiate<GameObject>(this.medPlatform[mediumCounter % 2]);
					platform.transform.GetChild(0).localScale = new Vector3((float)((mediumCounter % 4 >= 2) ? -1 : 1), 1f);
					mediumCounter++;
				}
			}
			else
			{
				platform = UnityEngine.Object.Instantiate<GameObject>(this.bigPlatform[bigCounter % 2]);
				platform.transform.GetChild(0).localScale = new Vector3((float)((bigCounter % 4 >= 2) ? -1 : 1), 1f);
				bigCounter++;
			}
			platform.transform.position = spawnPos;
			this.platforms.Add(platform.gameObject);
			while (spawnDistance > 0f)
			{
				spawnDistance -= CupheadTime.FixedDelta * this.GetPlatformFallSpeed();
				yield return wait;
			}
		}
		yield break;
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x001ACEF8 File Offset: 0x001AB2F8
	private IEnumerator move_horizontal_cr()
	{
		LevelProperties.Saltbaker.DoomPillar p = base.properties.CurrentState.doomPillar;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			float t = Mathf.InverseLerp(0f, p.phaseTime, this.phaseTimer);
			this.leftPillar.transform.position = Vector3.Lerp(new Vector3(base.transform.position.x - p.pillarPosition.min, base.transform.position.y), new Vector3(base.transform.position.x - p.pillarPosition.max, base.transform.position.y), t);
			this.rightPillar.transform.position = Vector3.Lerp(new Vector3(base.transform.position.x + p.pillarPosition.min, base.transform.position.y), new Vector3(base.transform.position.x + p.pillarPosition.max, base.transform.position.y), t);
			this.phaseTimer += CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x001ACF14 File Offset: 0x001AB314
	private void Update()
	{
		this.platforms.RemoveAll((GameObject g) => g == null);
		foreach (GameObject gameObject in this.platforms)
		{
			if (gameObject.transform.position.y < (float)Level.Current.Ground - 400f)
			{
				UnityEngine.Object.Destroy(gameObject.gameObject);
			}
			else
			{
				gameObject.transform.position += Vector3.down * this.GetPlatformFallSpeed() * CupheadTime.Delta;
			}
		}
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController)
			{
				levelPlayerController.animationController.spriteRenderer.sortingOrder = ((levelPlayerController.motor.MoveDirection.y <= 0) ? 10 : 510);
			}
		}
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x001AD090 File Offset: 0x001AB490
	private void Die()
	{
		this.leftAnimator.Play("Death", -1, 0f);
		this.rightAnimator.Play("Death", -1, 0.5f);
		this.SFX_SALTB_P4_TornadoPillar_LoopStop();
		this.darkHeart.Die();
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x001AD0CF File Offset: 0x001AB4CF
	private void SFX_SALTB_P4_TornadoPillars_Loop()
	{
		AudioManager.PlayLoop("sfx_DLC_Saltbaker_P4_Tornado_Left_Loop");
		AudioManager.PlayLoop("sfx_DLC_Saltbaker_P4_Tornado_Right_Loop");
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x001AD0E5 File Offset: 0x001AB4E5
	private void SFX_SALTB_P4_TornadoPillar_LoopStop()
	{
		AudioManager.Stop("sfx_DLC_Saltbaker_P4_Tornado_Left_Loop");
		AudioManager.Stop("sfx_DLC_Saltbaker_P4_Tornado_Right_Loop");
	}

	// Token: 0x040035F9 RID: 13817
	[Header("Other")]
	[SerializeField]
	private GameObject leftPillar;

	// Token: 0x040035FA RID: 13818
	[SerializeField]
	private GameObject rightPillar;

	// Token: 0x040035FB RID: 13819
	[SerializeField]
	private Animator leftAnimator;

	// Token: 0x040035FC RID: 13820
	[SerializeField]
	private Animator rightAnimator;

	// Token: 0x040035FD RID: 13821
	[SerializeField]
	private SaltbakerLevelHeart darkHeart;

	// Token: 0x040035FE RID: 13822
	[SerializeField]
	private GameObject[] smallPlatform;

	// Token: 0x040035FF RID: 13823
	[SerializeField]
	private GameObject[] medPlatform;

	// Token: 0x04003600 RID: 13824
	[SerializeField]
	private GameObject[] bigPlatform;

	// Token: 0x04003601 RID: 13825
	private List<GameObject> platforms = new List<GameObject>();

	// Token: 0x04003602 RID: 13826
	[SerializeField]
	private SaltbakerLevelGlassChunk chunkPrefab;

	// Token: 0x04003603 RID: 13827
	private List<SaltbakerLevelGlassChunk> chunks = new List<SaltbakerLevelGlassChunk>();

	// Token: 0x04003604 RID: 13828
	[SerializeField]
	private string chunkOrder;

	// Token: 0x04003605 RID: 13829
	private PatternString chunkOrderString;

	// Token: 0x04003606 RID: 13830
	[SerializeField]
	private string chunkPosition;

	// Token: 0x04003607 RID: 13831
	private PatternString chunkPositionString;

	// Token: 0x04003608 RID: 13832
	[SerializeField]
	private string chunkSpawnTime;

	// Token: 0x04003609 RID: 13833
	private PatternString chunkSpawnTimeString;

	// Token: 0x0400360A RID: 13834
	private bool[] chunkFlip = new bool[8];

	// Token: 0x0400360B RID: 13835
	private float phaseTimer;

	// Token: 0x0400360C RID: 13836
	public bool suppressCenterPlatforms;
}
