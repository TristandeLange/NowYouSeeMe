using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInView : MonoBehaviour
{
    private bool recentlyViewed;
    private float countdownTimer = 2;
    // Start is called before the first frame update
    void Start()
    {
        recentlyViewed = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (recentlyViewed==true)
        {
            if (countdownTimer > 0)
            {
                countdownTimer -= Time.deltaTime;
                //Debug.Log(countdownTimer.ToString());
            }
            else
            {
                Debug.Log("Time IS OUT MUTHAFUCKA");
                DeathEvent();
            }
        }
    }

    public void HitByRay() 
    {
        recentlyViewed=true;
        countdownTimer = 2;
    }

    private void DeathEvent()
    {
        recentlyViewed = false;
        //Then I trigger some death animation, destroy the player object, and a game over screen appears(can use additive loading for that)
    }
}
