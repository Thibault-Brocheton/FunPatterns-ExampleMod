// Example mod: adds a few extra block paint patterns next to the vanilla ones.
// Server side: declare the patterns here. Client side: a PaintPatternLibrary in the mod scene holds one mask texture per pattern,
// named exactly like the pattern Name (FunSmiley.png for "FunSmiley"). Keep names unique so vanilla patterns are never replaced.

namespace FunPatterns
{
    using System.Collections.Generic;
    using Eco.Gameplay.Painting;
    using Eco.Shared.Localization;

    public class FunPaintPatterns : PaintPatternSet
    {
        static readonly LocString Group = Localizer.DoStr("Fun");

        public override IEnumerable<PaintPattern> Patterns => new[]
        {
            new PaintPattern("FunSmiley",   Localizer.DoStr("Smiley"),     Group, 900, Localizer.DoStr("A big smile, for cheerful doors and welcome mats.")),
            new PaintPattern("FunHeart",    Localizer.DoStr("Heart"),      Group, 901, Localizer.DoStr("A heart, to show some love to your neighbours.")),
            new PaintPattern("FunSkull",    Localizer.DoStr("Skull"),      Group, 902, Localizer.DoStr("A skull, to warn people away from the mine shaft.")),
            new PaintPattern("FunPaw",      Localizer.DoStr("Paw Print"),  Group, 903, Localizer.DoStr("A paw print, leave a trail of them along a path.")),
            new PaintPattern("FunGhost",    Localizer.DoStr("Ghost"),      Group, 904, Localizer.DoStr("A little ghost, for haunted houses.")),
            new PaintPattern("FunMushroom", Localizer.DoStr("Mushroom"),   Group, 905, Localizer.DoStr("A spotted mushroom, straight out of the forest.")),
        };
    }
}
