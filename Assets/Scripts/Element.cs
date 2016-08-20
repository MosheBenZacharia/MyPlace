using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ITHB.UI
{
	public abstract class Element : MonoBehaviour
	{
		public abstract string TooltipName {get;set;}
	}
}