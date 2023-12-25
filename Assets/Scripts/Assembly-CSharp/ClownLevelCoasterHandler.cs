using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000562 RID: 1378
public class ClownLevelCoasterHandler : LevelProperties.Clown.Entity
{
	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060019EF RID: 6639 RVA: 0x000ED1F0 File Offset: 0x000EB5F0
	// (remove) Token: 0x060019F0 RID: 6640 RVA: 0x000ED228 File Offset: 0x000EB628
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnCoasterLeave;

	// Token: 0x060019F1 RID: 6641 RVA: 0x000ED25E File Offset: 0x000EB65E
	public override void LevelInit(LevelProperties.Clown properties)
	{
		base.LevelInit(properties);
		this.finalRun = false;
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x000ED26E File Offset: 0x000EB66E
	public void StartCoaster()
	{
		this.isRunning = true;
		base.StartCoroutine(this.coaster_cr());
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x000ED284 File Offset: 0x000EB684
	private IEnumerator coaster_cr()
	{
		LevelProperties.Clown.Coaster p = base.properties.CurrentState.coaster;
		string[] coasterPattern = p.coasterTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		float coasterSize = this.redCoaster.GetComponent<Renderer>().bounds.size.x;
		if (base.properties.CurrentState.stateName == LevelProperties.Clown.States.Swing)
		{
			while (this.swing.state == ClownLevelClownSwing.State.Intro)
			{
				yield return null;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, p.initialDelay);
		while (this.isRunning)
		{
			yield return null;
			ClownLevelCoaster coaster = UnityEngine.Object.Instantiate<ClownLevelCoaster>(this.coasterPrefab);
			coaster.Init(this.backTrackStart.position, this.frontTrackStart.position, p, (float)coasterPattern.Length, coasterSize, this.warningLight);
			Transform lastInstantiatedRoot = coaster.pieceRoot;
			for (int i = 0; i < coasterPattern.Length; i++)
			{
				if (i % 2 == 1)
				{
					ClownLevelCoasterPiece clownLevelCoasterPiece = UnityEngine.Object.Instantiate<ClownLevelCoasterPiece>(this.blueCoaster);
					clownLevelCoasterPiece.Init(lastInstantiatedRoot.position);
					lastInstantiatedRoot = clownLevelCoasterPiece.newPieceRoot;
					clownLevelCoasterPiece.transform.parent = coaster.transform;
					if (i == coasterPattern.Length)
					{
						lastInstantiatedRoot = clownLevelCoasterPiece.tailRoot;
					}
					if (coasterPattern[i][0] == 'F')
					{
						ClownLevelRiders clownLevelRiders = UnityEngine.Object.Instantiate<ClownLevelRiders>(this.ridersPrefab);
						ClownLevelRiders clownLevelRiders2 = UnityEngine.Object.Instantiate<ClownLevelRiders>(this.ridersPrefab);
						clownLevelRiders.transform.position = clownLevelCoasterPiece.ridersFrontRoot.position;
						clownLevelRiders.transform.parent = clownLevelCoasterPiece.ridersFrontRoot.transform;
						clownLevelRiders.inFront = true;
						clownLevelCoasterPiece.riders.Add(clownLevelRiders);
						clownLevelRiders2.transform.position = clownLevelCoasterPiece.ridersBackRoot.position;
						clownLevelRiders2.transform.parent = clownLevelCoasterPiece.ridersBackRoot.transform;
						clownLevelRiders2.inFront = false;
						clownLevelCoasterPiece.riders.Add(clownLevelRiders2);
					}
				}
				else
				{
					ClownLevelCoasterPiece clownLevelCoasterPiece2 = UnityEngine.Object.Instantiate<ClownLevelCoasterPiece>(this.redCoaster);
					clownLevelCoasterPiece2.Init(lastInstantiatedRoot.position);
					lastInstantiatedRoot = clownLevelCoasterPiece2.newPieceRoot;
					clownLevelCoasterPiece2.transform.parent = coaster.transform;
					if (i == coasterPattern.Length)
					{
						lastInstantiatedRoot = clownLevelCoasterPiece2.tailRoot;
					}
					if (coasterPattern[i][0] == 'F')
					{
						ClownLevelRiders clownLevelRiders3 = UnityEngine.Object.Instantiate<ClownLevelRiders>(this.ridersPrefab);
						ClownLevelRiders clownLevelRiders4 = UnityEngine.Object.Instantiate<ClownLevelRiders>(this.ridersPrefab);
						clownLevelRiders3.transform.position = clownLevelCoasterPiece2.ridersFrontRoot.position;
						clownLevelRiders3.transform.parent = clownLevelCoasterPiece2.ridersFrontRoot.transform;
						clownLevelRiders3.inFront = true;
						clownLevelCoasterPiece2.riders.Add(clownLevelRiders3);
						clownLevelRiders4.transform.position = clownLevelCoasterPiece2.ridersBackRoot.position;
						clownLevelRiders4.transform.parent = clownLevelCoasterPiece2.ridersBackRoot.transform;
						clownLevelRiders4.inFront = false;
						clownLevelCoasterPiece2.riders.Add(clownLevelRiders4);
					}
				}
			}
			GameObject tail = UnityEngine.Object.Instantiate<GameObject>(this.tailPrefab);
			tail.transform.position = lastInstantiatedRoot.position;
			tail.transform.parent = coaster.transform;
			coaster.BackCoasterSetup();
			while (coaster != null)
			{
				yield return null;
			}
			if (this.OnCoasterLeave != null)
			{
				this.OnCoasterLeave();
			}
			if (this.finalRun)
			{
				this.isRunning = false;
				this.finalRun = false;
				yield break;
			}
			yield return CupheadTime.WaitForSeconds(this, p.mainLoopDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x000ED29F File Offset: 0x000EB69F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.redCoaster = null;
		this.blueCoaster = null;
		this.ridersPrefab = null;
		this.tailPrefab = null;
		this.coasterPrefab = null;
	}

	// Token: 0x0400230E RID: 8974
	public bool finalRun;

	// Token: 0x0400230F RID: 8975
	public bool isRunning;

	// Token: 0x04002310 RID: 8976
	[SerializeField]
	private ClownLevelClownSwing swing;

	// Token: 0x04002311 RID: 8977
	[SerializeField]
	private ClownLevelLights warningLight;

	// Token: 0x04002312 RID: 8978
	[SerializeField]
	private Transform frontTrackStart;

	// Token: 0x04002313 RID: 8979
	[SerializeField]
	private Transform backTrackStart;

	// Token: 0x04002314 RID: 8980
	[SerializeField]
	private ClownLevelCoasterPiece redCoaster;

	// Token: 0x04002315 RID: 8981
	[SerializeField]
	private ClownLevelCoasterPiece blueCoaster;

	// Token: 0x04002316 RID: 8982
	[SerializeField]
	private ClownLevelRiders ridersPrefab;

	// Token: 0x04002317 RID: 8983
	[SerializeField]
	private GameObject tailPrefab;

	// Token: 0x04002318 RID: 8984
	[SerializeField]
	private ClownLevelCoaster coasterPrefab;
}
