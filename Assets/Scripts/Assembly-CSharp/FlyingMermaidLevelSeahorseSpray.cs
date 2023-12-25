using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200069B RID: 1691
public class FlyingMermaidLevelSeahorseSpray : AbstractPausableComponent
{
	// Token: 0x060023DA RID: 9178 RVA: 0x00150EA8 File Offset: 0x0014F2A8
	private void Update()
	{
		if (this.ended)
		{
			return;
		}
		foreach (PlanePlayerMotor planePlayerMotor in this.playerInfos.Keys)
		{
			if (!(planePlayerMotor == null))
			{
				if (Mathf.Abs(planePlayerMotor.transform.position.x - base.transform.position.x) < this.width / 2f && planePlayerMotor.player.center.y < this.topRoot.position.y)
				{
					this.playerInfos[planePlayerMotor].force.enabled = true;
					this.playerInfos[planePlayerMotor].timeSinceFx += CupheadTime.Delta;
					if (this.playerInfos[planePlayerMotor].timeSinceFx >= this.playerInfos[planePlayerMotor].fxWaitTime)
					{
						Effect effect = this.effectPrefab.Create(planePlayerMotor.player.center + new Vector3(0f, -40f));
						int num = (this.playerInfos[planePlayerMotor].lastFxVariant + UnityEngine.Random.Range(0, 3)) % 3;
						effect.animator.SetInteger("Effect", num);
						this.playerInfos[planePlayerMotor].lastFxVariant = num;
						this.playerInfos[planePlayerMotor].fxWaitTime = UnityEngine.Random.Range(0.125f, 0.17f);
						this.playerInfos[planePlayerMotor].timeSinceFx = 0f;
					}
				}
				else
				{
					this.playerInfos[planePlayerMotor].force.enabled = false;
					this.playerInfos[planePlayerMotor].fxWaitTime = 0f;
				}
			}
		}
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x001510CC File Offset: 0x0014F4CC
	public void Init(LevelProperties.FlyingMermaid.Seahorse properties)
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null))
			{
				PlanePlayerMotor.Force force = new PlanePlayerMotor.Force(new Vector2(0f, properties.waterForce), false);
				planePlayerController.motor.AddForce(force);
				FlyingMermaidLevelSeahorseSpray.PlayerInfo playerInfo = new FlyingMermaidLevelSeahorseSpray.PlayerInfo();
				playerInfo.force = force;
				this.playerInfos[planePlayerController.motor] = playerInfo;
			}
		}
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x00151178 File Offset: 0x0014F578
	public void End()
	{
		this.ended = true;
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null))
			{
				planePlayerController.motor.RemoveForce(this.playerInfos[planePlayerController.motor].force);
			}
		}
	}

	// Token: 0x04002CA3 RID: 11427
	public float width = 20f;

	// Token: 0x04002CA4 RID: 11428
	private Dictionary<PlanePlayerMotor, FlyingMermaidLevelSeahorseSpray.PlayerInfo> playerInfos = new Dictionary<PlanePlayerMotor, FlyingMermaidLevelSeahorseSpray.PlayerInfo>();

	// Token: 0x04002CA5 RID: 11429
	[SerializeField]
	private Effect effectPrefab;

	// Token: 0x04002CA6 RID: 11430
	[SerializeField]
	private Transform topRoot;

	// Token: 0x04002CA7 RID: 11431
	private bool ended;

	// Token: 0x0200069C RID: 1692
	private class PlayerInfo
	{
		// Token: 0x04002CA8 RID: 11432
		public PlanePlayerMotor.Force force;

		// Token: 0x04002CA9 RID: 11433
		public float timeSinceFx;

		// Token: 0x04002CAA RID: 11434
		public float fxWaitTime;

		// Token: 0x04002CAB RID: 11435
		public int lastFxVariant = -1;
	}
}
