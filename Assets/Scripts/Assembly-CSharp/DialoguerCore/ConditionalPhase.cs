using System;
using System.Collections.Generic;
using DialoguerEditor;

namespace DialoguerCore
{
	// Token: 0x02000B76 RID: 2934
	public class ConditionalPhase : AbstractDialoguePhase
	{
		// Token: 0x060046B7 RID: 18103 RVA: 0x0024F07D File Offset: 0x0024D47D
		public ConditionalPhase(VariableEditorScopes scope, VariableEditorTypes type, int variableId, VariableEditorGetEquation equation, string getValue, List<int> outs) : base(outs)
		{
			this.scope = scope;
			this.type = type;
			this.variableId = variableId;
			this.equation = equation;
			this.getValue = getValue;
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x0024F0AC File Offset: 0x0024D4AC
		protected override void onStart()
		{
			VariableEditorTypes variableEditorTypes = this.type;
			if (variableEditorTypes != VariableEditorTypes.Boolean)
			{
				if (variableEditorTypes != VariableEditorTypes.Float)
				{
					if (variableEditorTypes == VariableEditorTypes.String)
					{
						this._parsedString = this.getValue;
						if (this.scope == VariableEditorScopes.Local)
						{
							this._checkString = this._localVariables.strings[this.variableId];
						}
						else
						{
							this._checkString = Dialoguer.GetGlobalString(this.variableId);
						}
					}
				}
				else
				{
					if (!Parser.FloatTryParse(this.getValue, out this._parsedFloat))
					{
						Debug.LogError("[ConditionalPhase] Could Not Parse Float: " + this.getValue, null);
					}
					if (this.scope == VariableEditorScopes.Local)
					{
						this._checkFloat = this._localVariables.floats[this.variableId];
					}
					else
					{
						this._checkFloat = Dialoguer.GetGlobalFloat(this.variableId);
					}
				}
			}
			else
			{
				if (!bool.TryParse(this.getValue, out this._parsedBool))
				{
					Debug.LogError("[ConditionalPhase] Could Not Parse Bool: " + this.getValue, null);
				}
				if (this.scope == VariableEditorScopes.Local)
				{
					this._checkBool = this._localVariables.booleans[this.variableId];
				}
				else
				{
					this._checkBool = Dialoguer.GetGlobalBoolean(this.variableId);
				}
			}
			bool flag = false;
			VariableEditorTypes variableEditorTypes2 = this.type;
			if (variableEditorTypes2 != VariableEditorTypes.Boolean)
			{
				if (variableEditorTypes2 != VariableEditorTypes.Float)
				{
					if (variableEditorTypes2 == VariableEditorTypes.String)
					{
						VariableEditorGetEquation variableEditorGetEquation = this.equation;
						if (variableEditorGetEquation != VariableEditorGetEquation.Equals)
						{
							if (variableEditorGetEquation == VariableEditorGetEquation.NotEquals)
							{
								if (this._parsedString != this._checkString)
								{
									flag = true;
								}
							}
						}
						else if (this._parsedString == this._checkString)
						{
							flag = true;
						}
					}
				}
				else
				{
					switch (this.equation)
					{
					case VariableEditorGetEquation.Equals:
						if (this._checkFloat == this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.NotEquals:
						if (this._checkFloat != this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.GreaterThan:
						if (this._checkFloat > this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.LessThan:
						if (this._checkFloat < this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.EqualOrGreaterThan:
						if (this._checkFloat >= this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.EqualOrLessThan:
						if (this._checkFloat <= this._parsedFloat)
						{
							flag = true;
						}
						break;
					}
				}
			}
			else
			{
				VariableEditorGetEquation variableEditorGetEquation2 = this.equation;
				if (variableEditorGetEquation2 != VariableEditorGetEquation.Equals)
				{
					if (variableEditorGetEquation2 == VariableEditorGetEquation.NotEquals)
					{
						if (this._parsedBool != this._checkBool)
						{
							flag = true;
						}
					}
				}
				else if (this._parsedBool == this._checkBool)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.Continue(0);
			}
			else
			{
				this.Continue(1);
			}
			base.state = PhaseState.Complete;
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x0024F3BC File Offset: 0x0024D7BC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Set Variable Phase\nScope: ",
				this.scope.ToString(),
				"\nType: ",
				this.type.ToString(),
				"\nVariable ID: ",
				this.variableId,
				"\nEquation: ",
				this.equation.ToString(),
				"\nGet Value: ",
				this.getValue,
				"\nTrue Out: ",
				this.outs[0],
				"\nFalse Out: ",
				this.outs[1],
				"\n"
			});
		}

		// Token: 0x04004C93 RID: 19603
		public readonly VariableEditorScopes scope;

		// Token: 0x04004C94 RID: 19604
		public readonly VariableEditorTypes type;

		// Token: 0x04004C95 RID: 19605
		public readonly int variableId;

		// Token: 0x04004C96 RID: 19606
		public readonly VariableEditorGetEquation equation;

		// Token: 0x04004C97 RID: 19607
		public readonly string getValue;

		// Token: 0x04004C98 RID: 19608
		private bool _parsedBool;

		// Token: 0x04004C99 RID: 19609
		private bool _checkBool;

		// Token: 0x04004C9A RID: 19610
		private float _parsedFloat;

		// Token: 0x04004C9B RID: 19611
		private float _checkFloat;

		// Token: 0x04004C9C RID: 19612
		private string _parsedString;

		// Token: 0x04004C9D RID: 19613
		private string _checkString;
	}
}
