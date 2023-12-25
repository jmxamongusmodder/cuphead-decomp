using System;
using UnityEngine;

// Token: 0x02000511 RID: 1297
public class BeeLevelBackgroundGroup : AbstractMonoBehaviour
{
	// Token: 0x06001714 RID: 5908 RVA: 0x000CF8D8 File Offset: 0x000CDCD8
	private void Start()
	{
		this.level = (Level.Current as BeeLevel);
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x000CF8EC File Offset: 0x000CDCEC
	private void Update()
	{
		if (base.transform.localPosition.y < -800f)
		{
			this.SetY(base.transform.localPosition.y + (float)this.count * 455f);
			this.Randomize();
		}
	}

	// Token: 0x06001716 RID: 5910 RVA: 0x000CF944 File Offset: 0x000CDD44
	private void FixedUpdate()
	{
		this.SetY(base.transform.localPosition.y + this.level.Speed * CupheadTime.Delta);
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x000CF984 File Offset: 0x000CDD84
	public void Init(BeeLevelPlatforms platforms, int groupCount)
	{
		this.level = (Level.Current as BeeLevel);
		this.count = groupCount;
		this.platforms = UnityEngine.Object.Instantiate<BeeLevelPlatforms>(platforms);
		this.platforms.transform.SetParent(base.transform);
		this.platforms.Init();
		this.Randomize();
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x000CF9DB File Offset: 0x000CDDDB
	public void Randomize()
	{
		this.DisableAll();
		this.platforms.Randomize(this.level.MissingPlatformCount);
		this.variations[UnityEngine.Random.Range(0, this.variations.Length)].SetActive(true);
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x000CFA14 File Offset: 0x000CDE14
	private void DisableAll()
	{
		foreach (GameObject gameObject in this.variations)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x000CFA47 File Offset: 0x000CDE47
	public void SetY(float y)
	{
		base.transform.SetPosition(new float?(0f), new float?(y), new float?(0f));
	}

	// Token: 0x0400205C RID: 8284
	private const float MIN_Y = -800f;

	// Token: 0x0400205D RID: 8285
	[SerializeField]
	private GameObject[] variations;

	// Token: 0x0400205E RID: 8286
	private BeeLevel level;

	// Token: 0x0400205F RID: 8287
	private BeeLevelPlatforms platforms;

	// Token: 0x04002060 RID: 8288
	private int count;

	// Token: 0x04002061 RID: 8289
	private int lastCount;
}
