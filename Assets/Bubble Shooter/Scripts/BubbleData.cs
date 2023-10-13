public enum BubbleColor
{
    Green,
    Yellow,
    Red,
    Blue,
    Pink,
    White
}

public enum NeighbourDirection
{
    Right,
    Left,
    TopRight,
    TopLeft,
    BottomRight,
    BottomLeft
}

[System.Serializable]
public class NeighbourData
{
    public Bubble bubble;
    public NeighbourDirection direction;
}
