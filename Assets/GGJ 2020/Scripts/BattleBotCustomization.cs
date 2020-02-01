using UnityEngine;

namespace BrokenBattleBots
{
    public class BattleBotCustomization : MonoBehaviour
    {
        public enum NodeType
        {
            Head,
            ArmLeft,
            ArmRight,
            ShoulderLeft,
            ShoulderRight,
            Chest,
            Bottom,
        }

        public Transform nodeHead;
        public Transform nodeArmLeft;
        public Transform nodeArmRight;
        public Transform nodeBottom;
        public Transform nodeShoulderLeft;
        public Transform nodeShoulderRight;

        private void Update ()
        {
            
        }
    }
}
