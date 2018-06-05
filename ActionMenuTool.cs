//Used to create custom tool in the action menu of a form

//add using Infragistics.Win;
//add using Infragistics.Win.UltraWinGrid;

//Tool Object
void AddMyCustomTool()
{
    // verify we have Actions Menu
    if (!baseToolbarsManager.Tools.Exists("ActionsMenu")) return;

    var actionMenu = baseToolbarsManager.Tools["ActionsMenu"] as
    Infragistics.Win.UltraWinToolbars.PopupMenuTool;

    if (actionMenu == null) return;
    // create and configure the new tool
    var tool = new
   Infragistics.Win.UltraWinToolbars.ButtonTool("MyToolKey"); //this is the key for the tool
    tool.SharedProps.Caption = "My Tool"; //this is the name that appears in the caption
    //MainController.setToolImage(tool, "Ellipses");//if you want an image associated with tool
    // add the Tool to the toolbar manager and the actions menu
    baseToolbarsManager.Tools.Add(tool);
    actionMenu.Tools.Add(tool);
    actionMenu.Tools[tool.Key].InstanceProps.IsFirstInGroup = false;
}

//Instantiate Custom Tool, in this case during the form load event

private void SalesOrderForm_Load(object sender, EventArgs args)
{
    AddMyCustomTool();
}


//Call Custom Tool from tool click event
private void baseToolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs args)
{
    if (args.Tool.Key == "MyToolKey")
    {
      //do something here
    }
}
