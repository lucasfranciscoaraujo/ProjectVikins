﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;
using Assets.Script.Models;

namespace Assets.Script.BLL
{
    public class PlayerFunctions : Shared.BLLFunctions<DAL.Player, Models.PlayerViewModel>
    {
        public PlayerFunctions()
            : base("PlayerId")
        {
            SetListContext();
        }
        public override int Create(Player model)
        {
            ListContext.Add(model);
            return model.PlayerId;
        }

        public override void SetListContext()
        {
            this.ListContext = context.GetPlayers();
        }

        public override void UpdateStats(PlayerViewModel model)
        {
            var player = this.GetDataById(model.PlayerId);

            if (model.LastMoviment.HasValue) player.LastMoviment = model.LastMoviment.Value;
            if (model.Life.HasValue) player.Life = model.Life.Value;
            if (model.SpeedRun.HasValue) player.SpeedRun = model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) player.SpeedWalk = model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) player.AttackMin = model.AttackMin.Value;
            if (model.AttackMax.HasValue) player.AttackMax = model.AttackMax.Value;
        }

        public override void Decrease(PlayerViewModel model)
        {
            var player = this.GetDataById(model.PlayerId);
            
            if (model.Life.HasValue) player.Life = player.Life - model.Life.Value;
            if (model.SpeedRun.HasValue) player.SpeedRun = player.SpeedRun - model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) player.SpeedWalk = player.SpeedWalk - model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) player.AttackMin = player.AttackMin - model.AttackMin.Value;
            if (model.AttackMax.HasValue) player.AttackMax = player.AttackMax - model.AttackMax.Value;
        }

        public override void Increase(PlayerViewModel model)
        {
            var player = this.GetDataById(model.PlayerId);

            if (model.Life.HasValue) player.Life = player.Life + model.Life.Value;
            if (model.SpeedRun.HasValue) player.SpeedRun = player.SpeedRun + model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) player.SpeedWalk = player.SpeedWalk + model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) player.AttackMin = player.AttackMin + model.AttackMin.Value;
            if (model.AttackMax.HasValue) player.AttackMax = player.AttackMax + model.AttackMax.Value;
        }
    }
}