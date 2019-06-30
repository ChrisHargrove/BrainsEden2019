using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    public GameObject SoundManagerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiation();
    }

    private void Instantiation()
    {
        GameObject SoundManagerInstance = GameObject.FindGameObjectWithTag("SoundManager");
        GameObject SoundManager;

        if (SoundManagerInstance == null) {
            SoundManager = Instantiate(SoundManagerPrefab);
        }
        else {
            SoundManager = SoundManagerInstance;
        }
    }
}
