namespace SNGames.BubbleShooter
{
    [System.Serializable]
    public enum BubbleType
    {
        Green,
        Yellow,
        Red,
        Blue,
        Pink,
        White,
        Purple,
        NonDestructable,
        PowerUp_Bomb,
        PowerUp_Colored
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
        MoveNextBubbleToCurrentBubble,
    }
}