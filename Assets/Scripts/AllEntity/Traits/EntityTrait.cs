using Assets.Scripts.AllEntity.Traits;
using UnityEngine;
using System;

namespace Assets.Scripts.AllEntity
{
    public static class EntityTrait
    {
        public static void Move(this ITrait<CanMove> trait, Entity entity)
        {
            if (entity.isMoved)
            {
                if (entity.GetMoveVector().x < 0)
                {
                    entity.SetFlip(true);
                }
                else if (entity.GetMoveVector().x != 0)
                {
                    entity.SetFlip(false);
                }

                entity.GetRigidbody().velocity = new Vector2(entity.GetMoveVector().x * entity.GetSpeed(), entity.GetRigidbody().velocity.y);
            }
        }

        public static void Jump(this ITrait<CanJump> trait, Entity entity)
        {
            entity.GetRigidbody().AddForce(new Vector2(0, entity.GetJumpForce()), ForceMode2D.Impulse);
        }

        public static void Climb(this ITrait<CanClimb> trait, Entity entity)
        {
            entity.GetRigidbody().AddForce(new Vector2(0, entity.GetJumpForce()), ForceMode2D.Force);
        }

        public static void AgressiveLogics(this ITrait<CanAgressiveLogics> trait, AIEntity entity)
        {
            entity.SetTimePatrul(entity.GetTimePatrul() - Time.deltaTime);

            if (entity.RechargeTimeJump())
            {
                if (entity.СheckTheWall())
                {
                    if (entity.IsGrounded)
                    {
                        entity.IsJumped = true;
                        entity.SetTimeBtwJump(entity.GetStartTimeBtwJump());
                    }
                }
            }

            entity.СheckTheMapEnd();
            if (entity.GetTimePatrul() > 2)
            {
                if (entity.CheckThePlayer(-1) == true) { }
            }
            if (entity.CheckThePlayer(-1) == false)
            {
                if (entity.GetTimePatrul() <= 0)
                {
                    entity.RandomMove();
                }
            }
        }

        public static void PeacefulLogics(this ITrait<CanPeacefulLogics> trait, AIEntity entity)
        {
            entity.SetTimePatrul(entity.GetTimePatrul() - Time.deltaTime);

            if (entity.RechargeTimeJump())
            {
                if (entity.СheckTheWall())
                {
                    if (entity.IsGrounded)
                    {
                        entity.IsJumped = true;
                        entity.SetTimeBtwJump(entity.GetStartTimeBtwJump());
                    }
                }
            }

            entity.СheckTheMapEnd();
            if (entity.GetTimePatrul() > 2f)
            {
                entity.CheckThePlayer(1);
            }
            if (entity.CheckThePlayer(1) == false)
            {
                if (entity.GetTimePatrul() <= 0)
                {
                    entity.RandomMove();
                }
            }
        }

        public static void AttackOneUnit(this ITrait<CanAttackOneUnit> trait, IEntityAttack entity,int vector,int knockback)
        {
            if (entity.GetTimeBtwAttack() <= 0)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(entity.GetAttackPosition().position, entity.GetAttackRange(), entity.GetEnemies());
                if (enemiesToDamage.Length > 0)
                {
                    enemiesToDamage[0].GetComponent<Entity>().DealDamage(entity.GetDamage());
                    enemiesToDamage[0].GetComponent<Entity>().StopMove(knockback/100+0.2f);
                    enemiesToDamage[0].GetComponent<Entity>().GetRigidbody().AddForce(new Vector2(knockback * vector, knockback), ForceMode2D.Impulse);
                }
                entity.SetTimeBtwAttack(entity.GetStartTimeBtwAttack());
            }
        }

        public static void AttackSplash(this ITrait<CanAttackSplash> trait, IEntityAttack entity)
        {
            if (entity.GetTimeBtwAttack() <= 0)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(entity.GetAttackPosition().position, entity.GetAttackRange(), entity.GetEnemies());
                if (enemiesToDamage.Length > 0)
                {
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Entity>().DealDamage(entity.GetDamage());
                    }
                }
                entity.SetTimeBtwAttack(entity.GetStartTimeBtwAttack());
            }
        }
    }
}
