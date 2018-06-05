//Some examples of using EpiDataViews

//You can create custom EpiDataViews
EpiDataView edvCustomView, edvNewView;
edvCustomerView = new EpiDataView();
//or 
EpiDataView edvCustomView = new EpiDataView();

//refernce EpiDataView object to native EpiDataView, usually in the InitializeCustomCode method. The Wizard does this too
private EpiDataView edvporView; 
public void InitializeCustomCode()
{
  this.edvporView = ((EpiDataView)(this.oTrans.EpiDataViews["porView"]));
}
//Always cleanup your EpiDataViews
public void DestroyCustomCode()
{
  this.edvporView = null;
}

//Reference field within EpiDataView

edvNewView.dataView[edvNewView.Row]["FieldName"] = someValue;

//EpiDataView will have an underlying DataTable so when creating a custom one, it needs to have a DataTable behind

EpiDataView edvCustomView = new EpiDataView();
DataTable dataTable = new DataTable();

//Define the columns and data types of those columns

DataColumn dateCol = new DataColumn("DateField", typeof(DateTime)));
//or a more terse way 
dataTable.Columns.Add(new DataColumn("OrderTotal", typeof(decimal)));
//another way yet
DataColumn colString = new DataColumn("StringCol");
colString.DataType = System.Type.GetType("System.String");
dataTable.Columns.Add(colString); 

//The DataTable should also have a SysRowID column, otherwise weird stuff can happen
dataTable.Columns.Add(new DataColumn("SysRowID", typeof(Guid));
//Default data type is string
dt.Columns.Add(new DataColumn("Customer"));
//After all the columns are defined for the data table, set the dataView of the EpiDataView to the dataTable.DefaultView
edvCustomView.dataView = dataTable.DefaultView;
//Then, add the new EpiDataView to the oTrans object. I did all of this in the Initialization method
oTrans.Add("ViewName", edvCustomView);

//again, make sure to clean up any custom datatables or epidataviews in the DestroyCustomCode method
this.edvCustomView = null;
this.dataTable = null;

//You can use the oTrans.NotifyAll method to trigger certain EpiTransaction notify events against a specific dataview
oTrans.NotifyAll(EpiTransaction.NotifyType.Initialize, edvCustomView);

//if you are wanting to create a new record in a custom EpiDataView, you need to add a row to the datatable underneath it before setting the dataView
//Add Row to DataTable
DataRow defaultRow = dataTable.NewRow();
defaultRow["OrderDate"] = System.DateTime.Now;
defaultRow["SysRowID"] = Guid.NewGuid();
dataTable.Rows.Add(defaultRow);
edvCustomView.dataView = dataTable.DefaultView;
oTrans.Add("ViewName", edvCustomView);

