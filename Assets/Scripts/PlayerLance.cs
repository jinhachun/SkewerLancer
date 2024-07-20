using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLance : MonoBehaviour
{
    [SerializeField] GameObject Lance;
    [SerializeField] GameObject LanceSprite;
    Player player;
    public int foodCount = 0;

    private void Awake()
    { 
        player = GetComponent<Player>();  
    }
    public Vector2 foodVector()
    {
        Vector3 position = new Vector3(-1f,0f,0f);
        position += new Vector3(-0.3f * (foodCount), 0.6f * (foodCount), 0f);
        foodCount++;
        return position;
    }
    public void ResetLance()
    {
        for (int i = 0; i < Lance.transform.childCount; i++)
        {
            if(Lance.transform.GetChild(i).gameObject != LanceSprite)
            {
                Destroy(Lance.transform.GetChild(i).gameObject);
            }
        }
        foodCount = 0;
    }

}
