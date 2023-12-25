using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x02000C51 RID: 3153
	[AddComponentMenu("Event/Rewired Standalone Input Module")]
	public class RewiredStandaloneInputModule : PointerInputModule
	{
		// Token: 0x06004D9E RID: 19870 RVA: 0x002763B8 File Offset: 0x002747B8
		protected RewiredStandaloneInputModule()
		{
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x00276423 File Offset: 0x00274823
		// (set) Token: 0x06004DA0 RID: 19872 RVA: 0x0027642C File Offset: 0x0027482C
		public bool UseAllRewiredGamePlayers
		{
			get
			{
				return this.useAllRewiredGamePlayers;
			}
			set
			{
				bool flag = value != this.useAllRewiredGamePlayers;
				this.useAllRewiredGamePlayers = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06004DA1 RID: 19873 RVA: 0x00276459 File Offset: 0x00274859
		// (set) Token: 0x06004DA2 RID: 19874 RVA: 0x00276464 File Offset: 0x00274864
		public bool UseRewiredSystemPlayer
		{
			get
			{
				return this.useRewiredSystemPlayer;
			}
			set
			{
				bool flag = value != this.useRewiredSystemPlayer;
				this.useRewiredSystemPlayer = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x00276491 File Offset: 0x00274891
		// (set) Token: 0x06004DA4 RID: 19876 RVA: 0x002764A3 File Offset: 0x002748A3
		public int[] RewiredPlayerIds
		{
			get
			{
				return (int[])this.rewiredPlayerIds.Clone();
			}
			set
			{
				this.rewiredPlayerIds = ((value == null) ? new int[0] : ((int[])value.Clone()));
				this.SetupRewiredVars();
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06004DA5 RID: 19877 RVA: 0x002764CD File Offset: 0x002748CD
		// (set) Token: 0x06004DA6 RID: 19878 RVA: 0x002764D5 File Offset: 0x002748D5
		public bool UsePlayingPlayersOnly
		{
			get
			{
				return this.usePlayingPlayersOnly;
			}
			set
			{
				this.usePlayingPlayersOnly = value;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06004DA7 RID: 19879 RVA: 0x002764DE File Offset: 0x002748DE
		// (set) Token: 0x06004DA8 RID: 19880 RVA: 0x002764E6 File Offset: 0x002748E6
		public bool MoveOneElementPerAxisPress
		{
			get
			{
				return this.moveOneElementPerAxisPress;
			}
			set
			{
				this.moveOneElementPerAxisPress = value;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06004DA9 RID: 19881 RVA: 0x002764EF File Offset: 0x002748EF
		// (set) Token: 0x06004DAA RID: 19882 RVA: 0x002764F7 File Offset: 0x002748F7
		public bool allowMouseInput
		{
			get
			{
				return this.m_allowMouseInput;
			}
			set
			{
				this.m_allowMouseInput = value;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06004DAB RID: 19883 RVA: 0x00276500 File Offset: 0x00274900
		// (set) Token: 0x06004DAC RID: 19884 RVA: 0x00276508 File Offset: 0x00274908
		public bool allowMouseInputIfTouchSupported
		{
			get
			{
				return this.m_allowMouseInputIfTouchSupported;
			}
			set
			{
				this.m_allowMouseInputIfTouchSupported = value;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06004DAD RID: 19885 RVA: 0x00276511 File Offset: 0x00274911
		private bool isMouseSupported
		{
			get
			{
				return Input.mousePresent && this.m_allowMouseInput && (!this.isTouchSupported || this.m_allowMouseInputIfTouchSupported);
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06004DAE RID: 19886 RVA: 0x00276543 File Offset: 0x00274943
		// (set) Token: 0x06004DAF RID: 19887 RVA: 0x0027654B File Offset: 0x0027494B
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06004DB0 RID: 19888 RVA: 0x00276554 File Offset: 0x00274954
		// (set) Token: 0x06004DB1 RID: 19889 RVA: 0x0027655C File Offset: 0x0027495C
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x00276565 File Offset: 0x00274965
		// (set) Token: 0x06004DB3 RID: 19891 RVA: 0x0027656D File Offset: 0x0027496D
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x00276576 File Offset: 0x00274976
		// (set) Token: 0x06004DB5 RID: 19893 RVA: 0x0027657E File Offset: 0x0027497E
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x00276587 File Offset: 0x00274987
		// (set) Token: 0x06004DB7 RID: 19895 RVA: 0x0027658F File Offset: 0x0027498F
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x00276598 File Offset: 0x00274998
		// (set) Token: 0x06004DB9 RID: 19897 RVA: 0x002765A0 File Offset: 0x002749A0
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06004DBA RID: 19898 RVA: 0x002765A9 File Offset: 0x002749A9
		// (set) Token: 0x06004DBB RID: 19899 RVA: 0x002765B1 File Offset: 0x002749B1
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06004DBC RID: 19900 RVA: 0x002765BA File Offset: 0x002749BA
		// (set) Token: 0x06004DBD RID: 19901 RVA: 0x002765C2 File Offset: 0x002749C2
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x002765CC File Offset: 0x002749CC
		protected override void Awake()
		{
			base.Awake();
			this.isTouchSupported = Input.touchSupported;
			TouchInputModule component = base.GetComponent<TouchInputModule>();
			if (component != null)
			{
				component.enabled = false;
			}
			this.InitializeRewired();
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x0027660C File Offset: 0x00274A0C
		public override void UpdateModule()
		{
			this.CheckEditorRecompile();
			if (this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			if (this.isMouseSupported)
			{
				this.m_LastMousePosition = this.m_MousePosition;
				this.m_MousePosition = Input.mousePosition;
			}
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x00276674 File Offset: 0x00274A74
		public override bool IsModuleSupported()
		{
			return true;
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x00276678 File Offset: 0x00274A78
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			bool flag = this.m_ForceModuleActive;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null)
				{
					if (!this.usePlayingPlayersOnly || player.isPlaying)
					{
						flag |= player.GetButtonDown(this.m_SubmitButton);
						flag |= player.GetButtonDown(this.m_CancelButton);
						if (this.moveOneElementPerAxisPress)
						{
							flag |= (player.GetButtonDown(this.m_HorizontalAxis) || player.GetNegativeButtonDown(this.m_HorizontalAxis));
							flag |= (player.GetButtonDown(this.m_VerticalAxis) || player.GetNegativeButtonDown(this.m_VerticalAxis));
						}
						else
						{
							flag |= !Mathf.Approximately(player.GetAxisRaw(this.m_HorizontalAxis), 0f);
							flag |= !Mathf.Approximately(player.GetAxisRaw(this.m_VerticalAxis), 0f);
						}
					}
				}
			}
			if (this.isMouseSupported)
			{
				flag |= ((this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f);
				flag |= Input.GetMouseButtonDown(0);
			}
			if (this.isTouchSupported)
			{
				for (int j = 0; j < Input.touchCount; j++)
				{
					Touch touch = Input.GetTouch(j);
					flag |= (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary);
				}
			}
			return flag;
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x00276840 File Offset: 0x00274C40
		public override void ActivateModule()
		{
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			base.ActivateModule();
			if (this.isMouseSupported)
			{
				Vector2 vector = Input.mousePosition;
				this.m_MousePosition = vector;
				this.m_LastMousePosition = vector;
			}
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x002768C4 File Offset: 0x00274CC4
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x002768D4 File Offset: 0x00274CD4
		public override void Process()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			if (!this.ProcessTouchEvents() && this.isMouseSupported)
			{
				this.ProcessMouseEvent();
			}
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x00276954 File Offset: 0x00274D54
		private bool ProcessTouchEvents()
		{
			if (!this.isTouchSupported)
			{
				return false;
			}
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					bool pressed;
					bool flag;
					PointerEventData touchPointerEventData = base.GetTouchPointerEventData(touch, out pressed, out flag);
					this.ProcessTouchPress(touchPointerEventData, pressed, flag);
					if (!flag)
					{
						this.ProcessMove(touchPointerEventData);
						this.ProcessDrag(touchPointerEventData);
					}
					else
					{
						base.RemovePointerData(touchPointerEventData);
					}
				}
			}
			return Input.touchCount > 0;
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x002769E0 File Offset: 0x00274DE0
		private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					base.HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					float num = unscaledTime - pointerEvent.clickTime;
					if (num < 0.3f)
					{
						pointerEvent.clickCount++;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x00276C34 File Offset: 0x00275034
		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null)
				{
					if (!this.usePlayingPlayersOnly || player.isPlaying)
					{
						if (player.GetButtonDown(this.m_SubmitButton))
						{
							ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
							break;
						}
						if (player.GetButtonDown(this.m_CancelButton))
						{
							ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
							break;
						}
					}
				}
			}
			return baseEventData.used;
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x00276D20 File Offset: 0x00275120
		private Vector2 GetRawMoveVector()
		{
			if (this.recompiling)
			{
				return Vector2.zero;
			}
			Vector2 zero = Vector2.zero;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null)
				{
					if (!this.usePlayingPlayersOnly || player.isPlaying)
					{
						if (this.moveOneElementPerAxisPress)
						{
							float num = 0f;
							if (player.GetButtonDown(this.m_HorizontalAxis))
							{
								num = 1f;
							}
							else if (player.GetNegativeButtonDown(this.m_HorizontalAxis))
							{
								num = -1f;
							}
							float num2 = 0f;
							if (player.GetButtonDown(this.m_VerticalAxis))
							{
								num2 = 1f;
							}
							else if (player.GetNegativeButtonDown(this.m_VerticalAxis))
							{
								num2 = -1f;
							}
							zero.x += num;
							zero.y += num2;
						}
						else
						{
							zero.x += player.GetAxisRaw(this.m_HorizontalAxis);
							zero.y += player.GetAxisRaw(this.m_VerticalAxis);
						}
						flag |= (player.GetButtonDown(this.m_HorizontalAxis) || player.GetNegativeButtonDown(this.m_HorizontalAxis));
						flag2 |= (player.GetButtonDown(this.m_VerticalAxis) || player.GetNegativeButtonDown(this.m_VerticalAxis));
					}
				}
			}
			if (flag)
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (flag2)
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x00276F4C File Offset: 0x0027534C
		protected bool SendMoveEventToSelectedObject()
		{
			if (this.recompiling)
			{
				return false;
			}
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			bool flag2 = this.CheckButtonOrKeyMovement(unscaledTime);
			bool flag3 = flag2;
			if (!flag3)
			{
				if (this.m_RepeatDelay > 0f)
				{
					if (flag && this.m_ConsecutiveMoveCount == 1)
					{
						flag3 = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
					}
					else
					{
						flag3 = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
					}
				}
				else
				{
					flag3 = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag3)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			if (axisEventData.moveDir == MoveDirection.None)
			{
				return false;
			}
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x002770B8 File Offset: 0x002754B8
		private bool CheckButtonOrKeyMovement(float time)
		{
			bool flag = false;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null)
				{
					if (!this.usePlayingPlayersOnly || player.isPlaying)
					{
						flag |= (player.GetButtonDown(this.m_HorizontalAxis) || player.GetNegativeButtonDown(this.m_HorizontalAxis));
						flag |= (player.GetButtonDown(this.m_VerticalAxis) || player.GetNegativeButtonDown(this.m_VerticalAxis));
					}
				}
			}
			return flag;
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x00277160 File Offset: 0x00275560
		protected void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x0027716C File Offset: 0x0027556C
		protected void ProcessMouseEvent(int id)
		{
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData();
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x0027724C File Offset: 0x0027564C
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x00277298 File Offset: 0x00275698
		protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					float num = unscaledTime - buttonData.clickTime;
					if (num < 0.3f)
					{
						buttonData.clickCount++;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x002774BC File Offset: 0x002758BC
		protected virtual void OnApplicationFocus(bool hasFocus)
		{
			this.m_HasFocus = hasFocus;
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x002774C5 File Offset: 0x002758C5
		private bool ShouldIgnoreEventsOnNoFocus()
		{
			return !ReInput.isReady || ReInput.configuration.ignoreInputWhenAppNotInFocus;
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x002774DD File Offset: 0x002758DD
		private void InitializeRewired()
		{
			if (!ReInput.isReady)
			{
				global::Debug.LogError("Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?", null);
				return;
			}
			ReInput.EditorRecompileEvent += this.OnEditorRecompile;
			this.SetupRewiredVars();
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0027750C File Offset: 0x0027590C
		private void SetupRewiredVars()
		{
			if (this.useAllRewiredGamePlayers)
			{
				IList<Player> list = (!this.useRewiredSystemPlayer) ? ReInput.players.Players : ReInput.players.AllPlayers;
				this.playerIds = new int[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					this.playerIds[i] = list[i].id;
				}
			}
			else
			{
				int num = this.rewiredPlayerIds.Length + ((!this.useRewiredSystemPlayer) ? 0 : 1);
				this.playerIds = new int[num];
				for (int j = 0; j < this.rewiredPlayerIds.Length; j++)
				{
					this.playerIds[j] = ReInput.players.GetPlayer(this.rewiredPlayerIds[j]).id;
				}
				if (this.useRewiredSystemPlayer)
				{
					this.playerIds[num - 1] = ReInput.players.GetSystemPlayer().id;
				}
			}
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0027760E File Offset: 0x00275A0E
		private void CheckEditorRecompile()
		{
			if (!this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			this.recompiling = false;
			this.InitializeRewired();
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x00277634 File Offset: 0x00275A34
		private void OnEditorRecompile()
		{
			this.recompiling = true;
			this.ClearRewiredVars();
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x00277643 File Offset: 0x00275A43
		private void ClearRewiredVars()
		{
			Array.Clear(this.playerIds, 0, this.playerIds.Length);
		}

		// Token: 0x040051A9 RID: 20905
		private const string DEFAULT_ACTION_MOVE_HORIZONTAL = "UIHorizontal";

		// Token: 0x040051AA RID: 20906
		private const string DEFAULT_ACTION_MOVE_VERTICAL = "UIVertical";

		// Token: 0x040051AB RID: 20907
		private const string DEFAULT_ACTION_SUBMIT = "UISubmit";

		// Token: 0x040051AC RID: 20908
		private const string DEFAULT_ACTION_CANCEL = "UICancel";

		// Token: 0x040051AD RID: 20909
		private int[] playerIds;

		// Token: 0x040051AE RID: 20910
		private bool recompiling;

		// Token: 0x040051AF RID: 20911
		private bool isTouchSupported;

		// Token: 0x040051B0 RID: 20912
		[SerializeField]
		[Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")]
		private bool useAllRewiredGamePlayers;

		// Token: 0x040051B1 RID: 20913
		[SerializeField]
		[Tooltip("Allow the Rewired System Player to control the UI.")]
		private bool useRewiredSystemPlayer;

		// Token: 0x040051B2 RID: 20914
		[SerializeField]
		[Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")]
		private int[] rewiredPlayerIds = new int[1];

		// Token: 0x040051B3 RID: 20915
		[SerializeField]
		[Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")]
		private bool usePlayingPlayersOnly;

		// Token: 0x040051B4 RID: 20916
		[SerializeField]
		[Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
		private bool moveOneElementPerAxisPress;

		// Token: 0x040051B5 RID: 20917
		private float m_PrevActionTime;

		// Token: 0x040051B6 RID: 20918
		private Vector2 m_LastMoveVector;

		// Token: 0x040051B7 RID: 20919
		private int m_ConsecutiveMoveCount;

		// Token: 0x040051B8 RID: 20920
		private Vector2 m_LastMousePosition;

		// Token: 0x040051B9 RID: 20921
		private Vector2 m_MousePosition;

		// Token: 0x040051BA RID: 20922
		private bool m_HasFocus = true;

		// Token: 0x040051BB RID: 20923
		[SerializeField]
		private string m_HorizontalAxis = "UIHorizontal";

		// Token: 0x040051BC RID: 20924
		[SerializeField]
		[Tooltip("Name of the vertical axis for movement (if axis events are used).")]
		private string m_VerticalAxis = "UIVertical";

		// Token: 0x040051BD RID: 20925
		[SerializeField]
		[Tooltip("Name of the action used to submit.")]
		private string m_SubmitButton = "UISubmit";

		// Token: 0x040051BE RID: 20926
		[SerializeField]
		[Tooltip("Name of the action used to cancel.")]
		private string m_CancelButton = "UICancel";

		// Token: 0x040051BF RID: 20927
		[SerializeField]
		[Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x040051C0 RID: 20928
		[SerializeField]
		[Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
		private float m_RepeatDelay;

		// Token: 0x040051C1 RID: 20929
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements.")]
		private bool m_allowMouseInput = true;

		// Token: 0x040051C2 RID: 20930
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
		private bool m_allowMouseInputIfTouchSupported = true;

		// Token: 0x040051C3 RID: 20931
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		[Tooltip("Forces the module to always be active.")]
		private bool m_ForceModuleActive;
	}
}
