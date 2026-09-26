# FunPatterns — Example paint pattern mod for Eco 14.2

A small sample mod showing how to add your own **block paint patterns** in Eco **14.2**.
It adds six patterns in a "Fun" group of the pattern picker: smiley, heart, skull, paw print, ghost and mushroom.

> Requires Eco **14.2** or later. Paint patterns and layered face painting do not exist in earlier versions.

## Install

Copy the files into your server's `Mods/UserCode/FunPatterns/` folder:

```
Mods/UserCode/FunPatterns/
├── FunPaintPatterns.cs
└── funpatterns.unity3d
```

The bundle is sent to players automatically when they join.

## How paint patterns work

Players paint block faces with a paint sprayer. Each face holds up to **3 stacked layers**, and each layer is a
**pattern** (a mask) tinted with the selected paint color. Hold the construct key while holding a paint sprayer to open
the pattern picker. Mod patterns are added next to the vanilla ones and never replace them.

A pattern has two halves, matched by **name**:

| Side   | What                                                          | In this mod                             |
|--------|---------------------------------------------------------------|-----------------------------------------|
| Server | Declares the pattern: name, display name, group, order, tooltip | `FunPaintPatterns.cs` (`PaintPatternSet`) |
| Client | The mask texture                                              | `PaintPatternLibrary` in `Client/FunPatterns.unity` |

### Server: `PaintPatternSet`

```csharp
public class FunPaintPatterns : PaintPatternSet
{
    static readonly LocString Group = Localizer.DoStr("Fun");

    public override IEnumerable<PaintPattern> Patterns => new[]
    {
        //            Name (unique!) Display name              Group  Order  Tooltip (optional)
        new PaintPattern("FunSmiley", Localizer.DoStr("Smiley"), Group, 900,   Localizer.DoStr("A big smile...")),
        ...
    };
}
```

* The class is discovered automatically, no registration needed.
* **Name** must be unique across the game and all mods: prefix it with your mod name (`FunSmiley`, not `Smiley`).
  On a clash, the first one loaded wins.
* **Group** is the row title in the picker. **Order** sorts patterns; vanilla uses 100–599, so use 900+.

### Client: mask textures

* Square PNG, 128×128 or 256×256 (other sizes are resampled to 128×128).
* **Only alpha is used**: draw a white shape on a transparent background; the color comes from the player's paint.
* Draw it **upright**. The mask covers exactly one block face: keep a margin for standalone symbols, touch the edges
  for lines and borders that continue on the next block.
* Import setting: **Alpha Source = Input Texture Alpha**, otherwise the pattern paints the whole face.
* The file name must be **exactly the pattern Name** (`FunSmiley.png` for `"FunSmiley"`).

In the mod scene: a disabled root game object with a `PaintPatternLibrary` component, one element per texture.
Then build the bundle as usual.

## Make your own

1. Copy this mod and rename everything (folder, namespace, class, bundle, pattern names).
2. Replace the PNGs in `Client/Patterns/` and the entries in the `.cs` file, keeping names in sync.
3. Rebuild the bundle with the Eco ModKit and drop both files in `Mods/UserCode/<YourMod>/`.

## Good to know

* A server pattern without a client texture is hidden from the picker (a warning is logged on the client).
* Patterns are saved by name. If the mod is removed, painted faces keep their layers but show nothing
  ("Missing pattern"); reinstalling the mod brings them back.

See `ModdingPaintPatterns.md` in the Eco ModKit docs for the full guide.
