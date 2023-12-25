using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000932 RID: 2354
public class MapCastleZones : MonoBehaviour
{
	// Token: 0x06003710 RID: 14096 RVA: 0x001FB800 File Offset: 0x001F9C00
	private void OnEnable()
	{
		foreach (MapCastleZoneCollider mapCastleZoneCollider in this.zones)
		{
			mapCastleZoneCollider.OnMapCastleZoneCollision += this.onMapCastleZoneCollision;
		}
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x001FB840 File Offset: 0x001F9C40
	private void OnDisable()
	{
		foreach (MapCastleZoneCollider mapCastleZoneCollider in this.zones)
		{
			mapCastleZoneCollider.OnMapCastleZoneCollision -= this.onMapCastleZoneCollision;
		}
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x001FB87E File Offset: 0x001F9C7E
	private void showLadder(MapCastleZoneCollider zone)
	{
		this.ladder.transform.position = zone.interactionPoint.position;
		this.ladder.returnPositions = zone.returnPositions;
		base.StartCoroutine(this.showLadder_cr(zone));
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x001FB8BC File Offset: 0x001F9CBC
	private IEnumerator showLadder_cr(MapCastleZoneCollider zone)
	{
		this.ladder.animator.SetBool("Down", true);
		yield return this.ladder.animator.WaitForAnimationToStart(this, "Drop", false);
		AudioManager.Play("worldmap_kog_ladder_down");
		this.ladder.EnableShadow(zone.enableLadderShadow);
		yield return this.ladder.animator.WaitForAnimationToEnd(this, "Drop", false, true);
		this.ladder.enabled = true;
		yield break;
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x001FB8DE File Offset: 0x001F9CDE
	private void hideLadder()
	{
		base.StartCoroutine(this.hideLadder_cr());
	}

	// Token: 0x06003715 RID: 14101 RVA: 0x001FB8F0 File Offset: 0x001F9CF0
	private IEnumerator hideLadder_cr()
	{
		this.ladder.animator.SetBool("Down", false);
		yield return this.ladder.animator.WaitForAnimationToStart(this, "Up", false);
		AudioManager.Play("worldmap_kog_ladder_up");
		this.ladder.enabled = false;
		yield break;
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x001FB90C File Offset: 0x001F9D0C
	private void onMapCastleZoneCollision(MapCastleZoneCollider collider, GameObject other, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Enter)
		{
			if (this.currentZone == null)
			{
				PlayerData data = PlayerData.Data;
				if (data.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length)
				{
					if (collider.zone == MapCastleZones.Zone.Dock)
					{
						this.currentZone = collider;
					}
				}
				else if (collider.zone != MapCastleZones.Zone.Dock)
				{
					if (data.currentChessBossZone == MapCastleZones.Zone.None)
					{
						int num = data.CountLevelsCompleted(Level.worldDLCBossLevels);
						int count = data.usedChessBossZones.Count;
						if (count <= num && !data.usedChessBossZones.Contains(collider.zone))
						{
							this.currentZone = collider;
							data.currentChessBossZone = collider.zone;
							PlayerData.SaveCurrentFile();
						}
					}
					else if (data.currentChessBossZone == collider.zone)
					{
						this.currentZone = collider;
					}
				}
				if (this.currentZone != null)
				{
					this.showLadder(this.currentZone);
				}
			}
			if (collider == this.currentZone)
			{
				this.currentZonePlayerCount++;
			}
		}
		else if (phase == CollisionPhase.Exit && collider == this.currentZone)
		{
			this.currentZonePlayerCount--;
			if (this.currentZonePlayerCount == 0)
			{
				this.currentZone = null;
				this.hideLadder();
			}
		}
	}

	// Token: 0x04003F3F RID: 16191
	private static readonly MapCastleZones.Zone[] RegularZones = new MapCastleZones.Zone[]
	{
		MapCastleZones.Zone.OldMan,
		MapCastleZones.Zone.RumRunners,
		MapCastleZones.Zone.Cowgirl,
		MapCastleZones.Zone.DogFight,
		MapCastleZones.Zone.SnowCult
	};

	// Token: 0x04003F40 RID: 16192
	[SerializeField]
	private MapCastleZoneCollider[] zones;

	// Token: 0x04003F41 RID: 16193
	[SerializeField]
	private MapLevelLoaderLadder ladder;

	// Token: 0x04003F42 RID: 16194
	private MapCastleZoneCollider currentZone;

	// Token: 0x04003F43 RID: 16195
	private int currentZonePlayerCount;

	// Token: 0x02000933 RID: 2355
	public enum Zone
	{
		// Token: 0x04003F45 RID: 16197
		None,
		// Token: 0x04003F46 RID: 16198
		OldMan,
		// Token: 0x04003F47 RID: 16199
		RumRunners,
		// Token: 0x04003F48 RID: 16200
		Cowgirl,
		// Token: 0x04003F49 RID: 16201
		DogFight,
		// Token: 0x04003F4A RID: 16202
		SnowCult,
		// Token: 0x04003F4B RID: 16203
		Dock
	}
}
