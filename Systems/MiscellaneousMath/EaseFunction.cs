using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CrystalMoon.Systems.MiscellaneousMath
{
    public abstract class EaseFunction
	{
		public static readonly EaseFunction Linear = new PolynomialEase((float x) => { return x; });

		public static readonly EaseFunction EaseQuadIn = new PolynomialEase((float x) => { return x * x; });
		public static readonly EaseFunction EaseQuadOut = new PolynomialEase((float x) => { return 1f - EaseQuadIn.Ease(1f - x); });
		public static readonly EaseFunction EaseQuadInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? 2f * x * x : -2f * x * x + 4f * x - 1f; });

		public static readonly EaseFunction EaseCubicIn = new PolynomialEase((float x) => { return x * x * x; });
		public static readonly EaseFunction EaseCubicOut = new PolynomialEase((float x) => { return 1f - EaseCubicIn.Ease(1f - x); });
		public static readonly EaseFunction EaseCubicInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? 4f * x * x * x : 4f * x * x * x - 12f * x * x + 12f * x - 3f; });

		public static readonly EaseFunction EaseQuarticIn = new PolynomialEase((float x) => { return x * x * x * x; });
		public static readonly EaseFunction EaseQuarticOut = new PolynomialEase((float x) => { return 1f - EaseQuarticIn.Ease(1f - x); });
		public static readonly EaseFunction EaseQuarticInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? 8f * x * x * x * x : -8f * x * x * x * x + 32f * x * x * x - 48f * x * x + 32f * x - 7f; });

		public static readonly EaseFunction EaseQuinticIn = new PolynomialEase((float x) => { return x * x * x * x * x; });
		public static readonly EaseFunction EaseQuinticOut = new PolynomialEase((float x) => { return 1f - EaseQuinticIn.Ease(1f - x); });
		public static readonly EaseFunction EaseQuinticInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? 16f * x * x * x * x * x : 16f * x * x * x * x * x - 80f * x * x * x * x + 160f * x * x * x - 160f * x * x + 80f * x - 15f; });

		public static readonly EaseFunction EaseCircularIn = new PolynomialEase((float x) => { return 1f - (float)Math.Sqrt(1.0 - Math.Pow(x, 2)); });
		public static readonly EaseFunction EaseCircularOut = new PolynomialEase((float x) => { return (float)Math.Sqrt(1.0 - Math.Pow(x - 1.0, 2)); });
		public static readonly EaseFunction EaseCircularInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? (1f - (float)Math.Sqrt(1.0 - Math.Pow(x * 2, 2))) * 0.5f : (float)((Math.Sqrt(1.0 - Math.Pow(-2 * x + 2, 2)) + 1) * 0.5); });


        public static readonly EaseFunction EaseInBack = new PolynomialEase(Easing.InBack);
        public static readonly EaseFunction EaseOutBack = new PolynomialEase(Easing.OutBack);
        public static readonly EaseFunction EaseInOutBack = new PolynomialEase(Easing.InOutBack);


        public static readonly EaseFunction EaseInBounce = new PolynomialEase(Easing.InBounce);
        public static readonly EaseFunction EaseOutBounce = new PolynomialEase(Easing.OutBounce);
        public static readonly EaseFunction EaseInOutBounce = new PolynomialEase(Easing.InOutBounce);

        public static readonly EaseFunction EaseInCirc = new PolynomialEase(Easing.InCirc);
        public static readonly EaseFunction EaseOutCirc = new PolynomialEase(Easing.OutCirc);
        public static readonly EaseFunction EaseInOutCirc = new PolynomialEase(Easing.InOutCirc);

        public static readonly EaseFunction EaseInElastic = new PolynomialEase(Easing.InElastic);
        public static readonly EaseFunction EaseOutElastic = new PolynomialEase(Easing.OutElastic);
        public static readonly EaseFunction EaseInOutElastic = new PolynomialEase(Easing.InOutElastic);


        public static readonly EaseFunction EaseInExpo= new PolynomialEase((float f) => Easing.InExpo(f, 8f));
        public static readonly EaseFunction EaseOutExpo = new PolynomialEase((float f) => Easing.OutExpo(f, 8f));
        public static readonly EaseFunction EaseInOutExpo = new PolynomialEase((float f) => Easing.InOutExpo(f, 8f));

        public abstract float Ease(float time);
	}

	public class PolynomialEase : EaseFunction
	{
		private Func<float, float> _function;

		public PolynomialEase(Func<float, float> func)
		{
			_function = func;
		}

		public override float Ease(float time)
		{
			time = MathHelper.Clamp(time, 0f, 1f);
			return _function(time);
		}

		//removed because not needed for spirit
		//public static EaseFunction Generate(int factor, int type)
		//{
		//}
	}

	public class EaseBuilder : EaseFunction
	{
		private List<EasePoint> _points;

		public EaseBuilder()
		{
			_points = new List<EasePoint>();
		}

		public void AddPoint(float x, float y, EaseFunction function) => AddPoint(new Vector2(x, y), function);

		public void AddPoint(Vector2 vector, EaseFunction function)
		{
			if (vector.X < 0f) throw new ArgumentException("X value of point is not in valid range!");

			EasePoint newPoint = new EasePoint(vector, function);
			if (_points.Count == 0)
			{
				_points.Add(newPoint);
				return;
			}

			EasePoint last = _points[_points.Count - 1];
			if (last.Point.X > vector.X) throw new ArgumentException("New point has an x value less than the previous point when it should be greater or equal");

			_points.Add(newPoint);
		}

		public override float Ease(float time)
		{
			Vector2 prevPoint = Vector2.Zero;
			EasePoint usePoint = _points[0];
			for (int i = 0; i < _points.Count; i++)
			{
				usePoint = _points[i];
				if (time <= usePoint.Point.X)
				{
					break;
				}
				prevPoint = usePoint.Point;
			}
			float dist = usePoint.Point.X - prevPoint.X;
			float progress = (time - prevPoint.X) / dist;
			if (progress > 1f) progress = 1f;
			return MathHelper.Lerp(prevPoint.Y, usePoint.Point.Y, usePoint.Function.Ease(progress));
		}

		private struct EasePoint
		{
			public Vector2 Point;
			public EaseFunction Function;

			public EasePoint(Vector2 p, EaseFunction func)
			{
				Point = p;
				Function = func;
			}
		}
	}
}