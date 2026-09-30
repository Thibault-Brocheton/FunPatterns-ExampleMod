// Example mod: adds a few extra world object paint textures next to the vanilla ones.
// Server side: declare the textures here. Client side: a PaintTextureLibrary in the mod scene holds one seamless mask per texture,
// named exactly like the texture Name (FunHearts.png for "FunHearts"). Black = background, red = color 2, green = color 3.

namespace FunPatterns
{
    using System.Collections.Generic;
    using Eco.Gameplay.Painting;
    using Eco.Shared.Localization;

    public class FunPaintTextures : PaintTextureSet
    {
        static readonly LocString Group = Localizer.DoStr("Fun");

        public override IEnumerable<PaintTexture> Textures => new[]
        {
            //               Name         Display name                   Group  Colors Size (m) Order  Tooltip description
            new PaintTexture("FunHearts", Localizer.DoStr("Hearts"),     Group, 2,     1.5f,    900,   Localizer.DoStr("Scattered hearts over the background.")),
            new PaintTexture("FunStars",  Localizer.DoStr("Stars"),      Group, 3,     1.5f,    901,   Localizer.DoStr("Stars: background, then the stars, then their centers.")),
            new PaintTexture("FunPaws",   Localizer.DoStr("Paw Prints"), Group, 2,     2.0f,    902,   Localizer.DoStr("Trails of paw prints over the background.")),
            new PaintTexture("FunCandy",  Localizer.DoStr("Candy Cane"), Group, 3,     1.0f,    903,   Localizer.DoStr("Diagonal stripes: background, then wide stripes, then thin ones.")),
        };
    }
}
