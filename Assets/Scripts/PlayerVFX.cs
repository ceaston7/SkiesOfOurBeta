using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    //Thrusters
    public GameObject thrustFore;
    public GameObject thrustStarboardFore;
    public GameObject thrustStarboardStern;
    public GameObject thrustPortFore;
    public GameObject thrustPortStern;
    public GameObject thrustStern;

    public void Thrust(Thruster thruster, bool show){
        switch(thruster){
            case Thruster.fore:
                thrustFore.SetActive(show);
                break;
            case Thruster.starboardFore:
                thrustStarboardFore.SetActive(show);
                break;
            case Thruster.starboardStern:
                thrustStarboardStern.SetActive(show);
                break;
            case Thruster.portFore:
                thrustPortFore.SetActive(show);
                break;
            case Thruster.portStern:
                thrustPortStern.SetActive(show);
                break;
            case Thruster.stern:
                thrustStern.SetActive(show);
                break;
            default:
                break;
        }
    }

    public enum Thruster{
        fore = 0,
        starboardFore,
        starboardStern,
        portFore,
        portStern,
        stern
    }
}
