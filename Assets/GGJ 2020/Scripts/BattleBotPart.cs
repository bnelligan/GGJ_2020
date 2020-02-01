using UnityEngine;

namespace BrokenBattleBots
{
    public class BattleBotPart : MonoBehaviour
    {
        public BattleBotPartSocket Socket;
        public Rigidbody Rigidbody;
        public Type PartType;

        [System.Flags]
        public enum Type
        {
            Head,
            ArmLeft,
            ArmRight,
            ShoulderLeft,
            ShoulderRight,
            Chest,
            Bottom,
        }
    }
}