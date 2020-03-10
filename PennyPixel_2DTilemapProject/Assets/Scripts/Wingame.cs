using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Wingame : MonoBehaviour
{
    public GameObject Player;
    public Text action;
    // Start is called before the first frame update
    void Start()
    {
        action.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        var Distance =  this.transform.position.x - Player.transform.position.x;
        if(Distance <= .5f)
        {
            action.text = "Press E To Open";
            if(Input.GetButtonDown("Action"))
            {
                SceneManager.LoadScene("Winscreen");
            }
        }
        
    }
}
