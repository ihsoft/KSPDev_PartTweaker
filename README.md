# PartTweaker

Various modules to tweak different settings on the part or its model(s).

## ModuleMaterialTweaker

Replaces the specified color parameter on the requested models.

```
MODULE
{
	name = ModuleMaterialTweaker
	modelNames = planet1,planet2
	varDisplayName = Color
	materialVarName = _Color
	showAlpha = false
	showRGB = true
	allowInFlight = false
	colorR = 1.0
	colorG = 0.5
	colorB = 1.0
	colorA = 1.0
}
```
`modelNames` values are case-insensitive.

See sources for the defaults and comments.

Value for `materialVarName` depends on the shader used. See Unity docs for more details.

## ModuleModelTweaker

Hides (deletes, to be more specific) renderers on the specified models.

```
MODULE
{
	name = ModuleMeshVisibility
	modelNames = planet1,planet2
	visibleInScene = EditorOnly
}
```

Allowed values for `visibleInScene`:
* `EditorOnly`
* `FlightOnly`
* `Any` - same as no module at all.
* `IconOnly` - only present meshes in the icon.
