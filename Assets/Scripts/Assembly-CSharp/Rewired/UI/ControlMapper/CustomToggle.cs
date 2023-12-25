using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C2C RID: 3116
	[AddComponentMenu("")]
	public class CustomToggle : Toggle, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06004C3A RID: 19514 RVA: 0x00272B9D File Offset: 0x00270F9D
		// (set) Token: 0x06004C3B RID: 19515 RVA: 0x00272BA5 File Offset: 0x00270FA5
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

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06004C3C RID: 19516 RVA: 0x00272BAE File Offset: 0x00270FAE
		// (set) Token: 0x06004C3D RID: 19517 RVA: 0x00272BB6 File Offset: 0x00270FB6
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

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06004C3E RID: 19518 RVA: 0x00272BBF File Offset: 0x00270FBF
		// (set) Token: 0x06004C3F RID: 19519 RVA: 0x00272BC7 File Offset: 0x00270FC7
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

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06004C40 RID: 19520 RVA: 0x00272BD0 File Offset: 0x00270FD0
		// (set) Token: 0x06004C41 RID: 19521 RVA: 0x00272BD8 File Offset: 0x00270FD8
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

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06004C42 RID: 19522 RVA: 0x00272BE1 File Offset: 0x00270FE1
		// (set) Token: 0x06004C43 RID: 19523 RVA: 0x00272BE9 File Offset: 0x00270FE9
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

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06004C44 RID: 19524 RVA: 0x00272BF2 File Offset: 0x00270FF2
		// (set) Token: 0x06004C45 RID: 19525 RVA: 0x00272BFA File Offset: 0x00270FFA
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

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06004C46 RID: 19526 RVA: 0x00272C03 File Offset: 0x00271003
		// (set) Token: 0x06004C47 RID: 19527 RVA: 0x00272C0B File Offset: 0x0027100B
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

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06004C48 RID: 19528 RVA: 0x00272C14 File Offset: 0x00271014
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x06004C49 RID: 19529 RVA: 0x00272C20 File Offset: 0x00271020
		// (remove) Token: 0x06004C4A RID: 19530 RVA: 0x00272C58 File Offset: 0x00271058
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event UnityAction _CancelEvent;

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x06004C4B RID: 19531 RVA: 0x00272C8E File Offset: 0x0027108E
		// (remove) Token: 0x06004C4C RID: 19532 RVA: 0x00272C97 File Offset: 0x00271097
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

		// Token: 0x06004C4D RID: 19533 RVA: 0x00272CA0 File Offset: 0x002710A0
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x00272CFC File Offset: 0x002710FC
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x00272D58 File Offset: 0x00271158
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x00272DB4 File Offset: 0x002711B4
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x00272E0E File Offset: 0x0027120E
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x00272E42 File Offset: 0x00271242
		protected override void Start()
		{
			base.Start();
			ControlMapper.OnPlayerChange += this.UpdateColors;
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x00272E5B File Offset: 0x0027125B
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.update_text_color_on_enable_cr());
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x00272E70 File Offset: 0x00271270
		protected override void OnDestroy()
		{
			ControlMapper.OnPlayerChange -= this.UpdateColors;
		}

		// Token: 0x06004C55 RID: 19541 RVA: 0x00272E84 File Offset: 0x00271284
		private IEnumerator update_text_color_on_enable_cr()
		{
			yield return new WaitForEndOfFrame();
			this.UpdateAssociatedImageColors(base.currentSelectionState);
			yield break;
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x00272E9F File Offset: 0x0027129F
		private void UpdateColors()
		{
			this.UpdateAssociatedImageColors(base.currentSelectionState);
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x00272EAD File Offset: 0x002712AD
		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			this.UpdateColors();
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x00272EBC File Offset: 0x002712BC
		public override void OnSubmit(BaseEventData eventData)
		{
			base.OnSubmit(eventData);
			this.UpdateColors();
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x00272ECC File Offset: 0x002712CC
		protected void UpdateAssociatedImageColors(Selectable.SelectionState state)
		{
			if (state == Selectable.SelectionState.Highlighted)
			{
				this.checkImage.color = this.checkOverrideColor[(!base.isOn) ? 3 : 2];
			}
			else
			{
				this.checkImage.color = this.checkOverrideColor[(!base.isOn) ? 3 : ControlMapper.CurrentPlayer()];
			}
			this.checkBoxImage.color = this.checkBoxOverrideColor[(int)state];
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x00272F60 File Offset: 0x00271360
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			this.UpdateAssociatedImageColors(state);
			ColorBlock colors = base.colors;
			colors.normalColor = new Color(base.colors.normalColor.r, base.colors.normalColor.g, base.colors.normalColor.b, 0f);
			base.colors = colors;
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

		// Token: 0x06004C5B RID: 19547 RVA: 0x0027307C File Offset: 0x0027147C
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, (!instant) ? base.colors.fadeDuration : 0f, true, true);
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x002730C7 File Offset: 0x002714C7
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x002730E8 File Offset: 0x002714E8
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x00273165 File Offset: 0x00271565
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x00273175 File Offset: 0x00271575
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06004C60 RID: 19552 RVA: 0x00273188 File Offset: 0x00271588
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

		// Token: 0x06004C61 RID: 19553 RVA: 0x002731ED File Offset: 0x002715ED
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x040050C6 RID: 20678
		[SerializeField]
		private Image checkImage;

		// Token: 0x040050C7 RID: 20679
		[SerializeField]
		private Image checkBoxImage;

		// Token: 0x040050C8 RID: 20680
		[SerializeField]
		private Color[] checkOverrideColor;

		// Token: 0x040050C9 RID: 20681
		[SerializeField]
		private Color[] checkBoxOverrideColor;

		// Token: 0x040050CA RID: 20682
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x040050CB RID: 20683
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x040050CC RID: 20684
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x040050CD RID: 20685
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x040050CE RID: 20686
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x040050CF RID: 20687
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x040050D0 RID: 20688
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x040050D1 RID: 20689
		private bool isHighlightDisabled;
	}
}
