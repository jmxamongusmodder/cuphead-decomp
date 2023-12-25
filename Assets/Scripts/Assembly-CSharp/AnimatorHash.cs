using System;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public static class AnimatorHash
{
	// Token: 0x020002F2 RID: 754
	public static class level_oldman_man
	{
		// Token: 0x020002F3 RID: 755
		public static class Layer
		{
			// Token: 0x04001149 RID: 4425
			public static readonly int BaseLayer;

			// Token: 0x0400114A RID: 4426
			public static readonly int Beard = 1;

			// Token: 0x0400114B RID: 4427
			public static readonly int Eyes = 2;

			// Token: 0x0400114C RID: 4428
			public static readonly int GnomeA = 3;

			// Token: 0x0400114D RID: 4429
			public static readonly int Cauldron = 4;

			// Token: 0x0400114E RID: 4430
			public static readonly int GnomeB = 5;
		}

		// Token: 0x020002F4 RID: 756
		public static class ShortHash
		{
			// Token: 0x0400114F RID: 4431
			public static readonly int Idle_Part_1 = Animator.StringToHash("Idle_Part_1");

			// Token: 0x04001150 RID: 4432
			public static readonly int Idle_Part_2 = Animator.StringToHash("Idle_Part_2");

			// Token: 0x04001151 RID: 4433
			public static readonly int Spit_Transition_A = Animator.StringToHash("Spit_Transition_A");

			// Token: 0x04001152 RID: 4434
			public static readonly int Spit_Loop = Animator.StringToHash("Spit_Loop");

			// Token: 0x04001153 RID: 4435
			public static readonly int Spit_Transition_B = Animator.StringToHash("Spit_Transition_B");

			// Token: 0x04001154 RID: 4436
			public static readonly int Spit_Intro_Continued = Animator.StringToHash("Spit_Intro_Continued");

			// Token: 0x04001155 RID: 4437
			public static readonly int Spit_Outro = Animator.StringToHash("Spit_Outro");

			// Token: 0x04001156 RID: 4438
			public static readonly int Phase_Trans = Animator.StringToHash("Phase_Trans");

			// Token: 0x04001157 RID: 4439
			public static readonly int Phase_Trans_Cont = Animator.StringToHash("Phase_Trans_Cont");

			// Token: 0x04001158 RID: 4440
			public static readonly int Beard_Boil = Animator.StringToHash("Beard_Boil");

			// Token: 0x04001159 RID: 4441
			public static readonly int Blank = Animator.StringToHash("Blank");

			// Token: 0x0400115A RID: 4442
			public static readonly int Loop = Animator.StringToHash("Loop");
		}

		// Token: 0x020002F5 RID: 757
		public static class FullHash
		{
			// Token: 0x0400115B RID: 4443
			public static readonly int BaseLayer_Idle_Part_1 = Animator.StringToHash("Base Layer.Idle_Part_1");

			// Token: 0x0400115C RID: 4444
			public static readonly int BaseLayer_Idle_Part_2 = Animator.StringToHash("Base Layer.Idle_Part_2");

			// Token: 0x0400115D RID: 4445
			public static readonly int BaseLayer_Spit_Transition_A = Animator.StringToHash("Base Layer.Spit_Transition_A");

			// Token: 0x0400115E RID: 4446
			public static readonly int BaseLayer_Spit_Loop = Animator.StringToHash("Base Layer.Spit_Loop");

			// Token: 0x0400115F RID: 4447
			public static readonly int BaseLayer_Spit_Transition_B = Animator.StringToHash("Base Layer.Spit_Transition_B");

			// Token: 0x04001160 RID: 4448
			public static readonly int BaseLayer_Spit_Intro_Continued = Animator.StringToHash("Base Layer.Spit_Intro_Continued");

			// Token: 0x04001161 RID: 4449
			public static readonly int BaseLayer_Spit_Outro = Animator.StringToHash("Base Layer.Spit_Outro");

			// Token: 0x04001162 RID: 4450
			public static readonly int BaseLayer_Phase_Trans = Animator.StringToHash("Base Layer.Phase_Trans");

			// Token: 0x04001163 RID: 4451
			public static readonly int BaseLayer_Phase_Trans_Cont = Animator.StringToHash("Base Layer.Phase_Trans_Cont");

			// Token: 0x04001164 RID: 4452
			public static readonly int Beard_Beard_Boil = Animator.StringToHash("Beard.Beard_Boil");

			// Token: 0x04001165 RID: 4453
			public static readonly int Eyes_Blank = Animator.StringToHash("Eyes.Blank");

			// Token: 0x04001166 RID: 4454
			public static readonly int Eyes_Spit_Loop = Animator.StringToHash("Eyes.Spit_Loop");

			// Token: 0x04001167 RID: 4455
			public static readonly int GnomeA_Loop = Animator.StringToHash("GnomeA.Loop");

			// Token: 0x04001168 RID: 4456
			public static readonly int Cauldron_Loop = Animator.StringToHash("Cauldron.Loop");

			// Token: 0x04001169 RID: 4457
			public static readonly int GnomeB_Loop = Animator.StringToHash("GnomeB.Loop");
		}

		// Token: 0x020002F6 RID: 758
		public static class Parameter
		{
			// Token: 0x0400116A RID: 4458
			public static readonly int IsSpitAttack = Animator.StringToHash("IsSpitAttack");

			// Token: 0x0400116B RID: 4459
			public static readonly int IsSpitAttackEyeLoop = Animator.StringToHash("IsSpitAttackEyeLoop");

			// Token: 0x0400116C RID: 4460
			public static readonly int Phase2 = Animator.StringToHash("Phase2");
		}
	}
}
