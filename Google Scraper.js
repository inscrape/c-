Logo = Var.Image("Scrapers\\Google-G.png");
Title = "MiMFa Google Results Scraper";
Description = "Scrape Data from Google Search Results Pages";

UrlFormat = "https://google.com/search?q={0}";
InitialUrls = ["https://google.com"];

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

DialogMode = DialogType.Classic;
DriverMode = DriverType.Chrome;

CurrentRowToResult = true;
StoreHistory = true;

ForeColor = Var.Color("WhiteSmoke");
BackColor = Var.Color(10, 10, 10);

function GetResultsLabels() {
    return array(GetResultsLabels()).concat([
        "TITLE",
        "DESCRIPTION",
        "URL"
    ]);
}

function FetchData() {
    let pointer = Browser.GetPointerJS();
    let FromItem = 0;
    if (!pointer.SelectById("rso").IsExists().Wait(30000))
        throw "The page is not loaded successfully!";
    else do {
        // Loop through Search Results
        let fitem = FromItem;
        for (let item of pointer.SelectByXPath("//*[@id='rso']//div[@data-snc]").All().Slice(FromItem)) {
            SetField("TITLE", item.Select("a h3").GetValue());
            SetField("DESCRIPTION", item.Select("div[data-sncf]>div>span").GetValue());
            SetField("URL", item.Select("div[data-snhf]>div>div>span>a").GetAttribute("HREF"));
            FromItem++;
            FlushRecord();
        }
        // Navigate to Next Page if Available
        if (fitem == FromItem) break;
        if (pointer.SelectById("pnnext").IsExists().Parse(false)) {
            pointer.SelectById("pnnext").Click();
            FromItem = 0;
        }
        else {
            pointer.Scroll();
            wait(5000);
        }
    } while ( // Repeat Until All Results are Scraped
        pointer.SelectById("rso").IsExists().Wait(30000)
        || confirm("Could not find the search results!\r\nDo you want to try again?")
    );
}
