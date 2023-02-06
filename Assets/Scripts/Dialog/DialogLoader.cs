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
    private void Start()
    {
        var a = story.NodeLinks.First();
        Speak(a.TargetNodeGUID);
    }

    private void Speak(string nextNodeGUID)
    {

        StoryNodeData a = story.StoryNodeData.Find(x => x.NodeGUID == nextNodeGUID);
        DialogueContainer b = Resources.Load<DialogueContainer>(a.path.Split('.')[0]);

        var narrativeData = b.NodeLinks.First(); //Entrypoint node
        ProceedToNarrative(narrativeData.TargetNodeGUID, b);

    }

    private void ProceedToNarrative(string narrativeDataGUID, DialogueContainer dialogue)
    {
        var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
        var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
        dialogueText.text = ProcessProperties(text,dialogue);
        var buttons = buttonContainer.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        foreach (var choice in choices)
        {
            var button = Instantiate(choicePrefab, buttonContainer);
            button.GetComponentInChildren<Text>().text = ProcessProperties(choice.PortName, dialogue);
            button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGUID, dialogue));
        }

    }

    private string ProcessProperties(string text, DialogueContainer dialogue)
    {
        foreach (var exposedProperty in dialogue.ExposedProperties)
        {
            text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
        }
        return text;
    }
}


