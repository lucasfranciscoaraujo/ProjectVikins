﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL;
using UnityEngine;

namespace Assets.Script.Controller
{
   public class ItensController
    {
        private readonly HealthItemFunctions HealthItemFunctions = new HealthItemFunctions();
        
        public DAL.HealthItem HealthGetInitialData(Vector3 position)
        {
            var data = HealthItemFunctions.GetDataByInitialPosition(position);
            if (data == null)
            {
                data = DAL.ProjectVikingsContext.defaultHealthItem;
                data.InitialX = position.x;
                data.InitialY = position.y;
            }
            return data;
        }
    }
}