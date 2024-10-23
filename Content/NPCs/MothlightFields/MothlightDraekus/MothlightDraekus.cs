using CrystalMoon.Content.Bases;
using CrystalMoon.Systems.Rigging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.NPCs.MothlightFields.MothlightDraekus
{
    internal class MothlightDraekus : BaseRiggedNPC
    {
        public Limb FrontLeg { get; set; }
        public Limb BackLeg { get; set; }
        public Limb Torso { get; set; }
        public Limb FrontArm { get; set; }
        public Limb BackArm { get; set; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 30;
            NPC.height = 80;
            NPC.damage = 32;
            NPC.knockBackResist = 0.5f;
            NPC.lifeMax = 120;

            //First we have to define the bones
            // FrontArm = MakeLimb().AddNewJoint(direction: new Vector2()

            /*
            Limb limb = Limbs["FrontArm"];
            Limb limb2 = Limbs["BacKLeg"];
            limb2.EndEffectorPos = Player.Center;
            */

            BackArm = MakeLimb()
                .AddNewJoint(direction: new Vector2(1, 1), length: 40, texture: $"{Texture}_BackArm");

            BackArm.Joints[0].DrawOrigin = new Vector2(10, 5);


            BackLeg = MakeLimb()
                .AddNewJoint(direction: new Vector2(1, 1), length: 30, texture: $"{Texture}_BackLeg")
                .AddNewJoint(direction: new Vector2(0, 1), length: 30, texture: $"{Texture}_BackFoot");
            BackLeg.Joints[0].DrawOrigin = Vector2.Zero;
            BackLeg.Joints[1].DrawOrigin = new Vector2(26, 2);

            Torso = MakeLimb()
               .AddNewJoint(direction: new Vector2(0, -1), length: 18, texture: $"{Texture}_TorsoBottom")
               .AddNewJoint(direction: new Vector2(1, -1), length: 10, texture: $"{Texture}_TorsoTop")
               .AddNewJoint(direction: new Vector2(1, -1), length: 40, texture: $"{Texture}_Head");

            Torso.Joints[0].DrawOrigin = new Vector2(22, 22);
            Torso.Joints[1].DrawOrigin = new Vector2(39, 60);
            Torso.Joints[2].DrawOrigin = new Vector2(0, 44);


            FrontLeg = MakeLimb()
                .AddNewJoint(direction: new Vector2(1, 1), length: 30, texture: $"{Texture}_FrontLeg")
                .AddNewJoint(direction: new Vector2(0, 1), length: 30, texture: $"{Texture}_FrontFoot");

            FrontLeg.Joints[0].DrawOrigin = Vector2.Zero;
            FrontLeg.Joints[1].DrawOrigin = new Vector2(26, 2);

            FrontArm = MakeLimb()
                .AddNewJoint(direction: new Vector2(1, 1), length: 40, texture: $"{Texture}_FrontArm")
                .AddNewJoint(direction: new Vector2(0, 1), length: 30, texture: $"{Texture}_Lamp");

            FrontArm.DrawBackwards = true;
            FrontArm.Joints[0].DrawOrigin = new Vector2(10, 5);
            FrontArm.Joints[1].DrawOrigin = new Vector2(16, 0);

            //Connect Arms
            BackArm.ParentJoint = Torso.Joints[2];
            BackArm.LocalOffset = new Vector2(0, -8);
            FrontArm.ParentJoint = Torso.Joints[2];

            //Connect Legs
            BackLeg.ParentJoint = Torso.Joints[0];
            BackLeg.LocalOffset = new Vector2(8, -4);

            FrontLeg.ParentJoint = Torso.Joints[0];
        }

        public override void AI()
        {
            base.AI();
            FrontArm.Joints[0].Rotation += 0.025f;
        }
    }
}
