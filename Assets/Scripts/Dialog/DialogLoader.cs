using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

public class DialogLoader : MonoBehaviour
{
    [SerializeField] private StoryContainer story;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button choicePrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private TextAnimation textAnimation;
    [SerializeField] private Sprite[] emotions;
    [SerializeField] private Image placeholder;

    private string currentMainStoryNode;
    private void Start()
    {
        textAnimation = GetComponent<TextAnimation>();
        placeholder.gameObject.SetActive(false);
        currentMainStoryNode = story.StoryNodeData.First().NodeGUID;
        Speak(currentMainStoryNode);
    }

    private void Speak(string nextNodeGUID)
    {

        StoryNodeData storyNodeData = story.StoryNodeData.Find(x => x.NodeGUID == nextNodeGUID);
        DialogueContainer dialogueContainer = Resources.Load<DialogueContainer>(storyNodeData.path.Split('.')[0]);
        currentMainStoryNode = storyNodeData.NodeGUID;

        var narrativeData = dialogueContainer.NodeLinks.First(); //Entrypoint node
        ProceedToNarrative(narrativeData.TargetNodeGUID, dialogueContainer);

    }

    private void GenerateMainBranch()
    {
        var choices = story.NodeLinks.Where(x => x.BaseNodeGUID == currentMainStoryNode);
        foreach (var choice in choices)
        {
            var button = Instantiate(choicePrefab, buttonContainer, false);
            button.GetComponentInChildren<TextMeshProUGUI>().text = ProcessProperties(choice.PortName, null);
            button.onClick.AddListener(() => {
                currentMainStoryNode = choice.TargetNodeGUID;
            });

        }
    }

    private void ProceedToNarrative(string narrativeDataGUID, DialogueContainer dialogue)
    {
        placeholder.gameObject.SetActive(true);
        StopAllCoroutines();

        var buttons = buttonContainer.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
        var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
        string processedText = ProcessProperties(text, dialogue);

        StartCoroutine(textAnimation.TextTyping(dialogueText, processedText, 2f, () => {

            if (choices.Count() > 0)
                foreach (var choice in choices)
                {
                    var button = Instantiate(choicePrefab, buttonContainer, false);
                    button.GetComponentInChildren<TextMeshProUGUI>().text = ProcessProperties(choice.PortName, null);
                    button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGUID, dialogue));
                }
            else
                GenerateMainBranch();

        }));
    }

    private string ProcessProperties(string text, DialogueContainer dialogue)
    {

        if (dialogue == null) return text;

        string[] preProcessed = text.Split('/');
        placeholder.sprite = emotions[int.Parse(preProcessed[0])];
        string temp = "";
        for(int i = 1; i < preProcessed.Length; i++)
        {
            temp += preProcessed[i];
        }
        text = temp;

        foreach (var exposedProperty in dialogue.ExposedProperties)
        {
            text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
        }
        return text;
    }
}

public enum emotion
{
    Flat,
    Questioning,
}


