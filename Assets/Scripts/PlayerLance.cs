using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLance : MonoBehaviour
{
    [SerializeField] GameObject Lance;
    public int foodCount = 0;

    public Vector2 foodVector()
    {
        Vector3 position = new Vector3(-1f,0f,0f);
        position += new Vector3(-0.3f * (foodCount), 0.6f * (foodCount), 0f);
        foodCount++;
        return position;
    }

}
