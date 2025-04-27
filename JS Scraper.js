Logo = Var.Image("Scrapers\\Script.png");
Title = "MiMFa JavaScript Scraper";
Description = "Scrape Data from pages by customizable JavaScript Codes";

UrlFormat = "https://google.com/search?q={0}";
InitialUrls = ["https://google.com"];

Source = Var.Address("Test.csv");
Destination = Var.Address("Test\\");
CurrentRoute = "0";
CurrentRowIndex = 1;
CurrentRowCount = 99;

ErrorFileActionMode = ActionType.Question;
ErrorTableActionMode = ActionType.Skip;
ErrorRowActionMode = ActionType.Question;
ErrorCellActionMode = ActionType.Skip;
ErrorDataActionMode = ActionType.Question;
ErrorActionMode = ActionType.Skip;

DriverMode = DriverType.Chrome;

CurrentRowToResult = true;
StoreHistory = true;

ForeColor = Var.Color("WhiteSmoke");
BackColor = Var.Color(10, 10, 10);

function FetchDataByFile(file) { Base.FetchDataByFile(file); }
function FetchDataByTable(table) { Base.FetchDataByTable(table); }
function FetchDataByRow(row) { Base.FetchDataByRow(row); }
function FetchDataByCell(cell) { Base.FetchDataByCell(cell); }
function FetchData() { eval(Argument); }

ArgumentBox.Label.Text = "Fetch Scripts";
Argument = `
if(!IsRun) return;
let pointer = Browser.GetPointerJS();
let FromItem = 0;
if (!pointer.SelectById("rso").IsExists().Wait(30000))
    throw "The page is not loaded successfully!";
else do {
    let fitem = FromItem;
    for (let item of pointer.SelectByXPath("//*[@id='rso']//div[@data-snc]").All().Slice(FromItem)) {
        SetField("TITLE", item.Select("a h3").GetValue());
        SetField("DESCRIPTION", item.Select("div[data-sncf]>div>span").GetValue());
        SetField("URL", item.Select("div[data-snhf]>div>div>span>a").GetAttribute("HREF"));
        FromItem++;
        FlushRecord();
    }
    if (pointer.SelectById("pnnext").IsExists().TryPerform(false)) {
        pointer.SelectById("pnnext").Click();
        FromItem = 0;
    }
    else {
        pointer.Scroll();
        wait(5000);
    }
} while (
    pointer.SelectById("rso").IsExists().Wait(30000)
    || confirm("Could not find the search results!\r\nDo you want to try again?")
);`;

