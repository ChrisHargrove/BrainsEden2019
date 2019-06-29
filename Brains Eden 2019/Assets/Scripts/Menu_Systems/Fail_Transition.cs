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
    private bool has_tower_been_set = true;

    // Start is called before the first frame update
    void Start()
    {
        if (tower_object == null)
        {
            has_tower_been_set = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((tower_object == null) && (has_tower_been_set))
        {
            SceneManager.LoadScene(fail_scene_name);
        }
    }
}
