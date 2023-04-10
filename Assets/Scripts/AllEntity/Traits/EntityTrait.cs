using Assets.Scripts.AllEntity.Traits;
using UnityEngine;
using System;

namespace Assets.Scripts.AllEntity
{
    public static class EntityTrait
    {
        public static void Move(this ITrait<CanMove> trait, Entity entity)
        {
            if (entity.GetMoveVector().x < 0)
            {
                entity.GetRb().gameObject.GetComponent<Transform>().eulerAngles = new Vector3(0, 180, 0);
                entity.IsFlip = true;
            }
            else if (entity.GetMoveVector().x != 0)
            {
                entity.gameObject.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);                
                entity.IsFlip=false;
            }

           entity.GetRb().velocity = new Vector2(entity.GetMoveVector().x * entity.GetSpeed(), entity.GetRb().velocity.y);
        }

        public static void Jump(this ITrait<CanJump> trait, Entity entity)
        {
            entity.GetRb().AddForce(new Vector2(0, entity.GetJumpForce()), ForceMode2D.Impulse);
        }

        public static void Climb(this ITrait<CanClimb> trait, Entity entity)
        {
            entity.GetRb().AddForce(new Vector2(0, entity.GetJumpForce()), ForceMode2D.Force);
        }

        public static float AgressiveLogics(this ITrait<CanAgressiveLogics> trait, AIEntity entity)
        {
            Vector2 distance = new(Math.Abs(Raptor.GetInstance().GetRb().position.x - entity.GetRb().position.x), Math.Abs(Raptor.GetInstance().GetRb().position.y - entity.GetRb().position.y));

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
            if (distance.y < 7 && distance.x < entity.GetRadiusCheck() && distance.x >= entity.GetAttackRange() * 1.5f)
            {
                entity.SetMoveVector(new Vector2(Math.Abs(Raptor.GetInstance().GetRb().position.x - entity.GetRb().position.x) / (Raptor.GetInstance().GetRb().position.x - entity.GetRb().position.x), 0));
            }
            else if (distance.x <= entity.GetAttackRange() * 1.5f)
            {
                entity.SetMoveVector(0, 0);
            }
            else
            {
                if (entity.GetTimePatrul() <= 0)
                {
                    System.Random rand = new System.Random();
                    entity.SetTimePatrul(rand.Next(1, 10));
                    entity.SetMoveVectorPatrul(rand.Next(-1, 2));
                }
                else
                {
                    entity.SetTimePatrul(entity.GetTimePatrul()-Time.deltaTime);
                    entity.SetMoveVector(entity.GetMoveVectorPatrul(), 0);
                }
            }
            return entity.GetMoveVector().x;
        }

        public static float PeacefulLogics(this ITrait<CanPeacefulLogics> trait, AIEntity entity)
        {
            Vector2 distance = new(Math.Abs(Raptor.GetInstance().GetRb().position.x - entity.GetRb().position.x), Math.Abs(Raptor.GetInstance().GetRb().position.y - entity.GetRb().position.y));

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

            if (distance.y < 7 && distance.x < entity.GetRadiusCheck() && distance.x >= 0)
            {
                entity.SetMoveVector(new Vector2(-Math.Abs(Raptor.GetInstance().GetRb().position.x - entity.GetRb().position.x) / (Raptor.GetInstance().GetRb().position.x - entity.GetRb().position.x), 0));
            }
            else
            {
                if (entity.GetTimePatrul() <= 0)
                {
                    System.Random rand = new System.Random();
                    entity.SetTimePatrul(rand.Next(1, 10));
                    entity.SetMoveVectorPatrul (rand.Next(-1, 2));
                }
                else
                {
                    entity.SetTimePatrul(entity.GetTimePatrul() - Time.deltaTime);
                    entity.SetMoveVector(entity.GetMoveVectorPatrul(), 0);
                }
            }
            return entity.GetMoveVector().x;
        }

        public static void AttackOneUnit(this ITrait<CanAttackOneUnit> trait, Entity entity)
        {
            if (entity.GetTimeBtwAttack() <= 0)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(entity.GetAttackPosition().position, entity.GetAttackRange(), entity.GetEnemies());
                if (enemiesToDamage.Length > 0)
                {
                    enemiesToDamage[0].GetComponent<Entity>().GetDamage(entity.GetDamage());
                }
                entity.SetTimeBtwAttack(entity.GetStartTimeBtwAttack());
            }
        }

        public static void AttackSplash(this ITrait<CanAttackSplash> trait, Entity entity)
        {
            if (entity.GetTimeBtwAttack() <= 0)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(entity.GetAttackPosition().position, entity.GetAttackRange(), entity.GetEnemies());
                if (enemiesToDamage.Length > 0)
                {
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Entity>().GetDamage(entity.GetDamage());
                    }
                }
                entity.SetTimeBtwAttack(entity.GetStartTimeBtwAttack());
            }
        }
    }
}
