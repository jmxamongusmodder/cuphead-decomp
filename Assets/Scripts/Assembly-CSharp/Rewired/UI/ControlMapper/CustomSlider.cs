using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C2B RID: 3115
	[AddComponentMenu("")]
	public class CustomSlider : Slider, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x002726A4 File Offset: 0x00270AA4
		// (set) Token: 0x06004C1A RID: 19482 RVA: 0x002726AC File Offset: 0x00270AAC
		public Sprite disabledHighlightedSprite
		{
			get
			{
				return this._disabledHighlightedSprite;
			}
			set
			{
				this._disabledHighlightedSprite = value;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x002726B5 File Offset: 0x00270AB5
		// (set) Token: 0x06004C1C RID: 19484 RVA: 0x002726BD File Offset: 0x00270ABD
		public Color disabledHighlightedColor
		{
			get
			{
				return this._disabledHighlightedColor;
			}
			set
			{
				this._disabledHighlightedColor = value;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06004C1D RID: 19485 RVA: 0x002726C6 File Offset: 0x00270AC6
		// (set) Token: 0x06004C1E RID: 19486 RVA: 0x002726CE File Offset: 0x00270ACE
		public string disabledHighlightedTrigger
		{
			get
			{
				return this._disabledHighlightedTrigger;
			}
			set
			{
				this._disabledHighlightedTrigger = value;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06004C1F RID: 19487 RVA: 0x002726D7 File Offset: 0x00270AD7
		// (set) Token: 0x06004C20 RID: 19488 RVA: 0x002726DF File Offset: 0x00270ADF
		public bool autoNavUp
		{
			get
			{
				return this._autoNavUp;
			}
			set
			{
				this._autoNavUp = value;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06004C21 RID: 19489 RVA: 0x002726E8 File Offset: 0x00270AE8
		// (set) Token: 0x06004C22 RID: 19490 RVA: 0x002726F0 File Offset: 0x00270AF0
		public bool autoNavDown
		{
			get
			{
				return this._autoNavDown;
			}
			set
			{
				this._autoNavDown = value;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06004C23 RID: 19491 RVA: 0x002726F9 File Offset: 0x00270AF9
		// (set) Token: 0x06004C24 RID: 19492 RVA: 0x00272701 File Offset: 0x00270B01
		public bool autoNavLeft
		{
			get
			{
				return this._autoNavLeft;
			}
			set
			{
				this._autoNavLeft = value;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06004C25 RID: 19493 RVA: 0x0027270A File Offset: 0x00270B0A
		// (set) Token: 0x06004C26 RID: 19494 RVA: 0x00272712 File Offset: 0x00270B12
		public bool autoNavRight
		{
			get
			{
				return this._autoNavRight;
			}
			set
			{
				this._autoNavRight = value;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06004C27 RID: 19495 RVA: 0x0027271B File Offset: 0x00270B1B
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x06004C28 RID: 19496 RVA: 0x00272728 File Offset: 0x00270B28
		// (remove) Token: 0x06004C29 RID: 19497 RVA: 0x00272760 File Offset: 0x00270B60
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event UnityAction _CancelEvent;

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x06004C2A RID: 19498 RVA: 0x00272796 File Offset: 0x00270B96
		// (remove) Token: 0x06004C2B RID: 19499 RVA: 0x0027279F File Offset: 0x00270B9F
		public event UnityAction CancelEvent
		{
			add
			{
				this._CancelEvent += value;
			}
			remove
			{
				this._CancelEvent -= value;
			}
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x002727A8 File Offset: 0x00270BA8
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x00272804 File Offset: 0x00270C04
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x00272860 File Offset: 0x00270C60
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x002728BC File Offset: 0x00270CBC
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x00272916 File Offset: 0x00270D16
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x0027294C File Offset: 0x00270D4C
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (this.isHighlightDisabled)
			{
				Color disabledHighlightedColor = this._disabledHighlightedColor;
				Sprite disabledHighlightedSprite = this._disabledHighlightedSprite;
				string disabledHighlightedTrigger = this._disabledHighlightedTrigger;
				if (base.gameObject.activeInHierarchy)
				{
					Selectable.Transition transition = base.transition;
					if (transition != Selectable.Transition.ColorTint)
					{
						if (transition != Selectable.Transition.SpriteSwap)
						{
							if (transition == Selectable.Transition.Animation)
							{
								this.TriggerAnimation(disabledHighlightedTrigger);
							}
						}
						else
						{
							this.DoSpriteSwap(disabledHighlightedSprite);
						}
					}
					else
					{
						this.StartColorTween(disabledHighlightedColor * base.colors.colorMultiplier, instant);
					}
				}
			}
			else
			{
				base.DoStateTransition(state, instant);
			}
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x002729F0 File Offset: 0x00270DF0
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, (!instant) ? base.colors.fadeDuration : 0f, true, true);
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x00272A3B File Offset: 0x00270E3B
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x00272A5C File Offset: 0x00270E5C
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x00272AD9 File Offset: 0x00270ED9
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x00272AE9 File Offset: 0x00270EE9
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00272AFC File Offset: 0x00270EFC
		private void EvaluateHightlightDisabled(bool isSelected)
		{
			if (!isSelected)
			{
				if (this.isHighlightDisabled)
				{
					this.isHighlightDisabled = false;
					Selectable.SelectionState state = (!this.isDisabled) ? base.currentSelectionState : Selectable.SelectionState.Disabled;
					this.DoStateTransition(state, false);
				}
			}
			else
			{
				if (!this.isDisabled)
				{
					return;
				}
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
			}
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x00272B61 File Offset: 0x00270F61
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x040050BD RID: 20669
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x040050BE RID: 20670
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x040050BF RID: 20671
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x040050C0 RID: 20672
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x040050C1 RID: 20673
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x040050C2 RID: 20674
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x040050C3 RID: 20675
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x040050C4 RID: 20676
		private bool isHighlightDisabled;
	}
}
