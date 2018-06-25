using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LPDefinitions 
{
	//	GameMode Settings
	public static float GameMode_TransitionDelay = 2.0f;

	//	Character Settings
	public static float Character_Speed = 4.0f;
    public static float Character_GroundSlideFactor = 1f;
    public static float Character_MaxJumpHeight = 3.0f;
	public static float Character_MinJumpHeight = 0.5f;
	public static float Character_WallSlideSpeed = 20f;
    public static float Character_WallStickTime = .25f;
    public static float Character_ImpulseUp = 8.0f;
    public static float Character_ImpulseBack = 8.0f;
    public static float Character_HitCooldown = 0.2f;
    public static float Character_MotionThreshold = 0.5f;
    public static float Character_WeatherHeatFactor;
	public static float Character_WeatherColdFactor;
    public static int Character_MaxJumpCount = 1;
    public static int Character_MaxLife = 1;

    //	Smasher Settings
    public static float Smasher_Speed = 1.0f;
	public static float Smasher_GroundTime = 3.0f;
    public static float Smasher_BeforeFallTime = 0.5f;
    public static float Smasher_BackToPositionTime = 1.0f;
    
    //	Melee Settings
    public static float Melee_Speed = 1.5f;

    //	Shooter Settings
    public static float Shooter_Speed = 13f;
    public static float Shooter_BeforeShootTime = 1.0f;

    //	World Settings
    public static float World_Gravity = 8.0f;
	public static float World_WindSpeed = 1.0f;
	public static float World_WeatherTemperature = 1.0f;

    //	Platform Settings
    public static float Platform_Speed = 2f;
    public static float Platform_VanishingTime = 2f;
    public static float PlatformFalling_TimeBeforeActivate = 2f;
    public static float PlatformGlitch_TimeBeforeActivate = 0.5f;
    public static float PlatformGlitch_TimeToReset = 1.5f;

    //	Cutting Disc Settings
    public static float CuttingDisc_Speed = 1.0f;
	//	public static float CuttingDisc_Path;

	//	Glitch Settings
	public static float Glitch_Distance = 1.0f;
	public static float Glitch_Falloff = 1.0f;

    //  Collectables Settings
    public static float Coin_FollowFactor = 0.05f;
    public static float Magnet_LastDuration = 5.0f;
}

public enum PowerUp { Magnet, DoubleJump, Continue, None };
