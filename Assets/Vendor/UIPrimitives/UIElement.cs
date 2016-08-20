using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UIPrimitives;

namespace ITHB.UI
{
	public class UIElement : ITHB.UI.Element//, IPointerEnterHandler, IPointerExitHandler
	{

		public string tooltipName;

		////////////////////////////////////////
		//
		// Properties
		
		public override string TooltipName
		{
			get {
				return this.tooltipName;
			}
			set {
				this.tooltipName = value;
			}
		}
	}
}