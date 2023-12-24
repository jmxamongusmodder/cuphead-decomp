using System.Collections.Generic;
using UnityEngine;

public class FlyingMermaidLevelBackgroundChange : AbstractPausableComponent
{
	public List<Transform> points;
	public float speed;
	[SerializeField]
	private FlyingMermaidLevelCoralCluster toCopy;
}
