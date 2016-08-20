using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public abstract class Element : MonoBehaviour
	{
		public abstract string TooltipName {get;set;}
	}
}