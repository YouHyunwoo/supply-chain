using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class Node : MonoBehaviour
{
    private static readonly int ProgressPropertyId = Shader.PropertyToID("_Progress");

    [SerializeField] private bool _isSelectable = true;
    [SerializeField] private bool _lockOnAwake = false;
    [SerializeField] private bool _showProgressOnAwake = false;
    [SerializeField] private int _maxOutputCount = 1;
    private MaterialPropertyBlock _materialPropertyBlock;

    protected GameObject _selector;
    protected SpriteRenderer _lockSpriteRenderer;
    protected SpriteRenderer _progressSpriteRenderer;
    protected LineRenderer _lineRenderer;
    private int _outputCount = 0;
    private bool _isSelected = false;

    public List<TransportLine> transportLines = new ();

    public int OutputCount => _outputCount;
    public int MaxOutputCount => _maxOutputCount;

    protected virtual void Awake()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
        _selector = transform.Find("Selector").gameObject;
        _lockSpriteRenderer = transform.Find("Lock").GetComponent<SpriteRenderer>();
        _progressSpriteRenderer = transform.Find("Progress").Find("Bar").GetComponent<SpriteRenderer>();
        _lineRenderer = GetComponent<LineRenderer>();
        if (_lockOnAwake)
        {
            SetLocked(true);
        }
        if (_showProgressOnAwake)
        {
            ShowProgress(true);
            SetProgress(0f);
        }
    }

    private void Update()
    {
        UpdateTransportLines();
    }

    private void UpdateTransportLines()
    {
        foreach (var transportLine in transportLines)
        {
            transportLine.Update();
        }
    }

    public void UpdateTransportLineCandidate(List<Vector2Int> locations)
    {
        _lineRenderer.positionCount = locations.Count;

        for (int i = 0; i < locations.Count - 1; i++)
        {
            var fromNode = locations[i];
            var toNode = locations[i + 1];
            _lineRenderer.SetPosition(i, new Vector3(fromNode.x, fromNode.y, 0));
            _lineRenderer.SetPosition(i + 1, new Vector3(toNode.x, toNode.y, 0));
        }
    }

    public void AddTransportLine(List<Vector2Int> locations)
    {
        if (transportLines.Count > 0) _outputCount--;
        transportLines.Clear();
        var transportLine = new TransportLine
        {
            startingNode = this,
            locations = new List<Vector2Int>(locations)
        };
        transportLines.Add(transportLine);
        _outputCount++;
    }

    public virtual void ReceiveCargo(Cargo cargo) { }

    public void ShowSelector(bool visible)
    {
        _selector.SetActive(visible);
    }

    public void SetLocked(bool locked)
    {
        _lockSpriteRenderer.enabled = locked;
    }

    public void ShowProgress(bool show)
    {
        _progressSpriteRenderer.transform.parent.gameObject.SetActive(show);
    }

    public void SetProgress(float progress)
    {
        _progressSpriteRenderer.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetFloat(ProgressPropertyId, progress);
        _progressSpriteRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    public virtual void OnPointerEnter(BaseEventData eventData)
    {
        if (!_isSelectable) return;

        _isSelected = true;
        ShowSelector(true);
    }

    public virtual void OnPointerExit(BaseEventData eventData)
    {
        if (!_isSelected && !_isSelectable) return;

        _isSelected = false;
        ShowSelector(false);
    }
}