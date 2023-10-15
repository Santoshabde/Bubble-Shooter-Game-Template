namespace SNGames.BubbleShooter
{
    public enum BubbleType
    {
        Green,
        Yellow,
        Red,
        Blue,
        Pink,
        White,
        NonDestructable
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

    public enum InGameEvents
    {
        OnBubbleCollisionClearDataComplete,
        MoveNextBubbleToCurrentBubble
    }
}