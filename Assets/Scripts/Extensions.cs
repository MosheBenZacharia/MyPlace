using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text;

using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

using Debug = UnityEngine.Debug;

namespace MyPlace
{
	public static class Vector3Extension
    {
        public static Vector3 X(this Vector3 vector3, float x)
        {
            vector3.x = x;
            return vector3;
        }
        public static Vector3 Y(this Vector3 vector3, float y)
        {
            vector3.y = y;
            return vector3;
        }
        public static Vector3 Z(this Vector3 vector3, float z)
        {
            vector3.z = z;
            return vector3;
        }
    }

	public static class TransformExtension
	{
		public static void Recenter(this Transform transform)
		{
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
		public static bool IsActiveTransform(this Transform transform)
		{
//			Debug.Log (UnityEditor.Selection.activeTransform.gameObject.name);
//			Debug.Log (transform.gameObject.name);
#if UNITY_EDITOR
			return (transform == UnityEditor.Selection.activeTransform);
#else
			return false;
#endif
		}

		public static Transform GetActiveChildTransform(this Transform transform)
		{
			for (int i = 0; i < transform.childCount; ++i) {
				Transform childTransform = transform.GetChild(i);
				if(childTransform.gameObject.activeSelf)
					return childTransform;
			}
			return null;
		}
	}
	
	public static class ArrayExtension
	{
		/// <summary>
		/// Helper function to ensure an array is not null or empty
		/// </summary>
		public static bool IsValid<T>(this T[] array)
		{
			return (array != null) && (array.Length!=0);
		}
	}
	
	public static class StringExtension
	{
		/// <summary>
		/// Helper function to ensure a string is not null or empty
		/// </summary>
		public static bool IsValid(this string stringValue)
		{
			return !String.IsNullOrEmpty(stringValue);
		}

		public static string AddSpacesBeforeCaps(this string stringValue)
		{
			return System.Text.RegularExpressions.Regex.Replace(stringValue, "[A-Z]", " $0");
		}
	}

	public static class ColorExtension
	{
		public static Color Alpha(this Color color, float alpha)
		{
			color.a = alpha;
			return color;
		}

		public static Color Saturate(this Color color, float saturationDifference)
		{
			float f = -saturationDifference; // desaturate by 20%
			float L = 0.3f*color.r + 0.6f*color.g + 0.1f*color.b;
			color.r = color.r + f * (L - color.r);
			color.g = color.g + f * (L - color.g);
			color.b = color.b + f * (L - color.b);
			return color;
		}
	}

    public static class GameObjectExtension
	{
		public static void Show(this GameObject gameObject)
		{
			gameObject.SetActive(true);
		}

        public static void Hide(this GameObject gameObject)
        {
			gameObject.SetActive(false);
		}

		private static IEnumerator SetActive(GameObject gameObject, bool value, float delay)
		{
			yield return new WaitForSeconds(delay);

			gameObject.SetActive(value);
		}
    }
}