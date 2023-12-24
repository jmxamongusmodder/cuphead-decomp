using UnityEngine;
using System.Collections.Generic;

public class Path2D : MonoBehaviour
{
	public enum Space
	{
		Global = 0,
		Local = 1,
	}

	public Space space;
	public List<Vector2> nodes;
}
