using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("对话气泡")]
    public GameObject dialogueBubble; // UI面板
    public TextMeshProUGUI dialogueText;

    [Header("对话内容")]
    [TextArea]
    public string[] dialogueLines;   // 多条对话
    private int currentLineIndex = 0;

    [Header("对话设置")]
    public float lineDelay = 1f; 

    [Header("玩家")]
    public Player player;

    private bool isPlayerInRange = false;
    private bool dialoguePlaying = false;
    private bool hasTriggered = false; // 确保只触发一次对话

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && !hasTriggered && !dialoguePlaying)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;
        dialoguePlaying = true;
        hasTriggered = true; // 标记为已触发，避免重复触发
        currentLineIndex = 0;
        player.SetControlled(false);
        dialogueBubble.SetActive(true);
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            Invoke(nameof(NextLine), lineDelay);
        }
        else
        {
            EndDialogue();
        }
    }

    private void NextLine()
    {
        currentLineIndex++;
        ShowCurrentLine();
    }

    private void EndDialogue()
    {
        if (dialogueBubble != null)
            dialogueBubble.SetActive(false);
        player.SetControlled(true);
        dialoguePlaying = false;
    }
}
