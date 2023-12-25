using System;
using System.Collections;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000989 RID: 2441
public class MapEquipUICard : AbstractMonoBehaviour
{
	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06003914 RID: 14612 RVA: 0x00206070 File Offset: 0x00204470
	// (set) Token: 0x06003915 RID: 14613 RVA: 0x00206078 File Offset: 0x00204478
	public bool ReadyAndWaiting { get; private set; }

	// Token: 0x06003916 RID: 14614 RVA: 0x00206081 File Offset: 0x00204481
	private void Start()
	{
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeft;
	}

	// Token: 0x06003917 RID: 14615 RVA: 0x002060A5 File Offset: 0x002044A5
	private void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeft;
	}

	// Token: 0x06003918 RID: 14616 RVA: 0x002060C9 File Offset: 0x002044C9
	private void Update()
	{
		this.HandleInput();
	}

	// Token: 0x06003919 RID: 14617 RVA: 0x002060D4 File Offset: 0x002044D4
	private void HandleInput()
	{
		if (this.playerInput == null || !this.inputEnabled || this.equipUI.CurrentState != AbstractEquipUI.ActiveState.Active || InterruptingPrompt.IsInterrupting())
		{
			return;
		}
		MapEquipUICard.Side side = this.side;
		if (side != MapEquipUICard.Side.Front)
		{
			if (side == MapEquipUICard.Side.Back)
			{
				switch (this.back)
				{
				case MapEquipUICard.Back.Select:
					this.HandleInputBackSelect();
					break;
				case MapEquipUICard.Back.Ready:
					this.HandleInputBackReady();
					break;
				case MapEquipUICard.Back.Checklist:
					this.HandleInputChecklistReady();
					break;
				}
			}
		}
		else
		{
			this.HandleInputFront();
		}
	}

	// Token: 0x0600391A RID: 14618 RVA: 0x00206188 File Offset: 0x00204588
	private void HandleInputFront()
	{
		if (this.playerInput.GetButtonDown(14))
		{
			this.Close();
			return;
		}
		if (this.playerInput.GetButtonDown(18))
		{
			this.front.ChangeSelection(-1);
			return;
		}
		if (this.playerInput.GetButtonDown(20))
		{
			this.front.ChangeSelection(1);
			return;
		}
		if (this.front.checkListSelected)
		{
			if (this.playerInput.GetButtonDown(13))
			{
				int index = 0;
				if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
				{
					index = 1;
				}
				else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_3)
				{
					index = 2;
				}
				else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_4)
				{
					index = 3;
				}
				else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
				{
					index = 4;
				}
				this.checkList.SetCursorPosition(index, true);
				this.RotateToCheckList();
				return;
			}
		}
		else
		{
			if (this.playerInput.GetButtonDown(13))
			{
				this.RotateToBackSelect(this.front.Slot);
				return;
			}
			if (this.playerInput.GetButtonDown(15))
			{
				this.front.Unequip();
				return;
			}
		}
	}

	// Token: 0x0600391B RID: 14619 RVA: 0x002062C4 File Offset: 0x002046C4
	private void HandleInputBackSelect()
	{
		if (this.playerInput.GetButtonDown(14))
		{
			this.front.ChangeSelection(0);
			this.RotateToFront();
			return;
		}
		if (!this.backSelect.lockInput)
		{
			if (this.playerInput.GetButtonDown(15))
			{
				this.backSelect.Unequip();
				return;
			}
			if (this.playerInput.GetButtonDown(18))
			{
				this.backSelect.ChangeSelection(new Trilean2(-1, 0));
				return;
			}
			if (this.playerInput.GetButtonDown(20))
			{
				this.backSelect.ChangeSelection(new Trilean2(1, 0));
				return;
			}
			if (this.playerInput.GetButtonDown(16))
			{
				this.backSelect.ChangeSelection(new Trilean2(0, 1));
				return;
			}
			if (this.playerInput.GetButtonDown(19))
			{
				this.backSelect.ChangeSelection(new Trilean2(0, -1));
				return;
			}
			if (this.playerInput.GetButtonDown(11))
			{
				AudioManager.Play("menu_equipment_page");
				this.backSelect.ChangeSlot(1);
				return;
			}
			if (this.playerInput.GetButtonDown(12))
			{
				AudioManager.Play("menu_equipment_page");
				this.backSelect.ChangeSlot(-1);
				return;
			}
			if (this.playerInput.GetButtonDown(13))
			{
				this.backSelect.Accept();
				return;
			}
		}
	}

	// Token: 0x0600391C RID: 14620 RVA: 0x00206428 File Offset: 0x00204828
	private void HandleInputBackReady()
	{
		if (this.playerInput.GetButtonDown(13))
		{
			this.RotateToFront();
			return;
		}
		if (this.playerInput.GetButtonDown(15))
		{
			this.RotateToFront();
			return;
		}
	}

	// Token: 0x0600391D RID: 14621 RVA: 0x0020645C File Offset: 0x0020485C
	private void HandleInputChecklistReady()
	{
		if (this.playerInput.GetButtonDown(14))
		{
			this.RotateToFront();
			return;
		}
		if (this.playerInput.GetButtonDown(18))
		{
			this.checkList.ChangeSelection(-1);
			return;
		}
		if (this.playerInput.GetButtonDown(20))
		{
			this.checkList.ChangeSelection(1);
			return;
		}
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x002064C0 File Offset: 0x002048C0
	private void LateUpdate()
	{
		base.transform.localPosition = Vector2.Lerp(base.transform.localPosition, this.position, Time.deltaTime * 10f);
		base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, Quaternion.Euler(0f, 0f, this.roll), Time.deltaTime * 8f);
		if (this.rotation > 90f)
		{
			this.side = MapEquipUICard.Side.Back;
			this.SetBackActive(true);
			this.front.SetActive(false);
		}
		else
		{
			this.side = MapEquipUICard.Side.Front;
			this.SetBackActive(false);
			this.front.SetActive(true);
		}
	}

	// Token: 0x0600391F RID: 14623 RVA: 0x00206587 File Offset: 0x00204987
	private void Close()
	{
		if (!this.equipUI.Close())
		{
			this.RotateToBackReady();
		}
	}

	// Token: 0x06003920 RID: 14624 RVA: 0x002065A0 File Offset: 0x002049A0
	public void Init(PlayerId id, AbstractEquipUI equipUI)
	{
		this.playerID = id;
		this.equipUI = equipUI;
		this.playerInput = PlayerManager.GetPlayerInput(id);
		this.backSelect.transform.SetScale(new float?(-1f), null, null);
		this.backReady.transform.SetScale(new float?(-1f), null, null);
		this.checkList.transform.SetScale(new float?(-1f), null, null);
		foreach (Image image in this.cupheadImages)
		{
			image.gameObject.SetActive(false);
		}
		foreach (Image image2 in this.mugmanImages)
		{
			image2.gameObject.SetActive(false);
		}
		this.cupheadChaos.SetActive(false);
		this.mugmanChaos.SetActive(false);
		PlayerId playerId = this.playerID;
		if (playerId != PlayerId.PlayerOne)
		{
			if (playerId != PlayerId.PlayerTwo)
			{
				if (playerId != PlayerId.Any && playerId != PlayerId.None)
				{
				}
			}
			else
			{
				Image[] array3 = (!PlayerManager.player1IsMugman) ? this.mugmanImages : this.cupheadImages;
				foreach (Image image3 in array3)
				{
					image3.gameObject.SetActive(true);
				}
				GameObject gameObject = (!PlayerManager.player1IsMugman) ? this.cupheadChaos : this.mugmanChaos;
				this.cuphead2POverlay.SetActive(PlayerManager.player1IsMugman);
				this.mugman1POverlay.SetActive(false);
				gameObject.SetActive(Localization.language != Localization.Languages.English);
			}
		}
		else
		{
			Image[] array5 = (!PlayerManager.player1IsMugman) ? this.cupheadImages : this.mugmanImages;
			foreach (Image image4 in array5)
			{
				image4.gameObject.SetActive(true);
			}
			GameObject gameObject2 = (!PlayerManager.player1IsMugman) ? this.cupheadChaos : this.mugmanChaos;
			this.mugman1POverlay.SetActive(PlayerManager.player1IsMugman);
			this.cuphead2POverlay.SetActive(false);
			gameObject2.SetActive(Localization.language != Localization.Languages.English);
		}
		this.front.Init(this.playerID);
		this.backSelect.Init(this.playerID);
		this.checkList.Init(this.playerID);
	}

	// Token: 0x06003921 RID: 14625 RVA: 0x00206870 File Offset: 0x00204C70
	private void OnPlayerJoined(PlayerId playerId)
	{
		this.SetActive(true);
		if (this.playerID == PlayerId.PlayerTwo)
		{
			this.SetMultiplayerOut(true);
		}
		this.SetMultiplayerIn(false);
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x00206893 File Offset: 0x00204C93
	private void OnPlayerLeft(PlayerId playerId)
	{
		if (this.playerID == PlayerId.PlayerTwo)
		{
			this.SetMultiplayerOut(false);
			return;
		}
		this.SetSinglePlayerIn(false);
	}

	// Token: 0x06003923 RID: 14627 RVA: 0x002068B0 File Offset: 0x00204CB0
	public void SetActive(bool active)
	{
		if (base.gameObject == null)
		{
			return;
		}
		base.gameObject.SetActive(active);
	}

	// Token: 0x06003924 RID: 14628 RVA: 0x002068D0 File Offset: 0x00204CD0
	private void SetBackActive(bool active)
	{
		this.backSelect.SetActive(false);
		this.backReady.SetActive(false);
		this.checkList.SetActive(false);
		if (!active)
		{
			return;
		}
		switch (this.back)
		{
		case MapEquipUICard.Back.Select:
			this.backSelect.SetActive(active);
			break;
		case MapEquipUICard.Back.Ready:
			this.backReady.SetActive(active);
			break;
		case MapEquipUICard.Back.Checklist:
			this.checkList.SetActive(active);
			break;
		}
	}

	// Token: 0x06003925 RID: 14629 RVA: 0x0020695E File Offset: 0x00204D5E
	public void SetSinglePlayerIn(bool instant = false)
	{
		this.ResetToFront();
		this.SetPosition(Vector2.zero, instant);
		this.SetRoll(UnityEngine.Random.Range(-4f, 4f), instant);
	}

	// Token: 0x06003926 RID: 14630 RVA: 0x00206988 File Offset: 0x00204D88
	public void SetSinglePlayerOut(bool instant = false)
	{
		this.SetPosition(new Vector2(0f, -720f), instant);
		this.SetRoll(0f, instant);
	}

	// Token: 0x06003927 RID: 14631 RVA: 0x002069AC File Offset: 0x00204DAC
	public void SetMultiplayerIn(bool instant = false)
	{
		this.ResetToFront();
		Vector2 pos = new Vector2(-320f, 0f);
		if (this.playerID == PlayerId.PlayerTwo)
		{
			pos.x *= -1f;
		}
		this.SetPosition(pos, instant);
		this.SetRoll(UnityEngine.Random.Range(-4f, 4f), instant);
	}

	// Token: 0x06003928 RID: 14632 RVA: 0x00206A10 File Offset: 0x00204E10
	public void SetMultiplayerOut(bool instant = false)
	{
		Vector2 pos = new Vector2(-1280f, 0f);
		if (this.playerID == PlayerId.PlayerTwo)
		{
			pos.x *= -1f;
		}
		this.SetPosition(pos, instant);
		this.SetRoll(0f, instant);
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x00206A61 File Offset: 0x00204E61
	private void SetPosition(Vector2 pos, bool instant)
	{
		this.position = pos;
		if (instant)
		{
			base.transform.localPosition = this.position;
		}
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x00206A86 File Offset: 0x00204E86
	private void RotateToFront()
	{
		AudioManager.Play("menu_cardflip");
		this.front.Refresh();
		if (!this.CanRotate)
		{
			return;
		}
		this.ReadyAndWaiting = false;
		this.StartRotation(this.rotation, 0f);
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x00206AC1 File Offset: 0x00204EC1
	private void ResetToFront()
	{
		this.StopRotation();
		this.SetRotation(0f);
		this.ReadyAndWaiting = false;
	}

	// Token: 0x0600392C RID: 14636 RVA: 0x00206ADB File Offset: 0x00204EDB
	private void RotateToBackSelect(MapEquipUICard.Slot slot)
	{
		AudioManager.Play("menu_cardflip");
		this.backSelect.Setup(slot);
		if (!this.CanRotate)
		{
			return;
		}
		this.back = MapEquipUICard.Back.Select;
		this.StartRotation(this.rotation, 180f);
	}

	// Token: 0x0600392D RID: 14637 RVA: 0x00206B17 File Offset: 0x00204F17
	private void RotateToBackReady()
	{
		if (!this.CanRotate)
		{
			return;
		}
		this.ReadyAndWaiting = true;
		this.back = MapEquipUICard.Back.Ready;
		AudioManager.Play("menu_ready");
		this.StartRotation(this.rotation, 180f);
	}

	// Token: 0x0600392E RID: 14638 RVA: 0x00206B4E File Offset: 0x00204F4E
	private void RotateToCheckList()
	{
		AudioManager.Play("menu_cardflip");
		if (!this.CanRotate)
		{
			return;
		}
		this.back = MapEquipUICard.Back.Checklist;
		this.StartRotation(this.rotation, 180f);
	}

	// Token: 0x0600392F RID: 14639 RVA: 0x00206B7E File Offset: 0x00204F7E
	private void StartRotation(float start, float end)
	{
		this.StopRotation();
		if (!this.CanRotate)
		{
			return;
		}
		this.rotationCoroutine = this.rotate_cr(start, end, 0.15f);
		base.StartCoroutine(this.rotationCoroutine);
	}

	// Token: 0x06003930 RID: 14640 RVA: 0x00206BB2 File Offset: 0x00204FB2
	private void StopRotation()
	{
		if (this.rotationCoroutine != null)
		{
			base.StopCoroutine(this.rotationCoroutine);
		}
		this.rotationCoroutine = null;
	}

	// Token: 0x06003931 RID: 14641 RVA: 0x00206BD4 File Offset: 0x00204FD4
	private void SetRotation(float r)
	{
		this.rotation = r;
		this.container.SetLocalEulerAngles(null, new float?(this.rotation), null);
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x00206C10 File Offset: 0x00205010
	private IEnumerator rotate_cr(float start, float end, float time)
	{
		this.inputEnabled = false;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.SetRotation(EaseUtils.Ease(this.ROTATION_EASE, start, end, val));
			t += Time.deltaTime;
			yield return null;
		}
		this.SetRotation(end);
		this.inputEnabled = true;
		yield break;
	}

	// Token: 0x06003933 RID: 14643 RVA: 0x00206C40 File Offset: 0x00205040
	private void SetRoll(float r, bool instant)
	{
		this.roll = r;
		if (instant)
		{
			base.transform.SetLocalEulerAngles(null, null, new float?(r));
		}
	}

	// Token: 0x04004093 RID: 16531
	private const float POSITION_SPEED = 10f;

	// Token: 0x04004094 RID: 16532
	private const float ROLL_SPEED = 8f;

	// Token: 0x04004095 RID: 16533
	public RectTransform container;

	// Token: 0x04004096 RID: 16534
	[Header("Cards")]
	[SerializeField]
	private MapEquipUICardFront front;

	// Token: 0x04004097 RID: 16535
	[SerializeField]
	private MapEquipUICardBackSelect backSelect;

	// Token: 0x04004098 RID: 16536
	[SerializeField]
	private MapEquipUICardBackReady backReady;

	// Token: 0x04004099 RID: 16537
	[SerializeField]
	private MapEquipUIChecklist checkList;

	// Token: 0x0400409A RID: 16538
	[Header("Sprite Assets")]
	public Image[] cupheadImages;

	// Token: 0x0400409B RID: 16539
	public Image[] mugmanImages;

	// Token: 0x0400409C RID: 16540
	[SerializeField]
	private GameObject cupheadChaos;

	// Token: 0x0400409D RID: 16541
	[SerializeField]
	private GameObject mugmanChaos;

	// Token: 0x0400409E RID: 16542
	[SerializeField]
	private GameObject cuphead2POverlay;

	// Token: 0x0400409F RID: 16543
	[SerializeField]
	private GameObject mugman1POverlay;

	// Token: 0x040040A0 RID: 16544
	[NonSerialized]
	public bool CanRotate;

	// Token: 0x040040A1 RID: 16545
	private bool inputEnabled = true;

	// Token: 0x040040A2 RID: 16546
	private AbstractEquipUI equipUI;

	// Token: 0x040040A3 RID: 16547
	private MapEquipUICard.Side side;

	// Token: 0x040040A4 RID: 16548
	private MapEquipUICard.Back back;

	// Token: 0x040040A5 RID: 16549
	private Vector2 position;

	// Token: 0x040040A6 RID: 16550
	private float rotation;

	// Token: 0x040040A7 RID: 16551
	private float roll;

	// Token: 0x040040A8 RID: 16552
	private PlayerId playerID;

	// Token: 0x040040A9 RID: 16553
	private Player playerInput;

	// Token: 0x040040AB RID: 16555
	private const float ROTATION_FRONT = 0f;

	// Token: 0x040040AC RID: 16556
	private const float ROTATION_BACK = 180f;

	// Token: 0x040040AD RID: 16557
	private const float ROTATION_TIME = 0.15f;

	// Token: 0x040040AE RID: 16558
	private EaseUtils.EaseType ROTATION_EASE = EaseUtils.EaseType.easeOutBack;

	// Token: 0x040040AF RID: 16559
	private IEnumerator rotationCoroutine;

	// Token: 0x040040B0 RID: 16560
	private const float ROLL_MIN = 1f;

	// Token: 0x040040B1 RID: 16561
	private const float ROLL_MAX = 4f;

	// Token: 0x0200098A RID: 2442
	public enum Side
	{
		// Token: 0x040040B3 RID: 16563
		Front,
		// Token: 0x040040B4 RID: 16564
		Back
	}

	// Token: 0x0200098B RID: 2443
	public enum Slot
	{
		// Token: 0x040040B6 RID: 16566
		SHOT_A,
		// Token: 0x040040B7 RID: 16567
		SHOT_B,
		// Token: 0x040040B8 RID: 16568
		SUPER,
		// Token: 0x040040B9 RID: 16569
		CHARM
	}

	// Token: 0x0200098C RID: 2444
	public enum Back
	{
		// Token: 0x040040BB RID: 16571
		Select,
		// Token: 0x040040BC RID: 16572
		Ready,
		// Token: 0x040040BD RID: 16573
		Checklist
	}
}
