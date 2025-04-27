Logo        = Var.Image("Scrapers\\Custom.png"); 
Title       = Name; 
Description = "Scrape Data from " + Name; 

InitialUrls = ["https://google.com"];
UrlFormat = "{0}";

Argument = "table";
ArgumentBox.Label.Text = "Query";
HelpCenter.SetToolTip(ArgumentBox.Label, "A CSS selector to select main parts to fetch");

Source = Var.Address("Test.csv"); 
Destination = Var.Address("Test\\"); 
CurrentRoute = "0"; 
CurrentRowIndex = 0; 
CurrentRowCount = 99; 

ErrorFileActionMode = ActionType.Skip;
ErrorTableActionMode = ActionType.Skip;
ErrorRowActionMode = ActionType.Question; 
ErrorCellActionMode = ActionType.Skip; 
ErrorDataActionMode = ActionType.Question; 
ErrorActionMode = ActionType.Skip; 
    
DriverMode = DriverType.Chrome; 

CurrentRowToResult = true; 
StoreHistory = true; 

ForeColor = Var.Color("White"); 
BackColor = Var.Color(10, 10, 10); 


function FetchData() {
    // Initialize variable 'c' based on whether there's a current file and its WarpsCount property
    let c = HasCurrentFile ? CurrentFile.WarpsCount > -1 ? CurrentFile.WarpsCount : 1 : 0;

    // Get the PointerJS instance for interacting with the web page
    let pointer = Browser.GetPointerJS();

    do {
        // Check if the page element (specified by Argument) exists within 15 seconds
        if (!pointer.SelectByQuery(Argument).IsExists().Wait(15000))
            throw "The page is not loaded successfully!"; // Throw an error if the page is not loaded
        else
            // Iterate over all rows of the selected element
            for (const row of pointer.SelectByQuery(Argument).All()) {
                var i = c; // Initialize a counter for columns
                // Iterate over all cells within the row
                for (const cell of row.Children())
                    // Set the field value for each cell, performing a TryPerform operation to ensure no errors
                    SetField(i++, cell.GetContent().TryPerform(""));
                // If no cells were found, set the field value to the row content
                if (i <= c) SetField(c, row.GetContent().TryPerform(""));
                // If only one cell was found, adjust the counter and set the field value to the row content
                else if (i <= c + 1) SetField(--i, row.GetContent().TryPerform(""));
                // Flush the record (save the data)
                FlushRecord();
            }
    } while (!HasCurrentFile && (isEmpty(ContinueScript+BreakScript) || warning("Do you want to fetch data again!"))); // Repeat the process if there's no current file and user chooses to fetch data again
}
