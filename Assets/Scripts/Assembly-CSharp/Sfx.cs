using System;

// Token: 0x020003C8 RID: 968
public enum Sfx
{
	// Token: 0x040015A6 RID: 5542
	None,
	// Token: 0x040015A7 RID: 5543
	_ = -1,
	// Token: 0x040015A8 RID: 5544
	Player_Dash = 1,
	// Token: 0x040015A9 RID: 5545
	Player_Hit,
	// Token: 0x040015AA RID: 5546
	Player_Jump,
	// Token: 0x040015AB RID: 5547
	Player_Grounded,
	// Token: 0x040015AC RID: 5548
	Player_Parry,
	// Token: 0x040015AD RID: 5549
	Player_Game_Over,
	// Token: 0x040015AE RID: 5550
	Player_Revive,
	// Token: 0x040015AF RID: 5551
	Player_Plane_Hit = 30,
	// Token: 0x040015B0 RID: 5552
	Player_Plane_Shrink,
	// Token: 0x040015B1 RID: 5553
	Player_Map_Walk_One = 50,
	// Token: 0x040015B2 RID: 5554
	Player_Map_Walk_Two,
	// Token: 0x040015B3 RID: 5555
	Player_Weapon_Peashot = 100,
	// Token: 0x040015B4 RID: 5556
	Player_Weapon_Peashot_Miss,
	// Token: 0x040015B5 RID: 5557
	Player_Weapon_Peashot_Ex,
	// Token: 0x040015B6 RID: 5558
	Player_Weapon_Spread = 200,
	// Token: 0x040015B7 RID: 5559
	Player_Weapon_Spread_Miss,
	// Token: 0x040015B8 RID: 5560
	Player_Weapon_Spread_Ex,
	// Token: 0x040015B9 RID: 5561
	Player_Super_Start = 900,
	// Token: 0x040015BA RID: 5562
	Player_Super_Beam,
	// Token: 0x040015BB RID: 5563
	__,
	// Token: 0x040015BC RID: 5564
	Level_Announcer_Ready = 9000,
	// Token: 0x040015BD RID: 5565
	Level_Announcer_Begin,
	// Token: 0x040015BE RID: 5566
	Level_Boss_Death_Explosion,
	// Token: 0x040015BF RID: 5567
	Level_Announcer_Knockout,
	// Token: 0x040015C0 RID: 5568
	___,
	// Token: 0x040015C1 RID: 5569
	Levels_Pirate_Laugh = 10000,
	// Token: 0x040015C2 RID: 5570
	Levels_Pirate_Whistle,
	// Token: 0x040015C3 RID: 5571
	Levels_Pirate_Barrel_Smash,
	// Token: 0x040015C4 RID: 5572
	Levels_Pirate_Fish_Splash,
	// Token: 0x040015C5 RID: 5573
	Levels_Pirate_Peashot,
	// Token: 0x040015C6 RID: 5574
	Levels_Pirate_Shark_Bite,
	// Token: 0x040015C7 RID: 5575
	Leves_Pirate_Ship_Cannon,
	// Token: 0x040015C8 RID: 5576
	Levels_Pirate_Squid_Splash,
	// Token: 0x040015C9 RID: 5577
	Levels_Pirate_Whale_Open,
	// Token: 0x040015CA RID: 5578
	Levels_Pirate_Whale_Uvula_Spit,
	// Token: 0x040015CB RID: 5579
	Levels_Pirate_Whale_Uvula_Beam,
	// Token: 0x040015CC RID: 5580
	____,
	// Token: 0x040015CD RID: 5581
	Levels_Frogs_Short_Clap = 11000,
	// Token: 0x040015CE RID: 5582
	Levels_Frogs_Short_Rage_Shoot,
	// Token: 0x040015CF RID: 5583
	Levels_Frogs_Tall_Firefly,
	// Token: 0x040015D0 RID: 5584
	Levels_Frogs_Tall_Fan,
	// Token: 0x040015D1 RID: 5585
	Levels_Frogs_Morph,
	// Token: 0x040015D2 RID: 5586
	Levels_Frogs_Morph_Land,
	// Token: 0x040015D3 RID: 5587
	Levels_Frogs_Morph_Coin,
	// Token: 0x040015D4 RID: 5588
	Level_Frogs_Tall_Firefly_Die,
	// Token: 0x040015D5 RID: 5589
	Level_Frogs_Short_Roll,
	// Token: 0x040015D6 RID: 5590
	Level_Frogs_Morph_Slots_Cycle,
	// Token: 0x040015D7 RID: 5591
	Level_Frogs_Morph_Slots_Stop,
	// Token: 0x040015D8 RID: 5592
	Level_Frogs_Morph_Bison_Flame,
	// Token: 0x040015D9 RID: 5593
	_____,
	// Token: 0x040015DA RID: 5594
	Levels_Airship_Jelly_Hurt = 12000,
	// Token: 0x040015DB RID: 5595
	Levels_Airship_Jelly_Walk,
	// Token: 0x040015DC RID: 5596
	Levels_Airship_Jelly_Hit,
	// Token: 0x040015DD RID: 5597
	______,
	// Token: 0x040015DE RID: 5598
	Levels_Veggies_Ground = 13000,
	// Token: 0x040015DF RID: 5599
	Levels_Veggies_Potato_Spit,
	// Token: 0x040015E0 RID: 5600
	Levels_Veggies_Carrot_Hurt,
	// Token: 0x040015E1 RID: 5601
	Levels_Veggies_Carrot_Psychic_Start,
	// Token: 0x040015E2 RID: 5602
	Levels_Veggies_Carrot_Projectile_Death,
	// Token: 0x040015E3 RID: 5603
	_______,
	// Token: 0x040015E4 RID: 5604
	Level_Train_BlindSpecter_Intro = 14000,
	// Token: 0x040015E5 RID: 5605
	Level_Train_BlindSpecter_Shoot,
	// Token: 0x040015E6 RID: 5606
	Level_Train_Skeleton_Slap,
	// Token: 0x040015E7 RID: 5607
	Level_Train_Top_Explode,
	// Token: 0x040015E8 RID: 5608
	Level_Train_Skeleton_Up,
	// Token: 0x040015E9 RID: 5609
	Level_Train_Skeleton_Down,
	// Token: 0x040015EA RID: 5610
	Level_Train_Pumpkin_Die,
	// Token: 0x040015EB RID: 5611
	________,
	// Token: 0x040015EC RID: 5612
	Level_FlyingBird_Feathers = 15000,
	// Token: 0x040015ED RID: 5613
	Level_FlyingBird_Small_Bird_Death,
	// Token: 0x040015EE RID: 5614
	Level_FlyingBird_Intro_A,
	// Token: 0x040015EF RID: 5615
	Level_FlyingBird_Intro_B,
	// Token: 0x040015F0 RID: 5616
	Level_FlyingBird_Egg_Explode,
	// Token: 0x040015F1 RID: 5617
	Level_FlyingBird_Egg_Spit,
	// Token: 0x040015F2 RID: 5618
	Level_FlyingBird_Steamwhistle,
	// Token: 0x040015F3 RID: 5619
	Level_FlyingBird_Glove_Laser,
	// Token: 0x040015F4 RID: 5620
	Level_FlyingBird_Small_Shoot,
	// Token: 0x040015F5 RID: 5621
	_________,
	// Token: 0x040015F6 RID: 5622
	Level_Bee_Chain = 16000,
	// Token: 0x040015F7 RID: 5623
	Level_Bee_Grunt_Death,
	// Token: 0x040015F8 RID: 5624
	Level_Bee_Spit,
	// Token: 0x040015F9 RID: 5625
	Level_Bee_Magic_Start,
	// Token: 0x040015FA RID: 5626
	Level_Bee_Magic_Go,
	// Token: 0x040015FB RID: 5627
	Level_Bee_Intro,
	// Token: 0x040015FC RID: 5628
	Level_Bee_Security_Bomb_Throw,
	// Token: 0x040015FD RID: 5629
	Level_Bee_Security_Bomb_Explode,
	// Token: 0x040015FE RID: 5630
	Level_Bee_Intro_Knife,
	// Token: 0x040015FF RID: 5631
	Level_Bee_Intro_Snap,
	// Token: 0x04001600 RID: 5632
	__________,
	// Token: 0x04001601 RID: 5633
	Level_Dragon_Intro = 17000,
	// Token: 0x04001602 RID: 5634
	Level_Dragon_Eye_Shot,
	// Token: 0x04001603 RID: 5635
	Level_Dragon_Meteor_Spit,
	// Token: 0x04001604 RID: 5636
	Level_Dragon_Sucking_Air,
	// Token: 0x04001605 RID: 5637
	___________,
	// Token: 0x04001606 RID: 5638
	UI_PlayerToggle_001,
	// Token: 0x04001607 RID: 5639
	UI_PlayerToggle_002,
	// Token: 0x04001608 RID: 5640
	UI_PlayerToggle_003,
	// Token: 0x04001609 RID: 5641
	UI_PlayerSelect_Confirm,
	// Token: 0x0400160A RID: 5642
	UI_OpticalLoop,
	// Token: 0x0400160B RID: 5643
	UI_OpticalStart_001,
	// Token: 0x0400160C RID: 5644
	UI_OpticalStart_002,
	// Token: 0x0400160D RID: 5645
	___________________________________
}
