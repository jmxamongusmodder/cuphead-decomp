using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000530 RID: 1328
public class ChessCastleLevelCloud : AbstractPausableComponent
{
	// Token: 0x06001808 RID: 6152 RVA: 0x000D98F4 File Offset: 0x000D7CF4
	public void Initialize(ChessCastleLevel castle)
	{
		this.castle = castle;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x000D9900 File Offset: 0x000D7D00
	private void Start()
	{
		base.animator.SetInteger("Version", UnityEngine.Random.Range(0, 13));
		base.animator.Update(0f);
		Bounds bounds = new Bounds(Vector3.zero, new Vector3(1280f, 720f, 0f) / Level.Current.CameraSettings.zoom);
		Vector2 vector = base.GetComponent<SpriteRenderer>().sprite.bounds.size;
		base.transform.position = new Vector3(bounds.max.x + vector.x * 0.5f + 30f, UnityEngine.Random.Range(bounds.min.y + vector.y * 0.5f + 30f, bounds.max.y - vector.y * 0.5f - 30f), UnityEngine.Random.Range(0f, 1f));
		this.speed = this.speedRange.RandomFloat();
		base.StartCoroutine(this.destroy_cr());
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x000D9A34 File Offset: 0x000D7E34
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.castle = null;
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x000D9A44 File Offset: 0x000D7E44
	private void Update()
	{
		if (this.castle.rotating)
		{
			if (this.speedRampCoroutine != null)
			{
				base.StopCoroutine(this.speedRampCoroutine);
				this.speedRampCoroutine = null;
			}
			this.speedMultiplier = this.castle.rotationMultiplier;
		}
		else if (this.wasRotating)
		{
			this.speedRampCoroutine = base.StartCoroutine(this.speedRamp_cr());
		}
		this.wasRotating = this.castle.rotating;
		float num = this.speed * this.speedMultiplier;
		Vector3 position = base.transform.position;
		position.x -= num * CupheadTime.Delta;
		base.transform.position = position;
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x000D9B04 File Offset: 0x000D7F04
	private IEnumerator speedRamp_cr()
	{
		float elapsedTime = 0f;
		while (elapsedTime < 0.35f)
		{
			yield return null;
			elapsedTime += Time.deltaTime;
			this.speedMultiplier = Mathf.Lerp(this.castle.rotationMultiplier, 1f, elapsedTime / 0.35f);
		}
		this.speedMultiplier = 1f;
		this.speedRampCoroutine = null;
		yield break;
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x000D9B20 File Offset: 0x000D7F20
	private IEnumerator destroy_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 10f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002140 RID: 8512
	[SerializeField]
	private MinMax speedRange;

	// Token: 0x04002141 RID: 8513
	private ChessCastleLevel castle;

	// Token: 0x04002142 RID: 8514
	private float speed;

	// Token: 0x04002143 RID: 8515
	private float speedMultiplier = 1f;

	// Token: 0x04002144 RID: 8516
	private bool wasRotating;

	// Token: 0x04002145 RID: 8517
	private Coroutine speedRampCoroutine;
}
