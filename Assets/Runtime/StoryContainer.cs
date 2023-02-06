using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class StoryContainer : ScriptableObject
    {
        public string path;
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<StoryNodeData> StoryNodeData = new List<StoryNodeData>();
        public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
        public List<CommentBlockData> CommentBlockData = new List<CommentBlockData>();
    }
}