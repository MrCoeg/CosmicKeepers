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
    public class GraphUitility
    {
        private List<Edge> Edges => _graphView.edges.ToList();
        private List<DialogueNode> Nodes => _graphView.nodes.ToList().Cast<DialogueNode>().ToList();
        private List<StoryNode> storyNodes => _graphView.nodes.ToList().Cast<StoryNode>().ToList();

        private GraphView _graphView;
        private DialogueContainer _dialogueContainer;
        private StoryContainer _storyContainer;

        public static GraphUitility GetInstance(GraphView graphView)
        {
            return new GraphUitility
            {
                _graphView = graphView
            };
        }

        public void ConnectDialogueNodes()
        {
            Type branchingStories = new BranchingStoriesGraphView(new BranchingStoriesGraph()).GetType();
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
            Type branchingStories = new BranchingStoriesGraphView(new BranchingStoriesGraph()).GetType();
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
    }
}

