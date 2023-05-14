using Kafkaproducer.DTO;
using KafkaProducer.Services.Interfaces;
using PuppeteerSharp;
using System.Globalization;

namespace Kafkaproducer.Services
{
    internal class BasketNewsScrapper : IEventScrapper
    {
        public async Task<List<EventDTO>> GetEvents()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://www.basketnews.lt/rungtynes/tvarkarastis/viskas.html");

            var eventDivs = await page.QuerySelectorAllAsync("table .body_row");
            var events = new List<EventDTO>();
            var date = new DateTime();
            foreach (var eventDiv in eventDivs)
            {
                var gameDateDiv = await eventDiv.QuerySelectorAsync("td.game_date");

                var gameDateString = (await page.EvaluateFunctionAsync<string>("el => el.textContent", gameDateDiv)).Trim();

                if (gameDateString != "")
                {
                    date = ParseLithuanianDate(gameDateString);
                }

                var timeDiv = await eventDiv.QuerySelectorAsync(".times");
                var timeString = (await page.EvaluateFunctionAsync<string>("el => el.textContent", timeDiv)).Trim();
                var time = ParseTime(timeString);

                var startDate = date + time;
                var endDate = startDate.AddHours(3);

                var team1Div = await eventDiv.QuerySelectorAsync("td:nth-child(2)"); 
                var team2Div = await eventDiv.QuerySelectorAsync("td:nth-child(3)"); 

                var team1 = (await page.EvaluateFunctionAsync<string>("el => el.textContent", team1Div)).Trim();
                var team2 = (await page.EvaluateFunctionAsync<string>("el => el.textContent", team2Div)).Trim();

                var leagueEl = await eventDiv.QuerySelectorAsync("td:nth-child(5)");

                var league = (await page.EvaluateFunctionAsync<string>("el => el.textContent", leagueEl)).Trim();

                events.Add(new EventDTO(team2, team1, startDate, endDate, league, "basketball"));
            }

            return events;
        }

        private DateTime ParseLithuanianDate(string date)
        {
            string format = "dddd, MMMM dd 'd.'";
            CultureInfo culture = new CultureInfo("lt-LT");

            DateTime result;
            DateTime.TryParseExact(date, format, culture, DateTimeStyles.None, out result);

            return result;
        }

        private TimeSpan ParseTime(string timeString)
        {
            TimeSpan time;
            TimeSpan.TryParse(timeString, out time);
            
            return time;
        }
    }
}
