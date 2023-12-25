using System;
using UnityEngine;

// Token: 0x020005DC RID: 1500
public class DicePalacePachinkoLevelPlatforms : AbstractCollidableObject
{
	// Token: 0x06001DA5 RID: 7589 RVA: 0x00110824 File Offset: 0x0010EC24
	public void InitPlatforms(LevelProperties.DicePalacePachinko properties)
	{
		Vector3 vector;
		vector.y = (float)Level.Current.Ground;
		vector.x = (float)Level.Current.Left;
		vector.z = 0f;
		for (int i = 0; i < 3; i++)
		{
			int num;
			if (i != 0)
			{
				num = ((i % 2 != 0) ? 4 : 3);
			}
			else
			{
				num = 3;
			}
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = this.platformSprite.gameObject;
				Vector2 v;
				if (num == 4)
				{
					v.x = properties.CurrentState.pachinko.platformWidthFour;
				}
				else
				{
					v.x = properties.CurrentState.pachinko.platformWidthThree;
				}
				v.y = 1f;
				gameObject.transform.localScale = v;
				Vector3 position = vector;
				if (num == 3)
				{
					float num2 = (float)Level.Current.Width - gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 3.6f;
					position.x = position.x + num2 / (float)(num - 1) * (float)j + gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 1.8f;
				}
				else
				{
					position.x += (float)(Level.Current.Width / (num - 1) * j);
				}
				position.y = (float)Level.Current.Ground + Parser.FloatParse(properties.CurrentState.pachinko.platformHeights.Split(new char[]
				{
					','
				})[i]) + gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2f;
				if (j == 0)
				{
					position.x += gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
				}
				else if (j == num - 1)
				{
					position.x -= gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
				}
				GameObject gameObject2 = new GameObject();
				gameObject2.AddComponent<LevelPlatform>();
				gameObject2.GetComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * v.x, gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
				gameObject2.transform.position = position;
				UnityEngine.Object.Instantiate<GameObject>(gameObject, position, Quaternion.identity);
			}
		}
	}

	// Token: 0x04002682 RID: 9858
	private const int NUMBER_OF_ROWS = 3;

	// Token: 0x04002683 RID: 9859
	private const int MAX_NUMBER_OF_COLUMNS = 4;

	// Token: 0x04002684 RID: 9860
	[SerializeField]
	private SpriteRenderer platformSprite;
}
