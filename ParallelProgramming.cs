//this is a poor example in real life, but I'm casting a datatable to aan EnumerableRowCollection and performing a Parallel.ForEach

EnumerableRowCollection<DataRow> query = 
	from row in dtInput.AsEnumerable()
	select row;
Parallel.ForEach(query, row => 
{
	string whereClause = string.Format("Key1 like '{0}-%'", row["Donor"].ToString());
	//start log
	builder.Append(string.Format("Looking up Donor {0}...", row["Donor"].ToString())).AppendLine();		

	//the Hashtable stores the runtime search criteria
	System.Collections.Hashtable wcs = new Hashtable(1);
	wcs.Clear();	
	wcs.Add("UD100", whereClause);
	Ice.Lib.Searches.SearchOptions opts = Ice.Lib.Searches.SearchOptions.CreateRuntimeSearch(wcs, Ice.Lib.Searches.DataSetMode.RowsDataSet);	   
	adapterUD100.InvokeSearch(opts);

	int rowCount = adapterUD100.UD100Data.UD100.Rows.Count;
	if(rowCount>0)	
	{	
		foreach(DataRow udRow in adapterUD100.UD100Data.UD100.Rows)
		{	
			//modify here
			udRow.BeginEdit();
			udRow["Character04"] = row["OPO Name"];
			udRow["Character05"] = row["OPO Donor Number"];
			udRow["RowMod"] = "U";
			udRow.EndEdit();
			//update log
			builder.Append(string.Format("{0}: Success!", udRow["Key1"].ToString())).AppendLine();
		}
		//update
		adapterUD100.Update();										
	}
	else
	{
		//update log
		builder.Append(string.Format("Failed to find UD100 recs for {0}", row["Donor"].ToString())).AppendLine();
	}					
	txtProgress.Text = builder.ToString();
});
