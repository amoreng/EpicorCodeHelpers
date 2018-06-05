//this snippet shows how one can pass data from one form to another using the LaunchFormOptions class

//in your form where you are passing data from, instantiate a new LaunchFormOptions class

LaunchFormOptions lfo = new LaunchFormOptions();
lfo.ContextValue = yourValue;

//your value can be any object. In this example, I'm going to pass an EpiDataView into another form using LaunchFormOptions
EpiDataView edvOrderHed = (EpiDataView)oTrans.EpiDataViews["OrderHed"];
LaunchFormOptions lfo = new LaunchFormOptions();
lfo.ContextValue =  edvOrderHed;
//Then, call your form by MenuID that you are passing the data into
ProcessCaller.LaunchForm(oTrans, "MYMENUID", lfo);

//in the recipient form, you will need a way to accept the launch form options
//make sure to define the datatype of the LaunchFormOptions
private void UD01Form_Load(object sender, EventArgs args)
{
  if(UD01Form.LaunchFormOptions !=null)
  {
    edvCustomView = (EpiDataView)UD01Form.LaunchFormOptions.ContextValue;
    if(edvCustomView !=null)
    {
      //do stuff with the now filled out edvCustomView
    }
  }
}
