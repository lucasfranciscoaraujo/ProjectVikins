﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Script.Controller.Shared
{
    public abstract class _CharacterController<TViewModel> : MonoBehaviour, ICharacterController<TViewModel>
        where TViewModel : class
    {
        public System.Random rnd = new System.Random();
        public List<int> targetsAttacked = new List<int>();
        Type className;

        public _CharacterController(string function)
        {
            string item = "Assets.Script.BLL." + function;
        }

        public void GiveDamage(string target, int? damage, int? id)
        {
            if(!damage.HasValue && !id.HasValue) return;

            DecreaseStats(target, "Life", damage, id);
        }


        public Vector3 PositionAttack(Vector2 colSize, Helpers.PossibleMoviment direction)
        {
            switch (direction)
            {
                case Helpers.PossibleMoviment.Down:
                    return new Vector2(0, -(colSize.y / 2));
                case Helpers.PossibleMoviment.Down_Left:
                    return new Vector2(-(colSize.x / 2), -(colSize.y / 2));
                case Helpers.PossibleMoviment.Down_Right:
                    return new Vector2(colSize.x / 2, -(colSize.y / 2));
                case Helpers.PossibleMoviment.Left:
                    return new Vector2(-colSize.x, 0);
                case Helpers.PossibleMoviment.Right:
                    return new Vector2(colSize.x, 0);
                case Helpers.PossibleMoviment.Up:
                    return new Vector2(0, colSize.y / 2);
                case Helpers.PossibleMoviment.Up_Left:
                    return new Vector2(-(colSize.x / 2), colSize.y / 2);
                case Helpers.PossibleMoviment.Up_Right:
                    return new Vector2(colSize.x / 2, colSize.y / 2);
                default:
                    return new Vector2(0, 0);
            }
        }

        public object DecreaseStats(string target, string stats, object value, object id)
        {
            className = Type.GetType("Assets.Script.BLL." + target + "Functions");
            MethodInfo m = className.GetMethod("DecreaseStats");
            return m.Invoke(Activator.CreateInstance(className), new object[] { stats, value, id });
        }
        public object IncreaseStats(string target, string stats, object value, object id)
        {
            className = Type.GetType("Assets.Script.BLL." + target + "Functions");
            MethodInfo m = className.GetMethod("IncreaseStats");
            return m.Invoke(Activator.CreateInstance(className), new object[] { stats, value, id });
        }
        public void DecreaseMultipleStats(string target, Dictionary<string, object> datas, object id)
        {
            className = Type.GetType("Assets.Script.BLL." + target + "Functions");
            MethodInfo m = className.GetMethod("DecreaseMultipleStats");
            m.Invoke(Activator.CreateInstance(className), new object[] { datas, id });
        }
        public void IncreaseMultipleStats(string target, Dictionary<string, object> datas, object id)
        {
            className = Type.GetType("Assets.Script.BLL." + target + "Functions");
            MethodInfo m = className.GetMethod("IncreaseMultipleStats");
            m.Invoke(Activator.CreateInstance(className), new object[] { datas, id });
        }
        public void UpdateStats(string target, string stats, object value, object id)
        {
            className = Type.GetType("Assets.Script.BLL." + target + "Functions");
            MethodInfo m = className.GetMethod("UpdateStats");
            m.Invoke(Activator.CreateInstance(className), new object[] { stats, value, id });
        }
        public void UpdateMultipleStats(string target, Dictionary<string, object> datas, object id)
        {
            className = Type.GetType("Assets.Script.BLL." + target + "Functions");
            MethodInfo m = className.GetMethod("UpdateMultipleStats");
            m.Invoke(Activator.CreateInstance(className), new object[] { datas, id });
        }

        public abstract int GetDamage();
        public abstract void UpdateStats(TViewModel model);
        public abstract void Decrease(TViewModel model);
        public abstract void Increase(TViewModel model);
        public abstract void Attack(Transform transform, Vector3 size, LayerMask targetLayer);
        public abstract Vector3 PositionCenterAttack(Vector3 colSize, Transform transform);
    }
}