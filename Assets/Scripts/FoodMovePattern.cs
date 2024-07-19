using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovePattern : MonoBehaviour
{
    public GameObject game_area;
    public GameObject ship_prefab;

    public int ship_count = 0;
    public int ship_limit = 200;
    public int ships_per_frame = 1;

    public float spawn_circle_radius = 150.0f;


}
