using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A1 RID: 2209
public class CircusPlatformingLevelClouds : AbstractPausableComponent
{
	// Token: 0x06003366 RID: 13158 RVA: 0x001DE6F0 File Offset: 0x001DCAF0
	private void Start()
	{
		base.StartCoroutine(this.change_y_axis());
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x001DE700 File Offset: 0x001DCB00
	private IEnumerator change_y_axis()
	{
		float[] cloudStartPositionsX = new float[this.cloudPieces.Length];
		float[] cloudStartSpeedX = new float[this.cloudPieces.Length];
		for (int j = 0; j < this.cloudPieces.Length; j++)
		{
			cloudStartPositionsX[j] = this.cloudPieces[j].cloud.position.y;
			foreach (ScrollingSprite scrollingSprite in this.cloudPieces[j].cloud.GetComponentsInChildren<ScrollingSprite>())
			{
				cloudStartSpeedX[j] = scrollingSprite.speed;
			}
		}
		for (;;)
		{
			for (int i = 0; i < this.cloudPieces.Length; i++)
			{
				this.cloudPieces[i].cloud.SetPosition(null, new float?(Mathf.Lerp(cloudStartPositionsX[i], this.cloudPieces[i].cloudEndY, this.RelativePosition(this.cloudPieces[i].cameraRelativePosX))), null);
				if (CupheadLevelCamera.Current.transform.position != this.lastPosition)
				{
					if (this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>())
					{
						this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>().enabled = false;
					}
					foreach (ScrollingSprite scrollingSprite2 in this.cloudPieces[i].cloud.GetComponentsInChildren<ScrollingSprite>())
					{
						if (CupheadLevelCamera.Current.transform.position.x < this.lastPosition.x)
						{
							if (scrollingSprite2.speed > cloudStartSpeedX[i])
							{
								scrollingSprite2.speed -= this.incrementAmount;
							}
						}
						else if (scrollingSprite2.speed < cloudStartSpeedX[i] * this.cloudPieces[i].speedMultiplyAmount)
						{
							scrollingSprite2.speed += this.incrementAmount;
						}
					}
					this.cloudPieces[i].UpdateCurrentRelativePos(this.RelativePosition(this.cloudPieces[i].cameraRelativePosX));
					this.lastPosition = CupheadLevelCamera.Current.transform.position;
					yield return null;
				}
				else
				{
					if (this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>())
					{
						this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>().enabled = true;
						this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>().UpdateBasePosition();
					}
					foreach (ScrollingSprite scrollingSprite3 in this.cloudPieces[i].cloud.GetComponentsInChildren<ScrollingSprite>())
					{
						if (scrollingSprite3.speed > cloudStartSpeedX[i])
						{
							scrollingSprite3.speed -= this.incrementAmount;
						}
						else
						{
							scrollingSprite3.speed = cloudStartSpeedX[i];
						}
					}
					yield return null;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x001DE71C File Offset: 0x001DCB1C
	private float RelativePosition(float relativePosX)
	{
		float num = relativePosX - (float)Level.Current.Left;
		float num2 = CupheadLevelCamera.Current.transform.position.x - (float)Level.Current.Left;
		return num2 / num;
	}

	// Token: 0x04003BB4 RID: 15284
	[SerializeField]
	private CircusPlatformingLevelClouds.CloudPiece[] cloudPieces;

	// Token: 0x04003BB5 RID: 15285
	private Vector3 lastPosition;

	// Token: 0x04003BB6 RID: 15286
	[SerializeField]
	private float incrementAmount = 2f;

	// Token: 0x020008A2 RID: 2210
	[Serializable]
	public class CloudPiece
	{
		// Token: 0x0600336A RID: 13162 RVA: 0x001DE768 File Offset: 0x001DCB68
		public void UpdateCurrentRelativePos(float pos)
		{
			this.currentRelativePosX = pos;
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x001DE771 File Offset: 0x001DCB71
		public float CurrentRelativePosX()
		{
			return this.currentRelativePosX;
		}

		// Token: 0x04003BB7 RID: 15287
		public Transform cloud;

		// Token: 0x04003BB8 RID: 15288
		public float cloudEndY;

		// Token: 0x04003BB9 RID: 15289
		public float cameraRelativePosX;

		// Token: 0x04003BBA RID: 15290
		public float speedMultiplyAmount;

		// Token: 0x04003BBB RID: 15291
		private float currentRelativePosX;
	}
}
