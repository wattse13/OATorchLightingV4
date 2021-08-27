using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class ToolTip : MonoBehaviour
{
    [SerializeField]
    private Canvas toolTipCanvas;
    [SerializeField]
    private TextMeshProUGUI headerField;
    [SerializeField]
    private TextMeshProUGUI contentField;
    [SerializeField]
    private LayoutElement layoutElement;
    [SerializeField]
    private int characterWrapLimit;

    private void Awake()
    {
        toolTipCanvas.enabled = false;
    }

    private void OnEnable()
    {
        OnClickDelegate.OnHovered += ShowToolTip;
        OnClickDelegate.OnHoverExit += HideToolTip;
    }

    private void OnDisable()
    {
        OnClickDelegate.OnHovered -= ShowToolTip;
        OnClickDelegate.OnHoverExit -= HideToolTip;
    }

    private void Update()
    {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    public void ShowToolTip(GameObject myHoverPrefab)
    {
        toolTipCanvas.enabled = true;
        
        if (TryGetComponent(out EquipmentClass equipment))
        {
            headerField.text = equipment.Name;
            contentField.text = equipment.DescriptionCurrent;
        }
    }

    public void HideToolTip(GameObject equipment)
    {
        toolTipCanvas.enabled = false;
    }
}
