using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages all sounds emitted by the player and his weapons
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController2D))]
public class PlayerAudio : MonoBehaviour
{
    Player player;
    public static PlayerAudio instance;
    AudioSource audioSource;
    [SerializeField]
    AudioClip jump, land;
    [SerializeField] AudioClip[] steps;
    [SerializeField] AudioClip[] hurtSounds;
    int stepCounter = 0;
    int attackCounter = 0;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
        player = GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        GetComponent<CharacterController2D>().OnLandEvent.AddListener(PlayLand);
        GetComponentInChildren<AnimationEventManager>().Stepped.AddListener(PlayStep);
        GetComponent<PlayerCombat>().HitEvent.AddListener(PlayHit);
        player.TookDamage.AddListener(PlayHurt);
    }
    public void PlayJump()
    {
        Play(jump);
    }
    public void PlayLand()
    {
        Play(land);
    }
    void Play(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }
    public void PlayStep()
    {
        Play(steps[stepCounter]);
        stepCounter = (stepCounter + 1) % steps.Length;
    }
    public void PlayAttack(int i)
    {
        
        Play(player.GetCurrentWeapon().AttackSound(i));
        attackCounter = i;
    }
    public void PlayHit()
    {
        
        Play(player.GetCurrentWeapon().HitSound(attackCounter));
        
       
    }
    public void PlayHurt()
    {
        Play(hurtSounds[Random.Range(0, hurtSounds.Length)]);
    }
}
