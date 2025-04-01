using Flames.Maths;

namespace Flames
{

    public class CustomModel
    {
        public string name;
        // humanoid defaults
        public float nameY = 32.5f / 16.0f;
        public float eyeY = 26.0f / 16.0f;
        public Vec3F32 collisionBounds = new Vec3F32
        {
            X = 8.6f / 16.0f,
            Y = 28.1f / 16.0f,
            Z = 8.6f / 16.0f
        };
        public Vec3F32 pickingBoundsMin = new Vec3F32
        {
            X = (-8) / 16.0f,
            Y = 0 / 16.0f,
            Z = (-4) / 16.0f
        };
        public Vec3F32 pickingBoundsMax = new Vec3F32
        {
            X = 8 / 16.0f,
            Y = 32 / 16.0f,
            Z = 4 / 16.0f
        };
        public bool bobbing = true;
        public bool pushes = true;
        // if true, uses skin from your account
        public bool usesHumanSkin = true;
        public bool calcHumanAnims = true;
        public ushort uScale = 64;
        public ushort vScale = 64;
        public byte partCount;
    }

    public class CustomModelPart
    {
        /* min and max vec3 points */
        public Vec3F32 min;
        public Vec3F32 max;

        /* uv coords in order: top, bottom, front, back, left, right */
        public ushort[] u1;
        public ushort[] v1;
        public ushort[] u2;
        public ushort[] v2;
        /* rotation origin point */
        public Vec3F32 rotationOrigin;

        public Vec3F32 rotation = new Vec3F32
        {
            X = 0.0f,
            Y = 0.0f,
            Z = 0.0f,
        };
        public CustomModelAnim[] anims;
        public bool fullbright = false;
        public bool firstPersonArm = false;
    }

    public class CustomModelAnim
    {
        public CustomModelAnimType type = CustomModelAnimType.None;
        public CustomModelAnimAxis axis;

        public float a;
        public float b;
        public float c;
        public float d;
    }

    public enum CustomModelAnimType
    {
        None = 0,
        Head = 1,
        LeftLegX = 2,
        RightLegX = 3,
        LeftArmX = 4,
        LeftArmZ = 5,
        RightArmX = 6,
        RightArmZ = 7,
        Spin = 8,
        SpinVelocity = 9,
        SinRotate = 10,
        SinRotateVelocity = 11,
        SinTranslate = 12,
        SinTranslateVelocity = 13,
        SinSize = 14,
        SinSizeVelocity = 15,
        FlipRotate = 16,
        FlipRotateVelocity = 17,
        FlipTranslate = 18,
        FlipTranslateVelocity = 19,
        FlipSize = 20,
        FlipSizeVelocity = 21
    }

    public enum CustomModelAnimAxis
    {
        X = 0,
        Y = 1,
        Z = 2,
    }
}