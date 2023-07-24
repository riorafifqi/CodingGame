using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticLaser : Laser
{
    private const string MATERIAL_EMISSION_COLOR = "_EmissionColor";

    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    private Material blockMaterial;

    private void OnEnable()
    {
        EventManager.OnMovementFinishE += ToggleLaser;
        EventManager.OnResetLevelE += ResetLaser;
    }

    private void OnDisable()
    {
        EventManager.OnMovementFinishE -= ToggleLaser;
        EventManager.OnResetLevelE -= ResetLaser;
    }

    protected override void FetchComponent()
    {
        base.FetchComponent();
        blockMaterial = GetComponent<Renderer>().material;
        ResetLaser();
    }

    public void ToggleLaser()
    {
        if (isActive)
        {
            // turn off laser
            blockMaterial.SetColor(MATERIAL_EMISSION_COLOR, inactiveColor);
            isActive = false;
        }
        else
        {
            // turn on laser
            blockMaterial.SetColor(MATERIAL_EMISSION_COLOR, activeColor);
            isActive = true;
            SetActiveLineRenderer(true);
        }
    }

    private void ResetLaser()
    {
        // turn on laser
        blockMaterial.SetColor(MATERIAL_EMISSION_COLOR, activeColor);
        isActive = true;
        SetActiveLineRenderer(true);
    }
}

