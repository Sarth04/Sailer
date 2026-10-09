using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class SailController : MonoBehaviour
{
    [SerializeField] private Transform _sailScalePivot;
    [SerializeField, Range(0f, 100f)] private float _sailOpenness;
    [SerializeField, Min(0f)] private float _sailOpenSpeed = 60f;
    [SerializeField, Range(0.001f, 0.2f)] private float _minimumClosedSailScale = 0.01f;
    [SerializeField] private InputActionReference _adjustSailAction;

    private Vector3 _openScale;

    private void Start()
    {
        if (_sailScalePivot == null)
        {
            Debug.LogError("Assign the SailScalePivot transform in the Inspector.", this);
            enabled = false;
            return;
        }

        if (_adjustSailAction == null)
        {
            Debug.LogError("SailController: Assign an InputActionReference in the Inspector.", this);
            enabled = false;
            return;
        }

        if (_adjustSailAction.action == null)
        {
            Debug.LogError("SailController: The assigned InputActionReference does not resolve to an action.", this);
            enabled = false;
            return;
        }

        _openScale = _sailScalePivot.localScale;
        UpdateSailScale();
    }

    private void Update()
    {
        float input = _adjustSailAction.action.ReadValue<float>();

        if (Mathf.Approximately(input, 0f))
        {
            return;
        }

        float newSailOpenness = Mathf.Clamp(_sailOpenness + input * _sailOpenSpeed * Time.deltaTime, 0f, 100f);

        if (Mathf.Approximately(newSailOpenness, _sailOpenness))
        {
            return;
        }

        _sailOpenness = newSailOpenness;
        UpdateSailScale();
    }

    private void UpdateSailScale()
    {
        Vector3 scale = _openScale;
        scale.y = _openScale.y * Mathf.Lerp(_minimumClosedSailScale, 1f, _sailOpenness / 100f);
        _sailScalePivot.localScale = scale;
    }
}