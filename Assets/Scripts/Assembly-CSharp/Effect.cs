using System;
using UnityEngine;

[Serializable]
public class Effect : AbstractCollidableObject
{
	[SerializeField]
	protected bool randomRotation;
	[SerializeField]
	protected bool randomMirrorX;
	[SerializeField]
	protected bool randomMirrorY;
	public bool inUse;
	public bool removeOnEnd;
}
