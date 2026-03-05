using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnlyMobile : MonoBehaviour
{
    public float delay = 55f; // waktu tunggu (detik)
    public GameObject FadeOut;
    void Start()
    {
        //gameObject.SetActive(Application.isMobilePlatform); // UI keliatan di mobile doang
        Invoke(nameof(DisableObject), delay);
    }

    void DisableObject()
    {
        FadeOut.SetActive(false);
    }
}
