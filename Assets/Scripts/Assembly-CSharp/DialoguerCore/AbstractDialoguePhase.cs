using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DialoguerCore
{
	// Token: 0x02000B73 RID: 2931
	public abstract class AbstractDialoguePhase
	{
		// Token: 0x060046A3 RID: 18083 RVA: 0x0024ECF8 File Offset: 0x0024D0F8
		public AbstractDialoguePhase(List<int> outs)
		{
			if (outs != null)
			{
				int[] array = outs.ToArray();
				this.outs = (array.Clone() as int[]);
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x0024ED29 File Offset: 0x0024D129
		// (set) Token: 0x060046A5 RID: 18085 RVA: 0x0024ED34 File Offset: 0x0024D134
		public PhaseState state
		{
			get
			{
				return this._state;
			}
			protected set
			{
				this._state = value;
				switch (this._state)
				{
				case PhaseState.Start:
					this.onStart();
					break;
				case PhaseState.Action:
					this.onAction();
					break;
				case PhaseState.Complete:
					this.onComplete();
					break;
				}
			}
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x0024ED90 File Offset: 0x0024D190
		public void Start(DialoguerVariables localVars)
		{
			this.Reset();
			this._localVariables = localVars;
			this.state = PhaseState.Start;
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x0024EDA8 File Offset: 0x0024D1A8
		public virtual void Continue(int outId)
		{
			int num = 0;
			if (this.outs != null && this.outs[outId] >= 0)
			{
				num = this.outs[outId];
			}
			this.nextPhaseId = num;
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x0024EDE5 File Offset: 0x0024D1E5
		protected virtual void onStart()
		{
			this.state = PhaseState.Action;
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x0024EDEE File Offset: 0x0024D1EE
		protected virtual void onAction()
		{
			this.state = PhaseState.Complete;
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x0024EDF7 File Offset: 0x0024D1F7
		protected virtual void onComplete()
		{
			this.dispatchPhaseComplete(this.nextPhaseId);
			this.state = PhaseState.Inactive;
			this.Reset();
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x0024EE12 File Offset: 0x0024D212
		protected virtual void Reset()
		{
			this.nextPhaseId = ((this.outs == null || this.outs[0] < 0) ? 0 : this.outs[0]);
			this._localVariables = null;
		}

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x060046AC RID: 18092 RVA: 0x0024EE48 File Offset: 0x0024D248
		// (remove) Token: 0x060046AD RID: 18093 RVA: 0x0024EE80 File Offset: 0x0024D280
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AbstractDialoguePhase.PhaseCompleteHandler onPhaseComplete;

		// Token: 0x060046AE RID: 18094 RVA: 0x0024EEB6 File Offset: 0x0024D2B6
		private void dispatchPhaseComplete(int nextPhaseId)
		{
			if (this.onPhaseComplete != null)
			{
				this.onPhaseComplete(nextPhaseId);
			}
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x0024EECF File Offset: 0x0024D2CF
		public void resetEvents()
		{
			this.onPhaseComplete = null;
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x0024EED8 File Offset: 0x0024D2D8
		public override string ToString()
		{
			return "AbstractDialoguePhase";
		}

		// Token: 0x04004C8D RID: 19597
		public readonly int[] outs;

		// Token: 0x04004C8E RID: 19598
		protected int nextPhaseId;

		// Token: 0x04004C8F RID: 19599
		protected DialoguerVariables _localVariables;

		// Token: 0x04004C90 RID: 19600
		private PhaseState _state;

		// Token: 0x02000B74 RID: 2932
		// (Invoke) Token: 0x060046B2 RID: 18098
		public delegate void PhaseCompleteHandler(int nextPhaseId);
	}
}
