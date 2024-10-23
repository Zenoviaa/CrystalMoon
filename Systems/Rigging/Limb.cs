using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Systems.Rigging
{
    internal class Limb
    {
        private Vector2 _prevLocalPosition;
        public Limb()
        {
            Joints = new List<Joint>();
        }
        public List<Joint> Joints { get; init; }
        public Limb Parent { get; set; }

        public Joint ParentJoint { get; set; }
        public Vector2 LocalOffset { get; set; }
        public bool DrawBackwards { get; set; }
        public void AddJoint(Joint joint)
        {
            Joints.Add(joint);
        }

        public Limb AddNewJoint(Vector2 direction, float length)
        {
            Joint joint = new Joint(_prevLocalPosition);
            joint.StartDirection = direction.SafeNormalize(Vector2.Zero);
            joint.Length = length;
            AddJoint(joint);
            _prevLocalPosition += joint.StartDirection * length;
            return this;
        }
        public Limb AddNewJoint(Vector2 direction, float length, string texture)
        {
            Joint joint = new Joint(_prevLocalPosition);
            joint.StartDirection = direction.SafeNormalize(Vector2.Zero);
            joint.Length = length;
            joint.Texture = ModContent.Request<Texture2D>(texture).Value;
            AddJoint(joint);
            _prevLocalPosition += joint.StartDirection * length;
            return this;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 basePosition, Vector2 screenPos, Color drawColor)
        {
            if (DrawBackwards)
            {
                for (int i = Joints.Count - 1; i > 0; i--)
                {
                    Joint joint = Joints[i];
                    Vector2 jointDrawPos = basePosition;
                    if (ParentJoint != null)
                    {
                        jointDrawPos += ParentJoint.LocalPosition;
                    }
                    jointDrawPos += LocalOffset;
                    joint.Draw(spriteBatch, jointDrawPos, ref drawColor);
                }
            }
            else
            {
                for (int i = 0; i < Joints.Count; i++)
                {
                    Joint joint = Joints[i];
                    Vector2 jointDrawPos = basePosition;
                    if (ParentJoint != null)
                    {
                        jointDrawPos += ParentJoint.LocalPosition;
                    }
                    jointDrawPos += LocalOffset;
                    joint.Draw(spriteBatch, jointDrawPos, ref drawColor);
                }
            }
        }
        /*
        private float _jointChainLength;
        private float _distanceToTarget;
        private Vector2 _prevLocalPosition;
        public Limb()
        {
            Joints = new List<Joint>();
        }
        public Vector2 StartPosition { get; set; }
        public Vector2 IKTargetPosition { get; set; }
        public List<Joint> Joints { get; init; }

        public int Iterations { get; set; } = 10;
        public float Tolerance { get; set; } = 0.05f;
        public void AddJoint(Joint joint)
        {
            Joints.Add(joint);
        }

        public Limb AddNewJoint(Vector2 direction, float length)
        {
            Joint joint = new Joint(_prevLocalPosition);
            joint.StartDirection = direction.SafeNormalize(Vector2.Zero);
            joint.Length = length;
            AddJoint(joint);
            _prevLocalPosition += joint.StartDirection * length;
            return this;
        }

        private void Backward()
        {
            // Iterate through every position in the list until we reach the start of the chain
            for (int i = Joints.Count - 1; i >= 0; i -= 1)
            {
                // The last bone position should have the same position as the ikTarget
                Joint joint = Joints[i];
                if (i == Joints.Count - 1)
                {
                    joint.Position = IKTargetPosition;
                }
                else
                {
                    Joint nextJoint = Joints[i + 1];
                    joint.Position = nextJoint.Position + (joint.Position - nextJoint.Position).SafeNormalize(Vector2.Zero) * joint.Length;
                }
            }
        }

        private void Forward()
        {
            // Iterate through every position in the list until we reach the end of the chain
            for (int i = 0; i < Joints.Count; i += 1)
            {
                Joint joint = Joints[i];
                // The first bone position should have the same position as the startPosition
                if (i == 0)
                {
                    joint.Position = StartPosition;
                }
                else
                {
                    Joint previousJoint = Joints[i - 1];
                    joint.Position = previousJoint.Position + (joint.Position - previousJoint.Position).SafeNormalize(Vector2.Zero) * previousJoint.Length;
                }
            }
        }

        public void SolveIK()
        {
            // Get the jointPositions from the joints
            for (int i = 0; i < Joints.Count; i += 1)
            {
                Joints[i].Position = Joints[i].LocalPosition;
            }
            // Distance from the root to the ikTarget
            _distanceToTarget = Vector2.Distance(Joints[0].Position, IKTargetPosition);

            // IF THE TARGET IS NOT REACHABLE
            if (_distanceToTarget > _jointChainLength)
            {
                // Direction from root to ikTarget
                var direction = ikTarget.position - jointPositions[0];

                // Get the jointPositions
                for (int i = 1; i < jointPositions.Length; i += 1)
                {
                    jointPositions[i] = jointPositions[i - 1] + direction.normalized * boneLength[i - 1];
                }
            }
            // IF THE TARGET IS REACHABLE
            else
            {
                // Get the distance from the leaf bone to the ikTarget
                float distToTarget = Vector2.Distance(Joints[Joints.Count - 1].Position, IKTargetPosition);
                float counter = 0;
                // While the distance to target is greater than the tolerance let's iterate until we are close enough
                while (distToTarget > Tolerance)
                {
                    StartPosition = Joints[0].Position;
                    Backward();
                    Forward();
                    counter += 1;
                    // After x iterations break the loop to avoid an infinite loop
                    if (counter > Iterations)
                    {
                        break;
                    }
                }
            }
            // Apply the pole constraint
            PoleConstraint();

            // Apply the jointPositions and rotations to the joints
            for (int i = 0; i < jointPositions.Length - 1; i += 1)
            {
                jointTransforms[i].position = jointPositions[i];
                var targetRotation = Quaternion.FromToRotation(jointStartDirection[i], jointPositions[i + 1] - jointPositions[i]);
                jointTransforms[i].rotation = targetRotation * startRotation[i];
            }
            // Let's constrain the rotation of the last joint to the IK target and maintain the offset.
            Quaternion offset = lastJointStartRot * Quaternion.Inverse(ikTargetStartRot);
            jointTransforms.Last().rotation = ikTarget.rotation * offset;
        }


        public void Init()
        {
            //Ok so, I'm thinking the easiest way to set up the joins is to have a function where you like
            //MakeJoint(direction, length).MakeJoint(direction, length).MakeJoint(direction, length);

        }*/
    }
}
