using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text health;
    public Text bulletAmmo;
    public Text homingMissileAmmo;
    public Text mineAmmo;

    public Image homingMissileReloadIcon;
    public Image bulletReloadIcon;

    public PlayerControl player;

    const string healthStringBase = "Health: ";
    const string bulletStringBase = "Bullets: ";
    const string homingMissileStringBase = "H.Missiles: ";
    const string mineStringBase = "Mines: ";

    void Start(){
        health.text = healthStringBase;
        bulletAmmo.text = bulletStringBase;
        homingMissileAmmo.text = homingMissileStringBase;
        mineAmmo.text = mineStringBase;
    }

    void Update(){
        health.text = healthStringBase + player.currentHealth + "/" + player.maxHealth;
        bulletAmmo.text = bulletStringBase + player.currentBullets + "/" + player.maxBullets;
        homingMissileAmmo.text = homingMissileStringBase + player.currentMissiles + "/" + player.maxMissiles;
        mineAmmo.text = mineStringBase + player.currentMines + "/" + player.maxMines;
    }

    public IEnumerator ReloadProgress(WeaponType weapon, float endTime){
        float timer = 0f;
        Image reloadIcon;
        
        //Choose which reload icon to start
        switch(weapon){
            case WeaponType.bullet:
                    reloadIcon = bulletReloadIcon;
                    break;
            case WeaponType.homingMissile:
                reloadIcon = homingMissileReloadIcon;
                break;
            default:
                reloadIcon = homingMissileReloadIcon;
                break;
        }

        while (timer < endTime)
        {
            reloadIcon.fillAmount = timer / endTime;
            timer += Time.deltaTime;

            yield return null;
        }

        reloadIcon.fillAmount = 1f;
    }

    public enum WeaponType{
        bullet = 0,
        homingMissile = 1,
        mine = 2
    }
}
