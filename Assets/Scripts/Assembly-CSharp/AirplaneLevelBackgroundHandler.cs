using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class AirplaneLevelBackgroundHandler : MonoBehaviour
{
	// Token: 0x06001396 RID: 5014 RVA: 0x000AC382 File Offset: 0x000AA782
	private void Start()
	{
		this.hillsFrameIndex = this.hillsSprites.Length - 1;
		base.StartCoroutine(this.main_loop_cr());
		base.StartCoroutine(this.cloud_loop_cr());
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x000AC3B0 File Offset: 0x000AA7B0
	private IEnumerator cloud_loop_cr()
	{
		bool[] useAlternate = new bool[8];
		int[] lastCloud = new int[]
		{
			-1,
			-1,
			-1
		};
		for (int i = 0; i < 4; i++)
		{
			int num = UnityEngine.Random.Range(0, 8);
			if (num != 3 && num != lastCloud[0] && num != lastCloud[1] && num != lastCloud[2])
			{
				this.cloudRenderers[i].flipX = (num >= 4);
				this.cloudAnimators[i].Play((num % 4 * 2 + ((!useAlternate[num]) ? 1 : 0)).ToString(), 0, UnityEngine.Random.Range(0f, 1f));
				useAlternate[num] = !useAlternate[num];
				lastCloud[2] = lastCloud[1];
				lastCloud[1] = lastCloud[0];
				lastCloud[0] = num;
			}
		}
		for (;;)
		{
			int delay = UnityEngine.Random.Range(this.CLOUD_DELAY_FRAMES_MIN, this.CLOUD_DELAY_FRAMES_MAX);
			int t = 0;
			while (t < delay)
			{
				if (!CupheadTime.IsPaused())
				{
					t++;
				}
				yield return null;
			}
			int myRenderer = -1;
			for (int j = 0; j < this.cloudRenderers.Length; j++)
			{
				if (this.cloudRenderers[j].sprite == null)
				{
					myRenderer = j;
					break;
				}
			}
			if (myRenderer == -1)
			{
				yield return null;
			}
			else
			{
				int num2 = UnityEngine.Random.Range(0, 8);
				if (num2 == 3 && (float)UnityEngine.Random.Range(0, 1) < 0.75f)
				{
					num2 += UnityEngine.Random.Range(1, 3) * ((!MathUtils.RandomBool()) ? 1 : -1);
				}
				if (num2 != lastCloud[0] && num2 != lastCloud[1] && num2 != lastCloud[2])
				{
					this.cloudRenderers[myRenderer].flipX = (num2 >= 4);
					if (num2 == 3)
					{
						this.cloudRenderers[myRenderer].flipX = MathUtils.RandomBool();
					}
					this.cloudAnimators[myRenderer].Play((num2 % 4 * 2 + ((!useAlternate[num2]) ? 1 : 0)).ToString(), 0, 0f);
					useAlternate[num2] = !useAlternate[num2];
					lastCloud[2] = lastCloud[1];
					lastCloud[1] = lastCloud[0];
					lastCloud[0] = num2;
				}
			}
		}
		yield break;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x000AC3CC File Offset: 0x000AA7CC
	private IEnumerator play_object_cr(int objectNum, int myIndex)
	{
		this.groundControllerCoroutineCurrentObject.Add(objectNum);
		for (;;)
		{
			while (this.groundControllerCoroutineCurrentObject[myIndex] == -1)
			{
				yield return null;
			}
			objectNum = this.groundControllerCoroutineCurrentObject[myIndex];
			while (this.hillsFrameIndex < this.objects[objectNum].startFrame)
			{
				yield return null;
			}
			int myRenderer = -1;
			for (int i = 0; i < this.spriteRenderers.Length; i++)
			{
				if (this.spriteRenderers[i].sprite == null)
				{
					myRenderer = i;
					break;
				}
			}
			if (myRenderer != -1)
			{
				int frameCounter = 0;
				int curHillsFrameIndex = this.hillsFrameIndex;
				while (frameCounter < this.objects[objectNum].duration)
				{
					this.spriteRenderers[myRenderer].sprite = this.objectSprites[this.objects[objectNum].spriteIndex + frameCounter];
					this.spriteRenderers[myRenderer].sortingOrder = -100 - frameCounter * 2 + this.objects[objectNum].layerOffset;
					while (curHillsFrameIndex == this.hillsFrameIndex)
					{
						yield return null;
					}
					curHillsFrameIndex = this.hillsFrameIndex;
					frameCounter++;
				}
				this.spriteRenderers[myRenderer].sprite = null;
			}
			this.groundControllerCoroutineCurrentObject[myIndex] = -1;
		}
		yield break;
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x000AC3F8 File Offset: 0x000AA7F8
	private IEnumerator main_loop_cr()
	{
		bool[] startObject = new bool[this.objects.Length];
		for (;;)
		{
			this.hillsFrameIndex = (this.hillsFrameIndex + 1) % this.hillsSprites.Length;
			if (this.hillsFrameIndex == 0)
			{
				this.densityWavePosition += this.densityWaveRate;
				for (int i = 0; i < this.objects.Length; i++)
				{
					startObject[i] = (UnityEngine.Random.Range(0f, 1f) < 0.4f + Mathf.Sin(this.densityWavePosition) * 0.2f);
				}
				if (startObject[3] && startObject[5])
				{
					startObject[(!MathUtils.RandomBool()) ? 5 : 3] = false;
				}
				for (int j = 0; j < this.objects.Length; j++)
				{
					if (startObject[j])
					{
						int num = -1;
						for (int k = 0; k < this.groundControllerCoroutine.Count; k++)
						{
							if (this.groundControllerCoroutineCurrentObject[k] == -1)
							{
								num = k;
								break;
							}
						}
						if (num > -1)
						{
							this.groundControllerCoroutineCurrentObject[num] = j;
						}
						else
						{
							this.groundControllerCoroutine.Add(base.StartCoroutine(this.play_object_cr(j, this.groundControllerCoroutine.Count)));
						}
					}
				}
			}
			if (this.prepopulateCounter >= 48)
			{
				yield return new WaitForEndOfFrame();
			}
			this.hillsRenderer.sprite = this.hillsSprites[this.hillsFrameIndex];
			this.bgFillSprite.color = this.bgColor[this.hillsFrameIndex];
			if (this.prepopulateCounter >= 48)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f / this.frameRate);
			}
			else
			{
				yield return null;
			}
			this.prepopulateCounter++;
		}
		yield break;
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x000AC414 File Offset: 0x000AA814
	private void Update()
	{
		this.distantHillsTimer += CupheadTime.Delta;
		float num = this.distantHillsTimer % (this.distantHillsLoopTime * (float)this.distantHillsRenderers.Length) / (this.distantHillsLoopTime * (float)this.distantHillsRenderers.Length);
		for (int i = 0; i < this.distantHillsRenderers.Length; i++)
		{
			float num2 = EaseUtils.EaseOutCubic(this.distantHillsMaxScale, this.distantHillsMinScale, (num + (float)i * (1f / (float)this.distantHillsRenderers.Length)) % 1f);
			this.distantHillsRenderers[i].transform.localScale = new Vector3(num2, num2);
			this.distantHillsRenderers[i].sortingOrder = -490 - (int)((num * (float)this.distantHillsRenderers.Length + (float)i) % (float)this.distantHillsRenderers.Length);
			this.distantHillsRenderers[i].color = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(this.distantHillsFadeStartScale, this.distantHillsMinScale, num2));
		}
	}

	// Token: 0x04001CA1 RID: 7329
	private int CLOUD_DELAY_FRAMES_MIN = 5;

	// Token: 0x04001CA2 RID: 7330
	private int CLOUD_DELAY_FRAMES_MAX = 10;

	// Token: 0x04001CA3 RID: 7331
	[SerializeField]
	private float frameRate = 30f;

	// Token: 0x04001CA4 RID: 7332
	[SerializeField]
	private Color[] bgColor;

	// Token: 0x04001CA5 RID: 7333
	[SerializeField]
	private SpriteRenderer bgFillSprite;

	// Token: 0x04001CA6 RID: 7334
	[SerializeField]
	private Sprite[] hillsSprites;

	// Token: 0x04001CA7 RID: 7335
	[SerializeField]
	private SpriteRenderer hillsRenderer;

	// Token: 0x04001CA8 RID: 7336
	[SerializeField]
	private AirplaneLevelBackgroundHandler.bgObject[] objects;

	// Token: 0x04001CA9 RID: 7337
	[SerializeField]
	private SpriteRenderer[] spriteRenderers;

	// Token: 0x04001CAA RID: 7338
	[SerializeField]
	private Sprite[] objectSprites;

	// Token: 0x04001CAB RID: 7339
	[SerializeField]
	private SpriteRenderer[] cloudRenderers;

	// Token: 0x04001CAC RID: 7340
	[SerializeField]
	private Animator[] cloudAnimators;

	// Token: 0x04001CAD RID: 7341
	[SerializeField]
	private SpriteRenderer[] distantHillsRenderers;

	// Token: 0x04001CAE RID: 7342
	private float distantHillsTimer;

	// Token: 0x04001CAF RID: 7343
	[SerializeField]
	private float distantHillsLoopTime = 40f;

	// Token: 0x04001CB0 RID: 7344
	[SerializeField]
	private float distantHillsMaxScale = 3f;

	// Token: 0x04001CB1 RID: 7345
	[SerializeField]
	private float distantHillsMinScale = 0.04f;

	// Token: 0x04001CB2 RID: 7346
	[SerializeField]
	private float distantHillsFadeStartScale = 0.2f;

	// Token: 0x04001CB3 RID: 7347
	private List<Coroutine> groundControllerCoroutine = new List<Coroutine>();

	// Token: 0x04001CB4 RID: 7348
	private List<int> groundControllerCoroutineCurrentObject = new List<int>();

	// Token: 0x04001CB5 RID: 7349
	private int hillsFrameIndex;

	// Token: 0x04001CB6 RID: 7350
	private int prepopulateCounter;

	// Token: 0x04001CB7 RID: 7351
	private float densityWavePosition;

	// Token: 0x04001CB8 RID: 7352
	private float densityWaveRate = 0.5f;

	// Token: 0x020004B1 RID: 1201
	[Serializable]
	private struct bgObject
	{
		// Token: 0x04001CB9 RID: 7353
		public string nameForSanity;

		// Token: 0x04001CBA RID: 7354
		public int startFrame;

		// Token: 0x04001CBB RID: 7355
		public int duration;

		// Token: 0x04001CBC RID: 7356
		public int spriteIndex;

		// Token: 0x04001CBD RID: 7357
		public int layerOffset;
	}
}
