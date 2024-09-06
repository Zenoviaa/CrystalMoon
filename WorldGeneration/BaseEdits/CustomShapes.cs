using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.WorldBuilding;

namespace CrystalMoon.WorldGeneration.BaseEdits
{
    internal class UpsideDownMound : GenShape
    {
        private int _halfWidth;
        private int _height;

        public UpsideDownMound(int halfWidth, int height)
        {
            _halfWidth = halfWidth;
            _height = height;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            _ = _height;
            double num = _halfWidth;
            for (int i = -_halfWidth; i <= _halfWidth; i++)
            {
                int num2 = Math.Min(_height, (int)((0.0 - (double)(_height + 1) / (num * num)) * ((double)i + num) * ((double)i - num)));
                for (int j = 0; j < num2; j++)
                {
                    if (!UnitApply(action, origin, i + origin.X, origin.Y + j) && _quitOnFail)
                        return false;
                }
            }

            return true;
        }
    }
    internal class BrokenCircleShape : GenShape
    {
        private int _radius;
        private float _distortionAmount;
        public BrokenCircleShape(int radius,float distortionAmount) : base()
        {
            _radius = radius;
            _distortionAmount = distortionAmount;
        }

        public static float AperiodicSin(float x, float dx = 0f, float a = MathHelper.Pi, float b = MathHelper.E)
        {
            return (float)(Math.Sin(x * a + dx) + Math.Sin(x * b + dx)) * 0.5f;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            float offsetAngle = WorldGen.genRand.NextFloat(-10f, 10f);
            for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.Pi * 0.0084f)
            {
                float distortionAmt = AperiodicSin(f, offsetAngle, MathHelper.PiOver2, MathHelper.E * 0.5f) * _distortionAmount;
                int radius = (int)(_radius - distortionAmt * _radius);
                if (radius <= 0)
                    continue;

                float ax = (int)MathF.Sin(f) * radius;
                float ay = (int)MathF.Cos(f) * radius;
                for (int dx = 0; dx != ax; dx += Math.Sign(ax))
                {
                    for (int dy = 0; dy != ay; dy += Math.Sign(ay))
                        UnitApply(action, origin, origin.X + dx, origin.Y + dy, new object[0]);
                }
            }

            return true;
        }
    }

    internal class FastNoiseShape : GenShape
    {
        private int _radius;
        private float _threshold = 0f;
        private FastNoiseLite _fastNoiseLite;
        public FastNoiseShape(int radius, float threshold = 0.0f, float frequency = 0.01f, FastNoiseLite.NoiseType noiseType = FastNoiseLite.NoiseType.Perlin, int seed = 1337) : base()
        {
            _radius = radius;
            _threshold = threshold;
            _fastNoiseLite = new FastNoiseLite(seed);
            _fastNoiseLite.SetFrequency(frequency);
            _fastNoiseLite.SetSeed(seed);
            _fastNoiseLite.SetNoiseType(noiseType);
        }

        public override bool Perform(Point origin, GenAction action)
        {
            void Apply(int x, int y)
            {
                if(_fastNoiseLite.GetNoise(origin.X + x, origin.Y + y) < _threshold)
                {
                    UnitApply(action, origin, origin.X + x, origin.Y + y, new object[0]);
                }
            }


            for(int i = -_radius; i < _radius; i++)
            {
                for(int j = -_radius; j < _radius; j++)
                {
                    Apply(i, j);
                }
            }

            return true;
        }
    }
}
