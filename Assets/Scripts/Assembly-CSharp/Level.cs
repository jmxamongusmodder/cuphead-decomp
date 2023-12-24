using System;
using UnityEngine;

public class Level : AbstractPausableComponent
{
	[Serializable]
	public class IntroProperties
	{
	}

	[Serializable]
	public class Spawns
	{
		public Vector2 playerOne;
		public Vector2 playerTwo;
		public Vector2 playerOneSingle;
	}

	[Serializable]
	public class Bounds
	{
		public int left;
		public int right;
		public int top;
		public int bottom;
		public bool topEnabled;
		public bool bottomEnabled;
		public bool leftEnabled;
		public bool rightEnabled;
	}

	[Serializable]
	public class Camera
	{
		public Camera(CupheadLevelCamera.Mode mode, int left, int right, int top, int bottom)
		{
		}

		public CupheadLevelCamera.Mode mode;
		public float zoom;
		public bool moveX;
		public bool moveY;
		public bool stabilizeY;
		public float stabilizePaddingTop;
		public float stabilizePaddingBottom;
		public bool colliders;
		public Level.Bounds bounds;
		public VectorPath path;
		public bool pathMovesOnlyForward;
	}

	public class GoalTimes
	{
		public GoalTimes(float easy, float normal, float hard)
		{
		}

	}

	public enum Type
	{
		Battle = 0,
		Tutorial = 1,
		Platforming = 2,
	}

	public enum Mode
	{
		Easy = 0,
		Normal = 1,
		Hard = 2,
	}

	public LevelResources LevelResources;
	[SerializeField]
	protected Type type;
	[SerializeField]
	public PlayerMode playerMode;
	[SerializeField]
	protected bool allowMultiplayer;
	[SerializeField]
	public bool blockChalice;
	[SerializeField]
	protected IntroProperties intro;
	[SerializeField]
	protected Spawns spawns;
	[SerializeField]
	protected Bounds bounds;
	public int playerShadowSortingOrder;
	[SerializeField]
	protected Camera camera;
	public int BGMPlaylistCurrent;
}
