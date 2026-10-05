using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

/// An item's sprite and how big it is on screen.
public readonly record struct ItemArt(ISprite Sprite, Point Size);
