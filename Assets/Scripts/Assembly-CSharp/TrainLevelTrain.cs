using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200082E RID: 2094
public class TrainLevelTrain : LevelProperties.Train.Entity
{
	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x0600309C RID: 12444 RVA: 0x001C9AA3 File Offset: 0x001C7EA3
	// (set) Token: 0x0600309D RID: 12445 RVA: 0x001C9AAB File Offset: 0x001C7EAB
	public TrainLevelTrain.State state { get; private set; }

	// Token: 0x0600309E RID: 12446 RVA: 0x001C9AB4 File Offset: 0x001C7EB4
	protected override void Awake()
	{
		base.Awake();
		base.transform.SetPosition(new float?(455f), null, null);
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x001C9AEE File Offset: 0x001C7EEE
	public void OnBlindSpectreDeath()
	{
		base.StartCoroutine(this.blindSpectreDeath_cr());
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x001C9AFD File Offset: 0x001C7EFD
	public void OnSkeletonDeath()
	{
		base.StartCoroutine(this.skeletonDeath_cr());
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x001C9B0C File Offset: 0x001C7F0C
	public void OnLollipopsDeath()
	{
		base.StartCoroutine(this.lollipopsDeath_cr());
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x001C9B1C File Offset: 0x001C7F1C
	private IEnumerator blindSpectreDeath_cr()
	{
		this.state = TrainLevelTrain.State.Skeleton;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return base.TweenPositionX(base.transform.position.x, -960f, 2.5f, EaseUtils.EaseType.easeInOutSine);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (int i = 0; i < this.skeletonCars.Length; i++)
		{
			int i2 = (i != 1) ? 0 : 1;
			this.skeletonCars[i].Explode(i2);
		}
		AudioManager.Play("level_train_top_explode");
		this.skeleton.StartSkeleton();
		yield break;
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x001C9B38 File Offset: 0x001C7F38
	private IEnumerator skeletonDeath_cr()
	{
		this.state = TrainLevelTrain.State.LollipopGhouls;
		this.ghouls.Setup();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return base.TweenPositionX(base.transform.position.x, -2358f, 2.5f, EaseUtils.EaseType.easeInOutSine);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.ghouls.StartGhouls();
		yield break;
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x001C9B54 File Offset: 0x001C7F54
	private IEnumerator lollipopsDeath_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		yield return base.TweenPositionX(base.transform.position.x, -4816f, 2.5f, EaseUtils.EaseType.easeInSine);
		this.engineCar.PlayRage();
		yield return base.TweenPositionX(base.transform.position.x, -6016f, 2.5f, EaseUtils.EaseType.linear);
		this.engineCar.End();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.engineBoss.StartBoss();
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0400393B RID: 14651
	public const float TRAIN_MOVE_TIME = 2.5f;

	// Token: 0x0400393C RID: 14652
	public const float START_X = 455f;

	// Token: 0x0400393D RID: 14653
	public const float SKELETON_X = -960f;

	// Token: 0x0400393E RID: 14654
	public const float GHOUL_X = -2358f;

	// Token: 0x0400393F RID: 14655
	public const float ENGINE_MID_X = -4816f;

	// Token: 0x04003940 RID: 14656
	public const float ENGINE_X = -6016f;

	// Token: 0x04003942 RID: 14658
	[SerializeField]
	private TrainLevelSkeleton skeleton;

	// Token: 0x04003943 RID: 14659
	[SerializeField]
	private TrainLevelPassengerCar[] skeletonCars;

	// Token: 0x04003944 RID: 14660
	[Space(10f)]
	[SerializeField]
	private TrainLevelLollipopGhoulsManager ghouls;

	// Token: 0x04003945 RID: 14661
	[Space(10f)]
	[SerializeField]
	private TrainLevelEngineCar engineCar;

	// Token: 0x04003946 RID: 14662
	[SerializeField]
	private TrainLevelEngineBoss engineBoss;

	// Token: 0x0200082F RID: 2095
	public enum State
	{
		// Token: 0x04003948 RID: 14664
		BlindSpecter,
		// Token: 0x04003949 RID: 14665
		Skeleton,
		// Token: 0x0400394A RID: 14666
		LollipopGhouls,
		// Token: 0x0400394B RID: 14667
		Engine
	}
}
