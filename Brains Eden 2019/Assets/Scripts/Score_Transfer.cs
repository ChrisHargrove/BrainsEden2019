using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_Transfer : MonoBehaviour
{
    [SerializeField] public int player_score;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
