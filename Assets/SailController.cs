using UnityEngine;
using UnityEngine.InputSystem;

public sealed class SailController : MonoBehaviour
{
    [SerializeField] private Transform _sailScalePivot;
    [SerializeField] private Transform _mastPivot;
    [SerializeField, Range(0f, 100f)] private float _startingSailOpenness;
    [SerializeField] private InputActionReference _adjustSailFurling;
    [SerializeField] private InputActionReference _adjustSailRotation;
    [SerializeField, Range(0.001f, 0.2f)] private float _minimumClosedSailScale = 0.01f;
    [SerializeField, Range(0f, 45f)] private float _maxSailRotation = 45f;
    [SerializeField, Min(0f)] private float _sailOpenSpeed = 60f;
    [SerializeField, Min(0f)] private float _sailRotationSpeed = 30f;

    private Vector3 _startingSailScale;
    private Quaternion _startingMastRotation;
    private float _sailOpenness;
    private float _sailRotation;

    private void Start()
    {
        if (_sailScalePivot == null)
        {
            Debug.LogError("Assign the SailScalePivot transform in the Inspector.", this);
            enabled = false;
            return;
        }

        if (_adjustSailFurling == null)
        {
            Debug.LogError("SailController: Assign a Sail Furling InputActionReference in the Inspector.", this);
            enabled = false;
            return;
        }

        if (_adjustSailFurling.action == null)
        {
            Debug.LogError("SailController: The assigned Sail Furling InputActionReference does not resolve to an action.", this);
            enabled = false;
            return;
        }

        if (_adjustSailRotation == null)
        {
            Debug.LogError("SailController: Assign a Sail Rotation InputActionReference in the Inspector.", this);
            enabled = false;
            return;
        }

        if (_adjustSailRotation.action == null)
        {
            Debug.LogError("SailController: The assigned Sail Rotation InputActionReference does not resolve to an action.", this);
            enabled = false;
            return;
        }

        if (_mastPivot == null)
        {
            Debug.LogError("SailController: Assign the MastPivot transform in the Inspector.", this);
            enabled = false;
            return;
        }

        _sailOpenness = _startingSailOpenness;
        _startingSailScale = _sailScalePivot.localScale;
        _startingMastRotation = _mastPivot.localRotation;
        UpdateSailScale();
    }

    private void Update()
    {
        float furlingInput = _adjustSailFurling.action.ReadValue<float>();
        float rotationInput = _adjustSailRotation.action.ReadValue<float>();

        if (Mathf.Approximately(furlingInput, 0f) && Mathf.Approximately(rotationInput, 0f))
        {
            return;
        }

        float newSailOpenness = Mathf.Clamp(_sailOpenness + furlingInput * _sailOpenSpeed * Time.deltaTime, 0f, 100f);
        float newSailRotation = Mathf.Clamp(_sailRotation + rotationInput * _sailRotationSpeed * Time.deltaTime, -_maxSailRotation, _maxSailRotation);

        bool opennessChanged = !Mathf.Approximately(newSailOpenness, _sailOpenness);
        bool rotationChanged = !Mathf.Approximately(newSailRotation, _sailRotation);

        if (opennessChanged)
        {
            _sailOpenness = newSailOpenness;
            UpdateSailScale();
        }

        if (rotationChanged)
        {
            _sailRotation = newSailRotation;
            UpdateSailRotation();
        }
    }

    private void UpdateSailScale()
    {
        Vector3 scale = _startingSailScale;
        scale.y = scale.y * Mathf.Lerp(_minimumClosedSailScale, 1f, _sailOpenness / 100f);
        _sailScalePivot.localScale = scale;
    }

    private void UpdateSailRotation()
    {
        _mastPivot.localRotation = _startingMastRotation * Quaternion.AngleAxis(_sailRotation, Vector3.up);
    }
}