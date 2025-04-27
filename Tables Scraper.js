Logo = Var.Image("Scrapers\\Table.png");
Title = Name;
Description = "Scrape Data from " + Name;

InitialUrls = ["https://google.com"];
UrlFormat = "{0}";

Argument = "";
ArgumentBox.Label.Text = "Query";
HelpCenter.SetToolTip(ArgumentBox.Label, "A CSS selector to select your special tables! (leave empty othervise)");

Source = Var.Address("Test.csv");
Destination = Var.Address("Test\\");
CurrentRoute = "0";
CurrentRowIndex = 1;
CurrentRowCount = 99;

ErrorTableActionMode = ActionType.Skip;
ErrorRowActionMode = ActionType.Question;
ErrorCellActionMode = ActionType.Skip;
ErrorDataActionMode = ActionType.Question;
ErrorActionMode = ActionType.Skip;

DriverMode = DriverType.Chrome;

CurrentRowToResult = true;
StoreHistory = false;

ForeColor = Var.Color("White");
BackColor = Var.Color(10, 10, 10);

function FetchData() {
    // Initialize variable 'c' based on whether there's a current file and its WarpsCount property
    let c = HasCurrentFile ? CurrentFile.WarpsCount > -1 ? CurrentFile.WarpsCount : 1 : 0;

    // Get the PointerJS instance for interacting with the web page
    let pointer = Browser.GetPointerJS();

    // Check if the page element (table or specified Argument) exists within 30 seconds
    if (!pointer.Select(isEmpty(Argument) ? "table" : Argument).IsExists().Wait(30000))
        throw "The page is not loaded successfully!";
    else
        do {
            // Check if the table body element exists within 2 seconds
            if (pointer.Select(`${isEmpty(Argument) ? "table" : Argument}>tbody`).IsExists().Wait(2000))
                // Iterate over all table roots
                for (let tableroot of pointer.Select(isEmpty(Argument) ? "table" : Argument).All()) {
                    // Iterate over all tables within the table root
                    for (let table of tableroot.Children().All())
                        // Iterate over all rows within the table
                        for (let row of table.Children().All()) {
                            let i = c; // Initialize a counter for columns
                            // Iterate over all items (columns) within the row
                            for (let item of row.Children().All())
                                // Set the field value for each column item
                                SetField(i++, item.GetContent());
                            // Flush the record (save the data)
                            FlushRecord();
                        }
                    // Set an empty value for the first field and flush the record
                    SetField(0, "");
                    FlushRecord();
                }
            else
                // If no table body element is found, iterate over all tables directly
                for (let table of pointer.Select(isEmpty(Argument) ? "table" : Argument).All()) {
                    // Iterate over all rows within the table
                    for (let row of table.Children().All()) {
                        let i = c; // Initialize a counter for columns
                        // Iterate over all items (columns) within the row
                        for (let item of row.Children().All())
                            // Set the field value for each column item
                            SetField(i++, item.GetContent());
                        // Flush the record (save the data)
                        FlushRecord();
                    }
                    // Set an empty value for the first field and flush the record
                    SetField(0, "");
                    FlushRecord();
                }
            // Repeat the process if there's no current file and user chooses to fetch data again
        } while (!HasCurrentFile && (isEmpty(ContinueScript + BreakScript) || warning("Do you want to fetch data again!")));
}