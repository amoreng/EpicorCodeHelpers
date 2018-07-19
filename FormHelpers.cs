//These are some helpful snippets for improving usability with customized forms
//This is nothing groundbreaking, but helpful to me nonetheless 


//Center the form to screen
private void UD01Form_Load(object sender, EventArgs args)
{
  UD01Form.StartPosition = FormStartPosition.CenterScreen;
}

//Make the form fixed size and center screen
private void UD01Form_Load(object sender, EventArgs args)
{
  UD01Form.StartPosition = FormStartPosition.CenterScreen;
  UD01Form.MaximizeBox = false;
  UD01Form.MinimizeBox = false;
  UD01Form.FormBorderStyle = FormBorderStyle.FixedDialog;
}

//In whatever case, the form name (like UD01Form) will just be the name of the form itself and can be found in the tree view

//Give users a DialogResult (yes/no) for an action
DialogResult dialogResult = MessageBox.Show("Do the thing?", "The Thing", MessageBoxButtons.YesNo);
if(dialogResult == DialogResult.Yes)
{
    //Action is user clicked Yes
}
else if (dialogResult == DialogResult.No)
{
   return;
   //or do something else besides return    
}

//Set a drop down menu so a user cannot enter their own values and must pick from the list
//put this in the InitializeCustomCode method
//I think you also need these two assemblies referenced
//using Infragistics.Win;
//using Infragistics.Win.UltraWinGrid;
cmbMyCustomDropdown.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDown;

//set the users focus to a specific control. Be aware, this can trigger UI events
//This will change a little depending on the control type, but the premise is the same
//The control's EpiGuid property and type (like Erp.UI.Controls.Combo or Ice.Lib.Framework.EpiButton) can be found in the control properties
Erp.UI.Controls.Combos.InspectrCombo cmb = (Erp.UI.Controls.Combos.InspectrCombo)csm.GetNativeControlReference("fd9fe6a9-c62e-4a1e-86cf-57f55bb73815");
cmb.Focus();


//UltraGrid Read Only Techniques
//This loops through all columns in the band and de-activates if the visible position is the first one
foreach(var col in grdAllocations.DisplayLayout.Bands[0].Columns)
{				

  if(col.Header.VisiblePosition != 1)
  {
    col.CellActivation = Activation.Disabled;	
  }			
}
//this loops through the bands and inactives the entire band. I had little luck using the captions to filter
foreach(var col in grdAllocations.DisplayLayout.Bands)
{
  //!col.Header.Caption.ToUpper().Contains("Allocate")
  //MessageBox.Show("VP: " + col.Header.VisiblePosition.ToString()+ "\n" + "Name: " +col.Header.Caption.ToString());
  if(col.Header.VisiblePosition != 1)
  {					
    col.Override.AllowUpdate = DefaultableBoolean.False;
  }			
}

//this sets the column read only property on the datatable level before binding to a grid
//again, I used the ordinal not the caption to filter
foreach(DataColumn c in mergedDataSet.Tables[0].Columns)	
{			
  //msg += "P: " + c.Ordinal.ToString()+  " Caption: " +c.Caption.ToString() + " ColumnName: " + c.ColumnName.ToString() + "\n";
  if(c.Ordinal != 14)
  {	
    c.ReadOnly = true; 		
  }	
}
