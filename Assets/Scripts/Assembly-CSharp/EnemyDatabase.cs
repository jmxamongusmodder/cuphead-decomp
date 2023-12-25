using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class EnemyDatabase : ScriptableObject
{
	// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000458
	public static EnemyProperties GetProperties(EnemyID id)
	{
		if (id == EnemyID.Undefined)
		{
			return null;
		}
		if (id == EnemyID.blue_goblin)
		{
			return EnemyDatabase.Instance.enemyProperties[0];
		}
		if (id == EnemyID.pink_goblin)
		{
			return EnemyDatabase.Instance.enemyProperties[1];
		}
		if (id == EnemyID.blob_runner)
		{
			return EnemyDatabase.Instance.enemyProperties[3];
		}
		if (id == EnemyID.lobber)
		{
			return EnemyDatabase.Instance.enemyProperties[4];
		}
		if (id == EnemyID.flower_grunt)
		{
			return EnemyDatabase.Instance.enemyProperties[5];
		}
		if (id == EnemyID.mushroom)
		{
			return EnemyDatabase.Instance.enemyProperties[6];
		}
		if (id == EnemyID.chomper)
		{
			return EnemyDatabase.Instance.enemyProperties[7];
		}
		if (id == EnemyID.acorn)
		{
			return EnemyDatabase.Instance.enemyProperties[8];
		}
		if (id == EnemyID.spiker)
		{
			return EnemyDatabase.Instance.enemyProperties[10];
		}
		if (id == EnemyID.miner)
		{
			return EnemyDatabase.Instance.enemyProperties[28];
		}
		if (id == EnemyID.fan)
		{
			return EnemyDatabase.Instance.enemyProperties[29];
		}
		if (id == EnemyID.wind)
		{
			return EnemyDatabase.Instance.enemyProperties[2];
		}
		if (id == EnemyID.lobster)
		{
			return EnemyDatabase.Instance.enemyProperties[16];
		}
		if (id == EnemyID.barnacle)
		{
			return EnemyDatabase.Instance.enemyProperties[17];
		}
		if (id == EnemyID.urchin)
		{
			return EnemyDatabase.Instance.enemyProperties[18];
		}
		if (id == EnemyID.crab)
		{
			return EnemyDatabase.Instance.enemyProperties[19];
		}
		if (id == EnemyID.krill)
		{
			return EnemyDatabase.Instance.enemyProperties[20];
		}
		if (id == EnemyID.clam)
		{
			return EnemyDatabase.Instance.enemyProperties[21];
		}
		if (id == EnemyID.starfish)
		{
			return EnemyDatabase.Instance.enemyProperties[22];
		}
		if (id == EnemyID.ladybug)
		{
			return EnemyDatabase.Instance.enemyProperties[11];
		}
		if (id == EnemyID.dragonfly)
		{
			return EnemyDatabase.Instance.enemyProperties[12];
		}
		if (id == EnemyID.woodpecker)
		{
			return EnemyDatabase.Instance.enemyProperties[14];
		}
		if (id == EnemyID.acornmaker)
		{
			return EnemyDatabase.Instance.enemyProperties[9];
		}
		if (id == EnemyID.beetle)
		{
			return EnemyDatabase.Instance.enemyProperties[15];
		}
		if (id == EnemyID.dragonflyshot)
		{
			return EnemyDatabase.Instance.enemyProperties[13];
		}
		if (id == EnemyID.flyingfish)
		{
			return EnemyDatabase.Instance.enemyProperties[23];
		}
		if (id == EnemyID.satyr)
		{
			return EnemyDatabase.Instance.enemyProperties[24];
		}
		if (id == EnemyID.mudman)
		{
			return EnemyDatabase.Instance.enemyProperties[25];
		}
		if (id == EnemyID.smallmudman)
		{
			return EnemyDatabase.Instance.enemyProperties[26];
		}
		if (id == EnemyID.dragon)
		{
			return EnemyDatabase.Instance.enemyProperties[27];
		}
		if (id == EnemyID.wall)
		{
			return EnemyDatabase.Instance.enemyProperties[31];
		}
		if (id == EnemyID.flamer)
		{
			return EnemyDatabase.Instance.enemyProperties[30];
		}
		if (id == EnemyID.funhousewall)
		{
			return EnemyDatabase.Instance.enemyProperties[32];
		}
		if (id == EnemyID.funwall2)
		{
			return EnemyDatabase.Instance.enemyProperties[33];
		}
		if (id == EnemyID.rocket)
		{
			return EnemyDatabase.Instance.enemyProperties[34];
		}
		if (id == EnemyID.jack)
		{
			return EnemyDatabase.Instance.enemyProperties[35];
		}
		if (id == EnemyID.duck)
		{
			return EnemyDatabase.Instance.enemyProperties[36];
		}
		if (id == EnemyID.jackinbox)
		{
			return EnemyDatabase.Instance.enemyProperties[38];
		}
		if (id == EnemyID.tuba)
		{
			return EnemyDatabase.Instance.enemyProperties[39];
		}
		if (id == EnemyID.starcannon)
		{
			return EnemyDatabase.Instance.enemyProperties[40];
		}
		if (id == EnemyID.miniduck)
		{
			return EnemyDatabase.Instance.enemyProperties[37];
		}
		if (id == EnemyID.balloon)
		{
			return EnemyDatabase.Instance.enemyProperties[41];
		}
		if (id == EnemyID.pretzel)
		{
			return EnemyDatabase.Instance.enemyProperties[42];
		}
		if (id == EnemyID.arcade)
		{
			return EnemyDatabase.Instance.enemyProperties[43];
		}
		if (id == EnemyID.ballrunner)
		{
			return EnemyDatabase.Instance.enemyProperties[44];
		}
		if (id == EnemyID.magician)
		{
			return EnemyDatabase.Instance.enemyProperties[45];
		}
		if (id == EnemyID.polebot)
		{
			return EnemyDatabase.Instance.enemyProperties[46];
		}
		if (id == EnemyID.log)
		{
			return EnemyDatabase.Instance.enemyProperties[47];
		}
		if (id != EnemyID.hotdog)
		{
			return EnemyDatabase.defaultProperties;
		}
		return EnemyDatabase.Instance.enemyProperties[48];
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x000025FB File Offset: 0x000009FB
	public static EnemyDatabase Instance
	{
		get
		{
			if (EnemyDatabase._instance == null)
			{
				EnemyDatabase._instance = Resources.Load<EnemyDatabase>("EnemyDatabase/data_enemies");
			}
			return EnemyDatabase._instance;
		}
	}

	// Token: 0x04000001 RID: 1
	private static EnemyProperties defaultProperties = new EnemyProperties();

	// Token: 0x04000002 RID: 2
	public const string PATH = "EnemyDatabase/data_enemies";

	// Token: 0x04000003 RID: 3
	private static EnemyDatabase _instance;

	// Token: 0x04000004 RID: 4
	public List<EnemyProperties> enemyProperties;

	// Token: 0x04000005 RID: 5
	public int index;
}
