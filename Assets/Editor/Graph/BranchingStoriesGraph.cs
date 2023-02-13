using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Subtegral.DialogueSystem.DataContainers;


namespace Subtegral.DialogueSystem.Editor
{
    public class BranchingStoriesGraph : EditorWindow
    {
        private string _fileName = "New Branching Stories";

        private BranchingStoriesGraphView _graphView;
        private DialogueContainer _dialogueContainer;

        [MenuItem("Graph/Branching Stories Graph")]
        public static void CreateGraphViewWindow()
        {
            var window = GetWindow<BranchingStoriesGraph>();
            window.titleContent = new GUIContent("Branching Stories Graph");
        }

        private void ConstructGraphView()
        {
            _graphView = new BranchingStoriesGraphView(this)
            {
                name = "Branching Stories Graph",
            };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });

            toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });
            // toolbar.Add(new Button(() => _graphView.CreateNewDialogueNode("Dialogue Node")) {text = "New Node",});
            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (!string.IsNullOrEmpty(_fileName))
            {
                var saveUtility = GraphSaveUtility.GetInstance(_graphView);
                if (save)
                    saveUtility.SaveStoryGraph(_fileName);
                else
                    saveUtility.LoadNarrative(_fileName, true);
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid File name", "Please Enter a valid filename", "OK");
            }
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap { anchored = true };
            var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            _graphView.Add(miniMap);
        }

        private void GenerateBlackBoard()
        {
            var blackboard = new Blackboard(_graphView);
            blackboard.Add(new BlackboardSection { title = "Exposed Variables" });
            blackboard.addItemRequested = _blackboard =>
            {
                _graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
            };
            blackboard.editTextRequested = (_blackboard, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField)element).text;
                if (_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
                        "OK");
                    return;
                }

                var targetIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                _graphView.ExposedProperties[targetIndex].PropertyName = newValue;
                ((BlackboardField)element).text = newValue;
            };
            blackboard.SetPosition(new Rect(10, 30, 200, 300));
            _graphView.Add(blackboard);
            _graphView.Blackboard = blackboard;
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.DragExited)
            {

                string path = DragAndDrop.paths[0];
                Debug.Log(path);
                string fileName = path.Split('/')[path.Split('/').Length - 1];
                fileName = fileName.Split('.')[0];

                Debug.Log("Story : " + path.Contains("/Story/"));
                Debug.Log("Story : " + path.Contains("/Dialogue/"));
                if (path.Contains("Story"))
                {
                    var saveUtility = GraphSaveUtility.GetInstance(_graphView);
                    saveUtility.LoadNarrative(fileName, true);
                }
                else if(path.Contains("/Dialogue/"))
                {
                    path = path.Insert(17,"_");
                    path = path.Split('_')[1];

                    var mousePosition = rootVisualElement.ChangeCoordinatesTo(rootVisualElement.parent,
                        Event.current.mousePosition - position.position);
                    var graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePosition);
                    _graphView.CreateNewDialogueNode(fileName + "_" + path, graphMousePosition);
                }
            }
        }

        
    }
}



