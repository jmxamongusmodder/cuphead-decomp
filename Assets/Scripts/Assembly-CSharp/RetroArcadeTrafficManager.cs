using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000760 RID: 1888
public class RetroArcadeTrafficManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x06002921 RID: 10529 RVA: 0x0017FAF0 File Offset: 0x0017DEF0
	public void StartTraffic()
	{
		this.SpawnTrafficLights();
		this.StartUFO();
		base.StartCoroutine(this.move_ufo_cr());
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x0017FB0C File Offset: 0x0017DF0C
	public void StartUFO()
	{
		this.trafficUFO.gameObject.SetActive(true);
		AbstractPlayerController next = PlayerManager.GetNext();
		if (next.transform.position.x > 0f)
		{
			this.trafficUFO.transform.position = this.trafficGrid[0, 3].transform.position;
		}
		else
		{
			this.trafficUFO.transform.position = this.trafficGrid[3, 3].transform.position;
		}
	}

	// Token: 0x06002923 RID: 10531 RVA: 0x0017FBA0 File Offset: 0x0017DFA0
	private void SpawnTrafficLights()
	{
		this.trafficGrid = new GameObject[4, 4];
		float num = (float)(Level.Current.Width / 4);
		float num2 = (float)(Level.Current.Height / 4);
		Vector3 a = new Vector3((float)Level.Current.Left + num / 2f, (float)Level.Current.Ground + num2 / 2f);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Vector3 b = new Vector3((float)i * num, (float)j * num2);
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.trafficLightPrefab);
				this.trafficGrid[i, j] = gameObject;
				this.trafficGrid[i, j].transform.position = a + b;
				this.trafficGrid[i, j].transform.parent = base.transform;
			}
		}
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x0017FC98 File Offset: 0x0017E098
	private IEnumerator move_ufo_cr()
	{
		LevelProperties.RetroArcade.Traffic p = base.properties.CurrentState.traffic;
		int lightMainIndex = UnityEngine.Random.Range(0, p.lightOrderString.Length);
		string[] lightString = p.lightOrderString[lightMainIndex].Split(new char[]
		{
			','
		});
		int lightX = 0;
		int lightY = 0;
		this.positionsToTravel = new List<Vector3>();
		while (!this.trafficUFO.IsDead)
		{
			lightString = p.lightOrderString[lightMainIndex].Split(new char[]
			{
				','
			});
			for (int i = 0; i < lightString.Length; i++)
			{
				yield return CupheadTime.WaitForSeconds(this, p.lightDelay);
				string getLightCoordX = lightString[i].Substring(1);
				string getLightCoordY = lightString[i].Substring(0, 1);
				Parser.IntTryParse(getLightCoordX, out lightX);
				if (getLightCoordY != null)
				{
					if (!(getLightCoordY == "A"))
					{
						if (!(getLightCoordY == "B"))
						{
							if (!(getLightCoordY == "C"))
							{
								if (getLightCoordY == "D")
								{
									lightY = 3;
								}
							}
							else
							{
								lightY = 2;
							}
						}
						else
						{
							lightY = 1;
						}
					}
					else
					{
						lightY = 0;
					}
				}
				this.trafficGrid[lightX, lightY].GetComponent<Animator>().SetBool("IsGreen", true);
				this.positionsToTravel.Add(this.trafficGrid[lightX, lightY].transform.position);
			}
			this.trafficUFO.StartMoving(this.positionsToTravel, p.moveSpeed, p.moveDelay);
			while (this.trafficUFO.IsMoving && !this.trafficUFO.IsDead)
			{
				yield return null;
			}
			this.ResetLights();
			this.positionsToTravel.Clear();
			lightMainIndex = (lightMainIndex + 1) % p.lightOrderString.Length;
			yield return null;
		}
		this.EndPhase();
		yield break;
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x0017FCB3 File Offset: 0x0017E0B3
	private void EndPhase()
	{
		this.StopAllCoroutines();
		this.DestroyLights();
		UnityEngine.Object.Destroy(this.trafficUFO.gameObject);
		base.properties.DealDamageToNextNamedState();
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x0017FCDC File Offset: 0x0017E0DC
	private void ResetLights()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				this.trafficGrid[i, j].GetComponent<Animator>().SetBool("IsGreen", false);
			}
		}
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x0017FD2C File Offset: 0x0017E12C
	private void DestroyLights()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				UnityEngine.Object.Destroy(this.trafficGrid[i, j]);
			}
		}
	}

	// Token: 0x04003217 RID: 12823
	[SerializeField]
	private RetroArcadeTrafficUFO trafficUFO;

	// Token: 0x04003218 RID: 12824
	[SerializeField]
	private GameObject trafficLightPrefab;

	// Token: 0x04003219 RID: 12825
	private const int GRIDX = 4;

	// Token: 0x0400321A RID: 12826
	private const int GRIDY = 4;

	// Token: 0x0400321B RID: 12827
	private GameObject[,] trafficGrid;

	// Token: 0x0400321C RID: 12828
	private List<Vector3> positionsToTravel;
}
