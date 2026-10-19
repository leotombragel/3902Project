namespace CSE3902Project.Game.Collision;

/// which side of the player hit a tile
public enum CollisionSide
{
    None,
    Left,
    Right,
    Top, //head hit something above
    Bottom //feet landed on something
}
