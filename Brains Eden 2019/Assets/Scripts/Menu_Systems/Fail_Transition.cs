using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fail_Transition : MonoBehaviour
{
    // Attach this script to an object.
    // When the object is destroyed the script will change to the fail state
    [SerializeField] private GameObject tower_object;
    [SerializeField] private string fail_scene_name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tower_object == null)
        {
            SceneManager.LoadScene(fail_scene_name);
        }
    }
}
