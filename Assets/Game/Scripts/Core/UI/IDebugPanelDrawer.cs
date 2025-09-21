using System.Collections.Generic;

namespace AG.Core.UI
{
	public interface IDebugPanelDrawer
	{
		void AddDebugProperties(List<GUIUtils.Property> properties);
	}
}