# FunPatterns — Example paint pattern and paint texture mod for Eco 14.2

A small sample mod showing how to add your own **block paint patterns** and **world object paint textures** in Eco **14.2**.
It adds a "Fun" group to both pickers:

* Block patterns: smiley, heart, skull, paw print, ghost and mushroom.
* World object textures: hearts, stars, paw prints and candy cane.

> Requires Eco **14.2** or later. Paint patterns, layered face painting and world object paint textures do not exist in earlier versions.

## Install

Copy the files into your server's `Mods/UserCode/FunPatterns/` folder:

```
Mods/UserCode/FunPatterns/
├── FunPaintPatterns.cs
├── FunPaintTextures.cs
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

## How world object paint textures work

Paintable world objects have up to 3 paint areas. Aim a paint sprayer at one and hold the construct key to open the
texture picker. A texture is a seamless mask repeated over the area, with 2 or 3 colors painted one coat at a time:
the background first, then the mask colors. A textured area is locked until it is cleared.

| Side   | What                                                              | In this mod                             |
|--------|-------------------------------------------------------------------|-----------------------------------------|
| Server | Declares the texture: name, display name, group, colors, size...  | `FunPaintTextures.cs` (`PaintTextureSet`) |
| Client | The mask texture                                                  | `PaintTextureLibrary` in `Client/FunPatterns.unity` |

### Server: `PaintTextureSet`

```csharp
public class FunPaintTextures : PaintTextureSet
{
    static readonly LocString Group = Localizer.DoStr("Fun");

    public override IEnumerable<PaintTexture> Textures => new[]
    {
        //               Name         Display name               Group  Colors Size (m) Order  Tooltip (optional)
        new PaintTexture("FunHearts", Localizer.DoStr("Hearts"), Group, 2,     1.5f,    900,   Localizer.DoStr("Scattered hearts...")),
        new PaintTexture("FunStars",  Localizer.DoStr("Stars"),  Group, 3,     1.5f,    901,   Localizer.DoStr("Stars: background, then...")),
        ...
    };
}
```

* **Colors** counts the background: 2 (background + red) or 3 (background + red + green).
* **Size** is how many meters one repeat of the mask covers, whatever the object's scale.
* Same rules as patterns for **Name** (unique, prefixed) and **Order** (vanilla uses 100–299, use 900+).

### Client: mask textures

* Square PNG, 512×512 recommended, in `Client/Textures/`.
* **It must tile seamlessly**: left edge continues on the right one, top on the bottom.
* **Black = background (color 1), red = color 2, green = color 3.** Green paints over red; blue is ignored.
  `FunStars` uses red stars with green centers, `FunCandy` wide red stripes and thin green ones.
* The mask is projected from three sides, so avoid details that only read in one direction.
* Import settings: **sRGB (Color Texture) off**, **Wrap Mode = Repeat**.
* The file name must be **exactly the texture Name** (`FunHearts.png` for `"FunHearts"`).

In the mod scene: a second disabled root game object, `PaintTextures`, with a `PaintTextureLibrary` component.

## Make your own

1. Copy this mod and rename everything (folder, namespace, classes, bundle, pattern and texture names).
   Keep only the half you need: delete `FunPaintTextures.cs` and the `PaintTextures` object for patterns only, or the reverse.
2. Replace the PNGs in `Client/Patterns/` and `Client/Textures/` and the entries in the `.cs` files, keeping names in sync.
3. Rebuild the bundle with the Eco ModKit and drop the `.cs` files and the bundle in `Mods/UserCode/<YourMod>/`.

## Good to know

* A server pattern without a client texture is hidden from the picker (a warning is logged on the client).
* Patterns are saved by name. If the mod is removed, painted faces keep their layers but show nothing
  ("Missing pattern"); reinstalling the mod brings them back.
* Same for textures: a server texture without a client mask is hidden, and a removed mod leaves textured areas with
  their color only, clearable until the mod comes back.

See `ModdingPaintPatterns.md` and `ModdingPaintTextures.md` in the Eco ModKit docs for the full guide.
