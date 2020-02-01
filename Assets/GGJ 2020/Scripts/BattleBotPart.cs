// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;

namespace BrokenBattleBots
{
    [RequireComponent (typeof (Rigidbody))]
    public class BattleBotPart : MonoBehaviour
    {
        public BattleBotPartSocket Socket;
        public Rigidbody Rigidbody;
        public BattleBotPartType PartType;

        public enum BattleBotPartType
        {
            Head = 0,
            ArmLeft = 1,
            ArmRight = 2,
            ShoulderLeft = 3,
            ShoulderRight = 4,
            Chest = 5,
            Bottom = 6,
        }

        #if UNITY_EDITOR

        private void OnValidate ()
        {
            // Cache the part's rigidbody

            this.Rigidbody = this.GetComponent <Rigidbody> ();

            UnityEngine.Assertions.Assert.IsNotNull (this.Rigidbody, "Missing rigidbody");
        }

        #endif
    }
}