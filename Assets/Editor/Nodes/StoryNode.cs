using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Subtegral.DialogueSystem.Editor
{
    public class StoryNode : Node
    {
        public string DialogueName;
        public string path;
        public string GUID;
        public bool EntyPoint = false;
    }
}
