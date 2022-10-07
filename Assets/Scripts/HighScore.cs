using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Score"))
            GetComponent<Text>().text = PlayerPrefs.GetFloat("Score").ToString();
    }
}
