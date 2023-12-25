using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005CA RID: 1482
public class DicePalaceFlyingMemoryLevelGameManager : LevelProperties.DicePalaceFlyingMemory.Entity
{
	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x0010974C File Offset: 0x00107B4C
	public static DicePalaceFlyingMemoryLevelGameManager Instance
	{
		get
		{
			if (DicePalaceFlyingMemoryLevelGameManager.singletonGameManager == null)
			{
				DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = new GameObject
				{
					name = "GameManager"
				}.AddComponent<DicePalaceFlyingMemoryLevelGameManager>();
			}
			return DicePalaceFlyingMemoryLevelGameManager.singletonGameManager;
		}
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x0010978A File Offset: 0x00107B8A
	protected override void Awake()
	{
		base.Awake();
		this.hiddenPosition = base.transform.position;
		DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = this;
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x001097AC File Offset: 0x00107BAC
	public override void LevelInit(LevelProperties.DicePalaceFlyingMemory properties)
	{
		base.LevelInit(properties);
		this.patternOrder = properties.CurrentState.flippyCard.patternOrder.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.maxHP = properties.CurrentHealth;
		this.contactDimX = this.GridDimX + 1;
		this.contactDimY = this.GridDimY + 1;
		this.GenerateGrid();
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x00109819 File Offset: 0x00107C19
	private void Update()
	{
		if (this.checkForFlipped)
		{
			this.CheckIfFlipped();
		}
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x0010982C File Offset: 0x00107C2C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.contactPointPrefab = null;
		this.cardPrefab = null;
		this.botPrefab = null;
		DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = null;
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x00109850 File Offset: 0x00107C50
	private void GenerateGrid()
	{
		float x = this.cardPrefab.GetComponent<Renderer>().bounds.size.x;
		float y = this.cardPrefab.GetComponent<Renderer>().bounds.size.y;
		float num = x + 10f;
		float num2 = y + 10f;
		this.cards = new DicePalaceFlyingMemoryLevelCard[this.GridDimX, this.GridDimY];
		this.contactPoints = new DicePalaceFlyingMemoryLevelContactPoint[this.contactDimX, this.contactDimY];
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				Vector3 a = new Vector3((float)j * num, (float)(-(float)i) * num2);
				this.cards[j, i] = UnityEngine.Object.Instantiate<DicePalaceFlyingMemoryLevelCard>(this.cardPrefab);
				this.cards[j, i].transform.position = a + base.transform.position;
				this.cards[j, i].transform.parent = base.transform;
				this.AssignCards(j, i);
			}
		}
		for (int k = 0; k < this.contactDimY; k++)
		{
			for (int l = 0; l < this.contactDimX; l++)
			{
				Vector3 a2 = new Vector3((float)l * num - num / 2f, (float)(-(float)k) * num2 + num2 / 2f);
				this.contactPoints[l, k] = UnityEngine.Object.Instantiate<DicePalaceFlyingMemoryLevelContactPoint>(this.contactPointPrefab);
				this.contactPoints[l, k].transform.position = a2 + base.transform.position;
				this.contactPoints[l, k].transform.parent = base.transform;
				this.contactPoints[l, k].Xcoord = l;
				this.contactPoints[l, k].Ycoord = k;
			}
		}
		base.StartCoroutine(this.start_game_cr());
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x00109A94 File Offset: 0x00107E94
	private void AssignCards(int x, int y)
	{
		DicePalaceFlyingMemoryLevelCard.Card card = DicePalaceFlyingMemoryLevelCard.Card.Flowers;
		if (this.patternOrder[this.patternOrderIndex] == "1A")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Cuphead;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "1B")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Chips;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "2A")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Flowers;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "2B")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Shield;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "3A")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Spindle;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "3B")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Mugman;
		}
		int index = UnityEngine.Random.Range(0, this.chosenFlippedDownCards.Count);
		this.cards[x, y].card = card;
		this.cards[x, y].GetComponent<SpriteRenderer>().sprite = this.chosenFlippedDownCards[index];
		this.chosenFlippedDownCards.Remove(this.chosenFlippedDownCards[index]);
		this.patternOrderIndex = (this.patternOrderIndex + 1) % this.patternOrder.Length;
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x00109BEC File Offset: 0x00107FEC
	private IEnumerator start_game_cr()
	{
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				this.cards[j, i].FlipUp();
			}
		}
		float t = 0f;
		float time = 1.3f;
		Vector2 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.cardStopRoot.position, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.cardStopRoot.position;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.flippyCard.initialRevealTime);
		for (int k = 0; k < this.GridDimY; k++)
		{
			for (int l = 0; l < this.GridDimX; l++)
			{
				this.cards[l, k].EnableCards();
			}
		}
		this.checkForFlipped = true;
		if (base.properties.CurrentState.bots.botsOn)
		{
			base.StartCoroutine(this.spawning_bots_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x00109C08 File Offset: 0x00108008
	private IEnumerator slide_cr(Vector3 endPosition)
	{
		float t = 0f;
		float time = 1.3f;
		Vector2 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPosition, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = endPosition;
		yield break;
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x00109C2C File Offset: 0x0010802C
	private void CheckIfFlipped()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				if (this.cards[j, i].flippedUp && !this.cards[j, i].permanentlyFlipped)
				{
					num++;
					this.cards[j, i].GetComponent<Collider2D>().enabled = false;
					if (num >= 2)
					{
						this.checkForFlipped = false;
						if (this.cards[num2, num3].card == this.cards[j, i].card)
						{
							this.matchMade = true;
							this.cards[j, i].permanentlyFlipped = true;
							this.cards[num2, num3].permanentlyFlipped = true;
						}
						else
						{
							this.matchMade = false;
						}
						base.StartCoroutine(this.disable_all_cards_cr());
						num = 0;
					}
					else
					{
						num2 = j;
						num3 = i;
					}
				}
			}
		}
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x00109D4C File Offset: 0x0010814C
	private IEnumerator disable_all_cards_cr()
	{
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				if (!this.cards[j, i].permanentlyFlipped)
				{
					this.cards[j, i].DisableCard();
				}
				else
				{
					this.cards[j, i].GetComponent<Collider2D>().enabled = false;
				}
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		base.StartCoroutine(this.open_timer_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x00109D68 File Offset: 0x00108168
	private IEnumerator open_timer_cr()
	{
		float HPToLower = this.maxHP / (float)(this.GridDimX * this.GridDimY / 2);
		if (this.matchMade)
		{
			base.StartCoroutine(this.slide_cr(this.hiddenPosition));
			this.matchCounter++;
			while (this.stuffedToy.currentlyColliding)
			{
				yield return null;
			}
			this.stuffedToy.Open();
			if (this.matchCounter == this.GridDimX * this.GridDimY / 2)
			{
				yield break;
			}
			while (base.properties.CurrentHealth >= this.maxHP - HPToLower * (float)this.matchCounter)
			{
				yield return null;
			}
		}
		else
		{
			this.stuffedToy.guessedWrong = true;
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				if (!this.cards[j, i].permanentlyFlipped)
				{
					this.cards[j, i].EnableCards();
				}
			}
		}
		base.StartCoroutine(this.slide_cr(this.cardStopRoot.position));
		this.stuffedToy.Closed();
		this.checkForFlipped = true;
		yield return null;
		yield break;
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x00109D84 File Offset: 0x00108184
	private void SpawnBot(int xCoord, int yCoord, bool moveOnY)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		DicePalaceFlyingMemoryLevelBot dicePalaceFlyingMemoryLevelBot = UnityEngine.Object.Instantiate<DicePalaceFlyingMemoryLevelBot>(this.botPrefab);
		dicePalaceFlyingMemoryLevelBot.Init(base.properties.CurrentState.bots, this.contactPoints[xCoord, yCoord], moveOnY, base.properties.CurrentState.bots.botsHP, next);
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x00109DE0 File Offset: 0x001081E0
	private IEnumerator spawning_bots_cr()
	{
		LevelProperties.DicePalaceFlyingMemory.Bots p = base.properties.CurrentState.bots;
		string[] spawnPattern = p.spawnOrder.GetRandom<string>().Split(new char[]
		{
			','
		});
		int number = 0;
		int Xcoord = 0;
		int Ycoord = 0;
		bool Yset = false;
		for (;;)
		{
			for (int i = 0; i < spawnPattern.Length; i++)
			{
				string[] spawnLocation = spawnPattern[i].Split(new char[]
				{
					':'
				});
				foreach (string text in spawnLocation)
				{
					if (text[0] == 'U')
					{
						Ycoord = 0;
						Yset = true;
					}
					else if (text[0] == 'D')
					{
						Ycoord = this.contactDimY - 1;
						Yset = true;
					}
					else if (text[0] == 'L')
					{
						Xcoord = 0;
						Yset = false;
					}
					else if (text[0] == 'R')
					{
						Xcoord = this.contactDimX - 1;
						Yset = false;
					}
					else
					{
						Parser.IntTryParse(text, out number);
					}
				}
				if (Yset)
				{
					Xcoord = number;
				}
				else
				{
					Ycoord = number;
				}
				this.SpawnBot(Xcoord, Ycoord, Yset);
				yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040025EB RID: 9707
	private static DicePalaceFlyingMemoryLevelGameManager singletonGameManager;

	// Token: 0x040025EC RID: 9708
	public DicePalaceFlyingMemoryLevelContactPoint[,] contactPoints;

	// Token: 0x040025ED RID: 9709
	public int contactDimX;

	// Token: 0x040025EE RID: 9710
	public int contactDimY;

	// Token: 0x040025EF RID: 9711
	[SerializeField]
	private Transform cardStopRoot;

	// Token: 0x040025F0 RID: 9712
	[SerializeField]
	private List<Sprite> chosenFlippedDownCards;

	// Token: 0x040025F1 RID: 9713
	[SerializeField]
	private DicePalaceFlyingMemoryLevelContactPoint contactPointPrefab;

	// Token: 0x040025F2 RID: 9714
	[SerializeField]
	private DicePalaceFlyingMemoryLevelStuffedToy stuffedToy;

	// Token: 0x040025F3 RID: 9715
	[SerializeField]
	private DicePalaceFlyingMemoryLevelCard cardPrefab;

	// Token: 0x040025F4 RID: 9716
	[SerializeField]
	private DicePalaceFlyingMemoryLevelBot botPrefab;

	// Token: 0x040025F5 RID: 9717
	private DicePalaceFlyingMemoryLevelCard[,] cards;

	// Token: 0x040025F6 RID: 9718
	private Vector3 hiddenPosition;

	// Token: 0x040025F7 RID: 9719
	private int GridDimX = 4;

	// Token: 0x040025F8 RID: 9720
	private int GridDimY = 3;

	// Token: 0x040025F9 RID: 9721
	private int patternOrderIndex;

	// Token: 0x040025FA RID: 9722
	private int matchCounter;

	// Token: 0x040025FB RID: 9723
	private float maxHP;

	// Token: 0x040025FC RID: 9724
	private float space;

	// Token: 0x040025FD RID: 9725
	private bool checkForFlipped;

	// Token: 0x040025FE RID: 9726
	private bool matchMade;

	// Token: 0x040025FF RID: 9727
	private string[] patternOrder;
}
