using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup, ICollectable
{
    public int experienceGranted;
    public void Collect()
    {
        Debug.Log("Called");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
