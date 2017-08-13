using System.IO;
using System.Linq;
using Alfred.Services.Models;
using Newtonsoft.Json;

namespace Alfred.Services
{
    public class PeacePrizeWinnerService : IPeacePrizeWinnerService
    {
        private readonly RootObject _prizes;
        public PeacePrizeWinnerService()
        {
            _prizes = ReadPrizesFromFile();
        }

        public Prize GetPrizeByYear(long year)
        {
            return _prizes.Prizes.FirstOrDefault(x => x.Year == year.ToString());
        }

        private RootObject ReadPrizesFromFile()
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Alfred.Services.Data.PeacePrizeWinners.json"))
            {
                using (var sr = new StreamReader(stream))
                {
                    var result = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<RootObject>(result);
                }
            }
        }

    }
}
