using Microsoft.Xna.Framework;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

namespace CrystalMoon.Systems.Camera
{
    internal class SmoothCameraModifier : ICameraModifier
    {
        private Vector2? _oldCameraPosition;
        public string UniqueIdentity => "crystalmoon_camerasmooth";
        public bool Finished => false;

        public void Update(ref CameraInfo cameraPosition)
        {
            _oldCameraPosition ??= cameraPosition.OriginalCameraPosition;
            float maxSmoothValue = 0.03f;
            var config = ModContent.GetInstance<CrystalMoonClientConfig>();
            float smoothValue = MathHelper.Lerp(1f, maxSmoothValue, config.CameraSmoothness / 100f);
            cameraPosition.CameraPosition = Vector2.Lerp(_oldCameraPosition.Value, cameraPosition.CameraPosition, smoothValue);
            _oldCameraPosition = cameraPosition.CameraPosition;
        }
    }
}
