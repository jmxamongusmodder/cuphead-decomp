using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VectorPath
{
	[SerializeField]
	private List<Vector3> _points;
	[SerializeField]
	private bool _closed;
}
