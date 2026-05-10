using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class PersonPageTests
{
    private IWebDriver driver;
    private StringBuilder verificationErrors;
    private const string BaseURL = "http://localhost:5091";
    private bool acceptNextAlert = true;

    private Process? _blazorProcess;

    [OneTimeSetUp]
    public void StartBlazorServer()
    {
        var webProjectPath = Path.GetFullPath(Path.Combine(
            Assembly.GetExecutingAssembly().Location,
            "../../../../../../src/DatesAndStuff.Web/DatesAndStuff.Web.csproj"
            ));

        var webProjFolderPath = Path.GetDirectoryName(webProjectPath);

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            //Arguments = $"run --project \"{webProjectPath}\"",
            WorkingDirectory = webProjFolderPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        _blazorProcess = Process.Start(startInfo);

        var client = new HttpClient();
        var timeout = TimeSpan.FromSeconds(30);
        var start = DateTime.Now;

        while (DateTime.Now - start < timeout)
        {
            try
            {
                var result = client.GetAsync(BaseURL).Result;
                if (result.IsSuccessStatusCode)
                {
                    break;
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(1000);
            }
        }
    }

    [OneTimeTearDown]
    public void StopBlazorServer()
    {
        if (_blazorProcess != null && !_blazorProcess.HasExited)
        {
            _blazorProcess.Kill(true);
            _blazorProcess.Dispose();
        }
    }

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
        verificationErrors = new StringBuilder();
    }

    [TearDown]
    public void TeardownTest()
    {
        try
        {
            driver.Quit();
            driver.Dispose();
        }
        catch (Exception)
        {
            // Ignore errors if unable to close the browser
        }
        Assert.That(verificationErrors.ToString(), Is.EqualTo(""));
    }

    [Test]
    [TestCase(5, 5250)]
    [TestCase(10, 5500)]
    [TestCase(20, 6000)]
    public void Person_SalaryIncrease_ShouldIncrease(int percentage, double expectedSalary)
    {
        driver.Navigate().GoToUrl(BaseURL);
        driver.FindElement(By.XPath("//*[@data-test='PersonPageNavigation']")).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        var input = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));
        input.Clear();
        input.SendKeys(percentage.ToString());

        var submitButton = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']")));
        submitButton.Click();

        var salaryLabel = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='DisplayedSalary']")));
        var salaryAfterSubmission = double.Parse(salaryLabel.Text, System.Globalization.CultureInfo.InvariantCulture);

        salaryAfterSubmission.Should().BeApproximately(expectedSalary, 0.001);
    }

    [Test]
    [TestCase(-20)]
    [TestCase(-30)]
    public void SalaryInput_BelowMinimum_ShowsValidationErrors(int percentage)
    {
        driver.Navigate().GoToUrl(BaseURL);
        driver.FindElement(By.XPath("//*[@data-test='PersonPageNavigation']")).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var input = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));

        input.Clear();
        input.SendKeys(percentage.ToString());

        var submitButton = driver.FindElement(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']"));
        submitButton.Click();

        var summaryError = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("ul.validation-errors li.validation-message")));
        summaryError.Text.Should().Be("The specified percentag should be between -10 and infinity.");

        var inlineError = driver.FindElement(By.CssSelector("div.col-md-10 div.validation-message"));
        inlineError.Text.Should().Be("The specified percentag should be between -10 and infinity.");
    }
    [Test]
    public void BlazeDemo_FindFlights_ShouldSucceed()
    {
        driver.Navigate().GoToUrl("https://blazedemo.com/");
        driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='destination of the week! The Beach!'])[1]/following::div[1]")).Click();

        new SelectElement(driver.FindElement(By.Name("fromPort"))).SelectByText("Mexico City");
        new SelectElement(driver.FindElement(By.Name("toPort"))).SelectByText("Dublin");

        driver.FindElement(By.XPath("//input[@value='Find Flights']")).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementExists(By.TagName("table")));

        var flightRows = driver.FindElements(By.XPath("//table/tbody/tr"));

        flightRows.Count.Should().BeGreaterThanOrEqualTo(3);
    }

    [Test]
    public void BlazeDemo_FindCheapFlights_ShouldTakeScreenshot()
    {
        driver.Navigate().GoToUrl("https://blazedemo.com/");

        new SelectElement(driver.FindElement(By.Name("fromPort"))).SelectByText("Mexico City");
        new SelectElement(driver.FindElement(By.Name("toPort"))).SelectByText("Dublin");

        driver.FindElement(By.XPath("//input[@value='Find Flights']")).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementExists(By.TagName("table")));

        double maxPrice = 400.00;
        bool foundCheapFlight = false;

        var flightRows = driver.FindElements(By.XPath("//table/tbody/tr"));

        foreach (var row in flightRows)
        {
            var priceText = row.FindElement(By.XPath("./td[6]")).Text;

            var priceValue = double.Parse(priceText.Replace("$", ""), System.Globalization.CultureInfo.InvariantCulture);

            if (priceValue < maxPrice)
            {
                foundCheapFlight = true;
                break;
            }
        }

        if (foundCheapFlight)
        {
            ITakesScreenshot camera = (ITakesScreenshot)driver;
            Screenshot screenshot = camera.GetScreenshot();

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"BlazeDemo_OlcsoJarat_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string filePath = Path.Combine(desktopPath, fileName);

            screenshot.SaveAsFile(filePath);
        }
    }

    private bool IsElementPresent(By by)
    {
        try
        {
            driver.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    private bool IsAlertPresent()
    {
        try
        {
            driver.SwitchTo().Alert();
            return true;
        }
        catch (NoAlertPresentException)
        {
            return false;
        }
    }

    private string CloseAlertAndGetItsText()
    {
        try
        {
            IAlert alert = driver.SwitchTo().Alert();
            string alertText = alert.Text;
            if (acceptNextAlert)
            {
                alert.Accept();
            }
            else
            {
                alert.Dismiss();
            }
            return alertText;
        }
        finally
        {
            acceptNextAlert = true;
        }
    }
}