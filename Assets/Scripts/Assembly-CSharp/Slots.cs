using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006AC RID: 1708
[Serializable]
public class Slots
{
	// Token: 0x06002446 RID: 9286 RVA: 0x00154EAD File Offset: 0x001532AD
	public void Init(MonoBehaviour parent)
	{
		this.parent = parent;
	}

	// Token: 0x06002447 RID: 9287 RVA: 0x00154EB6 File Offset: 0x001532B6
	public void Spin()
	{
		this.parent.StartCoroutine(this.spin_cr());
	}

	// Token: 0x06002448 RID: 9288 RVA: 0x00154ECC File Offset: 0x001532CC
	private IEnumerator spin_cr()
	{
		this.left.StartSpin();
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.mid.StartSpin();
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.right.StartSpin();
		yield break;
	}

	// Token: 0x06002449 RID: 9289 RVA: 0x00154EE7 File Offset: 0x001532E7
	public void Stop(Slots.Mode mode)
	{
		this.parent.StartCoroutine(this.stop_cr(mode));
	}

	// Token: 0x0600244A RID: 9290 RVA: 0x00154EFC File Offset: 0x001532FC
	private IEnumerator stop_cr(Slots.Mode mode)
	{
		this.left.StopSpin(mode);
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.mid.StopSpin(mode);
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.right.StopSpin(mode);
		yield break;
	}

	// Token: 0x0600244B RID: 9291 RVA: 0x00154F1E File Offset: 0x0015331E
	public void StartFlash()
	{
		this.parent.StartCoroutine(this.startFlash_cr());
	}

	// Token: 0x0600244C RID: 9292 RVA: 0x00154F34 File Offset: 0x00153334
	private IEnumerator startFlash_cr()
	{
		this.left.Flash();
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.mid.Flash();
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.right.Flash();
		yield break;
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x00154F4F File Offset: 0x0015334F
	public void OnDestroy()
	{
		this.left = null;
		this.mid = null;
		this.right = null;
	}

	// Token: 0x04002D02 RID: 11522
	private const float DELAY = 0.2f;

	// Token: 0x04002D03 RID: 11523
	[SerializeField]
	private FrogsLevelMorphedSlot left;

	// Token: 0x04002D04 RID: 11524
	[SerializeField]
	private FrogsLevelMorphedSlot mid;

	// Token: 0x04002D05 RID: 11525
	[SerializeField]
	private FrogsLevelMorphedSlot right;

	// Token: 0x04002D06 RID: 11526
	private MonoBehaviour parent;

	// Token: 0x020006AD RID: 1709
	public enum Mode
	{
		// Token: 0x04002D08 RID: 11528
		Snake,
		// Token: 0x04002D09 RID: 11529
		Tiger,
		// Token: 0x04002D0A RID: 11530
		Bison,
		// Token: 0x04002D0B RID: 11531
		Oni
	}
}
