//this code can be used to hide the native controls on a form
//it is particularly useful when creating "empty" custom forms off of a UD App form 

//simply call this method during your form load event and make sure to add 
using Infragistics.Win.UltraWinToolbars;

private void HideAllNativeControls()
{
  // Hide Native Toolbar Controls 
  baseToolbarsManager.Tools["NewTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["RefreshTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["DeleteTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["SaveTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["EditMenu"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["HelpMenu"].SharedProps.Visible=false;	
  baseToolbarsManager.Tools["ToolsMenu"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["ActionsMenu"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["FileMenu"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["AttachmentTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["ClearTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["CopyTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["CutTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["PasteTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["UndoTool"].SharedProps.Visible=false;
  baseToolbarsManager.Tools["PrimarySearchTool"].SharedProps.Visible=false;
}

//check out this blog for more info http://epicor-development.blogspot.com/2014/04/epicor-9-toolbars.html
