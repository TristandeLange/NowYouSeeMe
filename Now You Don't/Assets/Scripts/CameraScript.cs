using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private FOVScript FOV;

    public GameObject Player;
    public float rotationalSpeed;

    public bool powered = true;

    private void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Player != null) 
        {
            Vector3 direction = Player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            FOV.SetAimDirection(direction);
            FOV.setOrigin(transform.position);
        }

        if (!powered) 
        {
            FOV.setPowered(false);
        }
        else 
        {
            FOV.setPowered(true);
        }
    }

}
