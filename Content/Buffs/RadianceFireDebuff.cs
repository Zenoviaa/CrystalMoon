using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CrystalMoon.Content.Buffs
{
    internal struct RadianceFireDebuffDraw : IDrawPrims
    {
        public RadianceFireDebuffDraw(Vector2 startingPoint, Vector2 drawVelocity)
        {
            StartingPoint = startingPoint;
            DrawVelocity = drawVelocity;
        }

        public Vector2 StartingPoint { get; set; }
        public Vector2 DrawVelocity { get; set; }
        public float Offset { get; set; }
        public float Size { get; set; }
        public void DrawPrims(ref PrimitiveDraw draw)
        {
            int num = 32;
            draw.OldPos = new Vector2[num];
            for (int i = 0; i < draw.OldPos.Length; i++)
            {
                //Get the progress and we're gonan calculate points
                float f = i;
                float length = draw.OldPos.Length;
                float progress = f / length;
                Vector2 velocity = DrawVelocity;

                //Degrees/Radians Offset
                float range = 5;
                float degreesOffset = MathHelper.Lerp(-range, range, MathUtil.Osc(0f, 1f, offset: f * 0.25f + Offset, speed: 3));
                float radiansOffset = MathHelper.ToRadians(degreesOffset);
                velocity = velocity.RotatedBy(radiansOffset);

                Vector2 pos = Vector2.Lerp(StartingPoint, StartingPoint + velocity, progress);
                draw.OldPos[i] = pos;
            }

            draw.OldRot = new float[num];
            draw.ColorFunction = ColorFunction;
            draw.WidthFunction = WidthFunction;
            draw.Offset = null;
        }

        private Color ColorFunction(float completionRatio)
        {
            Color startColor = Color.Lerp(Color.Transparent, Color.White, completionRatio / 0.2f);
            Color c = Color.Lerp(startColor, Color.Lerp(new Color(147, 72, 11), Color.Red, completionRatio / 0.4f), completionRatio);
            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            return MathHelper.Lerp(Size, 0, Easing.OutCirc(completionRatio));
        }
    }

    internal class RadianceFireDebuffDrawSystem :
        IPrimitivesBatch
    {
        private List<IDrawPrims> _prims;
        private UnifiedRandom _random;
        public List<IDrawPrims> Prims
        {
            get
            {
                //Initializing the Prims List
                _random ??= new UnifiedRandom();
                _prims ??= new List<IDrawPrims>();
                _prims.Clear();
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.HasBuff<RadianceFireDebuff>())
                    {
                        _random.SetSeed(npc.whoAmI * 100);
                        float widthExtent = npc.width / 2f;
                        float heightExtent = npc.height / 2f;
                        float velocity = 64;
                        float remainingBuffTime = (float)npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<RadianceFireDebuff>())];
                        float scaleMult = 1f;
                        if(remainingBuffTime < 30)
                        {
                            scaleMult = MathHelper.Lerp(0f, 1f, remainingBuffTime / 30f);
                        }

                        //We'll make three fires
                        //This entire system actually worked first try im gonna cry ;-;
                        //We're finally getting better at coding guys!!!

                        //First Fire
                        RadianceFireDebuffDraw radianceFireDebuffDraw = new RadianceFireDebuffDraw(
                            startingPoint: npc.Center + new Vector2(-widthExtent/8f, -heightExtent / 4f) + _random.NextVector2Circular(4, 4),
                            drawVelocity: -Vector2.UnitY * velocity * scaleMult * _random.NextFloat(0.55f, 0.85f));

                        //This will make it so that not every fire is looking the same lol
                        radianceFireDebuffDraw.Offset = npc.whoAmI;
                        radianceFireDebuffDraw.Size = 48 * scaleMult * _random.NextFloat(0.75f, 1.25f);
                        _prims.Add(radianceFireDebuffDraw);

                        //Second Fire
                        radianceFireDebuffDraw = new RadianceFireDebuffDraw(
                           startingPoint: npc.Center + new Vector2(-widthExtent, -heightExtent / 4f) + _random.NextVector2Circular(4, 4),
                           drawVelocity: -Vector2.UnitY * velocity * 0.5f * scaleMult * _random.NextFloat(0.25f, 1.25f));

                        radianceFireDebuffDraw.Offset = npc.whoAmI + 5;
                        radianceFireDebuffDraw.Size = 8 * scaleMult * _random.NextFloat(0.75f, 1.25f);
                        _prims.Add(radianceFireDebuffDraw);

                        //Third Fire
                        radianceFireDebuffDraw = new RadianceFireDebuffDraw(
                           startingPoint: npc.Center + new Vector2(widthExtent, heightExtent / 2f) + _random.NextVector2Circular(4, 4),
                           drawVelocity: -Vector2.UnitY * velocity * 0.75f * scaleMult * _random.NextFloat(0.25f, 1.25f));

                        radianceFireDebuffDraw.Offset = npc.whoAmI + 15;
                        radianceFireDebuffDraw.Size = 16 * scaleMult * _random.NextFloat(0.75f, 1.25f);
                        _prims.Add(radianceFireDebuffDraw);
                    }
                }


                return _prims;
            }
        }

        public BaseShader Shader
        {
            get
            {
                //Get Radiance Shader
                var shader = MagicRadianceShader.Instance;
                shader.PrimaryTexture = TextureRegistry.DottedTrail;
                shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;
                shader.OutlineTexture = TextureRegistry.DottedTrailOutline;
                shader.PrimaryColor = Color.Lerp(Color.White, new Color(255, 207, 79), 0.5f);
                shader.NoiseColor = new Color(206, 101, 0);
                shader.Distortion = 0.2f;
                shader.Speed = 3;
                shader.Power = 0.01f;
                return shader;
            }
        }

        public BlendState BlendState => BlendState.Additive;
        public SamplerState SamplerState => SamplerState.PointWrap;
    }

    internal class RadianceFireDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            base.Update(npc, ref buffIndex);
            npc.lifeRegen -= 8;

            if (Main.rand.NextBool(8))
            {
                if (Main.rand.NextBool(2))
                {
                    Color color = Color.RosyBrown;
                    color.A = 0;
                    Vector2 velocity = -Vector2.UnitY * 4;
                    Vector2 spawnPoint = npc.Center + Main.rand.NextVector2Circular(npc.width / 2f, npc.height / 2f);
                    Particle.NewBlackParticle<FireSmokeParticle>(spawnPoint, velocity, color);
                }
                else
                {
                    Vector2 velocity = -Vector2.UnitY * 4;
                    Vector2 spawnPoint = npc.Center + Main.rand.NextVector2Circular(npc.width / 2f, npc.height / 2f);
                    Particle.NewBlackParticle<FireHeatParticle>(spawnPoint, velocity, new Color(255, 255, 255, 0));
                }
            }

        }
    }
}
