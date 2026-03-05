using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogPanel;
    public GameObject MobilePanel;
    public GameObject PausePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
    public Player player;

    public float typingSpeed = 0.03f;

    private string[] lines;
    private int index;
    private bool isTyping;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (dialogPanel.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogText.text = lines[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialog(string npcName, string[] dialogLines)
    {
        dialogPanel.SetActive(true);
        MobilePanel.SetActive(false);
        PausePanel.SetActive(false);
        nameText.text = npcName;
        lines = dialogLines;
        index = 0;

        if (player != null)
            player.canMove = false;

        StartCoroutine(TypeLine());
    }


    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogPanel.SetActive(false);
            MobilePanel.SetActive(true);
            PausePanel.SetActive(true);

            if (player != null) 
            player.canMove = true;
        }

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char c in lines[index])
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
