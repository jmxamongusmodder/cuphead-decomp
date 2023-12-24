using System;

public class MouseLevelBackgroundHopper : AbstractMonoBehaviour
{
	[Serializable]
	public class Hop
	{
		public float height;
		public float time;
	}

	public Hop[] hops;
}
