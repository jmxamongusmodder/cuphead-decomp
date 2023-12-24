using System;
using UnityEngine;

[Serializable]
public class OldManLevelPlatform
{
	public Transform platform;
	public Transform sockBulletPos;
	public bool isMoving;
	public bool removed;
	public float effectiveVel;
	public OldManLevelGnomeClimber activeClimber;
}
