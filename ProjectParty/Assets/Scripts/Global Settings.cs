using System;
using Unity;
using UnityEngine.UI;
using UnityEngine.UIElements;

public static class GlobalSettings
{
    //LOKALE VARIABLEN (NICHT SYNKRONISIERT)
    public static string selectedCharacter = "Player";
    public static string myTeam = "TeamA";

    //FÜR UI
    public static float specialAttackEnergy = 0;
    public static float maxSpecialAttackEnergy = 100;
    public static int ammunition = 0;
    public static bool objectCanBeTaken = false;



    //NETZWERKVARIABLEN (SYNKRONISIERT)
    public static int PointsTeamA = 0;
    public static int PointsTeamB = 0;


}
