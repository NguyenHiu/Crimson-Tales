using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource, sfxSource, drinkSfxSource;
    [SerializeField] AudioClip backgroundAudio;
    [SerializeField] AudioClip swordSwingAudio;
    [SerializeField] AudioClip heroDamageAudio;
    [SerializeField] AudioClip goblinSwordSwingAudio;
    [SerializeField] AudioClip goblinDamageAudio;
    [SerializeField] AudioClip npcAudio;
    [SerializeField] AudioClip drinkPotion;
    [SerializeField] AudioClip openChest;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip openInventory;

    // Start is called before the first frame update
    void Start()
    {
        drinkSfxSource.clip = drinkPotion;
        musicSource.clip = backgroundAudio;
        musicSource.loop = true;
        drinkSfxSource.loop = false;
        musicSource.Play();
    }

    public void StopMusicBackground()
    {
        musicSource.Stop();
    }

    public void SwordSound()
    {
        sfxSource.PlayOneShot(swordSwingAudio);
    }

    public void GoblinSwordSound()
    {
        sfxSource.PlayOneShot(goblinSwordSwingAudio);
    }

    public void HeroDamageAudio()
    {
        sfxSource.PlayOneShot(heroDamageAudio);
    }

    public void GoblinDamageAudio()
    {
        sfxSource.PlayOneShot(goblinDamageAudio);
    }

    public void NPCTalk()
    {
        sfxSource.PlayOneShot(npcAudio);
    }

    public void DrinkPotion()
    {
        drinkSfxSource.Play();
    }

    public void StopDrinkPotion()
    {
        drinkSfxSource.Stop();
    }

    public void OpenChest()
    {
        sfxSource.PlayOneShot(openChest);
    }

    public void Explosion()
    {
        sfxSource.PlayOneShot(explosion);
    }

    public void Death()
    {
        sfxSource.PlayOneShot(death);
    }

    public void OpenInventory()
    {
        sfxSource.PlayOneShot(openInventory);
    }
}
