using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public GameObject asalskor;
    public TMP_Text teksskor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (asalskor == null || teksskor == null)
            return;

        Player player = asalskor.GetComponent<Player>();
        if (player == null)
            return;

        teksskor.text = ": " + player.skor.ToString();
    }
}
