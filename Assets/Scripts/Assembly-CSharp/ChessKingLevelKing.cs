using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200053C RID: 1340
public class ChessKingLevelKing : LevelProperties.ChessKing.Entity
{
	// Token: 0x17000334 RID: 820
	// (get) Token: 0x0600185C RID: 6236 RVA: 0x000DC856 File Offset: 0x000DAC56
	// (set) Token: 0x0600185D RID: 6237 RVA: 0x000DC85E File Offset: 0x000DAC5E
	public bool GOT_PARRIED { get; private set; }

	// Token: 0x0600185E RID: 6238 RVA: 0x000DC868 File Offset: 0x000DAC68
	public void StartGame()
	{
		this.rats = new List<ChessKingLevelRat>();
		base.StartCoroutine(this.timer_cr());
		LevelProperties.ChessKing.King king = base.properties.CurrentState.king;
		this.trialPoolMainIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.king.trialPool.Length);
		this.kingAttackStringMainIndex = UnityEngine.Random.Range(0, king.kingAttackString.Length);
		string[] array = king.kingAttackString[this.kingAttackStringMainIndex].Split(new char[]
		{
			','
		});
		this.kingAttackStringIndex = UnityEngine.Random.Range(0, array.Length);
		base.StartCoroutine(this.create_trial_cr());
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x000DC90F File Offset: 0x000DAD0F
	private void Update()
	{
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x000DC911 File Offset: 0x000DAD11
	public override void LevelInit(LevelProperties.ChessKing properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x000DC91C File Offset: 0x000DAD1C
	public void StateChange()
	{
		LevelProperties.ChessKing.King king = base.properties.CurrentState.king;
		this.trialPoolMainIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.king.trialPool.Length);
		this.kingAttackStringMainIndex = UnityEngine.Random.Range(0, king.kingAttackString.Length);
		string[] array = king.kingAttackString[this.kingAttackStringMainIndex].Split(new char[]
		{
			','
		});
		this.kingAttackStringIndex = UnityEngine.Random.Range(0, array.Length);
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x000DC99E File Offset: 0x000DAD9E
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this.GOT_PARRIED = true;
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x000DC9D2 File Offset: 0x000DADD2
	public void BecomeParryable()
	{
		this.GOT_PARRIED = false;
		base.animator.Play("Parryable");
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x000DC9EC File Offset: 0x000DADEC
	private IEnumerator create_trial_cr()
	{
		this.parryPoints = new List<ChessKingLevelParryPoint>();
		LevelProperties.ChessKing.King p = base.properties.CurrentState.king;
		string[] trial = p.trialPool[this.trialPoolMainIndex].Split(new char[]
		{
			','
		});
		this.GOT_PARRIED = false;
		for (int j = 0; j < trial.Length; j++)
		{
			string[] array = trial[j].Split(new char[]
			{
				':'
			});
			Vector3 dir = Vector3.zero;
			float x = 0f;
			float y = 0f;
			float amount = 0f;
			bool flag = false;
			for (int k = 0; k < array.Length; k++)
			{
				switch (k)
				{
				case 0:
					Parser.FloatTryParse(array[k], out x);
					break;
				case 1:
					Parser.FloatTryParse(array[k], out y);
					break;
				case 2:
					flag = true;
					dir = this.GetDir(array[k]);
					break;
				case 3:
					Parser.FloatTryParse(array[k], out amount);
					break;
				}
			}
			ChessKingLevelParryPoint chessKingLevelParryPoint = UnityEngine.Object.Instantiate<ChessKingLevelParryPoint>(this.parryPoint);
			Vector3 pos = new Vector3((float)Level.Current.Left, (float)Level.Current.Ground) + new Vector3(x, y);
			if (flag)
			{
				chessKingLevelParryPoint.Init(pos, dir, p.bluePointSpeed, amount);
			}
			else
			{
				chessKingLevelParryPoint.Init(pos);
			}
			this.parryPoints.Add(chessKingLevelParryPoint);
		}
		for (int i = 0; i < this.parryPoints.Count; i++)
		{
			this.parryPoints[i].Activate();
			while (!this.parryPoints[i].GOT_PARRIED)
			{
				if (this.groundTrigger.PLAYER_FALLEN)
				{
					break;
				}
				yield return null;
			}
			if (!this.challengeActivated)
			{
				this.groundTrigger.CheckPlayer(true);
				this.MoveBluePoints();
				this.challengeActivated = true;
			}
		}
		this.EndChallenge();
		yield return null;
		yield break;
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x000DCA07 File Offset: 0x000DAE07
	private void EndChallenge()
	{
		base.StartCoroutine(this.end_challenge());
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x000DCA18 File Offset: 0x000DAE18
	private IEnumerator end_challenge()
	{
		if (!this.groundTrigger.PLAYER_FALLEN)
		{
			this.BecomeParryable();
			while (!this.GOT_PARRIED)
			{
				if (this.groundTrigger.PLAYER_FALLEN)
				{
					break;
				}
				yield return null;
			}
		}
		this.challengeActivated = false;
		base.animator.Play("Idle");
		this.groundTrigger.CheckPlayer(false);
		this.ClearPoints();
		if (!this.GOT_PARRIED)
		{
			this.Attack();
		}
		while (this.isAttacking)
		{
			yield return null;
		}
		LevelPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
		while (!player.motor.Grounded)
		{
			yield return null;
		}
		this.trialPoolMainIndex = (this.trialPoolMainIndex + 1) % base.properties.CurrentState.king.trialPool.Length;
		base.StartCoroutine(this.create_trial_cr());
		yield break;
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x000DCA34 File Offset: 0x000DAE34
	private void ClearPoints()
	{
		for (int i = 0; i < this.parryPoints.Count; i++)
		{
			UnityEngine.Object.Destroy(this.parryPoints[i].gameObject);
		}
		this.parryPoints.Clear();
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x000DCA80 File Offset: 0x000DAE80
	private void MoveBluePoints()
	{
		for (int i = 0; i < this.parryPoints.Count; i++)
		{
			this.parryPoints[i].MovePoint();
		}
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x000DCABC File Offset: 0x000DAEBC
	private IEnumerator timer_cr()
	{
		float t = 0f;
		float time = base.properties.CurrentState.king.kingAttackTimer;
		for (;;)
		{
			if (!this.challengeActivated && !this.isAttacking)
			{
				if (t < time)
				{
					t += CupheadTime.Delta;
				}
				else
				{
					this.Attack();
					t = 0f;
				}
			}
			else
			{
				t = 0f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x000DCAD8 File Offset: 0x000DAED8
	private Vector3 GetDir(string part)
	{
		if (part[0] == 'R')
		{
			return Vector3.right;
		}
		if (part[0] == 'L')
		{
			return Vector3.left;
		}
		if (part[0] == 'U')
		{
			return Vector3.up;
		}
		return Vector3.down;
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x000DCB28 File Offset: 0x000DAF28
	private void Attack()
	{
		this.isAttacking = true;
		LevelProperties.ChessKing.King king = base.properties.CurrentState.king;
		string[] array = king.kingAttackString[this.kingAttackStringMainIndex].Split(new char[]
		{
			','
		});
		string text = array[this.kingAttackStringIndex];
		if (text != null)
		{
			if (!(text == "F"))
			{
				if (!(text == "B"))
				{
					if (text == "R")
					{
						base.StartCoroutine(this.rat_attack_cr());
					}
				}
				else
				{
					base.StartCoroutine(this.beam_attack_cr());
				}
			}
			else
			{
				base.StartCoroutine(this.full_screen_attack_cr());
			}
		}
		if (this.kingAttackStringIndex < array.Length - 1)
		{
			this.kingAttackStringIndex++;
		}
		else
		{
			this.kingAttackStringMainIndex = (this.kingAttackStringMainIndex + 1) % king.kingAttackString.Length;
			this.kingAttackStringIndex = 0;
		}
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x000DCC28 File Offset: 0x000DB028
	private IEnumerator full_screen_attack_cr()
	{
		LevelProperties.ChessKing.Full p = base.properties.CurrentState.full;
		base.animator.SetBool("isAnti", true);
		yield return CupheadTime.WaitForSeconds(this, p.fullAttackAnti);
		this.fullAttack.SetActive(true);
		yield return CupheadTime.WaitForSeconds(this, p.fullAttackDuration);
		this.fullAttack.SetActive(false);
		base.animator.SetBool("isAnti", false);
		yield return CupheadTime.WaitForSeconds(this, p.fullAttackRecovery);
		this.isAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x000DCC44 File Offset: 0x000DB044
	private IEnumerator beam_attack_cr()
	{
		LevelProperties.ChessKing.Beam p = base.properties.CurrentState.beam;
		base.animator.SetBool("isAnti", true);
		yield return CupheadTime.WaitForSeconds(this, p.beamAttackAnti);
		this.beamAttack.SetActive(true);
		yield return CupheadTime.WaitForSeconds(this, p.beamAttackDuration);
		this.beamAttack.SetActive(false);
		base.animator.SetBool("isAnti", false);
		yield return CupheadTime.WaitForSeconds(this, p.beamAttackRecovery);
		this.isAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x000DCC60 File Offset: 0x000DB060
	private IEnumerator rat_attack_cr()
	{
		LevelProperties.ChessKing.Rat p = base.properties.CurrentState.rat;
		base.animator.SetBool("isAnti", true);
		yield return CupheadTime.WaitForSeconds(this, p.ratSummonAnti);
		if (this.rats.Count < p.maxRatNumber)
		{
			ChessKingLevelRat chessKingLevelRat = UnityEngine.Object.Instantiate<ChessKingLevelRat>(this.ratPrefab);
			chessKingLevelRat.Init(this.ratSpawn.position, p.ratSpeed);
			this.rats.Add(chessKingLevelRat);
		}
		yield return CupheadTime.WaitForSeconds(this, p.ratSummonDuration);
		base.animator.SetBool("isAnti", false);
		yield return CupheadTime.WaitForSeconds(this, p.ratSummonRecovery);
		this.isAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x0400218E RID: 8590
	public static float multiplayerDamageNerf = 8f;

	// Token: 0x0400218F RID: 8591
	private const float Y_SPAWN = -300f;

	// Token: 0x04002190 RID: 8592
	[SerializeField]
	private ChessKingLevelRat ratPrefab;

	// Token: 0x04002191 RID: 8593
	private List<ChessKingLevelRat> rats;

	// Token: 0x04002192 RID: 8594
	[SerializeField]
	private Transform ratSpawn;

	// Token: 0x04002193 RID: 8595
	[SerializeField]
	private GameObject beamAttack;

	// Token: 0x04002194 RID: 8596
	[SerializeField]
	private GameObject fullAttack;

	// Token: 0x04002195 RID: 8597
	[SerializeField]
	private ChessKingLevelGroundTrigger groundTrigger;

	// Token: 0x04002196 RID: 8598
	[SerializeField]
	private ChessKingLevelParryPoint parryPoint;

	// Token: 0x04002197 RID: 8599
	private List<ChessKingLevelParryPoint> parryPoints;

	// Token: 0x04002199 RID: 8601
	private int kingAttackStringMainIndex;

	// Token: 0x0400219A RID: 8602
	private int kingAttackStringIndex;

	// Token: 0x0400219B RID: 8603
	private int trialPoolMainIndex;

	// Token: 0x0400219C RID: 8604
	private bool challengeActivated;

	// Token: 0x0400219D RID: 8605
	private bool isAttacking;
}
