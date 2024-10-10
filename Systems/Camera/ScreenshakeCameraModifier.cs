﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

namespace CrystalMoon.Systems.Camera
{
    internal class ScreenshakeCameraModifier : ICameraModifier
    {
        private float _shakeAmt;

        public string UniqueIdentity { get; private set; }
        public bool Finished { get; private set; }

        public ScreenshakeCameraModifier(Vector2 position, float distance, float strength, string uniqueId = null)
        {
            CrystalMoonClientConfig config = ModContent.GetInstance<CrystalMoonClientConfig>();
            if (!config.ShakeToggle)
                return;
            _shakeAmt = strength * (1f - Main.LocalPlayer.Center.Distance(position) / distance) * 0.5f;
            UniqueIdentity = uniqueId;
        }

        public void Update(ref CameraInfo cameraPosition)
        {
            if (_shakeAmt > 0.5f)
            {
                _shakeAmt *= 0.92f;
                Vector2 shake = new Vector2(Main.rand.NextFloat(_shakeAmt), Main.rand.NextFloat(_shakeAmt));
                cameraPosition.CameraPosition += shake;

            }
            else
            {
                Finished = true;
            }
        }
    }
}
