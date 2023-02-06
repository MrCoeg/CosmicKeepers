using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Subtegral.DialogueSystem.DataContainers;
using UnityEngine.UIElements;

namespace Subtegral.DialogueSystem.Editor
{
    public class GraphSaveUtility
    {
        private List<Edge> Edges => _graphView.edges.ToList();
        private List<DialogueNode> Nodes => _graphView.nodes.ToList().Cast<DialogueNode>().ToList();
        private List<StoryNode> storyNodes => _graphView.nodes.ToList().Cast<StoryNode>().ToList();


        private List<Group> CommentBlocks =>
            _graphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        private DialogueContainer _dialogueContainer;
        private StoryContainer _storyContainer;

        private GraphView _graphView;
        public static GraphSaveUtility GetInstance(GraphView graphView)
        {
            return new GraphSaveUtility
                {
                    _graphView = graphView
                };
        }

        public bool SaveGraph(string fileName)
        {
            var dialogueContainerObject = ScriptableObject.CreateInstance<DialogueContainer>();
            if (!SaveNodes(fileName, dialogueContainerObject)) return false;
            SaveExposedProperties(dialogueContainerObject);
            SaveCommentBlocks(dialogueContainerObject);

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            string path = $"Assets/Resources/DialogueSystem/Dialogue/{fileName}.asset";

            UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath(path, typeof(DialogueContainer));

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset)) 
			{
                AssetDatabase.CreateAsset(dialogueContainerObject, path);
            }
            else 
			{
                DialogueContainer container = loadedAsset as DialogueContainer;
                container.path = path;
                container.NodeLinks = dialogueContainerObject.NodeLinks;
                container.DialogueNodeData = dialogueContainerObject.DialogueNodeData;
                container.ExposedProperties = dialogueContainerObject.ExposedProperties;
                container.CommentBlockData = dialogueContainerObject.CommentBlockData;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
            return true;
        }
        private bool SaveNodes(string fileName, DialogueContainer dialogueContainerObject)
        {
            if (!Edges.Any()) return false;
            var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
            for (var i = 0; i < connectedSockets.Count(); i++)
            {

                var outputNode = (connectedSockets[i].output.node as DialogueNode);
                var inputNode = (connectedSockets[i].input.node as DialogueNode);
                dialogueContainerObject.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedSockets[i].output.portName,
                    TargetNodeGUID = inputNode.GUID
                });
            }

            foreach (var node in Nodes.Where(node => !node.EntyPoint))
            {
                dialogueContainerObject.DialogueNodeData.Add(new DialogueNodeData
                {
                    NodeGUID = node.GUID,
                    DialogueText = node.DialogueText,
                    Position = node.GetPosition().position
                });
            }

            return true;
        }
        private void SaveExposedProperties(DialogueContainer dialogueContainer)
        {
            Type branchingStories = new BranchingStoriesGraphView(ScriptableObject.CreateInstance<BranchingStoriesGraph>()).GetType();
            if (_graphView.GetType() != branchingStories)
            {
                dialogueContainer.ExposedProperties.Clear();
                var storyGraph = (StoryGraphView)_graphView;
                dialogueContainer.ExposedProperties.AddRange(storyGraph.ExposedProperties);
            }

        }
        private void SaveCommentBlocks(DialogueContainer dialogueContainer)
        {
            foreach (var block in CommentBlocks)
            {
                var nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>().Select(x => x.GUID)
                    .ToList();

                dialogueContainer.CommentBlockData.Add(new CommentBlockData
                {
                    ChildNodes = nodes,
                    Title = block.title,
                    Position = block.GetPosition().position
                });
            }
        }


        public void SaveStoryGraph(string fileName)
        {
            var dialogueContainerObject = ScriptableObject.CreateInstance<StoryContainer>();
            if (!SaveStoryNodes(fileName, dialogueContainerObject)) return;
            SaveStoryExposedProperties(dialogueContainerObject);
            SaveStoryCommentBlocks(dialogueContainerObject);

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/DialogueSystem/Story/{fileName}.asset", typeof(StoryContainer));

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset))
            {
                AssetDatabase.CreateAsset(dialogueContainerObject, $"Assets/Resources/DialogueSystem/Story/{fileName}.asset");
            }
            else
            {
                StoryContainer container = loadedAsset as StoryContainer;
                container.NodeLinks = dialogueContainerObject.NodeLinks;
                container.StoryNodeData = dialogueContainerObject.StoryNodeData;
                container.ExposedProperties = dialogueContainerObject.ExposedProperties;
                container.CommentBlockData = dialogueContainerObject.CommentBlockData;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
        }
        private bool SaveStoryNodes(string fileName, StoryContainer storyContainerObject)
        {
            if (!Edges.Any()) return false;
            var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
            for (var i = 0; i < connectedSockets.Count(); i++)
            {
                var outputNode = (connectedSockets[i].output.node as StoryNode);
                var inputNode = (connectedSockets[i].input.node as StoryNode);
                storyContainerObject.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedSockets[i].output.portName,
                    TargetNodeGUID = inputNode.GUID
                });
            }

            foreach (var node in storyNodes.Where(node => !node.EntyPoint))
            {
                storyContainerObject.StoryNodeData.Add(new StoryNodeData
                {
                    NodeGUID = node.GUID,
                    dialogueName = node.DialogueName,
                    path = node.path,
                    Position = node.GetPosition().position
                });

            }

            return true;
        }
        private void SaveStoryExposedProperties(StoryContainer storyContainer)
        {
            storyContainer.ExposedProperties.Clear();
            var storyGraph = (BranchingStoriesGraphView)_graphView;
            storyContainer.ExposedProperties.AddRange(storyGraph.ExposedProperties);
        }
        private void SaveStoryCommentBlocks(StoryContainer storyContainer)
        {
            foreach (var block in CommentBlocks)
            {
                var nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>().Select(x => x.GUID)
                    .ToList();

                storyContainer.CommentBlockData.Add(new CommentBlockData
                {
                    ChildNodes = nodes,
                    Title = block.title,
                    Position = block.GetPosition().position
                });
            }
        }

        public void LoadNarrative(string fileName, bool isStory)
        {

            if (isStory)
            {
                _storyContainer = Resources.Load<StoryContainer>($"DialogueSystem/Story/{fileName}");

                if (_storyContainer == null)
                {
                    EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
                    return;
                }
            }
            else
            {
                _dialogueContainer = Resources.Load<DialogueContainer>($"DialogueSystem/Dialogue/{fileName}");

                if (_dialogueContainer == null)
                {
                    EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
                    return;
                }
            }

            ClearGraph();
            GenerateDialogueNodes();
            ConnectDialogueNodes();
            AddExposedProperties();
            GenerateCommentBlocks();
        }



        /// <summary>
        /// Set Entry point GUID then Get All Nodes, remove all and their edges. Leave only the entrypoint node. (Remove its edge too)
        /// </summary>
        private void ClearGraph()
        {
            Type branchingStories = new BranchingStoriesGraphView(ScriptableObject.CreateInstance<BranchingStoriesGraph>()).GetType();
            if (_graphView.GetType() != branchingStories)
            {
                var storyGraph = (StoryGraphView)_graphView;
                Nodes.Find(x => x.EntyPoint).GUID = _dialogueContainer.NodeLinks[0].BaseNodeGUID;
                foreach (var perNode in Nodes)
                {
                    if (perNode.EntyPoint) continue;
                    Edges.Where(x => x.input.node == perNode).ToList()
                        .ForEach(edge => _graphView.RemoveElement(edge));
                    storyGraph.RemoveElement(perNode);
                }
            }
            else
            {
                var storyGraph = (BranchingStoriesGraphView)_graphView;
                storyNodes.Find(x => x.EntyPoint).GUID = _storyContainer.NodeLinks[0].BaseNodeGUID;
                foreach (var perNode in storyNodes)
                {
                    if (perNode.EntyPoint) continue;
                    Edges.Where(x => x.input.node == perNode).ToList()
                        .ForEach(edge => _graphView.RemoveElement(edge));
                    storyGraph.RemoveElement(perNode);
                }
            }

        }

        /// <summary>
        /// Create All serialized nodes and assign their guid and dialogue text to them
        /// </summary>
        private void GenerateDialogueNodes()
        {
            Type branchingStories = new BranchingStoriesGraphView(ScriptableObject.CreateInstance<BranchingStoriesGraph>()).GetType();

            if (_graphView.GetType() != branchingStories)
            {
                var storyGraph = (StoryGraphView)_graphView;
                foreach (var perNode in _dialogueContainer.DialogueNodeData)
                {
                    var tempNode = storyGraph.CreateNode(perNode.DialogueText, Vector2.zero);
                    tempNode.GUID = perNode.NodeGUID;
                    storyGraph.AddElement(tempNode);

                    var nodePorts = _dialogueContainer.NodeLinks.Where(x => x.BaseNodeGUID == perNode.NodeGUID).ToList();
                    nodePorts.ForEach(x => storyGraph.AddChoicePort(tempNode, x.PortName));
                }
            }
            else
            {
                var storyGraph = (BranchingStoriesGraphView)_graphView;
                foreach (var perNode in _storyContainer.StoryNodeData)
                {
                    var tempNode = storyGraph.CreateNode(perNode.dialogueName + "_" + perNode.path, Vector2.zero);
                    tempNode.GUID = perNode.NodeGUID;
                    storyGraph.AddElement(tempNode);

                    var nodePorts = _storyContainer.NodeLinks.Where(x => x.BaseNodeGUID == perNode.NodeGUID).ToList();
                    nodePorts.ForEach(x => storyGraph.AddChoicePort(tempNode, x.PortName));
                }
            }
        }

        private void ConnectDialogueNodes()
        {
            Type branchingStories = new BranchingStoriesGraphView(ScriptableObject.CreateInstance<BranchingStoriesGraph>()).GetType();

            if (_graphView.GetType() != branchingStories)
            {
                var storyGraph = (StoryGraphView)_graphView;
                for (var i = 0; i < Nodes.Count; i++)
                {
                    var k = i; //Prevent access to modified closure
                    var connections = _dialogueContainer.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[k].GUID).ToList();
                    for (var j = 0; j < connections.Count(); j++)
                    {
                        var targetNodeGUID = connections[j].TargetNodeGUID;
                        var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                        LinkNodesTogether(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                        targetNode.SetPosition(new Rect(
                            _dialogueContainer.DialogueNodeData.First(x => x.NodeGUID == targetNodeGUID).Position,
                            storyGraph.DefaultNodeSize));
                    }
                }
            }
            else
            {
                var storyGraph = (BranchingStoriesGraphView)_graphView;
                for (var i = 0; i < storyNodes.Count; i++)
                {
                    var k = i; //Prevent access to modified closure
                    var connections = _storyContainer.NodeLinks.Where(x => x.BaseNodeGUID == storyNodes[k].GUID).ToList();
                    for (var j = 0; j < connections.Count(); j++)
                    {
                        var targetNodeGUID = connections[j].TargetNodeGUID;
                        var targetNode = storyNodes.First(x => x.GUID == targetNodeGUID);
                        LinkNodesTogether(storyNodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                        targetNode.SetPosition(new Rect(
                            _storyContainer.StoryNodeData.First(x => x.NodeGUID == targetNodeGUID).Position,
                            storyGraph.DefaultNodeSize));
                    }
                }
            }

        }

        private void LinkNodesTogether(Port outputSocket, Port inputSocket)
        {
            Type branchingStories = new BranchingStoriesGraphView(ScriptableObject.CreateInstance<BranchingStoriesGraph>()).GetType();

            if (_graphView.GetType() != branchingStories)
            {
                var storyGraph = (StoryGraphView)_graphView;
                var tempEdge = new Edge()
                {
                    output = outputSocket,
                    input = inputSocket
                };
                tempEdge?.input.Connect(tempEdge);
                tempEdge?.output.Connect(tempEdge);
                storyGraph.Add(tempEdge);
            }
            else
            {
                var storyGraph = (BranchingStoriesGraphView)_graphView;
                var tempEdge = new Edge()
                {
                    output = outputSocket,
                    input = inputSocket
                };
                tempEdge?.input.Connect(tempEdge);
                tempEdge?.output.Connect(tempEdge);
                storyGraph.Add(tempEdge);
            }

        }

        private void AddExposedProperties()
        {
            Type branchingStories = new BranchingStoriesGraphView(ScriptableObject.CreateInstance<BranchingStoriesGraph>()).GetType();

            if (_graphView.GetType() != branchingStories)
            {
                var storyGraph = (StoryGraphView)_graphView;
                storyGraph.ClearBlackBoardAndExposedProperties();
                foreach (var exposedProperty in _dialogueContainer.ExposedProperties)
                {
                    storyGraph.AddPropertyToBlackBoard(exposedProperty);
                }
            }
            else
            {
                var storyGraph = (BranchingStoriesGraphView)_graphView;
                storyGraph.ClearBlackBoardAndExposedProperties();
                foreach (var exposedProperty in _storyContainer.ExposedProperties)
                {
                    storyGraph.AddPropertyToBlackBoard(exposedProperty);
                }
            }

        }

        private void GenerateCommentBlocks()
        {
            Type branchingStories = new BranchingStoriesGraphView(ScriptableObject.CreateInstance<BranchingStoriesGraph>()).GetType();

            if (_graphView.GetType() != branchingStories)
            {
                var storyGraph = (StoryGraphView)_graphView;
                foreach (var commentBlock in CommentBlocks)
                {
                    storyGraph.RemoveElement(commentBlock);
                }

                foreach (var commentBlockData in _dialogueContainer.CommentBlockData)
                {
                    var block = storyGraph.CreateCommentBlock(new Rect(commentBlockData.Position, storyGraph.DefaultCommentBlockSize),
                         commentBlockData);
                    block.AddElements(Nodes.Where(x => commentBlockData.ChildNodes.Contains(x.GUID)));
                }
            }
            else
            {
                var storyGraph = (BranchingStoriesGraphView)_graphView;
                foreach (var commentBlock in CommentBlocks)
                {
                    storyGraph.RemoveElement(commentBlock);
                }

                foreach (var commentBlockData in _storyContainer.CommentBlockData)
                {
                    var block = storyGraph.CreateCommentBlock(new Rect(commentBlockData.Position, storyGraph.DefaultCommentBlockSize),
                         commentBlockData);
                    block.AddElements(storyNodes.Where(x => commentBlockData.ChildNodes.Contains(x.GUID)));
                }
            }

        }
    }
}