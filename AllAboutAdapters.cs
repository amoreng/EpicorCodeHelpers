/*Adapters are used inside of form customizations*/

//First thing to do is always add the adapter either via the Adapter wizard or by manually adding the references. 
//This will include, typically, an  Adapter and a Contracts.BO assembly. 
//Adapters are very similar to the WCF services, but the adapter carries the data along with it. 
//In your Usings, you will want to add the following (among other things, if applicable) 

using Ice/Erp.Adapters;
using Ice/Erp.BO;

/*Examples*/
//Use ShipTo Adapter and loop thru results of a GetByID method
ShipToAdapter adapterShipTo = new ShipToAdapter(oTrans);
adapterShipTo.BOConnect();
bool result = adapterShipTo.GetByID(custNum, shipToCustNum);
if(result)
{
   foreach(var row in adapterShipTo.ShipToData.ShipTo.Rows)
   {
      //do something
   }
}
//Instantiate and use UD05 adapter
UD05Adapter adapterUD05 = new UD05Adapter(oTrans);			
adapterUD05.BOConnect();
//add a new record to this table (UD05)
//Get a New Row			
bool newRec = adapterUD05.GetaNewUD05();
int rc = adapterUD05.UD05Data.UD05.Rows.Count-1;
DataRow dr = adapterUD05.UD05Data.UD05.Rows[rc];
dr.BeginEdit();
  dr["Company"] = "YourComanyID";
  dr["Key1"] = YourKey1.ToString();
  dr["Key2"] = YourKey2.ToString();
  dr["Key3"] = "";
  dr["Key4"] = "";
  dr["Key5"] = "";
  dr["Date01"] = DateTime.Now;
  dr["Character01"] = "MyValues";
dr.EndEdit();
//Update and Dispose
adapterUD05.Update();
adapterUD05.Dispose();

//Instantiate UD100 adapter, edit a record and then create a child record
UD100Adapter UDAdapter = new UD100Adapter(oTrans);
UDAdapter.BOConnect();		
try
{
  if (UDAdapter.GetByID(sn,part, "", "",""))
  {
    DataRow dr = UDAdapter.UD100Data.UD100.Rows[0]; 
      dr.BeginEdit(); 
      dr["Checkbox01"] = true;
      dr["ShortChar04"] = QuoteNum.ToString();
      dr["Number20"] = QuoteLine;
      dr["RowMod"] = "U";
      dr.EndEdit(); 
    bool resultUpdate = UDAdapter.Update(); 
    bool _100A = UDAdapter.GetaNewUD100a(sn, part, "", "", "");
    int rc = UDAdapter.UD100Data.UD100A.Rows.Count -1;
    DataRow drA = UDAdapter.UD100Data.UD100A.Rows[rc];
    drA.BeginEdit();
      drA["ChildKey1"] = DateTime.Now.ToString();
      drA["ChildKey2"] = QuoteNum.ToString();
      drA["Character01"] = "Allocated";
      drA["Number01"] = CustNum;
    drA.EndEdit();
    resultUpdate = UDAdapter.Update(); 
    MessageBox.Show("Allocating Graft ID#: " + dr["Key1"].ToString());
    UDAdapter.Dispose();

    //send the allocation email
    try{
    SendAllocationEmail(QuoteNum, sn, part, partType);}
    catch(Exception ex){MessageBox.Show("Something wrong with allocation email: " + ex.Message);}
  }
}

//Call GetByID to return a record from a Quote Adapter
//typically, GetByID returns an boolean t/f
bool result = QuoteAdapter.GetByID(quoteNum);
if(result){}

//Access the QuoteHed.CustNum field data from a Quote adapter
int custNum = Convert.ToInt32(QuoteAdapter.QuoteData.QuoteHed.Rows[0]["CustNum"];

//Fill a DataTable from an adapter data and use the InvokeSearch method with a SearchOptions input. 
private static DataTable dtSerivceOption;

UserCodesAdapter adapterUserCodes = new UserCodesAdapter(oTrans);
adapterUserCodes.BOConnect();
SearchOptions opts = new SearchOptions(SearchMode.AutoSearch);
opts.PreLoadSearchFilter = "CodeTypeID = 'BoxOpts'";
opts.DataSetMode=DataSetMode.RowsDataSet;
adapterUserCodes.InvokeSearch(opts);

dtSerivceOption = adapterUserCodes.UserCodesData.UDCodes;
adapterUserCodes.Dispose();

//Call a Dynamic Query (BAQ) adapter and return results to a grid
//This is especially useful when returning data to a custom grid without needing to format the column headers in code. 
private void btnGet_Click(object sender, System.EventArgs args)
{
  //use dynamic query to populate grdBPM
  DynamicQueryAdapter adapterQuery = new DynamicQueryAdapter(oTrans);					  
  adapterQuery.BOConnect();
  adapterQuery.ExecuteByID("YourBAQIDHere");
  DataSet ds = adapterQuery.QueryResults;
  adapterQuery.Dispose();
  grdDisplayResults.DataSource = ds;
}

//Call a Dynamic Query adapter with parameters
//Multiple parameters can be added, just add a new parameter row 
private DataSet getInfoFromQuery(string partNum)
{
  DataSet dsLots = new DataSet();

  DynamicQueryAdapter qryAvailableLots = new DynamicQueryAdapter(oTrans);
  QueryExecutionDataSet parameters = new QueryExecutionDataSet();

  //set parameter: partNum
  DataRow paramRow = parameters.ExecutionParameter.NewRow();
  paramRow["ParameterID"] = "PartNum";
  paramRow["ParameterValue"] = partNum;
  paramRow["ValueType"] = "varchar(50)";
  paramRow["IsEmpty"] = "False";
  paramRow["RowMod"] = "";
  parameters.ExecutionParameter.Rows.Add(paramRow);

  qryAvailableLots.BOConnect(); 
  
  qryAvailableLots.ExecuteByID("YourParamQueryHere", parameters);
  dsLots = qryAvailableLots.QueryResults;
  qryAvailableLots.Dispose();		
  return dsLots;
}
  
//Call a series of methods on an adapter
IssueReturnAdapter adapterIssueReturn = new IssueReturnAdapter(this.oTrans);
adapterIssueReturn.BOConnect();
//get the material queue id
System.Guid pcMtlQueueRowID = (System.Guid)mqr["SysRowID"];
string pcTranType = "";
adapterIssueReturn.GetNewIssueReturn(pcTranType, pcMtlQueueRowID,"MaterialQueue");
adapterIssueReturn.IssueReturnData.Tables["IssueReturn"].Rows[0]["LotNum"] = lotNum;
adapterIssueReturn.IssueReturnData.Tables["IssueReturn"].Rows[0]["TranQty"] = tranQty;

bool prePerform = false;
adapterIssueReturn.PrePerformMaterialMovement(out prePerform);
//error handling for pre perform
string pcPartNum = (string)edvOrderRel.dataView[edvOrderRel.Row]["PartNum"];
string pcWhseCode = currentWarehouse; //(string)mqr["FromWhse"];
string pcBinNum = currentBin; //(string)mqr["FromBinNum"];
string pcLotNum = (string)mqr["LotNum"];
string pcPCID = ""; //added for 10.1 release AM
string pcDimCode = "";
decimal pdDimConvFactor = 1;
decimal pdTranQty = tranQty;
string pcNegQtyAction = "";
string pcMessage = "";
//parameter test
//MessageBox.Show(currentWarehouse + " " + currentBin);
adapterIssueReturn.NegativeInventoryTest(pcPartNum,pcWhseCode,pcBinNum,pcLotNum,pcPCID, pcDimCode,pdDimConvFactor,pdTranQty, out pcNegQtyAction,out pcMessage);
// Declare and Initialize Variables
bool plNegQtyAction = false;
string legalNumberMessage;
string partTranPKs;

//Assign Whse and Bin directly into the dataset
adapterIssueReturn.IssueReturnData.Tables["IssueReturn"].Rows[0]["ToWarehouseCode"] = currentWarehouse;
adapterIssueReturn.IssueReturnData.Tables["IssueReturn"].Rows[0]["ToBinNum"] = currentBin;

// Call Adapter method
adapterIssueReturn.PerformMaterialMovement(plNegQtyAction, out legalNumberMessage, out partTranPKs);
// Cleanup Adapter Reference
adapterIssueReturn.Dispose();


//Call another series of methods
/*Local Variables*/				
bool userInput = false;
string legalNumberMsg = string.Empty;
string partTranPKs = string.Empty;				

int orderNum = (int)row.Cells["PartAlloc_OrderNum"].Value;
int orderLine = (int)row.Cells["PartAlloc_OrderLine"].Value;
int orderRelNum = (int)row.Cells["PartAlloc_OrderRelNum"].Value;
string warehouseCode = (string)row.Cells["PartBin_WarehouseCode"].Value;
string binNum = (string)row.Cells["PartBin_BinNum"].Value;
string lotNum = (string)row.Cells["PartBin_LotNum"].Value;

string opMessage = string.Empty;
string msg = string.Empty;
/*Instantiate IssueReturn and PickedOrders*/
IssueReturnAdapter adapterIssueReturn = new IssueReturnAdapter(oTrans);				
UnpickTransactionAdapter adapterUnpick = new UnpickTransactionAdapter(oTrans);

/*Connect to adapters*/
adapterIssueReturn.BOConnect();			
adapterUnpick.BOConnect();

/*Unpick*/
adapterUnpick.GetNewUnpickTransaction("O");
adapterUnpick.ChangeOrderNum(orderNum);
adapterUnpick.ChangeOrderLine(orderLine);
adapterUnpick.ChangeOrderRelNum(orderRelNum, out opMessage);
adapterUnpick.ChangeLotNum(lotNum);
adapterUnpick.ChangeFromWhse(warehouseCode);
adapterUnpick.ChangeFromBin(binNum);

adapterUnpick.ChangeToWhse(warehouseCode);
adapterUnpick.ChangeToBin(binNum);		

adapterUnpick.PreUnpickTransaction(out msg);

/*Call Issue Return*/
adapterIssueReturn.GetNewIssueReturn("STK-STK", Guid.Empty, "Unpick");

//set adapterIssueReturn fields here			
adapterIssueReturn.IssueReturnData.IssueReturn[0].TranQty = 1;
adapterIssueReturn.IssueReturnData.IssueReturn[0].PartNum= (string)row.Cells["PartBin_PartNum"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].LotNum= (string)row.Cells["PartBin_LotNum"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].OrderNum = (int)row.Cells["PartAlloc_OrderNum"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].OrderLine = (int)row.Cells["PartAlloc_OrderLine"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].OrderRel = (int)row.Cells["PartAlloc_OrderRelNum"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].FromWarehouseCode = (string)row.Cells["PartBin_WarehouseCode"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].FromBinNum = (string)row.Cells["PartBin_BinNum"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].ToWarehouseCode = (string)row.Cells["PartBin_WarehouseCode"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].ToBinNum = (string)row.Cells["PartBin_BinNum"].Value;
adapterIssueReturn.IssueReturnData.IssueReturn[0].UM = "EA";

/*Call Issue Return*/
adapterIssueReturn.PrePerformMaterialMovement(out userInput);								
adapterIssueReturn.PerformMaterialMovement(false, out legalNumberMsg, out partTranPKs);

/*Unpick Transaction*/
adapterUnpick.UnpickTransaction();

/*Cleanup adapters*/
adapterIssueReturn.Dispose();
adapterUnpick.Dispose();


//Access a forms natively available adapter(s)
//you can find the name of the native adapter in a form by using the Form Event Wizard>Before Adapter Method to return a list of adapters in that form
var adapterInsp = ((InspProcessingAdapter)(this.csm.TransAdaptersHT["oTrans_inspAdapter"]));

                              
                              
//using the BOReader to access a Business Object
if (dtInput.Rows.Count>0)
{
	using(BOReaderImpl boReader = WCFServiceSupport.CreateImpl<Ice.Proxy.Lib.BOReaderImpl>((Session)oTrans.Session, Epicor.ServiceModel.Channels.ImplBase<Ice.Contracts.BOReaderSvcContract>.UriPath))
	{

		foreach (DataRow dr in dtInput.Rows)
		{
			adapterUD100.BOConnect();

			string donorNum = dr["Donor"].ToString();
			string whereClause = string.Format("Key1 like '{0}-%'", donorNum);	
			//MessageBox.Show(whereClause);				
		//find related UD100 records and update them on substring(Key1,0,7) = DonorNum and update UD100.Character04 to OPOAgency and UD100.Character05 to OPODonorNum

			var dsBOReader = boReader.GetList("Ice:BO:UD100", whereClause, "");
			int rowCount = dsBOReader.Tables[0].Rows.Count;
			if(dsBOReader.Tables[0].Rows.Count>0)
			{
				//MessageBox.Show(dsUD100.Tables[0].Rows[rowCount-1]["Key2"].ToString());
				//build out data structure for UD100 mass update
				//foreach
				{
					rowCount++;
				}
			}
			//update here

			adapterUD100.Dispose();
		}
	}			
}
