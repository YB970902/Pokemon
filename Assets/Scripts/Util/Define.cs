
namespace Define
{
    public static class Character
    {
        public enum Direction
        {
            Down = 0,
            Up,
            Left,
            Right,
        }
    }

    public static class Map
    {
        public enum TileType
        {
            Ground,
            Tree,
            Bush,
        }
    }

    public static class UI
    {
        public enum UIType
        {
            FullScreen, // 화면을 꽉 채우는 UI
            Windowed, // 화면을 다 채우지 않는 UI
        }
    }
}
