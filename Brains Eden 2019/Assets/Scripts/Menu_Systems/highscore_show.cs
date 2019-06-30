using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class highscore_show : MonoBehaviour
{
    [SerializeField] Vector3 default_position;
    [SerializeField] Vector3 highscore_offset = new Vector3(0.0f, 5000.0f, 0.0f);
    private Vector3 current_position = new Vector3();

    [SerializeField] Button back_button;
    [SerializeField] Button event_return_button;
    [SerializeField] UnityEngine.EventSystems.EventSystem event_sys;

    private float lerp_progress = 0.0f;

    public bool are_scores_shown = false;
    // Start is called before the first frame update
    void Start()
    {
        default_position = transform.position;
        back_button.onClick.AddListener(Back_To_Menu);
    }

    // Update is called once per frame
    void Update()
    {
        if (are_scores_shown)
        {
            transform.position = Vector3.Lerp(transform.position, default_position + highscore_offset, 0.2f);
            event_sys.SetSelectedGameObject(back_button.gameObject);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, default_position, 0.2f);
        }
        
    }

    void Back_To_Menu()
    {
        are_scores_shown = false;
        event_sys.SetSelectedGameObject(event_return_button.gameObject);
    }
}
