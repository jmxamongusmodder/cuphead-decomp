using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200077B RID: 1915
public class RobotLevelGem : AbstractCollidableObject
{
	// Token: 0x060029F4 RID: 10740 RVA: 0x001888D0 File Offset: 0x00186CD0
	public void InitFinalStage(RobotLevelHelihead parent, LevelProperties.Robot properties, bool isBlueGem)
	{
		this.parent = parent;
		this.properties = properties;
		RobotLevelHelihead robotLevelHelihead = this.parent;
		robotLevelHelihead.OnDeath = (Action)Delegate.Combine(robotLevelHelihead.OnDeath, new Action(this.OnDeath));
		this.rotation = 0f;
		if (this.isFirstWave)
		{
			this.bulletPrefab.CreatePool(200);
		}
		this.isBlueGem = isBlueGem;
		if (isBlueGem)
		{
			this.waveRotation = properties.CurrentState.blueGem.gemWaveRotation;
		}
		else
		{
			this.waveRotation = properties.CurrentState.redGem.gemWaveRotation;
		}
		if (this.isBlueGem)
		{
			this.pinkPattern = this.properties.CurrentState.blueGem.pinkString.Split(new char[]
			{
				','
			});
		}
		else
		{
			this.pinkPattern = this.properties.CurrentState.redGem.pinkString.Split(new char[]
			{
				','
			});
		}
		base.StartCoroutine(this.rotate_cr());
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x001889F8 File Offset: 0x00186DF8
	private IEnumerator attack_cr()
	{
		for (;;)
		{
			if (this.isBlueGem)
			{
				this.FireBullets(this.properties.CurrentState.blueGem.numberOfSpawnPoints, 0f);
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.blueGem.bulletSpawnDelay);
			}
			else
			{
				this.FireBullets(this.properties.CurrentState.redGem.numberOfSpawnPoints, 0f);
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.redGem.bulletSpawnDelay);
			}
		}
		yield break;
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x00188A14 File Offset: 0x00186E14
	private IEnumerator rotate_cr()
	{
		float rotationSpeed;
		MinMax rotationRange;
		if (this.isBlueGem)
		{
			rotationSpeed = this.properties.CurrentState.blueGem.robotRotationSpeed;
			rotationRange = this.properties.CurrentState.blueGem.gemRotationRange;
		}
		else
		{
			rotationSpeed = this.properties.CurrentState.redGem.robotRotationSpeed;
			rotationRange = this.properties.CurrentState.redGem.gemRotationRange;
		}
		for (;;)
		{
			if (this.waveRotation && (Vector3.Angle(Vector3.up, base.transform.right) > rotationRange.max || Vector3.Angle(Vector3.up, base.transform.right) < rotationRange.min))
			{
				rotationSpeed *= -1f;
			}
			this.rotation += rotationSpeed;
			base.transform.eulerAngles = Vector3.forward * this.rotation;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x00188A2F File Offset: 0x00186E2F
	public void OnAttackEnd()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x00188A37 File Offset: 0x00186E37
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bulletPrefab = null;
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x00188A46 File Offset: 0x00186E46
	private void OnDeath()
	{
		RobotLevelHelihead robotLevelHelihead = this.parent;
		robotLevelHelihead.OnDeath = (Action)Delegate.Remove(robotLevelHelihead.OnDeath, new Action(this.OnDeath));
		this.StopAllCoroutines();
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x00188A78 File Offset: 0x00186E78
	private void FireBullets(int count, float offset = 0f)
	{
		offset = this.rotation;
		for (int i = 0; i < count; i++)
		{
			float num = 360f * ((float)i / (float)count);
			if (this.isBlueGem)
			{
				this.bulletPrefab.Spawn(base.transform.position, Quaternion.Euler(new Vector3(0f, 0f, offset + num - 180f))).Init(this.properties.CurrentState.blueGem.bulletSpeed, (float)this.properties.CurrentState.blueGem.bulletSpeedAcceleration, (float)this.properties.CurrentState.blueGem.bulletSineWaveStrength, this.properties.CurrentState.blueGem.bulletWaveSpeedMultiplier, this.properties.CurrentState.blueGem.bulletLifeTime, this.isBlueGem, this.pinkPattern[this.pinkIndex][0] == 'P');
			}
			else
			{
				this.bulletPrefab.Spawn(base.transform.position, Quaternion.Euler(new Vector3(0f, 0f, offset + num - 180f))).Init(this.properties.CurrentState.redGem.bulletSpeed, (float)this.properties.CurrentState.redGem.bulletSpeedAcceleration, (float)this.properties.CurrentState.redGem.bulletSineWaveStrength, this.properties.CurrentState.redGem.bulletWaveSpeedMultiplier, this.properties.CurrentState.redGem.bulletLifeTime, this.isBlueGem, this.pinkPattern[this.pinkIndex][0] == 'P');
			}
			this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		}
	}

	// Token: 0x040032D4 RID: 13012
	[SerializeField]
	private RobotLevelGemProjectile bulletPrefab;

	// Token: 0x040032D5 RID: 13013
	private RobotLevelHelihead parent;

	// Token: 0x040032D6 RID: 13014
	private LevelProperties.Robot properties;

	// Token: 0x040032D7 RID: 13015
	private bool isFirstWave = true;

	// Token: 0x040032D8 RID: 13016
	private bool waveRotation;

	// Token: 0x040032D9 RID: 13017
	private int nextBulletIndex;

	// Token: 0x040032DA RID: 13018
	private float rotation;

	// Token: 0x040032DB RID: 13019
	private bool isBlueGem;

	// Token: 0x040032DC RID: 13020
	private string[] pinkPattern;

	// Token: 0x040032DD RID: 13021
	private int pinkIndex;
}
