using System;
using UnityEngine;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class StoryNodeData
    {
        public string NodeGUID;
        public string dialogueName;
        public string path;
        public Vector2 Position;
    }
}