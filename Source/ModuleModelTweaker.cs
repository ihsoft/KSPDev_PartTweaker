﻿// Material settings tweaker.
// Author: igor.zavoychinskiy@gmail.com 
// License: Public Domain.

using System;
using System.Linq;
using UnityEngine;

namespace KSPDev.PartTweaker {

/// <summary>Simple module that allows hiding selected model objects.</summary>
public sealed class ModuleModelTweaker : PartModule {

  /// <summary>Scenes when the meshes are visible.</summary>
  public enum VisibilityState {
    /// <summary>In the editor only.</summary>
    EditorOnly,
    /// <summary>In flight only.</summary>
    FlightOnly,
    /// <summary>Always.</summary>
    Any,
    /// <summary>Drop from all scenes but leave in the icon.</summary>
    IconOnly,
  }

  /// <summary>Comma separated list of model names to affect. Case-insensitive.</summary>
  [KSPField]
  public string modelNames = "";

  /// <summary>Defines when the model meshes should be visible.</summary>
  [KSPField]
  public VisibilityState visibleInScene = VisibilityState.Any;

  #region PartModule overrides
  /// <inheritdoc/>
  public override void OnInitialize() {
    // Use initialize instead of Start() due to the latter is not called in the editor.
    UpdateVisibility();
  }
  #endregion

  #region Local utility methods
  void UpdateVisibility() {
    if (visibleInScene == VisibilityState.Any
        || HighLogic.LoadedSceneIsEditor && visibleInScene == VisibilityState.EditorOnly
        || HighLogic.LoadedSceneIsFlight && visibleInScene == VisibilityState.FlightOnly) {
      return;
    }
    var arrModelNames = modelNames.Split(',').Select(x => x.Trim().ToLower());
    var objects = part.FindModelComponents<Transform>()
        .Where(x => arrModelNames.Contains(x.name.ToLower()))
        .Select(x => x.gameObject);
    foreach (var obj in objects) {
      if (obj == part.gameObject) {
        Debug.LogWarningFormat("Cannot drop root part's object {0} on {1}. Ignoring",
                               obj.name, part.name);
      } else {
        Debug.LogFormat("Drop object {0} on {1}", obj.name, part.name);
        DestroyImmediate(obj);
      }
    }
  }
  #endregion
}

}  // namespace
