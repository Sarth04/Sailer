using UnityEngine;
using UnityEngine.UIElements;

public class SailHud : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private SailController _sailController;

    private Label _sailOpennessLabel;
    private Label _sailRotationLabel;

    private void Start()
    {
        VisualElement root = _uiDocument.rootVisualElement;
        _sailOpennessLabel = root.Q<Label>("sail-openness-label");
        _sailRotationLabel = root.Q<Label>("sail-rotation-label");
    }

    private void Update()
    {
        _sailOpennessLabel.text = $"Sail openness: {_sailController.SailOpenness:0}%";
        _sailRotationLabel.text = $"Sail rotation: {_sailController.SailRotation:0}°";
    }
}