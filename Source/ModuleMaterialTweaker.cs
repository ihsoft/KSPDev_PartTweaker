﻿// Material settings tweaker.
// Author: igor.zavoychinskiy@gmail.com 
// License: Public Domain.

using System;
using System.Linq;
using UnityEngine;

namespace KSPDev.PartTweaker {

/// <summary>Simple module that allows overriding renderer material color property.</summary>
public sealed class ModuleMaterialTweaker : PartModule {

  /// <summary>Color property name in the material to change.</summary>
  /// <seealso href="https://docs.unity3d.com/Manual/MaterialsAccessingViaScript.html">
  /// Unity 3D: Material parameters</seealso>
  [KSPField]
  public string materialVarName = "";

  /// <summary>Name of the color to present in the editor.</summary>
  [KSPField]
  public string varDisplayName = "";

  /// <summary>Specifies if Alpha component is allowed to be customized.</summary>
  [KSPField]
  public bool showAlpha;

  /// <summary>Specifies if RGB components are allowed to be customized.</summary>
  [KSPField]
  public bool showRGB = true;

  /// <summary>Specifies if color adjusting settings should eb available in flight.</summary>
  [KSPField]
  public bool allowInFlight;

  /// <summary>Comma separated list of model names to affect. Case-insensitive.</summary>
  [KSPField]
  public string modelNames = "";

  /// <summary>R component of the color.</summary>
  [KSPField(isPersistant = true, guiName = "R", guiActiveEditor = true),
   UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
  public float colorR = 1f;

  /// <summary>G component of the color.</summary>
  [KSPField(isPersistant = true, guiName = "G", guiActiveEditor = true),
   UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
  public float colorG = 1f;

  /// <summary>B component of the color.</summary>
  [KSPField(isPersistant = true, guiName = "B", guiActiveEditor = true),
   UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
  public float colorB = 1f;
  
  /// <summary>Alpha component of the color.</summary>
  [KSPField(isPersistant = true, guiName = "Alpha", guiActiveEditor = true),
   UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
  public float colorA = 1f;

  #region PartModule overrides
  /// <inheritdoc/>
  public override void OnInitialize() {
    // Use initialize instead of Start() due to the latter is not called in the editor.
    base.OnInitialize();
    UpdateColor();
    SetColorComponent(Fields["colorR"], varDisplayName + " R");
    SetColorComponent(Fields["colorG"], varDisplayName + " G");
    SetColorComponent(Fields["colorB"], varDisplayName + " B");
    SetAlphaComponent(Fields["colorA"], varDisplayName + " Alpha");
  }

  /// <inheritdoc/>
  public override void OnAwake() {
    base.OnAwake();
    if (HighLogic.LoadedSceneIsEditor || allowInFlight) {
      if (showRGB) {
        Fields["colorR"].OnValueModified += (x => UpdateColor());
        Fields["colorG"].OnValueModified += (x => UpdateColor());
        Fields["colorB"].OnValueModified += (x => UpdateColor());
      }
      if (showAlpha) {
        Fields["colorA"].OnValueModified += (x => UpdateColor());
      }
    }
  }
  #endregion

  #region Local utility methods
  void UpdateColor() {
    var arrModelNames = modelNames.Split(',').Select(x => x.Trim().ToLower());
    var materials = part.FindModelComponents<Renderer>()
        .Where(x => arrModelNames.Contains(x.name.ToLower()))
        .Select(x => x.material);
    var newColor = new Color(colorR, colorG, colorB, colorA);
    foreach (var material in materials) {
      Debug.LogFormat("Adjust color on {0}: name={1}, value={2}, model={3}",
                      part.name, materialVarName, newColor, material.name);
      material.SetColor(materialVarName, newColor);
    }
  }

  void SetColorComponent(BaseField field, string guiName) {
    field.guiName = guiName;
    field.guiActiveEditor = showRGB;
    field.guiActive = showRGB && allowInFlight;
  }

  void SetAlphaComponent(BaseField field, string guiName) {
    field.guiName = guiName;
    field.guiActiveEditor = showAlpha;
    field.guiActive = showAlpha && allowInFlight;
  }
  #endregion
}

}  // namespace
