using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;

namespace BrokenBattleBots
{

    /// <summary>
    /// Player score
    /// </summary>
    public struct PlayerScore : IComponentData
    {
        public int Score;
        public int BestScore;
    }

    /// <summary>
    /// Tag entities with a score bounty that can be claimed
    /// </summary>
    public struct ScoreBounty : IComponentData
    {
        public int Score;
        public Entity Player;
    }

    /// <summary>
    /// Entity score can be claimed by a player
    /// </summary>
    public struct Tag_ClaimScore : IComponentData
    {
        // Nothing, just a tag
    }


    class ClaimScoreSystem : JobComponentSystem
    {
        EntityQuery playerScoreQuery;
        EntityQuery scoresToClaim;

        EndSimulationEntityCommandBufferSystem ecb_EndSim;

        protected override void OnCreate()
        {
            base.OnCreate();
            playerScoreQuery = GetEntityQuery(typeof(PlayerScore));
            scoresToClaim = GetEntityQuery(new ComponentType[] { typeof(ScoreBounty), typeof(Tag_ClaimScore) });
            ecb_EndSim = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // Schedule claim score job
            ClaimScoreJob claimJob = new ClaimScoreJob()
            {
                ecb = ecb_EndSim.CreateCommandBuffer(),
                score = playerScoreQuery.GetSingleton<PlayerScore>()
            };
            claimJob.Schedule(this, inputDeps).Complete();
            
            return inputDeps;
        }

        /// <summary>
        /// Claim all outstanding scores
        /// </summary>
        [RequireComponentTag(typeof(Tag_ClaimScore))]
        struct ClaimScoreJob : IJobForEachWithEntity<ScoreBounty>
        {
            public EntityCommandBuffer ecb;
            public PlayerScore score;
            
            public void Execute(Entity entity, int index, ref ScoreBounty bounty)
            {
                score.Score += bounty.Score;
                if(score.Score > score.BestScore)
                {
                    score.BestScore = score.Score;
                }
                ecb.RemoveComponent(entity, typeof(Tag_ClaimScore));
                
            }
        }
        
    }
}