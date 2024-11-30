
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

        public const string UIFadeOut = "Prefabs/UI/Direction/UIFadeOut";
        public const string UIBattle = "Prefabs/UI/Battle/UIBattle";
    }
    
    public static class Battle
    {
        public enum BattleState
        {
            None, // 배틀중이지 않음.
            Intro, // 최초 적과 조우
            SelectSkill, // 스킬 선택
            Attack, // 적에게 공격
            Damaged, // 피해
            PlayerDead, // 플레이어 죽음
            EnemyDead, // 적 죽음
            End, // 배틀 종료
        }
    }

    public static class Camera
    {
        public enum CameraMode
        {
            FollowPlayer,
            Battle,
        }
    }
}
