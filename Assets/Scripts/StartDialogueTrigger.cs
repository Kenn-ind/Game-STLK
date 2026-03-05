using UnityEngine;

public class StartDialogeTrigger : MonoBehaviour
{
    public string npcName;
    [TextArea(2, 6)]
    public string[] dialogLines;

    void Start()
    {
        DialogueManager.Instance.StartDialog(npcName, dialogLines);
    }
}
