//Helper to write a file out to the temp folder on the C drive

	private void WriteDebugToFile(string content)
	{
		//write content to file, debug only
		string path = @"c:\temp\ResponseContent.txt";
		// This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            string createText = "First Line:" + Environment.NewLine;
            File.WriteAllText(path, createText);
        }
        // This text is always added, making the file longer over time
        // if it is not deleted.
        string appendText = Environment.NewLine + "Response "+DateTime.Now.ToString() + Environment.NewLine + content;
        File.AppendAllText(path, appendText);
	}
