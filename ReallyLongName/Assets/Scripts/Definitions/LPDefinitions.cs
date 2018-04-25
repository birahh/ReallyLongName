using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LPDefinitions 
{
	//	Character Settings
	public static float Character_Speed = 5.0f;
    public static float Character_GroundSlideFactor = 1f;
    public static float Character_MaxJumpHeight = 4.0f;
	public static float Character_MinJumpHeight = 0.5f;
	public static float Character_WallSlideSpeed = 20f;
    public static float Character_WallStickTime = .25f;
    public static float Character_ImpulseUp = 8.0f;
    public static float Character_ImpulseBack = 8.0f;
    public static float Character_HitCooldown = 0.2f;
    public static int Character_MaxJumpCount = 2;
    //	public static float Character_Deprecated;
    public static float Character_WeatherHeatFactor;
	public static float Character_WeatherColdFactor;

	//	Smasher Settings
	public static float Smasher_Speed = 1.0f;
	public static float Smasher_GroundTime = 3.0f;
	public static float Smasher_BackToPositionTime = 1.0f;

	//	World Settings
	public static float World_Gravity = 8.0f;
	public static float World_WindSpeed = 1.0f;
	public static float World_WeatherTemperature = 1.0f;

	//	Bullet Settings
	public static float Bullet_Speed = 1.0f;
	public static float Bullet_Direction = 1.0f;

	//	Platform Settings
	public static float Platform_Speed = 1.0f;
	//	public static float Platform_Path;

	//	Cutting Disc Settings
	public static float CuttingDisc_Speed = 1.0f;
	//	public static float CuttingDisc_Path;

	//	Glitch Settings
	public static float Glitch_Distance = 1.0f;
	public static float Glitch_Falloff = 1.0f;

    //  Collectables Settings
    public static float Coin_FollowFactor = 0.05f;
}

public enum PowerUp { Magnet, DoubleJump, Speed, None };
