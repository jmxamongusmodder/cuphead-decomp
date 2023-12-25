using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C28 RID: 3112
	[AddComponentMenu("")]
	public class CustomButton : Button, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x00271901 File Offset: 0x0026FD01
		// (set) Token: 0x06004BE8 RID: 19432 RVA: 0x00271909 File Offset: 0x0026FD09
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

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x00271912 File Offset: 0x0026FD12
		// (set) Token: 0x06004BEA RID: 19434 RVA: 0x0027191A File Offset: 0x0026FD1A
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

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06004BEB RID: 19435 RVA: 0x00271923 File Offset: 0x0026FD23
		// (set) Token: 0x06004BEC RID: 19436 RVA: 0x0027192B File Offset: 0x0026FD2B
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

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06004BED RID: 19437 RVA: 0x00271934 File Offset: 0x0026FD34
		// (set) Token: 0x06004BEE RID: 19438 RVA: 0x0027193C File Offset: 0x0026FD3C
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

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06004BEF RID: 19439 RVA: 0x00271945 File Offset: 0x0026FD45
		// (set) Token: 0x06004BF0 RID: 19440 RVA: 0x0027194D File Offset: 0x0026FD4D
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

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06004BF1 RID: 19441 RVA: 0x00271956 File Offset: 0x0026FD56
		// (set) Token: 0x06004BF2 RID: 19442 RVA: 0x0027195E File Offset: 0x0026FD5E
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

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x00271967 File Offset: 0x0026FD67
		// (set) Token: 0x06004BF4 RID: 19444 RVA: 0x0027196F File Offset: 0x0026FD6F
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

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06004BF5 RID: 19445 RVA: 0x00271978 File Offset: 0x0026FD78
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06004BF6 RID: 19446 RVA: 0x00271984 File Offset: 0x0026FD84
		// (remove) Token: 0x06004BF7 RID: 19447 RVA: 0x002719BC File Offset: 0x0026FDBC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event UnityAction _CancelEvent;

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x06004BF8 RID: 19448 RVA: 0x002719F2 File Offset: 0x0026FDF2
		// (remove) Token: 0x06004BF9 RID: 19449 RVA: 0x002719FB File Offset: 0x0026FDFB
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

		// Token: 0x06004BFA RID: 19450 RVA: 0x00271A04 File Offset: 0x0026FE04
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x00271A60 File Offset: 0x0026FE60
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00271ABC File Offset: 0x0026FEBC
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x00271B18 File Offset: 0x0026FF18
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00271B72 File Offset: 0x0026FF72
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x00271BA6 File Offset: 0x0026FFA6
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.update_text_color_on_enable_cr());
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x00271BBC File Offset: 0x0026FFBC
		private IEnumerator update_text_color_on_enable_cr()
		{
			yield return new WaitForEndOfFrame();
			this.UpdateAssociatedTextColor(base.currentSelectionState);
			yield break;
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x00271BD8 File Offset: 0x0026FFD8
		protected void UpdateAssociatedTextColor(Selectable.SelectionState state)
		{
			if (this.associatedText)
			{
				if (this.textOverrideColor.Length == 4)
				{
					this.associatedText.color = this.textOverrideColor[(int)state];
				}
				else
				{
					switch (state)
					{
					case Selectable.SelectionState.Normal:
						this.associatedText.color = base.colors.normalColor;
						break;
					case Selectable.SelectionState.Highlighted:
						this.associatedText.color = base.colors.highlightedColor;
						break;
					case Selectable.SelectionState.Pressed:
						this.associatedText.color = base.colors.pressedColor;
						break;
					case Selectable.SelectionState.Disabled:
						this.associatedText.color = base.colors.disabledColor;
						break;
					}
				}
			}
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x00271CBC File Offset: 0x002700BC
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			this.UpdateAssociatedTextColor(state);
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
							this.DoSpriteSwap((state != Selectable.SelectionState.Highlighted) ? disabledHighlightedSprite : base.spriteState.highlightedSprite);
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

		// Token: 0x06004C03 RID: 19459 RVA: 0x00271D84 File Offset: 0x00270184
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, (!instant) ? base.colors.fadeDuration : 0f, true, true);
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x00271DCF File Offset: 0x002701CF
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x00271DF0 File Offset: 0x002701F0
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x00271E6D File Offset: 0x0027026D
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			AudioManager.Play("level_menu_move");
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x00271E87 File Offset: 0x00270287
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x00271E98 File Offset: 0x00270298
		public void SetNavOnToggle(bool setting)
		{
			base.navigation = new Navigation
			{
				mode = ((!setting) ? Navigation.Mode.None : Navigation.Mode.Automatic)
			};
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x00271EC8 File Offset: 0x002702C8
		public void Press()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			base.onClick.Invoke();
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x00271EEC File Offset: 0x002702EC
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
			}
		}

		// Token: 0x06004C0B RID: 19467 RVA: 0x00271F48 File Offset: 0x00270348
		public override void OnSubmit(BaseEventData eventData)
		{
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
				return;
			}
			this.DoStateTransition(Selectable.SelectionState.Pressed, false);
			base.StartCoroutine(this.OnFinishSubmit());
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x00271F98 File Offset: 0x00270398
		private IEnumerator OnFinishSubmit()
		{
			float fadeTime = base.colors.fadeDuration;
			float elapsedTime = 0f;
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.DoStateTransition(base.currentSelectionState, false);
			yield break;
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x00271FB4 File Offset: 0x002703B4
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

		// Token: 0x06004C0E RID: 19470 RVA: 0x00272019 File Offset: 0x00270419
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x040050AA RID: 20650
		[SerializeField]
		protected Text associatedText;

		// Token: 0x040050AB RID: 20651
		[SerializeField]
		private Color[] textOverrideColor;

		// Token: 0x040050AC RID: 20652
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x040050AD RID: 20653
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x040050AE RID: 20654
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x040050AF RID: 20655
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x040050B0 RID: 20656
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x040050B1 RID: 20657
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x040050B2 RID: 20658
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x040050B3 RID: 20659
		protected bool isHighlightDisabled;
	}
}
