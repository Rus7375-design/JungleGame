using UnityEngine;

[CreateAssetMenu(fileName = "SoundBank", menuName = "Game/Sound Bank")]
public class SoundBank : ScriptableObject
{
    [Header("Player (arrays: 2-3 versions = variation)")]
    public AudioClip[] jump;
    public AudioClip[] shoot;
    public AudioClip[] pickup;
    public AudioClip[] hurt;

    [Header("Enemies")]
    public AudioClip windupMelee;    
    public AudioClip windupCharge;  
    public AudioClip windupAim;      
    public AudioClip bombWarning;   
    public AudioClip bossShot;
    public AudioClip[] enemyDeath;
    public AudioClip[] explosion;

    [Header("Game state")]
    public AudioClip uiClick;
    public AudioClip win;
    public AudioClip lose;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;
}